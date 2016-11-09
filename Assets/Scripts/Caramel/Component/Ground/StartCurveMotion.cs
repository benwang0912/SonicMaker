using UnityEngine;
using System.Collections;

public class StartCurveMotion : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if(!Cm.CurveStart && collision.relativeVelocity.x > 45f)
        {
            //Curve Motion
            Debug.Log("CurveMotion Go!");
            rollingBall = collision.transform;
            Debug.Log("relative = " + collision.relativeVelocity + " velocity = " + rollingBall.GetComponent<Rigidbody>().velocity);
            OriginalVelocity = rollingBall.GetComponent<Rigidbody>().velocity;
            OriginalPosition = rollingBall.localPosition;
            deltaTime = 0f;
            Game.sonicstate = GameConstants.SonicState.CURVEMOTION;
            start = true;
            Cm.CurveStart = true;
        }
    }

    void Update()
    {
        if(start)
        {
            Debug.Log("YO");
            float temp = 2 * deltaTime * Mathf.PI - .5f * Mathf.PI;
            rollingBall.localPosition = Radius * (Mathf.Cos(temp) * Vector3.right + Mathf.Sin(temp) * Vector3.up) + Distance * deltaTime * Vector3.forward + CurveCenter;

            if ((deltaTime += Time.deltaTime) > 1.1f)
            {
                //ending
                rollingBall.localPosition = OriginalPosition + Distance * Vector3.forward;
                rollingBall.GetComponent<Rigidbody>().velocity = 50f * Vector3.right;
                Debug.Log(rollingBall.GetComponent<Rigidbody>().velocity);
                Game.sonicstate = GameConstants.SonicState.ROLLING;
                start = false;
                Cm.CurveStart = false;
            }
        }
    }

    public CurveManager Cm;
    public Vector3 OriginalVelocity, OriginalPosition, CurveCenter;
    public float Radius, Distance;
    public bool start = false;

    Transform rollingBall;
    float deltaTime;
}
