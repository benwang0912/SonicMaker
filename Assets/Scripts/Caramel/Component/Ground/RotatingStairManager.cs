using UnityEngine;
using System.Collections;

public class RotatingStairManager : MonoBehaviour
{
    void Awake()
    {
        delta = (2 * Mathf.PI) / Period;
        theta = 0f;
        count = StairList.childCount;
        distancePI = (2 * Mathf.PI) / count;
        halfRadius = Radius * .5f;
    }

    void Update()
    {
        for(int i = 0; i < count; ++i)
        {
            StairList.GetChild(i).localPosition = Radius * (Mathf.Cos(theta + i * distancePI) * Vector3.right + Mathf.Sin(theta + i * distancePI) * Vector3.up);
            PillarList.GetChild(i).localPosition = halfRadius * (Mathf.Cos(theta + i * distancePI) * Vector3.right + Mathf.Sin(theta + i * distancePI) * Vector3.up);
            PillarList.GetChild(i).localRotation = Quaternion.Euler(180f * (Mathf.Cos(i * distancePI + Mathf.PI * .5f)) / Mathf.PI * Vector3.right + -90f * Vector3.up + -90f * Vector3.forward);
        }

        theta += delta * Time.deltaTime;
    }

    public Transform StairList, PillarList;
    public float Period, Radius;

    float delta, theta, halfRadius, distancePI;
    int count;
}
