using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void LoadGame(){
        SceneManager.LoadScene("Game");
    }

    public void LoadTitle(){
        SceneManager.LoadScene("Title");
    }

    public void LoadCredits(){
        SceneManager.LoadScene("Credits");
    }

    public void LoadStory(){
        SceneManager.LoadScene("Story");
    }
}
