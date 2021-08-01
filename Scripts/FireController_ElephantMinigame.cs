using DemoObserver;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController_ElephantMinigame : MonoBehaviour
{
    //Singleton
    public static FireController_ElephantMinigame instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        if (instance != this)
        {
            Destroy(instance);
        }
    }
    //end Singleton

    [SerializeField] private Transform[] fireSpawnPos;
    [SerializeField] private GameObject firePrefab;
    public bool _bigSize;
    private GameObject fireball;
    private int phase;
    private bool[] windowHaveFire;


    private void Start()
    {
        windowHaveFire = new bool[fireSpawnPos.Length];
        for(int i = 0; i < fireSpawnPos.Length; i++)
        {
            windowHaveFire[i] = false;
        }
        phase = 1;
        StartCoroutine(StartPhase1());
    }

    public void SpawnRandomFireBall(bool bigSize)
    {
        List<int> posNotHaveFire = new List<int>();
        for (int i = 0; i < fireSpawnPos.Length; i++)
        {
            if (windowHaveFire[i] == false)
            {
                posNotHaveFire.Add(i);
            }
        }

        int randomIndexListWindow = UnityEngine.Random.Range(0, posNotHaveFire.Count);
        int randomIndex = posNotHaveFire[randomIndexListWindow];
        Vector3 spawnPos = new Vector3(fireSpawnPos[randomIndex].position.x, fireSpawnPos[randomIndex].position.y, 0);

        fireball = Instantiate(firePrefab, spawnPos, Quaternion.identity);

        windowHaveFire[randomIndex] = true;

        FireBall_ElephantMinigame fireballScript = fireball.GetComponent<FireBall_ElephantMinigame>();
        fireballScript.spawnIndex = randomIndex;
        fireballScript.isBigSize = bigSize;
    }

    public void ClearFireInWindow(int index)
    {
        windowHaveFire[index] = false;
    }

    //dem so dam chay
    public int GetCountFireLive()
    {
        int result = 0;
        for (int i = 0; i < fireSpawnPos.Length; i++)
        {
            if (windowHaveFire[i]) result++;
        }
        return result;
    }
    private void Update()
    {
        GetCountFireLive();

        if (phase == 1)
        {
            if (GetCountFireLive() == 1)
            {
                phase = 2;
            }
        }
        if (phase == 2)
        {
            SpawnRandomFireBall(true);
            SpawnRandomFireBall(true);
            SpawnRandomFireBall(false);
            SpawnRandomFireBall(false);
            phase = -1;
        }

        //Win game
        if (GetCountFireLive() == 0 && GameController_ElephantMinigame.isGameBegin)
        {
            this.PostEvent(EventID.OnEndMinigame, 3);
            GameController_ElephantMinigame.instance.isHoldMouse = false;
            GameController_ElephantMinigame.instance.isEndGame = true;
            GameController_ElephantMinigame.instance.waterBar.current_water = 0;
        }
    }

    void PopUpWin()
    {
        // khi co anim win game thi se dung cai nay de show 3 sao
    }

    private IEnumerator StartPhase1()
    {
        yield return new WaitForSeconds(6.5f);

        //phase1
        SpawnRandomFireBall(true);
        SpawnRandomFireBall(true);
        SpawnRandomFireBall(true);
        SpawnRandomFireBall(false);
        SpawnRandomFireBall(false);
        SpawnRandomFireBall(false);
    }
}
