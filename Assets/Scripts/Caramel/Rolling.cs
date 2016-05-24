using UnityEngine;
using System.Collections;

public class Rolling : MonoBehaviour {

    Vector3 rolling = new Vector3(1f, 90f, 0f);
    Material material;
    float speed, vibration = .05f;
    Color slowrolling = new Color(0.102f, 0.102f, 0.102f, 1f), quickrolling = new Color(0.75f, 0.75f, 0.75f, 1f);
    Vector3 original_position;
    bool isvibration = false;

	// Use this for initialization
	void Start ()
    {
        material = (Material)Resources.Load("Caramel/Materials/Material.001");
    }

    public void QuickRolling(bool b)
    {
        material.SetColor("_EmissionColor", b ? quickrolling : slowrolling);
        speed = b ? 1500f : 900f;
        original_position = transform.localPosition;
        isvibration = b;
    }
    /*
    public void Vibration(bool b)
    {
        original_position = transform.localPosition;
        isvibration = b;
    }
	*/
	// Update is called once per frame
	void Update ()
    {
        rolling += new Vector3(Time.deltaTime*speed, 0f, 0f);
        transform.localRotation = Quaternion.Euler(rolling);

        if(isvibration)
        {
            transform.localPosition += Vector3.left * vibration * Mathf.Cos(6.28f * Time.time);
        }
    }
}
