using UnityEngine;

public class AimLine : MonoBehaviour
{
    [SerializeField] private LineRenderer line;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float lineLength = 40f;

    void Update()
    {
        if (!line || !shootPoint) return;

        Vector3 start = shootPoint.position;
        Vector3 end = start + shootPoint.forward * lineLength;

        line.SetPosition(0, start);
        line.SetPosition(1, end);
    }
}
