using UnityEngine;
using System.Collections;

public class Ground : MonoBehaviour
{
    //in the Ground
    
    public int groundcount;

    void Awake ()
    {
        Transform ground = transform.GetChild(0);
        
        for (int i = 1; i <= groundcount; ++i)
        {
            Transform newground = Instantiate(ground);
            newground.parent = transform;
            newground.localPosition = new Vector3(i - groundcount/2, 0f, 0f);
        }
	}
}
