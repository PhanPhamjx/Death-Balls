using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBallFly : MonoBehaviour
{
    public float ballWeight = 1f;
    private Rigidbody2D deathballRb;
    // Start is called before the first frame update
    void Start()
    {
        deathballRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        MakeDeathBallsFly();
    }
    public void MakeDeathBallsFly()
    {
            /*Rigidbody2D rb = deathBalls.GetComponent<Rigidbody2D>()*/;
            if (deathballRb != null)
            {
                // Tạo lực đẩy lên dựa trên trọng lượng của bóng
                Vector2 force = Vector2.up * ballWeight;
            deathballRb.AddForce(force, ForceMode2D.Force);
            }
            
        
    }
}
