using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class backbtns : MonoBehaviour
{
    public GameObject leaderboard;
    [SerializeField] Button back;
    // Start is called before the first frame update
    void Start()
    {
        back.onClick.AddListener(() =>
        {
            leaderboard.SetActive(false);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
