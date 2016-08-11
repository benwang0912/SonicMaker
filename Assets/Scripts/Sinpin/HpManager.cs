using UnityEngine;
using System.Collections;

public class HpManager : MonoBehaviour {
    private GameObject heart, emptyHeart;
    private playerStats script;
    private float preHealth = 0;
    private UIGrid grid;
    private int emptyHeartCount = 0;
	// Use this for initialization
	void Start () {
        script = GameObject.Find("Sonic").GetComponentInChildren<playerStats>();
        heart = Resources.Load("sinpin/heart", typeof(GameObject)) as GameObject;
        emptyHeart = Resources.Load("sinpin/heart_empty", typeof(GameObject)) as GameObject;
        grid = NGUITools.FindInParents<UIGrid>(this.gameObject);

        resetHeart((int)script.maxHealth);
        script.Health = script.maxHealth;
        preHealth = script.maxHealth;
        grid.Reposition();

    }
	
	// Update is called once per frame
	void Update () {
        if (preHealth != script.Health)
        {
            float count = script.Health - preHealth;
            if ((int)count > 0)
            {
                drawHeart(count);
                preHealth = script.Health;
                grid.Reposition();
            }
            else {
                takeDamage(count*-1);
                preHealth = script.Health;
                grid.Reposition();
            }
        }
        if (transform.childCount > script.maxHealth)
        {
            while (transform.childCount > script.maxHealth)
            {
                NGUITools.Destroy(transform.GetChild(transform.childCount - 1));
            }
            grid.Reposition();
        }
	}
    void drawHeart(float count)
    {
        for(int i=0; i < count; i++) {
            if (emptyHeartCount > 0)
            {
                NGUITools.Destroy(GameObject.Find("heart_empty(Clone)"));
                emptyHeartCount -= 1;
            }
        }
        for (int i = 0; i < count; i++)
        {
            gameObject.AddChild(heart).transform.SetAsFirstSibling();
        }
    }
    void takeDamage(float count)
    {
        for (int i = 0; i < count; i++)
        {
            if (transform.childCount > 0)
            {
                NGUITools.Destroy(transform.GetChild(transform.childCount - 1 - emptyHeartCount).gameObject);
                gameObject.AddChild(emptyHeart);
                emptyHeartCount += 1;
            }
        }
    }
    void resetHeart(int count)
    {
        foreach(Transform child in grid.GetChildList())
        {
                NGUITools.Destroy(child);
        }
        emptyHeartCount = 0;
        for (int i = 0; i < count; i++)
        {
            gameObject.AddChild(heart);
        }
        
    }

}
