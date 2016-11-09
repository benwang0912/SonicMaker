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
    }

    public int XGroundCount, YGroundCount;
}
