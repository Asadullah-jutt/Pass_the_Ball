using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Events;


public class PQhandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI yourscoret;
    [SerializeField] TextMeshProUGUI highscoret;
    [SerializeField] TextMeshProUGUI yourscore;
    [SerializeField] TextMeshProUGUI highscore;
    [SerializeField] InputField highscorer;
    [SerializeField] TextMeshProUGUI highscorertxt;

    [SerializeField] Button playagain;
    [SerializeField] Button quit;
   // [SerializeField] Button lboard;
    public static PQhandler instance;
   // public GameObject leaderbaordimg;

    public UnityEvent<string, int> submitscoreevent;

    bool called = false;
    private void Awake()
    {
       // PlayerPrefs.DeleteAll();
        gameObject.SetActive(false);
        highscorer.gameObject.SetActive(false);
        instance = this;
        playagain.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(1);
        });
        quit.onClick.AddListener(() =>
        {
            Application.Quit();
        });
        //lboard.onClick.AddListener(() =>
        //{
        //    leaderbaordimg.SetActive(true);
        //});
    }

    public void SubmitScore()
    {
        submitscoreevent.Invoke(highscorer.text, PlayerPrefs.GetInt("yourscore", 0));
    }
    public void playagainn()
    {
        Debug.Log("aaa");
        SceneManager.LoadScene(1);
    }

    public void QQuit()
    {
        Application.Quit();
    }
    public void CheckScore()
    {
        if (called)
            return;
        called = true;
        gameObject.SetActive(true);
      ///  SubmitScore();
        int a = PlayerPrefs.GetInt("yourscore", 0);
        Debug.Log("NoHighScore");
        int b = PlayerPrefs.GetInt("highscore", 0); //int.Parse(leaderboard.scoree[leaderboard.scoree.Count - 1]);
        if (a > b)
        {
            Debug.Log("HighScore");
            highscoret.fontSize = 30;
            highscoret.text = "New HighScore";
            highscorer.gameObject.SetActive(true);
            PlayerPrefs.SetInt("highscore", a);
          //  ShowScore();
        }
        else
        { 
           ShowScore();
        }
        
      
    }

    public void Updatename()
    {
        PlayerPrefs.SetString("highscorername", highscorer.text);
        SubmitScore();
        highscorer.gameObject.SetActive(false);
       
        ShowScore();
    }

    void ShowScore()
    {
        highscoret.gameObject.transform.position = highscorer.gameObject.transform.position;
        highscorertxt.text = PlayerPrefs.GetString("highscorername"," ");
        yourscoret.text = "YourScore";
        highscoret.fontSize = 50;
        highscoret.text = "HighScore";
        highscore.text = PlayerPrefs.GetInt("highscore", 0).ToString();
        yourscore.text = PlayerPrefs.GetInt("yourscore", 0).ToString();

    }


    // Start is called before the first frame update

}
