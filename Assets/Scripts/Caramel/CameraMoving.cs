using UnityEngine;
using System.Collections;

public class CameraMoving : MonoBehaviour {

    // in the Main Camera

    Transform sonic, rollingball;
    Vector3 ToChangePosition;

    void Awake()
    {
        sonic = GameObject.Find("Sonic").transform;
        rollingball = GameObject.Find("RollingBall").transform;
        ToChangePosition = transform.localPosition;
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
        if(Game.sonicstate == GameConstants.SonicState.JUMPING || Game.sonicstate == GameConstants.SonicState.TOROLL || Game.sonicstate == GameConstants.SonicState.ROLLING)
        {
            ToChangePosition.x = rollingball.localPosition.x;
        }
        else
        {
            ToChangePosition.x = sonic.localPosition.x;
        }
        
        transform.localPosition = ToChangePosition;
    }
}
