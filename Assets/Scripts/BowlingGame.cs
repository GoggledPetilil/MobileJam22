using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingGame : MonoBehaviour
{
    [SerializeField] private GameObject m_PinPrefab;
    public int m_TotalPins;
    public float verticalMod = 0.8f;
    public float horizontalMod = 1.25f;
    private float yPos;
    //public float zOffset;

    public bool reset;

    // Start is called before the first frame update
    void Start()
    {
        yPos = transform.position.y;
        CreateFormation();
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
        List<Vector3> newpositions = new List<Vector3>();
        
        int height = Mathf.CeilToInt((Mathf.Sqrt(8*m_TotalPins+1f)-1f)/2);
        int slots = (int)(height * (height+1f)/2f);

        float width = 0.5f * (height-1f);
        Vector3 startPos = new Vector3(0, yPos, 0);
        //Vector3 startPos = new Vector3(width * horizontalMod, yPos, (float)(height-1f)*verticalMod);

        int finalRowCount = height - slots + m_TotalPins;
        for(int rowNum = 0; rowNum < height && newpositions.Count < m_TotalPins; rowNum++)
        {
            for(int i = 0; i < rowNum+1 && newpositions.Count < m_TotalPins; i++)
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
                newpositions.Add(position);

                GameObject instance = Instantiate(m_PinPrefab);
                instance.transform.position = position;
                instance.transform.SetParent(this.transform);
            }
        }
        
        /*Vector3 targetPos = Vector3.left;

        for(int i = 1; i <= rows; i++)
        {
            for(int j = 0; j < i; j++)
            {
                GameObject instance = Instantiate(m_PinPrefab);
                targetPos = new Vector3(targetPos.x + xOffset, yPos, targetPos.z);
                instance.transform.position = targetPos;
                instance.transform.SetParent(this.transform);
            }
            // Offset the new row
            targetPos = new Vector3((rowOffset * i) - xOffset/2, yPos, targetPos.z + zOffset);

        }*/
    }
}
