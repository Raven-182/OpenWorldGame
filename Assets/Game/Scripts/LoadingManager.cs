using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LadingManager : MonoBehaviour
{
    public TMP_Text loadText;
    public float loadTime = 5f;
    
    IEnumerator SceneAsyncLoad(string scene){
        AsyncOperation sceneOp = SceneManager.LoadSceneAsync(scene);
        sceneOp.allowSceneActivation= false;
        while(!sceneOp.isDone){
            float prog = Mathf.Clamp01(sceneOp.progress/0.9f);
            loadText.text = loadText.text = $"Loading... {(int)(prog * 100)}%" ;
            if (!sceneOp.allowSceneActivation && sceneOp.progress >= 0.9f)
            {
               yield return new WaitForSeconds(loadTime);
               sceneOp.allowSceneActivation = true;
                
            }
            yield return null;
        }

    }
 

    public void loadSettingsScene(){
        StartCoroutine(SceneAsyncLoad("SettingsScene"));
    }
    public void loadGameScene(){
        StartCoroutine(SceneAsyncLoad("GameScene"));
    }
    public void loadMainScene(){
        StartCoroutine(SceneAsyncLoad("MainScene"));
    }
}
