using UnityEngine;
using System.Collections;

public class PlaneManager : MonoBehaviour
{
    //in the Plane

    public void StartFalling()
    {
        if(!isStartFalling)
        {
            isStartFalling = true;
            StartCoroutine("ChildrenFalling");
        }
    }

    void Awake ()
    {
        Transform ground = transform.GetChild(0);
        Vector3 position = ground.localPosition;

        //normal plane ground
        for (int i = 1; i < groundcount; ++i)
        {
            Transform newground = Instantiate(ground);
            newground.parent = transform;
            newground.localPosition = i * Distance * Vector3.right + position;
        }

        if(temporary)
        {
            for(int i = 0; i < groundcount; ++i)
                transform.GetChild(i).gameObject.AddComponent<TemporaryGround>();
        }
	}

    IEnumerator ChildrenFalling()
    {
        Debug.Log("wait");
        yield return waiting;

        for (int i = 0; i < groundcount; ++i)
        {
            transform.GetChild(i).GetComponent<TemporaryGround>().Falling();
            yield return delay;
        }

        yield break;
    }

    public float Distance = 1f;
    public int groundcount;
    public bool temporary = false, isStartFalling = false;

    WaitForSeconds waiting = new WaitForSeconds(1f), delay = new WaitForSeconds(.3f);
}
