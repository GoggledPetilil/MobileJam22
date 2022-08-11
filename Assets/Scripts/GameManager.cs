using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool isDesktop;
    
    [Header("BGM Components")]
    [SerializeField] private AudioSource m_BGMPlayer;
    [SerializeField] private AudioClip m_TitleTheme;

    [Header("SFX Components")]
    [SerializeField] private AudioSource m_SFXPlayer;
    [SerializeField] private AudioClip m_ClickSFX;

    [Header("Components")]
    [SerializeField] private Joystick m_joystick;
    [SerializeField] private CanvasGroup m_loadingGroup;
    [SerializeField] private Slider m_LoadingsLider;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        m_loadingGroup.alpha = 0.0f;
    }

    public void PlayTitleTheme()
    {
        m_BGMPlayer.clip = m_TitleTheme;
        m_BGMPlayer.Play();
    }
    
    public void PlayClick()
    {
        PlaySFX(m_ClickSFX);
    }

    public void PlaySFX(AudioClip clip)
    {
        m_SFXPlayer.clip = clip;
        m_SFXPlayer.Play();
    }

    public void SwitchToDesktop()
    {
        isDesktop = true;
        m_joystick.gameObject.SetActive(false);
    }

    public void SwitchToMobile()
    {
        isDesktop = false;
        m_joystick.gameObject.SetActive(true);
    }

    public void LoadNewScene(int sceneID)
    {
        StartCoroutine(LoadAsynchronously(sceneID));
    }

    IEnumerator LoadAsynchronously(int sceneID)
    {
        m_loadingGroup.alpha = 0.0f;
        m_LoadingsLider.value = 0.0f;
        float dur = 0.5f;
        float t = 0.0f;
        while(t < 1.0f)
        {
            t += Time.deltaTime / dur;
            m_loadingGroup.alpha = t;
            yield return null;
        }

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID);

        while(!operation.isDone)
        {
            //float progress = Mathf.Clamp01(operation.progress / 0.9f);
            m_LoadingsLider.value = operation.progress;

            yield return null;
        }
    }
}
