using UnityEngine;
using System.Collections;

public class StartCurveMotion : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Start collision.relativeVelocity = " + collision.relativeVelocity);
        if(!Cm.CurveStart && ((isRight = collision.relativeVelocity.x > 45f) || collision.relativeVelocity.x < -45f))
        {
            //Curve Motion
            Debug.Log("CurveMotion Go!");
            rollingBall = collision.transform;
            Debug.Log("relative = " + collision.relativeVelocity + " velocity = " + rollingBall.GetComponent<Rigidbody>().velocity);
            OriginalVelocity = rollingBall.GetComponent<Rigidbody>().velocity;
            OriginalPosition = rollingBall.localPosition;
            deltaTime = 0f;
            Game.sonicstate = GameConstants.SonicState.CURVEMOTION;

            if(isRight)
            {
                center = CurveCenter;
                endingVelocity = EndingVelocity;
            }
            else
            {
                center = CurveCenter + Distance * Vector3.forward;
                endingVelocity = -EndingVelocity;
            }

            start = true;
            Cm.CurveStart = true;
            Debug.Log("isRight = " + isRight);
            Debug.Log("distance = " + Distance);
            Debug.Log("curve center = " + CurveCenter);
        }
    }

    void Update()
    {
        if(start)
        {
            float temp = 2 * deltaTime * Mathf.PI - .5f * Mathf.PI;
            
            rollingBall.localPosition = Radius * (Mathf.Cos(temp) * Vector3.right + Mathf.Sin(temp) * Vector3.up) + Distance * deltaTime * Vector3.forward + center;
            

            if (isRight)
            {
                if ((deltaTime += Time.deltaTime * Speed) > 1.1f)
                {
                    Debug.Log("OUUUUUUUUU");
                    if(Game.rollingball.gameObject.activeSelf)
                    {
                        Debug.Log("INNNNNNNNNNNN");
                        //ending
                        rollingBall.localPosition = OriginalPosition + Distance * Vector3.forward;

                        //rollingBall.GetComponent<Rigidbody>().velocity = 50f * Vector3.right;
                        rollingBall.GetComponent<Rigidbody>().velocity = endingVelocity;
                        Game.velocity = endingVelocity;
                        Debug.Log(rollingBall.GetComponent<Rigidbody>().velocity);

                        Game.sonicstate = GameConstants.SonicState.ROLLING;
                        start = false;
                        Cm.CurveStart = false;
                    }
                    else
                    {
                        Debug.Log("sonic");
                        Game.sonic.localPosition = OriginalPosition + Distance * Vector3.forward;
                        Game.sonic.GetComponent<Rigidbody>().velocity = endingVelocity;
                        Game.sonicstate = GameConstants.SonicState.NORMAL;
                        start = false;
                        Cm.CurveStart = false;
                    }
                }
            }
            else
            {
                if ((deltaTime -= Time.deltaTime * Speed) < -1.1f)
                {
                    Debug.Log("OUUUUUUUUU");
                    if (Game.rollingball.gameObject.activeSelf)
                    {
                        Debug.Log("INNNNNNNNNNNN");
                        //ending
                        rollingBall.localPosition = OriginalPosition - Distance * Vector3.forward;

                        //rollingBall.GetComponent<Rigidbody>().velocity = 50f * Vector3.right;
                        rollingBall.GetComponent<Rigidbody>().velocity = endingVelocity;
                        Game.velocity = endingVelocity;
                        Debug.Log(rollingBall.GetComponent<Rigidbody>().velocity);
                        Game.sonicstate = GameConstants.SonicState.ROLLING;
                        start = false;
                        Cm.CurveStart = false;
                    }
                    else
                    {
                        Debug.Log("sonic");
                        Game.sonic.localPosition = OriginalPosition - Distance * Vector3.forward;
                        Game.sonic.GetComponent<Rigidbody>().velocity = endingVelocity;
                        Game.sonicstate = GameConstants.SonicState.NORMAL;
                        start = false;
                        Cm.CurveStart = false;
                    }
                }
            }

        }
    }

    public static Vector3 EndingVelocity = 50 * Vector3.right;
    public static float Speed = 1.2f;

    public CurveManager Cm;
    public Vector3 OriginalVelocity, OriginalPosition, CurveCenter;
    public float Radius, Distance;
    public bool start = false, isRight = false;

    Transform rollingBall;
    Vector3 center, endingVelocity;
    float deltaTime;
}
