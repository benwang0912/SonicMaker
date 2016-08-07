using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Rolling : MonoBehaviour
{
    //in the Rolling Ball

    public float quickrollingspeed, slowrollingspeed, rollingpower, jumpforce, walkspeed, vibration, springforce;
    public UILabel time, coins;
    public GameObject sonic;
    
	void Awake ()
    {
        material = (Material)Resources.Load("Caramel/Materials/Material.001");
        rb = GetComponent<Rigidbody>();
    }

    public void ChangeToSonic(GameConstants.SonicState s)
    {
        sonic.SetActive(true);
        gameObject.SetActive(false);

        Game.velocity = rb.velocity;
        rb.velocity = Vector3.zero;
        sonic.transform.localPosition = transform.localPosition - Vector3.up * .2f;

        switch (s)
        {
            case GameConstants.SonicState.NORMAL:
                Game.sonicstate = GameConstants.SonicState.NORMAL;
                sonic.SendMessage("BackToSonic", GameConstants.SonicState.NORMAL);
                break;
            case GameConstants.SonicState.DEAD:
                Game.sonicstate = GameConstants.SonicState.DEAD;
                sonic.SendMessage("BackToSonic", GameConstants.SonicState.DEAD);
                break;
        }
    }

    public void BackToBall(GameConstants.SonicState s)
    {
        rb.velocity = Game.velocity;

        switch(s)
        {
            case GameConstants.SonicState.JUMPING:
                JumpRolling(Vector3.up * jumpforce);
                break;
            case GameConstants.SonicState.TOROLL:
                QuickRolling((sonic.transform.localRotation.eulerAngles.y - 91f) < 0 ? 1f : -1f);
                break;
        }
    }

    public void QuickRolling(float face)
    {
        Game.sonicstate = GameConstants.SonicState.TOROLL;
        material.SetColor("_EmissionColor", quickrollingcolor);
        rollingspeed = quickrollingspeed;
        isvibration = true;
        facedirection = face;
    }

    public void SlowRolling()
    {
        Game.sonicstate = GameConstants.SonicState.ROLLING;
        material.SetColor("_EmissionColor", slowrollingcolor);
        rollingspeed = slowrollingspeed;
        isvibration = false;

        rb.AddForce((facedirection == 1f ? Vector3.right : Vector3.left) * rollingpower);
    }

    public void JumpRolling(Vector3 v)
    {
        Game.sonicstate = GameConstants.SonicState.JUMPING;
        material.SetColor("_EmissionColor", slowrollingcolor);
        rollingspeed = slowrollingspeed;
        isvibration = false;
        rb.AddForce(v);
    }

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.transform.tag)
        {
            case "Spring_Y":
                    JumpRolling( Vector3.up * springforce * Mathf.Sign(collision.relativeVelocity.y));
                return;

            case "Spring_X":
                rb.AddForce(Vector3.right * springforce * Mathf.Sign(collision.relativeVelocity.x));
                return;
                
            case "Coin":
                Destroy(collision.gameObject);
                Game.coins += 1;
                coins.text = Game.coins.ToString();
                return;

            case "Enemy":
                Destroy(collision.gameObject);
                return;

            case "Ground":
                if (Game.sonicstate == GameConstants.SonicState.JUMPING)
                {
                    Debug.Log("YO");
                    //jumping end
                    ChangeToSonic(GameConstants.SonicState.NORMAL);
                }
                return;
        }
    }
    
    void Update ()
    {
        //falling
        if (transform.localPosition.y < -10.0f)
            ChangeToSonic(GameConstants.SonicState.DEAD);

        //game time setting
        Game.time += Time.deltaTime;
        time.text = ((int)Game.time / 60).ToString() + " : " + ((int)Game.time % 60).ToString();

        //isground
        groundray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(groundray, out groundrch))
        {
            if (groundrch.transform.tag == "Ground" && groundrch.distance < 1f)
            {
                //to calculate the movingdirection
                Vector3 normal = groundrch.normal;

                float y = -(normal.x * facedirection) / normal.y;
                movingdirection = new Vector3(facedirection, y);

                isground = true;
            }
            else
            {
                isground = false;
            }
        }

        //to return to sonic
        if (rb.velocity.magnitude <= 1f && Game.sonicstate == GameConstants.SonicState.ROLLING)
        {
            gameObject.SetActive(false);
            Game.sonicstate = GameConstants.SonicState.NORMAL;
            sonic.transform.localPosition = transform.localPosition;
            sonic.SetActive(true);
        }

        //to roll
        rolling.x += Time.deltaTime * rollingspeed;
        transform.localRotation = Quaternion.Euler(rolling);

        //to vibrate
        if(isvibration)
        {
            //be shifting??
            transform.localPosition += Vector3.right * vibration * Mathf.Cos(9.42f * Time.time);
        }

        //to keep rolling
        if (Input.GetKeyDown(KeyCode.Return) && Game.sonicstate == GameConstants.SonicState.TOROLL)
        {
            //music!?
        }

        //to change into rolling state
        if (Input.GetKeyUp(KeyCode.DownArrow) && Game.sonicstate == GameConstants.SonicState.TOROLL)
        {
            SlowRolling();
        }

        //to jump in rolling state
        if(Input.GetKey(KeyCode.Return) && (Game.sonicstate == GameConstants.SonicState.ROLLING || (Game.sonicstate == GameConstants.SonicState.JUMPING && isground)))
        {
            JumpRolling(Vector3.up * jumpforce);
        }

        if((Input.GetAxis("Horizontal") != 0 && Game.sonicstate == GameConstants.SonicState.JUMPING))
        {
            rb.AddForce(Vector3.right * Input.GetAxis("Horizontal") * walkspeed);
        }

        //to move in rolling state (slow only)
        if (Input.GetAxis("Horizontal") > 0f && facedirection != 1f && Game.sonicstate == GameConstants.SonicState.ROLLING)
        {
            //to turn right
            rb.AddForce(Vector3.right * walkspeed);
        }

        if (Input.GetAxis("Horizontal") < 0f && facedirection == 1f && Game.sonicstate == GameConstants.SonicState.ROLLING)
        {
            //to turn left
            rb.AddForce(Vector3.left * walkspeed);
        }
    }

    Vector3 rolling = new Vector3(0f, 90f, 0f), movingdirection;
    Material material;
    Rigidbody rb;
    Color slowrollingcolor = new Color(0.102f, 0.102f, 0.102f, 1f), quickrollingcolor = new Color(0.75f, 0.75f, 0.75f, 1f);
    Ray groundray;
    RaycastHit groundrch;
    bool isvibration = false, isground = false;
    float rollingspeed, facedirection;

    enum SonicMode
    {
        DEAD,
        NORMAL
    }
}
