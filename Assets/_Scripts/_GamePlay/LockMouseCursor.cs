using UnityEngine;

public class LockMouseCursor : MonoBehaviour
{
    void Start()
    {
        // Khóa con trỏ chuột vào giữa màn hình
        Cursor.lockState = CursorLockMode.Confined;

        // Ẩn con trỏ chuột (nếu muốn)
        Cursor.visible = false;
    }

    void Update()
    {
        // Kiểm tra nếu người dùng nhấn phím Esc để thoát khỏi khóa chuột
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Mở khóa và hiển thị lại con trỏ chuột
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
