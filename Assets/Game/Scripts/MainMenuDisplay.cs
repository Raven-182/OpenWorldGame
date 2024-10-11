using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuDisplay : MonoBehaviour
{

    public TMP_Text totalTimeText;
    public TMP_Text  gamesPlayedText;
    public TMP_Text  bestRunText;
    // Start is called before the first frame update
    void Start()
    {
        float totalTimePlayed = PlayerPrefs.GetFloat("TotalTimePlayed", 0f);
        int gamesPlayed = PlayerPrefs.GetInt("GamesPlayed", 0);
        float bestRun = PlayerPrefs.GetFloat("BestRun", 0f);

        totalTimeText.text = "Total Time: " + Mathf.Ceil(totalTimePlayed).ToString() + " s";
        gamesPlayedText.text = "Games Played: " + gamesPlayed.ToString();
        bestRunText.text = "Best Run: " + Mathf.Ceil(bestRun).ToString() + " s";
    }
}
