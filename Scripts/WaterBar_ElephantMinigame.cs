using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterBar_ElephantMinigame : MonoBehaviour
{
    public Image Bar;
    public float max_water = 100f;
    public float current_water;
    public float waterSpeed = 100f;
    public bool isWaterUp = true;
    public bool isDirecLionUp = true;
    public bool isClick = false;

    private void Start()
    {
        current_water = 0.01f;
        Bar.fillAmount = current_water / max_water;
    }

    public void IncreaseWater()
    {
        current_water += Time.deltaTime * waterSpeed;
        if (current_water > 100)
        {
            current_water = 100f;
        }
        Bar.fillAmount = current_water / max_water;
    }
    public void DecreaseWater()
    {
        current_water -= Time.deltaTime * waterSpeed / 10f;
        if (current_water < 0)
        {
            current_water = 0;
        }
        Bar.fillAmount = current_water / max_water;
    }
}


