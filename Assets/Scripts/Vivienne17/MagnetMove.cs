using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MagnetMove : MonoBehaviour {

    public Image clear_C, clear_L, clear_E, clear_A, clear_R;

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "CLEAR_C")
        {
            Destroy(collision.gameObject);
            Color temp = clear_C.color;
            temp.a = 1f;
            clear_C.color = temp;
        }
        else if (collision.gameObject.name == "CLEAR_L")
        {
            Destroy(collision.gameObject);
            Color temp = clear_L.color;
            temp.a = 1f;
            clear_L.color = temp;
        }
        else if (collision.gameObject.name == "CLEAR_E")
        {
            Destroy(collision.gameObject);
            Color temp = clear_E.color;
            temp.a = 1f;
            clear_E.color = temp;
        }
        else if (collision.gameObject.name == "CLEAR_A")
        {
            Destroy(collision.gameObject);
            Color temp = clear_A.color;
            temp.a = 1f;
            clear_A.color = temp;
        }
        else if (collision.gameObject.name == "CLEAR_R")
        {
            Destroy(collision.gameObject);
            Color temp = clear_R.color;
            temp.a = 1f;
            clear_R.color = temp;
        }
    }
}
