using UnityEngine;
using System.IO;
using System.Text;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class LeaderBoard : MonoBehaviour {

    public static LeaderBoard Instance;

    public string inputName;
    public Text Rank1, Username1, Score1;
    public Text Rank2, Username2, Score2;
    public Text Rank3, Username3, Score3;

    private string[] rankString = { "0", "0", "0"};
    private string[] usernameString = { "0", "0", "0" };
    private string[] scoreString = { "0", "0", "0" };

    private int count = 0;
    private int ranking = 0;

    // Use this for initialization
    void Start () {
        Instance = this;

        //open the file and save them into arrays
        string line;
        StreamReader theReader = new StreamReader(Application.dataPath + "/Scripts/Vivienne17/rankfile.txt", Encoding.Default);

        using (theReader)
        {
            do
            {
                line = theReader.ReadLine();

                if (line != null)
                {
                    string[] entries = line.Split(' ');
                    if (entries.Length > 0)
                    {
                        rankString[count] = entries[0];
                        usernameString[count] = entries[1];
                        scoreString[count] = entries[2];
                        count = count + 1;
                    }    
                }
            }while (line != null);
            // Done reading, close the reader and return true to broadcast success    
            theReader.Close();
        }
        if (rankString[0] == "1") {
            Rank1.text = "1";
            Username1.text = usernameString[0];
            Score1.text = scoreString[0];
            count = 1;
            if (rankString[1] == "2")
            {
                Rank2.text = "2";
                Username2.text = usernameString[1];
                Score2.text = scoreString[1];
                count = 2;
                if (rankString[2] == "3")
                {
                    Rank3.text = "3";
                    Username3.text = usernameString[2];
                    Score3.text = scoreString[2];
                    count = 3;

                }
            }
        }
    }

    public void sortTheBoard() {

        int newScore;
        newScore = Convert.ToInt32(GameFunction.Instance.ScoreText.text);
        int compareScore;
        //to check if the new score is better
        rankString[0] = "1";

        if (count == 0)
        {
            rankString[0] = "1";
            scoreString[0] = String.Copy(GameFunction.Instance.ScoreText.text);
            ranking = 1;
        }
        else if (count == 1)
        {
            compareScore = Convert.ToInt32(scoreString[0]);
            rankString[1] = "2";
            if (compareScore < newScore)
            {
                usernameString[1] = String.Copy(usernameString[0]);
                scoreString[1] = String.Copy(scoreString[0]);
                ranking = 1;
            }
            else {
                ranking = 2;
            }
            scoreString[ranking-1] = String.Copy(GameFunction.Instance.ScoreText.text);
        }
        else if (count == 2)
        {
            compareScore = Convert.ToInt32(scoreString[1]);
            rankString[2] = "3";
            if (newScore > compareScore)//1 2
            {
                compareScore = Convert.ToInt32(scoreString[0]);
                if (newScore > compareScore)//1
                {
                    usernameString[2] = String.Copy(usernameString[1]);
                    scoreString[2] = String.Copy(scoreString[1]);
                    usernameString[1] = String.Copy(usernameString[0]);
                    scoreString[1] = String.Copy(scoreString[0]);
                    ranking = 1;
                }
                else//2
                {
                    usernameString[2] = String.Copy(usernameString[1]);
                    scoreString[2] = String.Copy(scoreString[1]);
                    ranking = 2;
                }
            }
            else//3
            {
                ranking = 3;
            }
            scoreString[ranking - 1] = String.Copy(GameFunction.Instance.ScoreText.text);
        }
        else//check if it is the new record
        {
            compareScore = Convert.ToInt32(scoreString[2]);
            if (newScore > compareScore)//1 2 3
            {
                compareScore = Convert.ToInt32(scoreString[1]);
                if (newScore > compareScore)//1 2
                {
                    compareScore = Convert.ToInt32(scoreString[0]);
                    if (newScore > compareScore)//1
                    {
                        usernameString[2] = String.Copy(usernameString[1]);
                        scoreString[2] = String.Copy(scoreString[1]);
                        usernameString[1] = String.Copy(usernameString[0]);
                        scoreString[1] = String.Copy(scoreString[0]);
                        ranking = 1;
                    }
                    else//2
                    {
                        usernameString[2] = String.Copy(usernameString[1]);
                        scoreString[2] = String.Copy(scoreString[1]);
                        ranking = 2;
                    }

                }
                else//3
                {
                    ranking = 3;
                }
                scoreString[ranking - 1] = String.Copy(GameFunction.Instance.ScoreText.text);
            }
            else//no ranking
            {
            }
            
        } 
    }

    void StoreToTheFile()
    {
        FileStream fs = new FileStream(Application.dataPath + "/Scripts/Vivienne17/rankfile.txt", FileMode.Create);
        StreamWriter theWriter = new StreamWriter(fs);

        theWriter.WriteLine(rankString[0] + " " + usernameString[0] + " " + scoreString[0]);
        theWriter.WriteLine(rankString[1] + " " + usernameString[1] + " " + scoreString[1]);
        theWriter.WriteLine(rankString[2] + " " + usernameString[2] + " " + scoreString[2]);
        theWriter.Close();
    }

    public void StoreTheName()
    {
        usernameString[ranking - 1] = String.Copy(GameFunction.Instance.newUsername);
        StoreToTheFile();
    }
}
