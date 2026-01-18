using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;      // Player
    [SerializeField] private Vector3 offset = new Vector3(0, 5, -7);
    [SerializeField] private float moveSmooth = 10f;
    [SerializeField] private float rotateSmooth = 10f;

    private void LateUpdate()
    {
        if (!target) return;

        Vector3 desiredPos = target.position + target.rotation * offset;
        transform.position = Vector3.Lerp(transform.position, desiredPos, moveSmooth * Time.deltaTime);

        Quaternion targetRotation = Quaternion.Euler(0f, target.eulerAngles.y, 0f);

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            rotateSmooth * Time.deltaTime
        );
    }
}
