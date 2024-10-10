using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIMenu : MonoBehaviour
{
    private SoundManager soundManager;

    [Header("Menu")]
    public Button startButton;
    public Button difficultButton;
    public Button settingButton;
    public Button quitButton;
    public Button BT_Back;
    public GameObject pSetting;
    public Slider Sld_Sound;
    public Slider Sld_Music;
    private bool isSetting;


    void Start()
    {
        isSetting = false;
        SetupSoundManager();  // Thiết lập SoundManager
        SetupButtonEvents();  // Gán sự kiện cho các nút
        SetUpSliderEvent();

        if (Sld_Sound != null)
        {
            Sld_Sound.value = 0.8f;  // Đặt giá trị mặc định là 0.8 cho thanh âm thanh hiệu ứng
        }
        if (Sld_Music != null)
        {
            Sld_Music.value = 0.8f;  // Đặt giá trị mặc định là 0.8 cho thanh âm nhạc
        }
    }

    // Tìm SoundManager trong scene và gán nó vào biến soundManager
    private void SetupSoundManager()
    {
        GameObject soundManagerObj = GameObject.Find("SoundManager");
        if (soundManagerObj != null)
        {
            soundManager = soundManagerObj.GetComponent<SoundManager>();
        }
        else
        {
            Debug.LogError("SoundManager not found in the scene.");
        }
    }

    // Gán sự kiện cho các button
    private void SetupButtonEvents()
    {
        if (startButton != null)
        {
            startButton.onClick.AddListener(GameStart);
            startButton.onClick.AddListener(() => soundManager?.ClickMenu());  // Phát âm thanh khi click start button
        }
        if (difficultButton != null)
        {
            difficultButton.onClick.AddListener(() => Debug.Log("Difficulty button clicked"));
            difficultButton.onClick.AddListener(() => soundManager?.ClickMenu());  // Phát âm thanh khi click
        }
        if (settingButton != null)
        {
            settingButton.onClick.AddListener(ToggleSetting);
            settingButton.onClick.AddListener(() => soundManager?.ClickMenu());  // Phát âm thanh khi click

        }
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
            quitButton.onClick.AddListener(() => soundManager?.ClickMenu());  // Phát âm thanh khi click
        }
    }
    public void GameStart()
    {
        SceneManager.LoadScene("InGame");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    private void ToggleSetting()
    {
        isSetting = !isSetting;  // Đảo trạng thái của isSetting
        pSetting.SetActive(isSetting);  //  Bật/tắt panel dựa vào isSetting
    }
    private void SetUpSliderEvent()
    {
        if (Sld_Sound != null)
        {
            Sld_Sound.onValueChanged.AddListener(SetSFXVolume);
        }
        if (Sld_Music != null)
        {
            Sld_Music.onValueChanged.AddListener(SetMusicVolume);
        }
    }
    private void SetSFXVolume(float volume)
    {
        soundManager.SetSFXVolume(volume);
    }
    private void SetMusicVolume(float volume)
    {
        soundManager.SetMusicVolume(volume);
    }
}
