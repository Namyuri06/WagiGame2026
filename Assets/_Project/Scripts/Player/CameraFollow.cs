using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target; // 자동으로 찾을 비둘기 변수
    public float smoothSpeed = 5f;
    public float yOffset = 1.5f;

    void Start()
    {
        // 게임이 시작될 때 "doolgi"라는 이름을 가진 오브젝트를 알아서 찾아냄!
        GameObject player = GameObject.Find("doolgi");
        if (player != null)
        {
            target = player.transform;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = new Vector3(target.position.x, target.position.y + yOffset, transform.position.z);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
