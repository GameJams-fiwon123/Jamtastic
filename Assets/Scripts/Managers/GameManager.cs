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
    public float time = 180;

    public Transform floors;
    public Text timerText;
    public Text scoreText;
    public Text hiScoreText;

    public GameObject prefabFloor;
    public GameObject gameOverPanel;

    public RuntimeAnimatorController animPlayer;
    public RuntimeAnimatorController animMonster;

    public bool isStarted = true;

    public GameObject player;

    [HideInInspector]
    public Transform playerFloor { get; private set; }

    private Transform lastFloor;

    private float sec, min;

    float timeMonster;
    float randomTimeMonster;

    public float diffTransform = 1;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        min = time / 60;
        sec = time % 60;

        lastFloor = floors.GetChild(floors.childCount - 1);
        playerFloor = lastFloor;
        gameOverPanel.SetActive(false);
        RandomTimeMonster();
        AudioManager.instance.ChangeMusicToGoodGuy();
    }

    void Update()
    {
        if (isStarted)
        {
            ProcessTimeGame();

            if (!player.GetComponent<MonsterAI>().enabled)
                ProcessTimeMonster();

            scoreText.text = string.Format("{0:00000000}", score);
        }
        else
        {
            min = 0;
            sec = 0;
            timerText.text = "0:00";
        }
    }

    private void ProcessTimeGame()
    {
        sec -= Time.deltaTime;
        if (sec < 0)
        {
            sec = 59f;
            min--;
        }

        timerText.text = string.Format("{0:0}:{1:00}", min, sec);


        if (min < 0)
        {
            min = 0;
            sec = 0;
            FinishGame();
        }
    }

    private void ProcessTimeMonster()
    {
        timeMonster -= Time.deltaTime;

        if (timeMonster < 0)
        {
            timeMonster = 0;
            TransformMonster();
        }
    }

    public void BuildWall()
    {
        score += 50;
    }

    public void DestroyWall()
    {
        score -= 50;
    }

    public void FinishGame()
    {
        isStarted = false;
        if (score > PlayerPrefs.GetFloat("hiScore", 0f))
        {
            PlayerPrefs.SetFloat("hiScore", score);
        }
        hiScoreText.text = "Melhor Pontuação: \n" + PlayerPrefs.GetFloat("hiScore", 0f).ToString();
        gameOverPanel.SetActive(true);
    }
    public void SpawnFloor()
    {
        if (lastFloor.GetComponent<FloorManager>().id != 0)
        {
            score += 150;

            if (diffTransform < 6)
                diffTransform += 1;

            if (player.GetComponent<MonsterAI>().sliderEnergy.maxValue < 2f)
                player.GetComponent<MonsterAI>().sliderEnergy.maxValue += 0.1f;

            RandomTimeMonster();
        }

        Vector3 spawnPosition = lastFloor.position;
        spawnPosition.y += 3.200f;
        GameObject objFoor = Instantiate(prefabFloor, spawnPosition, lastFloor.rotation, floors);
        int nextId = lastFloor.GetComponent<FloorManager>().id + 1;
        lastFloor = objFoor.transform;
        lastFloor.GetComponent<FloorManager>().id = nextId;
        lastFloor.name = "Floor" + nextId;
    }

    public void BackNormal()
    {
        player.GetComponent<MonsterAI>().enabled = false;
        player.GetComponent<PlayerController>().enabled = true;
        player.transform.GetChild(0).gameObject.SetActive(false);
        player.GetComponent<Animator>().runtimeAnimatorController = animPlayer;
        RandomTimeMonster();
        AudioManager.instance.ChangeMusicToGoodGuy();
    }

    public void TransformMonster()
    {
        if (playerFloor.GetComponent<FloorManager>().id == 0)
        {
            RandomTimeMonster();
        }
        else
        {
            randomTimeMonster = 0f;
            player.GetComponent<MonsterAI>().enabled = true;
            player.GetComponent<MonsterAI>().ResetAI();
            player.GetComponent<PlayerController>().enabled = false;
            player.transform.GetChild(0).gameObject.SetActive(true);
            player.GetComponent<Animator>().runtimeAnimatorController = animMonster;
            AudioManager.instance.ChangeMusicToBadGuy();
        }
    }

    private void RandomTimeMonster()
    {
        randomTimeMonster = Random.Range(15f - diffTransform * 1.5f, 35f - diffTransform * 4f) - randomTimeMonster;
        if (randomTimeMonster < 0)
        {
            randomTimeMonster = 0f;
        }
        timeMonster = randomTimeMonster;
    }

    public void ChangeFloor(Transform floor)
    {
        this.playerFloor = floor;
    }

    public FloorManager GetFloor(int id)
    {
        if (id < GameManager.instance.floors.childCount && id >= 0)
        {
            return GameManager.instance.floors.GetChild(id).GetComponent<FloorManager>();
        }


        return null;
    }

}
