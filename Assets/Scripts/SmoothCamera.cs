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
            // ��ǥ ��ġ�� �÷��̾��� ��ġ + ������
            Vector3 targetPosition = player.position + offset;
            // ī�޶��� ��ġ�� �ε巴�� �̵�
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        }
    }
}
