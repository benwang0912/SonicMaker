using UnityEngine;
using System.Collections;

public class LeaderboardEntry : MonoBehaviour {

    public static LeaderboardEntry Instance;
    public string rankString, usernameString, scoreString;
    public UILabel rank, username, score;

	// Use this for initialization
	void Start () {
        Instance = this;
        rank.text = rankString;
        username.text = usernameString;
        score.text = scoreString;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
