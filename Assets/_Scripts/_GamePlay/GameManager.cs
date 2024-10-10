using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshPro scoreText;
    public TextMeshPro Txt_TimeSpawn;
    public GameObject UI;

    public static GameManager Instance;

    [Header("logic DeathBalls")]
    public float timeDeathBallApear = 60;
    public GameObject deathBallPrefab;
    private bool checkdeathballAppear;
    private float soundCoolDown = 0.2f;
    private float lastPlayTime = 0f;

    [Header("Check GameOver")]
    public GameObject checkGameOver;
    private Collider2D[] colliders = new Collider2D[10];
    public int ballforGameOver;

    public TextMeshPro Txt_countBalls;
    [Header("Pause Game")]
    public GameObject GUI_Pause;
   // public Button BT_Menu;
    private bool isPause;

    [Header("Timer Settings")]
    public float initialTimeSpawn = 2f;
    private float timeSpawn;
    private int _score;

    [Header("Camera view")]
    public Camera mainCamera;

    public int Score
    {
        get { return _score; }
        set
        {
            _score = value;
            UpdateScoreUI();
        }
    }

    public float TimeSpawn
    {
        get { return timeSpawn; }
        set
        {
            timeSpawn = value;
            TimeSpawnUI();
        }
    }

    void Awake()
    {
        SoundManager.instance.PlayMusic(SoundManager.instance.inGame);
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object between scene loads
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        //AssignButtonListeners();
        SoundManager.instance.PlayMusic(SoundManager.instance.inGame);
        FindReferences();
        StartCoroutine(TimeSpawnDeathBall(timeDeathBallApear));
        Score = 1;
        TimeSpawn = initialTimeSpawn;

    }

    void Update()
    {
        UpdateTimer();
        CheckGameOverPoints();
        SoundDeathBall();
        PauseGameButton();
       
    }

    public void PauseGame()
    {
        isPause = !isPause;  // Đảo trạng thái tạm dừng
        GUI_Pause.SetActive(isPause);  // Hiển thị hoặc ẩn giao diện Pause

        if (isPause)
        {
            Cursor.visible = true;  // Hiển thị con trỏ khi tạm dừng
            Time.timeScale = 0;     // Dừng thời gian game
            SoundManager.instance.PauseMusic();  // Dừng nhạc nền
        }
        else
        {
            Cursor.visible = false;  // Ẩn con trỏ khi tiếp tục
            Time.timeScale = 1;      // Tiếp tục thời gian game
            SoundManager.instance.ResumeMusic();  // Tiếp tục phát nhạc nền
        }
    }

    private void PauseGameButton()
    {
        // Kiểm tra nếu người chơi nhấn phím Esc hoặc P
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            PauseGame();  // Gọi hàm để bật/tắt Pause
        }
    }

    public void GameOver()
    {
        SoundManager.instance.musicSource.Stop();
        SceneManager.LoadScene("GameOver");
    }

    public void GameReset()
    {

        _score = 1;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene("InGame");
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    //private void AssignButtonListeners()
    //{
    //    if (BT_Menu != null)
    //    {
    //        BT_Menu.onClick.AddListener(QuitToMenu);
    //    }
    //    else
    //    {
    //        Debug.LogWarning("BT_Menu is not assigned in the inspector.");
    //    }
    //}
    private void UpdateTimer()
    {
        if (TimeSpawn >= 0)
        {
            TimeSpawn -= Time.deltaTime;
        }
        else
        {
            HandleTimerReset();
        }
    }

    private void HandleTimerReset()
    {
        TimeSpawn = initialTimeSpawn;
    }

    private void UpdateScoreUI()
    {
        scoreText = GameObject.Find("Txt_Score")?.GetComponent<TextMeshPro>();
        if (scoreText != null)
        {

            scoreText.text = "Score: " + _score.ToString();
        }
        else
        {
            //scoreText = GameObject.add.GetComponent<TextMeshPro>();
            Debug.LogWarning("scoreText is not assigned.");
        }
    }

    private void TimeSpawnUI()
    {
        if (Txt_TimeSpawn != null)
        {
            Txt_TimeSpawn.text = TimeSpawn.ToString("F1"); // Format with one decimal place
        }
    }

    private void SpawnDeathBall()
    {
        if (deathBallPrefab != null)
        {
            Instantiate(deathBallPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            checkdeathballAppear = true;

        }
        else
        {
            Debug.LogError("DeathBall prefab không được gán.");
        }
    }
    public void SoundDeathBall()
    {
        if (Time.time - lastPlayTime < soundCoolDown) return;
        if (checkdeathballAppear)
        {
            SoundManager.instance.PlaySfX(SoundManager.instance.blow);
            lastPlayTime = Time.time;
            checkdeathballAppear = false;
        }
    }

    private void CheckGameOverPoints()
    {
        checkGameOver = GameObject.Find("Check");
        if (checkGameOver == null)
        {
            Debug.LogWarning("checkGameOver object is null.");
            return;
        }

        Collider2D checkGameOverCollider = checkGameOver.GetComponent<Collider2D>();
        if (checkGameOverCollider == null)
        {
            Debug.LogWarning("Collider2D component not found on checkGameOver object.");
            return;
        }

        int colliderCount = Physics2D.OverlapCollider(checkGameOverCollider, new ContactFilter2D(), colliders);

        if (colliderCount >= 5)
        {
            GameOver();
        }
    }

    private IEnumerator TimeSpawnDeathBall(float timeInterval)
    {

        while (true)
        {
            SpawnDeathBall();

            yield return new WaitForSeconds(timeInterval);
        }
    }
    // Tìm và gán lại các tham chiếu cần thiết
    private void FindReferences()
    {
        if (scoreText == null)
        {
            scoreText = GameObject.Find("Txt_Score")?.GetComponent<TextMeshPro>();
            Debug.Log("ko tim thay Txt_Score");
        }

        if (Txt_TimeSpawn == null)
        {
            Txt_TimeSpawn = GameObject.Find("Txt_TimeSpawn")?.GetComponent<TextMeshPro>();
        }

        if (checkGameOver == null)
        {
            checkGameOver = GameObject.Find("Check");
        }
    }

}
