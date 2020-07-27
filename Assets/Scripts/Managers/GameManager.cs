using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
    public GameObject gameOverPanel;

    public bool isStarted = true;

    public GameObject player;

    [HideInInspector]
    public Transform playerFloor { get; private set; }

    private Transform lastFloor;

    float timeMonster;

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
        lastFloor = floors.GetChild(floors.childCount - 1);
        playerFloor = lastFloor;
        gameOverPanel.SetActive(false);
        RandomTimeMonster();
    }

    void Update()
    {
        if (isStarted)
        {

            ProcessTimeGame();
            ProcessTimeMonster();

        }
        else
        {
            time = 0;
            timerText.text = time.ToString("0");
        }
    }

    private void ProcessTimeGame()
    {
        time -= Time.deltaTime;
        timerText.text = time.ToString("0");


        if (time < 0)
        {
            time = 0;
            FinishGame();
        }
    }

    private void ProcessTimeMonster(){
        timeMonster -= Time.deltaTime;

        if (timeMonster < 0 && !player.GetComponent<MonsterAI>().enabled){
            timeMonster = 0;
            TransformMonster();
        }
    }

    public void BuildWall()
    {
        wallScore++;
        floorScore = wallScore / 6;
    }

    public void FinishGame()
    {
        isStarted = false;
        gameOverPanel.SetActive(true);
    }
    public void SpawnFloor()
    {
        Vector3 spawnPosition = lastFloor.position;
        spawnPosition.y += 3.200f;
        GameObject objFoor = Instantiate(prefabFloor, spawnPosition, lastFloor.rotation, floors);
        int nextId = lastFloor.GetComponent<FloorManager>().id + 1;
        lastFloor = objFoor.transform;
        lastFloor.GetComponent<FloorManager>().id = nextId;
        lastFloor.name = "Floor"+nextId;
    }

    public void BackNormal()
    {
        player.GetComponent<MonsterAI>().enabled = false;
        player.GetComponent<PlayerController>().enabled = true;
        player.transform.GetChild(0).gameObject.SetActive(false);
        RandomTimeMonster();
        AudioManager.instance.ChangeMusicToGoodGuy();
    }

    public void TransformMonster()
    {
        player.GetComponent<MonsterAI>().enabled = true;
        player.GetComponent<MonsterAI>().ResetAI();
        player.GetComponent<PlayerController>().enabled = false;
        player.transform.GetChild(0).gameObject.SetActive(true);
        AudioManager.instance.ChangeMusicToBadGuy();
    }

    private void RandomTimeMonster()
    {
        timeMonster = Random.Range(5f, 10f);
    }

    public void ChangeFloor(Transform floor){
        this.playerFloor = floor;
    }

    public FloorManager GetFloor(int id){
        if (id < GameManager.instance.floors.childCount && id >= 0)
        {
            return GameManager.instance.floors.GetChild(id).GetComponent<FloorManager>();
        }


        return null;
    }

}
