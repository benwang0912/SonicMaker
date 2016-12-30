using UnityEngine;
using System.Collections;

public class MiniMapCamScript : MonoBehaviour {

    private Transform player;
    private Camera minimapCam;
    private float bondaryX1, bondaryX2;
    private float bondaryY1, bondaryY2;
    // Use this for initialization
    void Start () {
        player = GameObject.Find("Sonic").transform;
        minimapCam = transform.GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        if (minimapCam.orthographicSize >= 65)
        {
            transform.position = new Vector3(-2.4f, 45.1f, -10f);
        }
        else {
            transform.position = followPlayer();
        }
	}

    private Vector3 followPlayer() {
        Vector3 position;

        position = new Vector3(Mathf.Clamp(player.transform.position.x, bondaryX1, bondaryX2),
                               Mathf.Clamp(player.transform.position.y, bondaryY1, bondaryY2),
                               -10
            );

        return  position;
    }

    public void scaleDownCam() {
        if(minimapCam.orthographicSize > 15)
            minimapCam.orthographicSize -= 12;
        setBondary();
    }
    public void scaleUpCam()
    {
        if (minimapCam.orthographicSize < 66 )
            minimapCam.orthographicSize += 12;
        setBondary();
    }

    private void setBondary() {
        float height = 2f * minimapCam.orthographicSize;
        float width = height * minimapCam.aspect;

        Vector3 topRight = minimapCam.ViewportToWorldPoint(new Vector3(1,1,minimapCam.nearClipPlane));
        
        bondaryX1 = -67 + (topRight.x - transform.position.x);
        bondaryX2 = 65 - (topRight.x - transform.position.x);
        bondaryY1 = -5 + (topRight.y - transform.position.y);
        bondaryY2 = 95 - (topRight.y - transform.position.y);
        
    }
}
