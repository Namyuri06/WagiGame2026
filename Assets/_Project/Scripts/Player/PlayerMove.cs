using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        // 좌우 방향키나 A/D 키 입력 받기
        float moveInput = Input.GetAxisRaw("Horizontal");

        // 직접 좌표를 이동시키는 코드
        transform.Translate(new Vector3(moveInput * moveSpeed * Time.deltaTime, 0, 0));
    }
}
