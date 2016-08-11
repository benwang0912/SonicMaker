using UnityEngine;

public class Spicks : MonoBehaviour
{
    //in the Spicks

    public int spickcount;

    void Awake()
    {
        Transform spick = transform.GetChild(0);

        for(int i = 1; i < spickcount; ++i)
        {
            Transform newspick = Instantiate(spick);
            newspick.parent = transform;
            newspick.localPosition = new Vector3(i, 0f);
        }
    }

}
