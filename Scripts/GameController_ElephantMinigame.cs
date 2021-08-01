using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DemoObserver;
using DG.Tweening;
public class GameController_ElephantMinigame : MonoBehaviour
{
    public static GameController_ElephantMinigame instance;
    public Button btnLake;
    public WaterBar_ElephantMinigame waterBar;
    public ParticleSystem waterParticle;
    [SerializeField] private Elephant_ElephantMinigame elephant;
    [SerializeField] private GameObject pivot;
    private bool isLake;
    private bool isPhunNuoc;
    private bool isDecreasingWater;
    public bool isHoldMouse;
    private float direction;
    private Vector2 mouseStartPos, mouseCurrentPos, mousePrevPos, mouseDefaultPos;
    private Vector3 makePivotPos;
    private float angelChanged;
    private float angelStart;
    public static bool isGameBegin;
    public bool isEndGame;
    public bool isFirst;
    public int checkPauseTimeGrow;
    public bool isResumeTimeGrow;
    private Camera mainCamera;
    [SerializeField] private GameObject tutorial1;
    [SerializeField] private GameObject tutorial2;


    public bool IsLake { get => isLake; set => isLake = value; }
    public bool IsPhunNuoc { get => isPhunNuoc; set => isPhunNuoc = value; }
    public bool IsDecreasingWater { get => isDecreasingWater; set => isDecreasingWater = value; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        isFirst = true;
        checkPauseTimeGrow = -1;
        isResumeTimeGrow = false;
        tutorial1.SetActive(false);
        tutorial2.SetActive(false);
        isHoldMouse = false;
        SetupGame();
        btnLake.onClick.AddListener(OnClickLake);
        makePivotPos = pivot.transform.position;
        UtilitisMinigame.AddMenuMinigame();
    }

    void SetupGame()
    {
        isGameBegin = false;
        isEndGame = false;
        mainCamera = Camera.main;

        MoveToElephant();
        Invoke("MoveToHouse", 4f);
        Invoke("MoveFullScreen", 9f);
        Invoke("StartGame", 11.5f);
        Invoke("OnClickLake", 11.5f);
        Invoke("ShowTutorial1", 14f);
        Invoke("StartGame", 14f);
    }

    void MoveToElephant()
    {
        mainCamera.transform.DOMove(new Vector3(-5.96f, -1.95f, mainCamera.transform.position.z), 2).SetEase(Ease.Linear);
        mainCamera.DOOrthoSize(4, 2);
    }

    void MoveToHouse()
    {
        mainCamera.transform.DOMove(new Vector3(6.8f, 1.31f, mainCamera.transform.position.z), 2).SetEase(Ease.Linear);
        mainCamera.DOOrthoSize(6.446302f, 2);
    }

    void MoveFullScreen()
    {
        mainCamera.transform.DOMove(new Vector3(0, 0, mainCamera.transform.position.z), 2).SetEase(Ease.Linear);
        mainCamera.DOOrthoSize(7f, 2);
    }

    void ShowTutorial1()
    {
        tutorial1.gameObject.SetActive(true);
        tutorial1.transform.DOMove(new Vector3((tutorial1.transform.position.x), -1, (tutorial1.transform.position.z)), 1f).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }

    void StartGame()
    {
        isGameBegin = true;
    }

    void ShowTutorial2()
    {
        tutorial2.gameObject.SetActive(true);
        tutorial2.transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 1).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }

    public void OnClickLake()
    {
        if (waterBar.current_water == 0 && isGameBegin)
            IsLake = true;
    }

    public bool IsDungImHutNuoc()
    {
        if (elephant.Speed == 0)
        {
            isGameBegin = true;
            return true;
        }
        else
        {
            isGameBegin = false;
            return false;
        }
    }

    public float AngelChanged(Vector2 oldPos, Vector2 newPos)
    {
        return (180 / Mathf.PI) * (Mathf.Abs(Mathf.Atan(newPos.y / newPos.x) - Mathf.Atan(oldPos.y / oldPos.x)));
    }


    private void Update()
    {
        IsDungImHutNuoc();

        if (Input.GetMouseButtonDown(0))
        {
            mouseStartPos = mainCamera.ScreenToWorldPoint(Input.mousePosition) - makePivotPos;
            mouseDefaultPos = (new Vector2(mouseStartPos.x, -5.58f)) - (Vector2)makePivotPos;
            mouseStartPos.x = mouseDefaultPos.x;
            angelStart = AngelChanged(mouseDefaultPos, mouseStartPos);
            if (mouseStartPos.y - mouseDefaultPos.y > 0)
            {
                direction = 1;
            }
            else direction = -1;
            waterParticle.transform.Rotate(0, 0, angelStart * 2.5f * direction);

            mouseCurrentPos = mouseStartPos;
            isHoldMouse = true;
            IsPhunNuoc = true;
            tutorial1.transform.DOKill();
            tutorial1.SetActive(false);
        }

        if (Input.GetMouseButtonUp(0))
        {
            IsPhunNuoc = false;
            isHoldMouse = false;
            mouseStartPos = Vector2.zero;
            mouseCurrentPos = Vector2.zero;
            waterParticle.Stop();
            waterParticle.transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if (isHoldMouse && waterBar.current_water > 0 && IsDecreasingWater)
        {
            waterParticle.Emit(1); //Play, Stop Particle
        }

        if (isHoldMouse)
        {
            mousePrevPos = mouseCurrentPos;
            mouseCurrentPos.y = mainCamera.ScreenToWorldPoint(Input.mousePosition).y - makePivotPos.y;
            mouseCurrentPos.x = mouseStartPos.x;


            if (mouseCurrentPos.y - mousePrevPos.y > 0)
            {
                direction = 1;
            }
            else direction = -1;
            if (angelChanged > 0)
            {
            waterParticle.transform.Rotate(0, 0, angelChanged * 3.5f * direction);

            }
        }


        angelChanged = AngelChanged(mousePrevPos, mouseCurrentPos);


        if (IsLake == true && IsDungImHutNuoc() == true)
        {
            mouseStartPos = Vector2.zero;
            mouseCurrentPos = Vector2.zero;
            mouseDefaultPos = new Vector2(9.8f, -5.58f) - (Vector2)makePivotPos;
            waterBar.IncreaseWater();
            IsPhunNuoc = false;
            IsDecreasingWater = false;
            isHoldMouse = false;
            if (waterBar.current_water == 100f)
            {
                IsLake = false;
            }
        }

        if (!IsLake)
        {
            IsDecreasingWater = true;
        }

        if (IsPhunNuoc)
        {
            waterBar.DecreaseWater();
        }

        // Show Tutorial 2
        if (waterBar.current_water == 0 && isGameBegin && isFirst)
        {
            ShowTutorial2();
            checkPauseTimeGrow = 1;
            isFirst = false;
        }

        if (isFirst == false && waterBar.current_water > 0)
        {
            isResumeTimeGrow = true;
        }

        if (isResumeTimeGrow)
        {
            //tutorial2.transform.DOKill();
            DOTween.Kill(tutorial2.transform);
            tutorial2.SetActive(false);
        }

        if (isResumeTimeGrow && waterBar.current_water == 100)
        {
            checkPauseTimeGrow = 0;
        }



        if (mouseCurrentPos.y - mousePrevPos.y > 0)
        {
            direction = 1;
        }
        else direction = -1;
        //if (waterParticle && Input.GetMouseButton(0))
        //{
        //    if (isGameBegin)
        //    {
        //        waterParticle.transform.Rotate(0, 0, angelChanged * 4f * direction);
        //    }
        //}
    }

}


