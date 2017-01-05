using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Spick : MonoBehaviour
{
    //in the spick

    BoxCollider bc;
    
	void Awake ()
    {
        bc = GetComponent<BoxCollider>();
	}

    void OnCollisionEnter(Collision collision)
    {
        if(!Spicks.isSpick)
        {
            Spicks.isSpick = true;

            GameObject cg = collision.gameObject;

            switch (Direction)
            {
                case Directions.Up:
                    if (collision.contacts[0].point.y > transform.position.y + 3f)
                    {
                        switch (cg.tag)
                        {
                            case "Sonic":
                                cg.GetComponent<Sonic>().GetHurt(collision.relativeVelocity);
                                break;
                            case "RollingBall":
                                cg.GetComponent<Rolling>().ChangeToSonic(GameConstants.SonicState.NORMAL);
                                Game.sonic.GetComponent<Sonic>().GetHurt(collision.relativeVelocity);
                                break;
                        }
                    }
                    else
                    {
                        Game.ComponentNotEffected();
                    }

                    break;

                case Directions.Down:
                    if (collision.contacts[0].point.y < transform.position.y - 3f)
                    {
                        switch (cg.tag)
                        {
                            case "Sonic":
                                cg.GetComponent<Sonic>().GetHurt(collision.relativeVelocity);
                                break;
                            case "RollingBall":
                                cg.GetComponent<Rolling>().ChangeToSonic(GameConstants.SonicState.NORMAL);
                                Game.sonic.GetComponent<Sonic>().GetHurt(collision.relativeVelocity);
                                break;
                        }
                    }
                    else
                    {
                        Game.ComponentNotEffected();
                    }

                    break;

                case Directions.Left:
                    if (collision.contacts[0].point.x < transform.position.x - 3f)
                    {
                        switch (cg.tag)
                        {
                            case "Sonic":
                                cg.GetComponent<Sonic>().GetHurt(collision.relativeVelocity);
                                break;
                            case "RollingBall":
                                cg.GetComponent<Rolling>().ChangeToSonic(GameConstants.SonicState.NORMAL);
                                Game.sonic.GetComponent<Sonic>().GetHurt(collision.relativeVelocity);
                                break;
                        }
                    }
                    else
                    {
                        Game.ComponentNotEffected();
                    }

                    break;

                case Directions.Right:
                    if (collision.contacts[0].point.x > transform.position.x + 3f)
                    {
                        switch (cg.tag)
                        {
                            case "Sonic":
                                cg.GetComponent<Sonic>().GetHurt(collision.relativeVelocity);
                                break;
                            case "RollingBall":
                                cg.GetComponent<Rolling>().ChangeToSonic(GameConstants.SonicState.NORMAL);
                                Game.sonic.GetComponent<Sonic>().GetHurt(collision.relativeVelocity);
                                break;
                        }
                    }
                    else
                    {
                        Game.ComponentNotEffected();
                    }

                    break;
            }

            //isHurt = true;
            Invoke("HurtDelay", Delay);
        }
        //this.collision = collision;
        //Debug.Log(transform.name);
        //StartCoroutine("HurtCoroutine");
        //Debug.Log("back");
        /*
        if (!isHurt)
        {
            this.collision = collision;
            //StartCoroutine("HurtCoroutine");
            collision.transform.localPosition -= 2f * Vector3.right;
        }
        */
    }

    void HurtDelay()
    {
        Spicks.isSpick = false;
    }

    void HurtCoroutine()
    {
        //isHurt = true;
        StopAllCoroutines();
        GameObject cg = collision.gameObject;

        Debug.Log("x = " + collision.contacts[0].point.x + " y = " + collision.contacts[0].point.y);
        Debug.Log(transform.position);

        switch (Direction)
        {
            case Directions.Up:
                if (collision.contacts[0].point.y > transform.position.y + 3f)
                {
                    switch (cg.tag)
                    {
                        case "Sonic":
                            if (cg.name == "RollingBall")
                            {
                                cg.GetComponent<Rolling>().ChangeToSonic(GameConstants.SonicState.NORMAL);
                                Debug.Log("ball");
                            }

                            Game.sonic.GetComponent<Sonic>().GetHurt(collision.relativeVelocity);

                            break;
                    }
                }
                break;

            case Directions.Down:
                if (collision.contacts[0].point.y < transform.position.y - 3f)
                {
                    switch (cg.tag)
                    {
                        case "Sonic":
                            if (cg.name == "RollingBall")
                            {
                                cg.GetComponent<Rolling>().ChangeToSonic(GameConstants.SonicState.NORMAL);
                            }

                            Game.sonic.GetComponent<Sonic>().GetHurt(collision.relativeVelocity);

                            break;
                    }
                }
                break;

            case Directions.Left:
                if (collision.contacts[0].point.x < transform.position.x - 3f)
                {
                    switch (cg.tag)
                    {
                        case "Sonic":
                            if (cg.name == "RollingBall")
                            {
                                cg.GetComponent<Rolling>().ChangeToSonic(GameConstants.SonicState.NORMAL);
                                cg.transform.localPosition -= 10f * Vector3.right;
                                //Debug.Log("ball");
                            }

                            Game.sonic.GetComponent<Sonic>().GetHurt(collision.relativeVelocity);

                            break;
                    }
                }
                break;

            case Directions.Right:
                if (collision.contacts[0].point.x > transform.position.x + 3f)
                {
                    switch (cg.tag)
                    {
                        case "Sonic":
                            if (cg.name == "RollingBall")
                            {
                                cg.GetComponent<Rolling>().ChangeToSonic(GameConstants.SonicState.NORMAL);
                            }

                            Game.sonic.GetComponent<Sonic>().GetHurt(collision.relativeVelocity);

                            break;
                    }
                }
                break;
        }

        //isHurt = true;
        Invoke("HurtDelay", Delay);
    }

    public enum Directions
    {
        Up,
        Down,
        Left,
        Right
    }

    public Spicks Spicks;
    public float Delay;
    public Directions Direction;

    //預防多次撞擊
    Collision collision;
    
}
