using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Rolling : MonoBehaviour
{
    //in the Rolling Ball

    public float QuickRollingSpeed, SlowRollingSpeed, rollingpower, jumpforce, walkspeed, vibration, springforce;
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

        rb.AddForce((facedirection == 1f ? Vector3.right : Vector3.left) * rollingpower);
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

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.transform.tag)
        {
            case "Ground":
                if (Game.sonicstate == GameConstants.SonicState.JUMPING && collision.contacts[0].point.y > collision.transform.position.y)
                {
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
            if (groundrch.distance < .8f)
            {
                //to calculate the movingdirection
                Vector3 normal = groundrch.normal;

                float y = -(normal.x * facedirection) / normal.y;
                movingdirection = new Vector3(facedirection, y);
                isground = true;
            }
            else
            {
                movingdirection = Vector3.zero;
                isground = false;
            }
        }
        
        //to roll
        rolling.x += Time.deltaTime * rollingspeed;
        transform.localRotation = Quaternion.Euler(rolling);

        //to vibrate
        if (isvibration)
        {
            //be shifting??
            transform.localPosition += Vector3.right * vibration * Mathf.Cos(9.42f * Time.time);
        }

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
                }
                
                break;

            case GameConstants.SonicState.ROLLING:
                //to return to sonic
                if (rb.velocity.magnitude <= 1f)
                {
                    ChangeToSonic(GameConstants.SonicState.NORMAL);
                    return;
                }

                if (Input.GetKey(KeyCode.Return) && isground)
                {
                    JumpRolling(Vector3.up * jumpforce);
                }

                
                //to move in rolling (slow only)
                if (Input.GetAxis("Horizontal") > 0f && facedirection != 1f)
                {
                    //face left => add right force
                    rb.AddForce(Vector3.right * walkspeed);
                }
                else if (Input.GetAxis("Horizontal") < 0f && facedirection == 1f)
                {
                    //face right => add left force
                    rb.AddForce(Vector3.left * walkspeed);
                }

                break;

            case GameConstants.SonicState.JUMPING:
                //seldom use
                if (Input.GetKey(KeyCode.Return) && isground)
                {
                    JumpRolling(Vector3.up * jumpforce);
                }

                //to move in jumping
                rb.AddForce(Vector3.right * Input.GetAxis("Horizontal") * walkspeed);

                break;
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
