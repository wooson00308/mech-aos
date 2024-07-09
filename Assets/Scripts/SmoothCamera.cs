using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public float smoothSpeed = 0.125f;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player Transform is not assigned.");
        }
    }

    void LateUpdate()
    {
        if (player != null)
        {
            // 목표 위치는 플레이어의 위치 + 오프셋
            Vector3 targetPosition = player.position + offset;
            // 카메라의 위치를 부드럽게 이동
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        }
    }
}
