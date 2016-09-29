using UnityEngine;

public class Spicks : MonoBehaviour
{
    //in the Spicks

    void Awake()
    {
        Transform spick = transform.GetChild(0);
        Spick s = spick.GetComponent<Spick>();
        s.Spicks = this;

        for(int i = 1; i < spickcount; ++i)
        {
            Transform newspick = Instantiate(spick);
            newspick.parent = transform;
            newspick.localPosition = new Vector3(i, 0f);
            newspick.localRotation = spick.localRotation;
            newspick.GetComponent<Spick>().Direction = s.Direction;
            newspick.name = i.ToString();
        }
    }


    public int spickcount;
    public bool isSpick = false;
}
