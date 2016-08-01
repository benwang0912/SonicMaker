using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameFunction : MonoBehaviour {

    public Text ScoreText;
    public int Score = 0;
    public static GameFunction Instance;

    public float time = 0;
    private float time2 = 0;

    public GameObject GameStart;
    public GameObject Count_Down;
    public GameObject GameOver;
    public GameObject HelpButton;
    public GameObject PlayButton;
    public GameObject StartImage;
    public bool isPlaying = false;
    public bool startPlay = false;

    // Use this for initialization
    void Start () {
        Instance = this;

        GameOver.SetActive(false);
        Count_Down.SetActive(false);
        GameStart.SetActive(false);
        HelpButton.SetActive(false);
        PlayButton.SetActive(true);
        StartImage.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
        if (startPlay == true)
        {
            time += Time.deltaTime;
            if (time >= 4.0f)
            {
                time2 += Time.deltaTime;
                isPlaying = true;
                if (SonicMove.Instance.Died == false)
                {
                    if (time2 > 0.5f && time2 < 1f)
                    {
                        Score += 5;
                        ScoreText.text = Score.ToString();
                        time2 = 0;
                    }
                }
                else {
                    GameOver.SetActive(true);
                }
            }
        }
        
	}

    public void AddScore() {
        Score += 20;
        ScoreText.text = Score.ToString();
    }

    public void StartGame() {
        startPlay = true;

        Count_Down.SetActive(true);
        HelpButton.SetActive(true);
        PlayButton.SetActive(false);
        StartImage.SetActive(false);
  //      SonicMove.Instance.anim.SetTrigger("isPlaying");
    }

    public void OverGame()
    {
        GameOver.SetActive(false);
    }
}
