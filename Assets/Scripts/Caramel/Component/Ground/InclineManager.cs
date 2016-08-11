using UnityEngine;
using System.Collections;

public class InclineManager : MonoBehaviour
{
    //in the Incline

    public int GroundCount;
    public float Angle;

    void Awake()
    {
        Transform ground = transform.GetChild(0);

        for(int i = 1; i < GroundCount; ++i)
        {
            Transform newground = Instantiate(ground);
            newground.parent = transform;
            newground.localPosition = new Vector3(i, 0f);
        }

        transform.localRotation = Quaternion.Euler(Vector3.forward * Angle);
    }
}
