using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public string m_LoadedMenu;
    [SerializeField] private GameObject m_MainHolder;
    [SerializeField] private GameObject m_PressStart;

    void Start()
    {
        m_LoadedMenu = "Title";
    }

    void Update()
    {
        switch(m_LoadedMenu)
        {
            case "Title":
                if(Input.touchCount > 0 || Input.GetKeyDown(KeyCode.Space))
                {
                    m_LoadedMenu = "Main";
                    StartGame();
                }
                break;
            case "Main":
                // stuff
                break;
            
        }
        
    }

    public void StartGame()
    {
        m_PressStart.SetActive(false);
        m_MainHolder.SetActive(true);
    }
}
