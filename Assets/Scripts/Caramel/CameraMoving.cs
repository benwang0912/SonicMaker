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
        /*
        float temp = transform.localPosition.x - sonic.localPosition.x;
        if (temp > 0.1f || temp < -0.1f)
        {
            transform.localPosition -= temp > 0 ? Vector3.right * speed * Time.deltaTime : Vector3.left * speed * Time.deltaTime;
        }
        */
        
        if (Game.sonicstate == GameConstants.SonicState.JUMPING || Game.sonicstate == GameConstants.SonicState.TOROLL || Game.sonicstate == GameConstants.SonicState.ROLLING || Game.sonicstate == GameConstants.SonicState.CURVEMOTION)
        {
            ToChangePosition.x = rollingball.localPosition.x;
            ToChangePosition.y = originalY + rollingball.localPosition.y;
        }
        else
        {
            ToChangePosition.x = sonic.localPosition.x;
            ToChangePosition.y = originalY + sonic.localPosition.y;
        }
        
        transform.localPosition = ToChangePosition;
    }
    
    public Transform sonic, rollingball;
    Vector3 ToChangePosition;
    float originalY;
}
