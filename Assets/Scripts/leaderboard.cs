using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;

public class leaderboard : MonoBehaviour
{
    string publickey = "9ad40afb16bf0d6e3a67f8202c2174467256765363d28361a709930468fc4c3b";

    public TextMeshProUGUI[] names, score;



    List<string> namee, scoree;
    public static List<string> insnamee, insscoree;

    public void GetLeaderboard()
    {
        LeaderboardCreator.GetLeaderboard(publickey, ((msg) =>
         {
             int looplength = namee.Count;
             if (msg.Length < namee.Count)
                 looplength = msg.Length;
             for(int i =0; i < looplength; i++ )
             {
               //  print(i);
                 namee[i] = msg[i].Username;
                 scoree[i] = msg[i].Score.ToString();
             }
             for (int i = 0; i < looplength; i++)
             {
                 //  print(i);
                 names[i].text = msg[i].Username;
                 score[i].text = msg[i].Score.ToString();
             }
         }));
    }



    public void SetLeaderBoardEntry(string nameee , int scoreee)
    {
        LeaderboardCreator.UploadNewEntry(publickey, nameee, scoreee, ((msg) => 
        {
          //  GetLeaderboard();
        }));
    }
    private void Awake()
    {
        insnamee = this.namee;
        insscoree = this.namee;

        namee = new List<string>();
        scoree = new List<string>();
        GetLeaderboard();
    }
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
