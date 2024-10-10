using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class _BalloonController : MonoBehaviour
{
    public string balloonType;
    public float maxScale = 1.8f;
    public float scaleSpeed = 1.0f;
    public Vector2 initialScale = new Vector2(0.6f, 0.6f);
    private int currentValue = 0;

    private bool isHolding = false;
    private bool hasBurst = false;
    public bool isBaloonInPoit;

    public GameObject posPump;

    private Collider2D colPump;
    private Rigidbody2D rb;
    private TextMeshPro balloonText;
    public ParticleSystem _Blow;
    private List<GameObject> collidingBalloons = new List<GameObject>();

    private bool checksoundHit;
    private bool checksoundCol;

    private float lastPlayTime = 0f;
    public float soundCooldown = 0.2f;
    void Start()
    {

        ;
        posPump = GameObject.Find("spawnPos");
        if (posPump != null)
        {
            colPump = posPump.GetComponent<Collider2D>();
            if (colPump == null)
            {
                colPump = posPump.AddComponent<CircleCollider2D>();
                colPump.isTrigger = true;
            }
        }
        else
        {
            Debug.Log("Không tìm thấy đối tượng với tag 'pump'.");
        }

        InitializeBalloon();
    }
    void Update()
    {

        HandleMouseInput();
        CheckBurstConditions();
        FollowPump();
        if (currentValue >= 1 && Time.timeScale > 0)
        {
            rb.gravityScale = -0.1f;
        }
        PlayFSX();
    }
    private void FollowPump()
    {
        if (!isHolding && isBaloonInPoit && currentValue <= 0)
        {
            this.transform.position = posPump.transform.position;
        }
    }
    private void InitializeBalloon()
    {
        transform.localScale = initialScale;
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component is missing on the balloon object.");
            return;
        }

        balloonText = GetComponentInChildren<TextMeshPro>();

        if (balloonText == null)
        {
            Debug.LogError("TextMeshPro component is missing in the balloon prefab.");
            return;
        }

        balloonText.text = currentValue.ToString();
    }
    public void InitializeRandomBalloons(float initialScale, int value, int speedScale)
    {
        this.initialScale = new Vector2(initialScale, initialScale);
        currentValue = value;
        scaleSpeed = speedScale;
        if (balloonText != null)
        {
            balloonText.text = currentValue.ToString();
        }

        // Thêm hoặc lấy Rigidbody2D component
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        rb.gravityScale = -0.5f; // Giá trị âm để bóng bay lên
        rb.AddForce(Vector2.up * 5f, ForceMode2D.Impulse); // Điều chỉnh lực tùy ý
    }
    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0) && !hasBurst)
        {
            isHolding = true;
            StartCoroutine(InflateBalloon());

        }

        if (Input.GetMouseButtonUp(0) && !hasBurst)
        {
            isHolding = false;
            OnReleaseBalloon();
        }
    }
    private void DestroyBalloonWithTag()
    {
        if (collidingBalloons.Count >= 3)
        {
            //GameManager here
            GameManager.Instance.Score += currentValue;

            // Avoid modifying the list while iterating over it
            List<GameObject> balloonsToDestroy = new List<GameObject>(collidingBalloons);
            foreach (GameObject balloon in balloonsToDestroy)
            {

                PlayExplosionEffect();
                Destroy(balloon, 0.1f);
            }
            collidingBalloons.Clear(); // Clear the list after destroying balloons

        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {

        OnCollisionStay2D(other);
        
    }
    private void OnCollisionStay2D(Collision2D other)
    {
        if (this.gameObject.CompareTag(other.gameObject.tag))
        {

            if (!collidingBalloons.Contains(this.gameObject)) collidingBalloons.Add(this.gameObject);
            if (!collidingBalloons.Contains(other.gameObject)) collidingBalloons.Add(other.gameObject);

            if (collidingBalloons.Count >= 3) DestroyBalloonWithTag();
        }

    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (this.gameObject.CompareTag(other.gameObject.tag))
        {
            collidingBalloons.Remove(other.gameObject);
            collidingBalloons.Remove(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == colPump)
        {
            isBaloonInPoit = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == colPump)
        {
            isBaloonInPoit = false;
        }
    }
    private void OnReleaseBalloon()
    {
        if (isBaloonInPoit)
        {

            balloonText.text = currentValue.ToString();
            rb.gravityScale = -0.2f;
        }
    }
    private void CheckBurstConditions()
    {
        if (!hasBurst && (transform.localScale.x > maxScale || currentValue >= 10))
        {
            hasBurst = true;

            // Trừ điểm nhưng đảm bảo không giảm dưới 0
            GameManager.Instance.Score = Mathf.Max(0, GameManager.Instance.Score - currentValue);
            PlayExplosionEffect();
            // SoundManager.instance.PlaySfX(SoundManager.instance.blow);
            Destroy(gameObject, 0.1f);
        }
    }
    private void PlayExplosionEffect()
    {
        if (_Blow != null)
        {
            ParticleSystem explosion = Instantiate(_Blow, transform.position, Quaternion.identity);
            explosion.Play();
            checksoundHit = true;
            Destroy(explosion.gameObject, 0.1f);
        }
    }
    private IEnumerator InflateBalloon()
    {
        while (isHolding)
        {
            if (isBaloonInPoit)
            {
                
                transform.localScale += Vector3.one * scaleSpeed * Time.deltaTime;
                balloonText.text = currentValue.ToString();
                if (transform.localScale.x > maxScale || currentValue >= 10)
                {

                    hasBurst = true;
                    // Trừ điểm nhưng đảm bảo không giảm dưới 0
                    GameManager.Instance.Score = Mathf.Max(0, GameManager.Instance.Score - currentValue);

                    PlayExplosionEffect();

                    Destroy(gameObject, 0.1f);
                    yield break;
                }

                currentValue++;
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                yield break;
            }
        }
    }
    public void PlayFSX()
    {
        if (Time.time - lastPlayTime < soundCooldown) return;

        if (checksoundHit)
        {
            SoundManager.instance.PlaySfX(SoundManager.instance.hit);
            lastPlayTime = Time.time; // Cập nhật thời gian lần phát âm thanh cuối cùng
        }

        if (checksoundCol)
        {
            SoundManager.instance.PlaySfX(SoundManager.instance.col);
            checksoundCol = false;  
            lastPlayTime = Time.time; // Tương tự với các âm thanh khác
        }
    }

}
