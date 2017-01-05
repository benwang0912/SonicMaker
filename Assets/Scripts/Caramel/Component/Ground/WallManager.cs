using UnityEngine;
using System.Collections;

public class WallManager : MonoBehaviour
{
    //in the Wall
    void Awake()
    {
        Transform tf = transform.GetChild(0);

        for(int i = 0; i < XGroundCount; ++i)
        {
            for(int j = 1; j < YGroundCount; ++j)
            {
                Transform newTf = Instantiate(tf);
                newTf.parent = transform;
                newTf.localPosition = i * Vector3.right + j * Vector3.up;
            }
        }

        //to add plane collider
        wallCollider = gameObject.AddComponent<BoxCollider>();
        wallCollider.center = new Vector3(0, YGroundCount / 2.0f - .5f);
        wallCollider.size = new Vector3(XGroundCount, YGroundCount, 3);
    }

    public int XGroundCount, YGroundCount;
    BoxCollider wallCollider;
}
