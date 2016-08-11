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
        float d = 2 * Mathf.PI / GroundCount, theta = d;

        ground.localPosition = new Vector3(Radius, 0f);
        ground.localRotation = Quaternion.Euler(Vector3.up * 180f + Vector3.forward * -90f);

        for(int i = 1; i < GroundCount; ++i)
        {
            Transform newground = Instantiate(ground);
            newground.parent = transform;
            newground.localPosition = new Vector3(Radius * Mathf.Cos(theta), Radius * Mathf.Sin(theta));
            newground.localRotation = Quaternion.Euler(Vector3.up * 180f + Vector3.forward * (-90f - (theta * 180 / Mathf.PI)));
            theta += d;
        }
    }
}
