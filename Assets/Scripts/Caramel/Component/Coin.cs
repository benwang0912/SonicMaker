using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Coin : MonoBehaviour
{
    //in the Coin
    public float rotationspeed;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CapsuleCollider>();
    }

    public static void ThrowAllCoins(Vector3 position)
    {
        float f = 1.57f, d = .2f, cos, sin;

        cos = Mathf.Cos(f);
        sin = Mathf.Abs(Mathf.Sin(f));
        Coin coin = ((GameObject)Instantiate(Resources.Load("Caramel/Components/Coin"), position + Vector3.right * cos * throwradius + Vector3.up * sin * throwradius, Quaternion.identity)).GetComponent<Coin>();
        f += d;
        coin.Throw(cos, sin);

        while (Game.coins > 0)
        {
            cos = Mathf.Cos(f);
            sin = Mathf.Abs(Mathf.Sin(f));
            coin = ((GameObject)Instantiate(coin.gameObject, position + Vector3.right * cos * throwradius + Vector3.up * sin * throwradius, Quaternion.identity)).GetComponent<Coin>();
            f += d;
            coin.Throw(cos, sin);
        }
    }

    public void Throw(float cos, float sin)
    {
        cc.isTrigger = true;

        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezePositionZ;
        rb.velocity = new Vector3(cos, sin) * throwmagnification;
        --Game.coins;
        Game.coinslabel.text = Game.coins.ToString();

        Destroy(gameObject, 4f);
    }

    void triggerdelay()
    {
        isdelay = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        switch(collision.transform.tag)
        {
            case "Sonic":
                Destroy(gameObject);
                ++Game.coins;
                Game.coinslabel.text = Game.coins.ToString();

                break;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        Transform ct = collider.transform;

        switch (ct.tag)
        {
            case "Ground":
                if(!isdelay)
                {
                    rb.velocity = new Vector3(rb.velocity.x * .8f, rb.velocity.y * -.8f);

                    isdelay = true;
                    Invoke("triggerdelay", 1f);
                }

                break;

            case "Sonic":
                Destroy(gameObject);
                ++Game.coins;
                Game.coinslabel.text = Game.coins.ToString();

                break;
        }
    }

    void Update()
    {
        //rolling animation
        r = r <= 360f ? r + rotationspeed * Time.deltaTime : r = -360f;
        transform.localRotation = Quaternion.Euler(0f, r, 0f);
    }

    static float throwradius = 5f, throwmagnification = 5f;

    Rigidbody rb;
    CapsuleCollider cc;
    float r = 0f;
    bool isdelay = false;
}
