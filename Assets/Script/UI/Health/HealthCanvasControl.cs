using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthCanvasControl : SingleTon<HealthCanvasControl>
{
    public GameObject healthControl;
    List<Image> snowList = new List<Image>();
    public Image PowerSlider;


    private void OnEnable()
    {
        for (int i = 0; i < healthControl.transform.childCount; i++)
        {
            snowList.Add(healthControl.transform.GetChild(i).GetComponent<Image>());
        }
    }
    private void Update()
    {
        PowerSlider.fillAmount = PlayerStateManager.Instance.playerPower / PlayerStateManager.Instance.playerMaxPower;

    }

    public void RefreshHealth()
    {
        for(int i= (int)PlayerStateManager.Instance.playHealth - 1; i < healthControl.transform.childCount;i++)
        {
            if (i < 0)
                break;
            snowList[i].gameObject.SetActive(false);
        }
    }


}
