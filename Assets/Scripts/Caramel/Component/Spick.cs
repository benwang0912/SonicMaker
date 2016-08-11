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
        GameObject cg = collision.gameObject;

        Debug.Log(collision.contacts[0].point.y);
        Debug.Log(transform.position.y);

        if (collision.contacts[0].point.y > transform.position.y + 3f)
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
    }
}
