using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class BossDetection : MonoBehaviour
{
    void Awake()
    {
        bc = GetComponent<BoxCollider>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Sonic")
        {
            Debug.Log("Boss Fight!!!");
            Boss.StartBossFight();
            Destroy(gameObject);
        }
    }

    public BossAI Boss;

    BoxCollider bc;
}
