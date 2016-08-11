using UnityEngine;
using System.Collections;

public class CurveManager : MonoBehaviour
{
    //in the Curve
    
    public float Radius;
    public int GroundCount;

    void Awake()
    {
        Transform ground = transform.GetChild(0);
        float d = (2 * Mathf.PI - 1f) / GroundCount, theta = d;

        ground.localPosition = new Vector3(0f, -Radius);
        ground.localRotation = Quaternion.Euler(Vector3.up * 180f);

        for(int i = 1; i < GroundCount; ++i)
        {
            Transform newground = Instantiate(ground);
            newground.parent = transform;
            newground.localPosition = new Vector3(Radius * Mathf.Cos(theta - Mathf.PI / 2), Radius * Mathf.Sin(theta - Mathf.PI / 2));
            newground.localRotation = Quaternion.Euler(Vector3.up * 180f + Vector3.forward * -(theta * 180 / Mathf.PI));
            theta += d;
        }
    }
}
