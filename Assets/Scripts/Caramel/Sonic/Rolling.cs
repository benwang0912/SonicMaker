using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Rolling : MonoBehaviour
{
    //in the Rolling Ball

    public float QuickRollingSpeed, SlowRollingSpeed, JumpForce, WalkSpeed, Vibration, SpringForce, AddedGravity;
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
        sonic.transform.localPosition = transform.localPosition - Vector3.up * .3f;

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

        //return sonic.GetComponent<Sonic>();
    }

    public void BackToBall(GameConstants.SonicState s)
    {
        rb.velocity = Game.velocity;

        switch(s)
        {
            case GameConstants.SonicState.JUMPING:
                JumpRolling(Vector3.up * JumpForce);
                break;
            case GameConstants.SonicState.TOROLL:
                SoundManager.instance.PlaySoundEffectSource(GameConstants.SpeedUpSoundEffect);
                QuickRolling((sonic.transform.localRotation.eulerAngles.y - 91f) < 0 ? 1f : -1f);
                break;
            default:
                SlowRolling();
                break;
        }
    }

    public void QuickRolling(float face)
    {
        Game.sonicstate = GameConstants.SonicState.TOROLL;
        material.SetColor("_EmissionColor", quickrollingcolor);
        rollingspeed = QuickRollingSpeed;
        isvibration = true;
        facedirection = face;
    }

    public void SlowRolling()
    {
        Game.sonicstate = GameConstants.SonicState.ROLLING;
        material.SetColor("_EmissionColor", slowrollingcolor);
        rollingspeed = SlowRollingSpeed;
        isvibration = false;

        rb.velocity = RollingSpeed * movingdirection;
    }

    public void JumpRolling(Vector3 v)
    {
        Game.sonicstate = GameConstants.SonicState.JUMPING;
        material.SetColor("_EmissionColor", slowrollingcolor);
        rollingspeed = SlowRollingSpeed;
        isvibration = false;
        transform.localPosition += Vector3.up;
        rb.AddForce(v);
    }

    public void JumpBack(Vector3 relativeVelocity)
    {
        rb.velocity = new Vector3(-Mathf.Sign(relativeVelocity.x) * BackX, BackY);
    }

    void OnCollisionEnter(Collision collision)
    {
        /*
        switch (collision.transform.tag)
        {
            case "Ground":
            case "Spick":
                if (Game.sonicstate == GameConstants.SonicState.JUMPING && collision.contacts[0].point.y > collision.transform.position.y)
                {
                    //jumping end
                    ChangeToSonic(GameConstants.SonicState.NORMAL);
                }
                return;
        }
        */
    }
    
    void Update ()
    {
        //to add gravity
        rb.AddForce(AddedGravity * Vector3.down);

        Debug.Log(Game.sonicstate);

        //falling
        if (transform.localPosition.y < -10.0f)
            ChangeToSonic(GameConstants.SonicState.DEAD);

        //game time setting
        Game.time += Time.deltaTime;
        time.text = ((int)Game.time / 60).ToString() + " : " + ((int)Game.time % 60).ToString();

        //isground
        groundray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(groundray, out groundrch) && Game.sonicstate != GameConstants.SonicState.CURVEMOTION)
        {
            if (groundrch.transform.tag == "Ground" && groundrch.distance < 1f)
            {
                if((rb.velocity.magnitude < 1 && Game.sonicstate != GameConstants.SonicState.TOROLL) || Game.sonicstate == GameConstants.SonicState.JUMPING)
                {
                    Debug.Log("Change to sonic");
                    ChangeToSonic(GameConstants.SonicState.NORMAL);
                    return;
                }

                //to calculate the movingdirection
                Vector3 normal = groundrch.normal;

                float y = -(normal.x * facedirection) / normal.y;
                Debug.Log("y = " + y);
                y *= .3f;
                movingdirection = new Vector3(facedirection, y);
                isground = true;
                if(Game.sonicstate == GameConstants.SonicState.ROLLING && rb.velocity.normalized != movingdirection)
                {
                    rb.velocity = rb.velocity.magnitude * movingdirection;
                }
            }
            else
            {
                movingdirection = Vector3.zero;
                isground = false;
            }
        }

        //to correct the moving direction

        //rb.velocity = movingdirection.magnitude != 0f ? movingdirection / movingdirection.magnitude * rb.velocity.magnitude : rb.velocity;
        
        //to roll
        rolling.x += Time.deltaTime * rollingspeed;
        transform.localRotation = Quaternion.Euler(rolling);

        /*
        //to vibrate
        if (isvibration)
        {
            //be shifting??
            transform.localPosition += Vector3.right * Vibration * Mathf.Cos(9.42f * Time.time);
        }
        */

        switch (Game.sonicstate)
        {
            case GameConstants.SonicState.TOROLL:

                if (Input.GetKeyUp(KeyCode.DownArrow))
                {
                    SlowRolling();
                }
                else if (Input.GetKeyDown(KeyCode.Return))
                {
                    //to keep rolling
                    //music!?
                    SoundManager.instance.PlaySoundEffectSource(GameConstants.SpeedUpSoundEffect);
                }
                
                break;

            case GameConstants.SonicState.ROLLING:
                //to return to sonic
                if (rb.velocity.magnitude <= 1f)
                {
                    Debug.Log(rb.velocity);
                    ChangeToSonic(GameConstants.SonicState.NORMAL);
                    return;
                }

                if (Input.GetKey(KeyCode.Return) && isground)
                {
                    JumpRolling(Vector3.up * JumpForce);
                }

                
                //to move in rolling (slow only)
                if (Input.GetAxis("Horizontal") > 0f && facedirection != 1f)
                {
                    //face left => add right force
                    Debug.Log("???");
                    rb.AddForce(Vector3.right * WalkSpeed);
                }
                else if (Input.GetAxis("Horizontal") < 0f && facedirection == 1f)
                {
                    //face right => add left force
                    Debug.Log("???");
                    rb.AddForce(Vector3.left * WalkSpeed);
                }

                break;

            case GameConstants.SonicState.JUMPING:
                //seldom use
                if (Input.GetKey(KeyCode.Return) && isground)
                {
                    JumpRolling(Vector3.up * JumpForce);
                }

                //to move in jumping
                rb.AddForce(Vector3.right * Input.GetAxis("Horizontal") * WalkSpeed);

                break;

            case GameConstants.SonicState.CURVEMOTION:
                break;
        }
    }

    public float BackX, BackY, RollingSpeed;

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
