using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class StartUI : MonoBehaviour
{
    [SerializeField] Button play;
    [SerializeField] Button howtoplay;
    [SerializeField] Button quit;
    [SerializeField] Button back;
    [SerializeField] GameObject img;

    private void Awake()
    {
        img.SetActive(false);
        play.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(1);
        });
        howtoplay.onClick.AddListener(() =>
        {
            img.SetActive(true);
        });
        quit.onClick.AddListener(() =>
        {
            Application.Quit();
        });
        back.onClick.AddListener(() =>
        {
            img.SetActive(false);
        });
    }
    // Start is called before the first frame update
    
}
