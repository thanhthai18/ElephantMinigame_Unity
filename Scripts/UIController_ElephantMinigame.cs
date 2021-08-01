using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController_ElephantMinigame : MonoBehaviour
{
    public GameObject waterBar_UI;
    private bool isShowWaterBarUI;
    [SerializeField] Elephant_ElephantMinigame elephant;
    void Start()
    {
        waterBar_UI.SetActive(false);
    }

    void Update()
    {
        if(elephant.Speed == 0)
        {
            waterBar_UI.SetActive(true);
        }
        
    }
}
