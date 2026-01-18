using UnityEngine;
using UnityEngine.AI;

public class EnemyVisualizer : MonoBehaviour
{


    [Header("Vision")]
    [SerializeField] private float viewDistance = 10f;
    [SerializeField] private float viewAngle = 60f;
    [SerializeField] private Color visionColor = Color.green;
    [SerializeField] private Color detectedColor = Color.red;


    [Header("References")]
    [SerializeField] private Transform target;

    [Header("Colors")]
    [SerializeField] private Color targetLineColor = Color.yellow;
    [SerializeField] private Color pathColor = Color.cyan;

    private void Update()
    {
        if (target != null)
        {
            Debug.DrawLine(transform.position, target.position, targetLineColor);
        }
        DrawVisionCone();
    }

    private void DrawVisionCone()
    {
        Vector3 origin = transform.position;
        Vector3 forward = transform.forward;

        Debug.DrawRay(origin, forward * viewDistance, visionColor);

        float halfAngle = viewAngle / 2f;

        Vector3 leftDir = Quaternion.Euler(0, -halfAngle, 0) * forward;
        Vector3 rightDir = Quaternion.Euler(0, halfAngle, 0) * forward;

        Debug.DrawRay(origin, leftDir * viewDistance, visionColor);
        Debug.DrawRay(origin, rightDir * viewDistance, visionColor);

        if (target != null)
        {
            Vector3 toTarget = (target.position - origin).normalized;
            float angleToTarget = Vector3.Angle(forward, toTarget);
            float distanceToTarget = Vector3.Distance(origin, target.position);

            if (angleToTarget <= halfAngle && distanceToTarget <= viewDistance)
            {
                Debug.DrawLine(origin, target.position, detectedColor);
            }
        }
    }
}