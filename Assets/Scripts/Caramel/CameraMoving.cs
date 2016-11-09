using UnityEngine;
using System.Collections;

public class CameraMoving : MonoBehaviour
{
    // in the Main Camera

    void Awake()
    {
        ToChangePosition = transform.localPosition;
        originalY = ToChangePosition.y - .8f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            if(Game.sonic.gameObject.activeSelf)
            {
                ToChangePosition.x = sonic.localPosition.x;
                ToChangePosition.y = originalY + sonic.localPosition.y;
            }
            else
            {
                ToChangePosition.x = rollingball.localPosition.x;
                ToChangePosition.y = originalY + rollingball.localPosition.y;
            }

            transform.localPosition = ToChangePosition;
        }
    }
    
    public Transform sonic, rollingball;
    public bool isMoving = true;
    Vector3 ToChangePosition;
    float originalY;
}
