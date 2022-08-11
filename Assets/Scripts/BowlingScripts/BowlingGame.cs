using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BowlingGame : MonoBehaviour
{
    [Header("Bowling Components")]
    [SerializeField] private GameObject m_PinPrefab;
    public List<BowlingPin> m_AllPins = new List<BowlingPin>();
    public List<Vector3> m_AllPinPos = new List<Vector3>();
    public GameObject m_Middle;
    private bool m_WaitingInput;
    private int m_Round;

    [Header("Bowling Parameters")]
    public int m_TotalPins;
    public float verticalMod = 0.8f;
    public float horizontalMod = 1.25f;
    private float yPos;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text m_ScoreText;
    [SerializeField] private TMP_Text m_BigScoreText;
    private int m_Score;

    [Header("Audio")]
    [SerializeField] private AudioSource m_as;
    [SerializeField] private AudioClip m_winSFX;
    [SerializeField] private AudioClip m_countingSFX;

    // Start is called before the first frame update
    void Start()
    {
        yPos = transform.position.y;
        CreateFormation();

        m_Score = 0;
        m_ScoreText.text = m_Score.ToString();
        m_BigScoreText.text = m_Score.ToString();
        m_BigScoreText.gameObject.SetActive(false);

        m_Round = 0;
        m_WaitingInput = false;
    }

    void Update()
    {
        if(m_WaitingInput == false) return;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            m_WaitingInput = false;
            m_Round++;
            StartCoroutine("OnToNextRound");
        }
    }

    void CreateFormation()
    {
        int height = Mathf.CeilToInt((Mathf.Sqrt(8*m_TotalPins+1f)-1f)/2);
        int slots = (int)(height * (height+1f)/2f);

        float width = 0.5f * (height-1f);
        Vector3 startPos = new Vector3(0, yPos, 0);

        int finalRowCount = height - slots + m_TotalPins;
        for(int rowNum = 0; rowNum < height && m_AllPins.Count < m_TotalPins; rowNum++)
        {
            for(int i = 0; i < rowNum+1 && m_AllPins.Count < m_TotalPins; i++)
            {
                float xOffset = 0f;
                if(rowNum+1 == height)
                {
                    if(finalRowCount != 1)
                    {
                        xOffset = Mathf.Lerp(rowNum/2f, -rowNum/2f, i/(finalRowCount-1f)) * horizontalMod;
                    }
                }
                else 
                {
                    xOffset = (i-rowNum / 2f) * horizontalMod;
                }
                float zOffset = (float)rowNum * verticalMod;

                Vector3 position = new Vector3(startPos.x + xOffset, yPos, startPos.z + zOffset);

                GameObject instance = Instantiate(m_PinPrefab);
                instance.transform.position = position;
                instance.transform.SetParent(this.transform);
                m_AllPins.Add(instance.GetComponent<BowlingPin>());
                m_AllPinPos.Add(position);
            }
        }
        m_Middle.transform.position = new Vector3(0.0f, 0.0f, height / 2f);
    }

    public void EndRound()
    {
        StartCoroutine("EndTheRound");
    }

    IEnumerator EndTheRound()
    {
        int startScore = m_Score;
        // Get each pin and see which ones were knocked over.
        foreach(BowlingPin pin in m_AllPins)
        {
            if(pin.isGrounded())
            {
                pin.FreezeBody();
            }
            if(pin.isKnockedOver() || pin.m_com.y > 0.8f || !pin.isGrounded())
            {
                if(pin.gameObject.activeSelf)
                {
                    // Only count pins that are actually still active.
                    pin.m_beenHit = true;
                    m_Score++;
                }
            }
        }
        int endScore = m_Score;

        // Push the Score to the center.
        float dur = 0.2f;
        float t = 0.0f;
        while(t < 1.0f)
        {
            t += Time.deltaTime / dur;
            m_ScoreText.rectTransform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);
            yield return null;
        }
        m_BigScoreText.gameObject.SetActive(true);
        t = 0.0f;
        while(t < 1.0f)
        {
            t += Time.deltaTime / dur;
            m_BigScoreText.rectTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
            yield return null;
        }

        // Count all the knocked pins
        yield return null;
        dur = 2.0f;
        t = 0.0f;
        if(endScore == startScore)
        {
            t = 0.99f;
        }

        m_as.clip = m_countingSFX;
        m_as.loop = true;
        m_as.Play();
        while(t < 1.0f)
        {
            t += Time.deltaTime / dur;
            m_BigScoreText.text = Mathf.Lerp(startScore, endScore, t).ToString("F0");
            yield return null;
        }
        m_as.Stop();
        m_ScoreText.text = m_Score.ToString("F0");

        m_as.clip = m_winSFX;
        m_as.loop = false;
        m_as.Play();

        if(m_Round == 0)
        {
            // Go to the second round.
            m_WaitingInput = true;
        }
        else 
        {
            // Show some kinda result screen??
            Debug.Log("Bowling section has ended!");
        }

        yield return null;
    }

    IEnumerator OnToNextRound()
    {
        // Return score back to corner.
        float dur = 0.2f;
        float t = 0.0f;
        while(t < 1.0f)
        {
            t += Time.deltaTime / dur;
            m_BigScoreText.rectTransform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);
            yield return null;
        }
        t = 0.0f;
        while(t < 1.0f)
        {
            t += Time.deltaTime / dur;
            m_ScoreText.rectTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
            yield return null;
        }

        // Remove all the dead pins
        // Put all the alive pins in their proper place again
        for(int i = 0; i < m_AllPins.Count; i++)
        {
            BowlingPin pin = m_AllPins[i];
            if(pin.m_beenHit == true && pin != null)
            {
                pin.gameObject.SetActive(false);
                //m_AllPinPos.RemoveAt(m_AllPins.IndexOf(pin));
                //m_AllPins.Remove(pin);
            }
            else if(pin != null)
            {
                pin.ResetObject();
                pin.transform.position = m_AllPinPos[i];
            }
        }
        yield return null;
        // Get everything ready for the second round.
        BowlingBall ball = GameObject.FindGameObjectWithTag("BowlingBall").GetComponent<BowlingBall>();
        BowlingPlayer player = GameObject.FindGameObjectWithTag("Player").GetComponent<BowlingPlayer>();
        ball.transform.SetParent(player.gameObject.transform);
        ball.ResetState();
        player.ResetState();
        Camera.main.GetComponent<BowlingCamera>().FocusOnPlayer();
        
        yield return null;
    }
}
