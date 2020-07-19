using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public float score = 0f;
    public int floorScore = 0;
    public int wallScore = 0;
    public float time = 180;

    public Transform floors;
    public Text timerText;

    public bool isStarted = true;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    void Update()
    {
        if (isStarted)
        {
            time -= Time.deltaTime;
            timerText.text = time.ToString("0");

            if (time < 0){
                time = 0;
                FinishGame();
            }
        } else {
            time = 0;
            timerText.text = time.ToString("0");
        }
    }

    public void BuildWall()
    {
        wallScore++;
        floorScore = wallScore / 6;
    }

    public void FinishGame(){
        isStarted = false;
    }

}
