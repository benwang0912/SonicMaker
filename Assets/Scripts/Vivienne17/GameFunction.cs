using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameFunction : MonoBehaviour {

    public Text ScoreText;
    public int Score = 0;
    public static GameFunction Instance;

    public string newUsername;

    public float time = 0;
    public float time2 = 0;

    public GameObject GameOver;
    public GameObject Count_Down;
    public GameObject StartImage;
    public GameObject HelpButton;
    public GameObject StopButton;
    public GameObject PlayButton;
    public GameObject Pause_Image;
    public GameObject Restart_Button;
    public GameObject HelpButton2;
    public GameObject CloseButton2;
    public GameObject Help_image;
    public GameObject CloseButton;
    public GameObject Restart_Button2;
    public GameObject InputName;
    public GameObject LeaderBoardEntry1;
    public GameObject LeaderBoardEntry2;
    public GameObject LeaderBoardEntry3;
    public GameObject Enter;
    public Text NewName;

    public bool isPlaying = false;
    public bool startPlay = false;

    public bool firstTime = true;

    // Use this for initialization
    void Start () {
        Instance = this;

        GameOver.SetActive(false);
        Count_Down.SetActive(false);
        StopButton.SetActive(false);
        Help_image.SetActive(false);
        Pause_Image.SetActive(false);
        Restart_Button.SetActive(false);
        HelpButton2.SetActive(false);
        CloseButton2.SetActive(false);
        CloseButton.SetActive(false);
        Restart_Button2.SetActive(false);
        HelpButton.SetActive(true);
        PlayButton.SetActive(true);
        StartImage.SetActive(true);
        LeaderBoardEntry1.SetActive(true);
        LeaderBoardEntry2.SetActive(true);
        LeaderBoardEntry3.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
        if (startPlay == true)
        {
            time += Time.deltaTime;
            if (time >= 4.0f && firstTime == true)
            {
                time2 += Time.deltaTime;
                isPlaying = true;
                firstTime = false;
                if (SonicMove.Instance.Died == false)
                {
                    if (time2 > 0.5f && time2 < 1f)
                    {
                        AddScore(5);
                        time2 = 0;
                    }
                }
                else {
                    GameOver.SetActive(true);
                }
            }
            if (firstTime == false && SonicMove.Instance.Died == false)
            {
                time2 += Time.deltaTime;
                if (time2 > 0.5f && time2 < 1f)
                {
                    AddScore(5);
                    time2 = 0;
                }
            }
        }
        
	}

    public void AddScore(int num) {
        Score = Score + num;
        ScoreText.text = Score.ToString();
    }

    public void StartGame() {
        startPlay = true;

        Count_Down.SetActive(true);
        StopButton.SetActive(true);
        HelpButton.SetActive(false);
        PlayButton.SetActive(false);
        StartImage.SetActive(false);
        LeaderBoardEntry1.SetActive(false);
        LeaderBoardEntry2.SetActive(false);
        LeaderBoardEntry3.SetActive(false);

    }

    public void TutorialOn()
    {
        Help_image.SetActive(true);
        CloseButton.SetActive(true);
        /*   PlayButton.SetActive(false);
           HelpButton.SetActive(false);
           Restart_Button.SetActive(false);
           StopButton.SetActive(false);
           CloseButton2.SetActive(false);
           HelpButton2.SetActive(false);
           Pause_Image.SetActive(false);*/
        StopButton.SetActive(false);
    }

    public void TutorialOff()
    {
        Help_image.SetActive(false);
        CloseButton.SetActive(false);
      /*  PlayButton.SetActive(true);
        HelpButton.SetActive(true);*/
    }

    public void TutorialOff2()
    {
        Help_image.SetActive(false);
        CloseButton.SetActive(false);
        Restart_Button.SetActive(true);
        StopButton.SetActive(true);
        CloseButton2.SetActive(true);
        HelpButton2.SetActive(true);
        Pause_Image.SetActive(true);
    }

    public void PauseGame()
    {
        isPlaying = false;

        StopButton.SetActive(false);
        Restart_Button.SetActive(true);
        CloseButton2.SetActive(true);
        HelpButton2.SetActive(true);
        Pause_Image.SetActive(true);
    }

    public void BackToGame() {
        StopButton.SetActive(true);
        Restart_Button.SetActive(false);
        CloseButton2.SetActive(false);
        HelpButton2.SetActive(false);
        Pause_Image.SetActive(false);
        isPlaying = true;
    }

    public void OverGame()
    {
        GameOver.SetActive(true);
        Restart_Button2.SetActive(true);
        InputName.SetActive(true);
        Enter.SetActive(true);
        LeaderBoard.Instance.sortTheBoard();
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void EnterTheName()
    {
        newUsername = NewName.text;
        LeaderBoard.Instance.StoreTheName();
    } 
}
