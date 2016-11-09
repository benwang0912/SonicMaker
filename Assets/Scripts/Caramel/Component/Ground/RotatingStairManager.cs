using UnityEngine;
using System.Collections;

public class RotatingStairManager : MonoBehaviour
{
    void Awake()
    {
        Center = transform.localPosition;
        delta = (2 * Mathf.PI) / Period;
        theta = 0f;
        count = StairList.childCount;
        distancePI = (2 * Mathf.PI) / count;
        halfRadius = Radius * .5f;

        for(int i = 0; i < count; ++i)
        {
            StairList.GetChild(i).GetComponent<Stair>().id = i;
        }
    }

    public void OnStair(int id)
    {
        IsOnStair = true;
        OnWhichStair = id;
        SonicOriginal = Sonic.localPosition;
    }

    void Update()
    {
        for(int i = 0; i < count; ++i)
        {
            StairList.GetChild(i).localPosition = Radius * (Mathf.Cos(theta + i * distancePI) * Vector3.right + Mathf.Sin(theta + i * distancePI) * Vector3.up);
            PillarList.GetChild(i).localPosition = halfRadius * (Mathf.Cos(theta + i * distancePI) * Vector3.right + Mathf.Sin(theta + i * distancePI) * Vector3.up);
            PillarList.GetChild(i).localRotation = Quaternion.Euler((i * distancePI + theta) * 180f / Mathf.PI * Vector3.right + -90f * Vector3.up + -90f * Vector3.forward);
        }

        if(IsOnStair)
        {
            if(Game.sonicstate != GameConstants.SonicState.JUMPING)
            {
                Sonic.localPosition = Center + Radius * (Mathf.Cos(theta + OnWhichStair * distancePI) * Vector3.right + Mathf.Sin(theta + OnWhichStair * distancePI) * Vector3.up);
            }
            else
            {
                IsOnStair = false;
            }
        }

        theta += delta * Time.deltaTime;
    }

    public Vector3 SonicOriginal, Center;
    public Transform StairList, PillarList, Sonic;
    public float Period, Radius;
    public int OnWhichStair;
    public bool IsOnStair = false;

    float delta, theta, halfRadius, distancePI;
    int count;
}
