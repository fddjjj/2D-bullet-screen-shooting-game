using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BagCanvasControl :SingleTon<BagCanvasControl>
{
    public TextMeshProUGUI toolName;
    public TextMeshProUGUI functionDescribe;
    public TextMeshProUGUI storyDescribe;
    public CharacterControl characterControl;
    public Canvas dragCanvas;
    public GridType currentGridType;
    public GridType targetGridType;
    public TotalGridControl equipedPropertyTotalGrid;
    public TotalGridControl shootPropertyTotalGrid;
    public TotalGridControl simplyPropertyTotalGrid;
    public GameObject unequipedLayout;

    protected override void Awake()
    {
        base.Awake();
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        //StartCoroutine(refresh());
        RefreshUnequipedLayoutGroup();
    }
    public void RefreshDescribe(string name,string function,string story)
    {
        toolName.text = name;
        functionDescribe.text = function;
        storyDescribe.text = story;
    }
    public void ClearDescribe()
    {
        toolName.text=null;
        functionDescribe.text=null;
        storyDescribe.text=null;
    }
    public bool CheckInEquipedProperty(Vector3 position)
    {
        for(int i=0;i<equipedPropertyTotalGrid.Grids.Length;i++)
        {
            RectTransform t =(RectTransform) equipedPropertyTotalGrid.Grids[i].gameObject.transform;
            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
            {
                return true;
            }
        }
        return false;
    }  
    public bool CheckInShootProperty(Vector3 position)
    {
        for(int i=0;i<shootPropertyTotalGrid.Grids.Length;i++)
        {
            RectTransform t =(RectTransform) shootPropertyTotalGrid.Grids[i].gameObject.transform;
            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
            {
                return true;
            }
        }
        return false;
    }   
    public bool CheckInSimplyProperty(Vector3 position)
    {
        for(int i=0;i<simplyPropertyTotalGrid.Grids.Length;i++)
        {
            RectTransform t =(RectTransform) simplyPropertyTotalGrid.Grids[i].gameObject.transform;
            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
            {
                return true;
            }
        }
        return false;
    }
    IEnumerator refresh()
    {
        yield return new WaitForSeconds(1f);
        RefreshUnequipedLayoutGroup();
        //Debug.Log("Refresh");
        yield break;
    }
    public void RefreshUnequipedLayoutGroup()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)unequipedLayout.transform);
    }
}
