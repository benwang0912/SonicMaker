using UnityEngine;
using System.Collections;

public class CurveCoinManager : MonoBehaviour
{
    void Awake()
    {
        Transform tf = transform.GetChild(0);
        float d = (2 * Mathf.PI) / Count, theta = d;
        float dz = Distance / Count, z = dz;
        float forward = IsForward ? 1f : -1f;

        tf.localPosition = -Radius * Vector3.up;

        for (int i = 0; i < Count; ++i)
        {
            Transform newTf = Instantiate(tf);
            newTf.parent = transform;
            float temp = theta - .5f * Mathf.PI;
            newTf.localPosition = Radius * (Mathf.Cos(temp) * Vector3.right + Mathf.Sin(temp) * Vector3.up) + forward * z * Vector3.forward;
            theta += d;
            z += dz;
        }
    }

    public float Radius, Distance;
    public int Count;
    public bool IsForward;
}
