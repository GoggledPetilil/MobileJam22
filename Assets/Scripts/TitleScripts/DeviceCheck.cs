using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeviceCheck : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject m_Holder;
    [SerializeField] private GameObject m_UIElements;
    [SerializeField] private TMP_Text m_InfoText;
    [SerializeField] private GameObject m_ButtonYes;
    [SerializeField] private GameObject m_ButtonNo;
    [SerializeField] private GameObject m_ButtonConfirm;
    [SerializeField] private Image m_BlackBG;
    [SerializeField] private TMP_Text m_PressStart;
    [SerializeField] private MainMenu m_mainMenu;
    private string m_DeviceName;

    [Header("Audio")]
    [SerializeField] private AudioSource m_audio;
    [SerializeField] private AudioClip m_PopUpSFX;

    // Start is called before the first frame update
    void Start()
    {
        m_Holder.SetActive(true);
        
        m_ButtonYes.SetActive(true);
        m_ButtonNo.SetActive(true);
        m_ButtonConfirm.SetActive(false);

        m_mainMenu.enabled = false;

        m_DeviceName = "Unknown Device";
        if(SystemInfo.deviceType == DeviceType.Handheld)
        {
            m_DeviceName = "Mobile";
        }
        else if(SystemInfo.deviceType == DeviceType.Desktop)
        {
            m_DeviceName = "Desktop";
        }

        PlayPopUpSFX();
        StartCoroutine("PopUpText");
    }

    public void DeactivateSelf()
    {
        m_UIElements.SetActive(false);
        StartCoroutine("FadeToGame");
    }

    public void ChoseDesktop()
    {
        ChoseDevice("Desktop");
        GameManager.instance.SwitchToDesktop();
        m_PressStart.text = "Press Space";
    }

    public void ChoseMobile()
    {
        ChoseDevice("Mobile");
        GameManager.instance.SwitchToMobile();
        m_PressStart.text = "Tap to Start";
    }

    private void ChoseDevice(string device)
    {
        m_ButtonYes.SetActive(false);
        m_ButtonNo.SetActive(false);
        m_ButtonConfirm.SetActive(true);

        PlayPopUpSFX();
        StartCoroutine("PopUpText");

        m_DeviceName = device;

        m_InfoText.text = ("The game has switched to\n" + m_DeviceName + " settings.");
    }

    private void PlayPopUpSFX()
    {
        m_audio.clip = m_PopUpSFX;
        m_audio.Play();
    }

    IEnumerator PopUpText()
    {
        RectTransform elements = m_UIElements.GetComponent<RectTransform>();
        float dur = 0.2f;
        float t = 0.0f;
        while(t < 1.0f)
        {
            t += Time.deltaTime / dur;
            elements.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
            yield return null;
        }
        yield return null;
    }

    IEnumerator FadeToGame()
    {
        Color c = m_BlackBG.color;
        float fadeDur = 1.5f;
        float t = 0.0f;
        while(t < 1.0f)
        {
            t += Time.deltaTime / fadeDur;
            m_BlackBG.color = new Color(c.r, c.g, c.b, 1.0f - t);
            yield return null;
        }
        m_BlackBG.color = new Color(c.r, c.g, c.b, 0.0f);
        m_BlackBG.gameObject.SetActive(false);
        m_mainMenu.enabled = true;
        yield return null;
    }
}
