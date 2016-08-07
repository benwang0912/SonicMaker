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
        
        if(collision.relativeVelocity.y < -5f)
        {
            switch (cg.tag)
            {
                case "Sonic":
                    if(cg.name == "RollingBall")
                    {
                        cg.GetComponent<Rolling>().ChangeToSonic(GameConstants.SonicState.NORMAL);
                    }

                    Game.sonic.GetComponent<Sonic>().GetHurt(collision.relativeVelocity);
                    break;
            }
        }
    }
}
