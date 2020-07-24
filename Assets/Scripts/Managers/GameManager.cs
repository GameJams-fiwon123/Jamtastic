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

    public GameObject prefabFloor;

    public bool isStarted = true;

    private Transform currentFloor;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        currentFloor = floors.GetChild(floors.childCount-1);
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
    public void SpawnFloor()
    {
        Vector3 spawnPosition =  currentFloor.position;
        spawnPosition.y += 3.200f;
        GameObject objFoor = Instantiate(prefabFloor, spawnPosition, currentFloor.rotation, floors);
        currentFloor = objFoor.transform;
    }

}
