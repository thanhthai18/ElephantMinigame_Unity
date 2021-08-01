using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DemoObserver;

public class FireBall_ElephantMinigame : MonoBehaviour
{
    public int spawnIndex;
    public int currentProgression;
    public int maxGrowth = 5;
    private float time;
    private bool isTiming;
    private bool isAutoGrowth;
    private GameObject waterParticle;
    private float timeGrow = 0f;
    public bool isBigSize;

    public bool IsAutoGrowth { get => isAutoGrowth; set => isAutoGrowth = value; }


    void Start()
    {
        if (isBigSize)
        {
            for (int i = 0; i < transform.childCount - 1; i++)
            {
                gameObject.transform.GetChild(i).gameObject.SetActive(false);
            }
            gameObject.transform.GetChild(transform.childCount - 1).gameObject.SetActive(true);
            currentProgression = transform.childCount;
        }
        if (!isBigSize)
        {
            for (int i = 1; i < transform.childCount; i++)
            {
                gameObject.transform.GetChild(i).gameObject.SetActive(false);
            }
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            currentProgression = 1;
        }
    }

    void SetIsTiming()
    {
        IsAutoGrowth = true;
    }

    private void Update()
    {
        if (GameController_ElephantMinigame.isGameBegin)
        {
            AutoGrowthFire();

            if (Input.GetMouseButtonUp(0))
            {
                Invoke("SetIsTiming", 1.5f);
            }
            if (IsAutoGrowth)
            {
                isTiming = false;
            }

            if (isTiming)
            {
                time += Time.deltaTime;
            }
            else
                time -= Time.deltaTime;

            if (time < 0)
            {
                time = 0;
                isTiming = false;
            }
        }
        if (GameController_ElephantMinigame.instance.isEndGame)
        {
            IsAutoGrowth = false;
            Invoke("PopUpLose", 1);
            GameController_ElephantMinigame.instance.waterBar.gameObject.SetActive(false);
            GameController_ElephantMinigame.instance.waterBar.current_water = 0;
            GameController_ElephantMinigame.instance.isHoldMouse = false;
        }
        if (GameController_ElephantMinigame.instance.checkPauseTimeGrow == 1)
        {
            IsAutoGrowth = false;
        }
        if (GameController_ElephantMinigame.instance.checkPauseTimeGrow == 0)
        {
            IsAutoGrowth = true;
            GameController_ElephantMinigame.instance.checkPauseTimeGrow = -1;
        }
    }

    private void ClearFireInWindow()
    {
        FireController_ElephantMinigame.instance.ClearFireInWindow(spawnIndex);
    }

    private void OnParticleCollision(GameObject other)
    {
        isTiming = true;
        IsAutoGrowth = false;

        if (currentProgression != 1)
        {
            if (time >= 0.4f)
            {
                gameObject.transform.GetChild(currentProgression - 1).gameObject.SetActive(false);
                time = 0;
                currentProgression--;
                if (currentProgression > 0 && currentProgression < maxGrowth)
                {
                    gameObject.transform.GetChild(currentProgression - 1).gameObject.SetActive(true);
                }
            }
        }
        if (currentProgression == 1)
        {
            if (time >= 2f)
            {
                time = 0f;
                Destroy(gameObject);
                ClearFireInWindow();
            }
        }
    }
    bool addendgame;
    public void AutoGrowthFire()
    {
        float tRandom = Random.Range(6f, 7f);
        if (IsAutoGrowth)
        {
            if (currentProgression == 1)
            {
                timeGrow += Time.deltaTime;
                if (timeGrow >= tRandom)
                {
                    gameObject.transform.GetChild(currentProgression - 1).gameObject.SetActive(false);
                    currentProgression = 5;
                    gameObject.transform.GetChild(currentProgression - 1).gameObject.SetActive(true);
                    timeGrow = 0f;
                }
            }
            if (currentProgression == maxGrowth)
            {
                timeGrow += Time.deltaTime;
                if (timeGrow >= 12f && !addendgame)
                {
                    addendgame = true;
                    gameObject.transform.localScale = new Vector3(2.3f, 2.2f, 2.2f);
                    FireController_ElephantMinigame.instance.SpawnRandomFireBall(true);
                    FireController_ElephantMinigame.instance.SpawnRandomFireBall(true);
                    FireController_ElephantMinigame.instance.SpawnRandomFireBall(true);
                    FireController_ElephantMinigame.instance.SpawnRandomFireBall(true);
                    FireController_ElephantMinigame.instance.SpawnRandomFireBall(true);
                    timeGrow = 0f;

                    //Lose game
                    GameController_ElephantMinigame.instance.isEndGame = true;
                }
            }
            if (currentProgression > 1 && currentProgression < maxGrowth)
            {
                timeGrow += Time.deltaTime;
                if (timeGrow >= 0.5f)
                {
                    gameObject.transform.GetChild(currentProgression - 1).gameObject.SetActive(false);
                    currentProgression++;
                    gameObject.transform.GetChild(currentProgression - 1).gameObject.SetActive(true);
                    timeGrow = 0f;
                }
            }
        }
        else
        {
            timeGrow -= Time.deltaTime;
            if (timeGrow <= 0f)
            {
                timeGrow = 0f;
            }
        }
    }

    void PopUpLose()
    {
        this.PostEvent(EventID.OnEndMinigame, 0);
    }
}
