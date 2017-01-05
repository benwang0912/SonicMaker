using UnityEngine;
using System.Collections;

public class InclineManager : MonoBehaviour
{
    //in the Incline

    void Awake()
    {
        Transform ground = transform.GetChild(0);

        for(int i = 1; i < GroundCount; ++i)
        {
            Transform newground = Instantiate(ground);
            newground.parent = transform;
            newground.localPosition = i * Distance * Vector3.right;
        }

        if (isBoxCollider)
        {
            //to add incline collider
            inclineCollider = gameObject.AddComponent<BoxCollider>();
            inclineCollider.center = new Vector3(GroundCount / 2.0f - .5f, 0);
            inclineCollider.size = new Vector3(GroundCount, 1, 3);
        }

        transform.localRotation = Quaternion.Euler(Vector3.forward * Angle);
    }
    
    public int GroundCount;
    public float Distance = 1f;
    public float Angle;
    public bool isBoxCollider = true;

    BoxCollider inclineCollider;
}
