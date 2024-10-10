using UnityEngine;

public class ObjectFollowMouse : MonoBehaviour
{
    public Transform Filled_Follower;  // Đối tượng theo dõi

    public float moveSpeed = 1f;       // Tốc độ di chuyển của vật thể dựa trên chuột
    public float followerSpeed = 0.5f; // Tốc độ theo dõi của đối tượng Filled_Follower
    public float minX = -3.18f;        // Giới hạn trái của vùng di chuyển
    public float maxX = 3.18f;         // Giới hạn phải của vùng di chuyển
    private void Start()
    {

    }
    void Update()
    {
        if (Time.timeScale > 0)
        {
            PumpMove();
            Follower();
        }
    }
    private void PumpMove()
    {
        // Lấy giá trị di chuyển theo trục X của chuột
        float mouseX = Input.GetAxis("Mouse X");
        // Tính toán khoảng cách di chuyển dựa trên tốc độ của chuột và hệ số di chuyển
        float moveDistance = mouseX * moveSpeed;

        // Cập nhật vị trí của vật thể dựa trên khoảng cách di chuyển
        Vector3 newPosition = transform.position + new Vector3(moveDistance, 0, 0);

        // Giới hạn vị trí của vật thể trong khoảng minX đến maxX
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);

        // Đặt vị trí mới cho vật thể
        transform.position = newPosition;
    }

    private void Follower()
    {

        // Di chuyển Filled_Follower về vị trí của đối tượng với độ trễ
        Filled_Follower.transform.position = Vector3.Lerp(Filled_Follower.transform.position, this.transform.position, followerSpeed * Time.deltaTime);

    }
}
