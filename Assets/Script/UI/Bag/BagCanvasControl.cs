using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BagCanvasControl :SingleTon<BagCanvasControl>
{
    public TextMeshProUGUI toolName;
    public TextMeshProUGUI functionDescribe;
    public TextMeshProUGUI storyDescribe;
    public CharacterControl characterControl;

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
}
