using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlayController : MonoBehaviour
{

    public float startTime = 35f; 
    private float currentTime;
    public TMP_Text countdownText;     
    public Color positiveColor = Color.green; 
    public Color negativeColor = Color.red; 
    private bool playerReachedGoal = false;
    public GameObject levelCompletedPanel;      
    public Canvas minimapCanvas;
    private float totalTimePlayed = 0f;    
    public CameraMachineController cameraMachineController;
    void Start()
    {
        currentTime = startTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerReachedGoal) return;
        if (!cameraMachineController.cutsceneOver) return; // waiting to let the cutscene end

        currentTime -= Time.deltaTime;
        totalTimePlayed += Time.deltaTime;

        if (currentTime < 0)
        {
            countdownText.color = negativeColor; 
        }
        else
        {
            countdownText.color = positiveColor;  
        }

        countdownText.text = Mathf.Ceil(currentTime).ToString();
    }
    IEnumerator EndRound()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("MainScene");
    }
    void SavePlayerProgress()
    {
        float previousTotalTime = PlayerPrefs.GetFloat("TotalTimePlayed", 0f);
        PlayerPrefs.SetFloat("TotalTimePlayed", previousTotalTime + totalTimePlayed);

        int gamesPlayed = PlayerPrefs.GetInt("GamesPlayed", 0);
        PlayerPrefs.SetInt("GamesPlayed", gamesPlayed + 1);

        float bestRun = PlayerPrefs.GetFloat("BestRun", 0f);
        if (startTime - currentTime < bestRun | bestRun == 0)
        {
            PlayerPrefs.SetFloat("BestRun", startTime - currentTime);
        }
        PlayerPrefs.Save(); 
    }
    public void PlayerReachedGoal()
    {
        playerReachedGoal = true;
        SavePlayerProgress();
        levelCompletedPanel.SetActive(true);
        minimapCanvas.enabled = false;
        StartCoroutine(EndRound());
    }
}
