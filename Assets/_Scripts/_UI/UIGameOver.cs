using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGameOver : MonoBehaviour
{
    private SoundManager soundManager;

    [Header("GameOver")]
    public Button retryButton;
    public Button backMenu;

    private void Awake()
    {
        DestroyGameManager(); // Xóa GameManager nếu cần thiết
    }

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SetupSoundManager();  // Thiết lập SoundManager
        SetupButtonEvents();  // Gán sự kiện cho các nút
    }

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

    private void SetupButtonEvents()
    {
        if (retryButton != null)
        {
            retryButton.onClick.AddListener(Retry);  // Retry button sẽ reload game
            retryButton.onClick.AddListener(() => soundManager?.ClickMenu());  // Phát âm thanh khi click retry
        }

        if (backMenu != null)
        {
            backMenu.onClick.AddListener(ExitToMenu);  // Quay về menu chính
            backMenu.onClick.AddListener(() => soundManager?.ClickMenu());  // Phát âm thanh khi click back
        }
    }

    public void Retry()
    {
        // Tải lại scene "InGame"
        SceneManager.LoadScene("InGame");
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    private void DestroyGameManager()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        if (gameManager != null)
        {
            Destroy(gameManager);
        }
    }
}
