using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BowlingGame : MonoBehaviour
{
    [SerializeField] private GameObject m_PinPrefab;
    List<BowlingPin> m_AllPins = new List<BowlingPin>();
    public GameObject m_Middle;
    public int m_TotalPins;
    public float verticalMod = 0.8f;
    public float horizontalMod = 1.25f;
    private float yPos;
    [SerializeField] private TMP_Text m_ScoreText;
    private int m_Score;

    public bool reset;

    // Start is called before the first frame update
    void Start()
    {
        yPos = transform.position.y;
        CreateFormation();

        m_Score = 0;
        m_ScoreText.text = m_Score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(reset)
        {
            reset = false;
            foreach(GameObject pin in GameObject.FindGameObjectsWithTag("BowlingPin"))
            {
                Destroy(pin);
            }
            CreateFormation();
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
            }
        }
        m_Middle.transform.position = new Vector3(0.0f, 0.0f, height / 2f);
    }

    public void KillDeadPins()
    {
        StartCoroutine("KillPins");
    }

    IEnumerator KillPins()
    {
        foreach(BowlingPin pin in m_AllPins)
        {
            if(pin.isGrounded())
            {
                pin.FreezeBody();
            }
            if(pin.isKnockedOver() || pin.m_com.y > 0.8f || !pin.isGrounded())
            {
                //pin.DestroySelf();
                m_Score++;
            }
        }

        yield return null;
        float t = 0.0f;
        float dur = 2.0f;
        while(t < 1.0f)
        {
            t += Time.deltaTime / dur;
            m_ScoreText.text = Mathf.Lerp(0, m_Score, t).ToString("F0");
            yield return null;
        }
    }
}
