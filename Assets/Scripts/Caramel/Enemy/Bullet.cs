using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class Bullet : MonoBehaviour
{ 
    void OnCollisionEnter(Collision collision)
    {
        Transform ct = collision.transform;

        switch (ct.tag)
        {
            case "Sonic":
                ct.GetComponent<Sonic>().GetHurt(collision.relativeVelocity);
                break;
            case "RollingBall":
                ct.GetComponent<Rolling>().ChangeToSonic(GameConstants.SonicState.NORMAL);
                break;
        }
    }
}
