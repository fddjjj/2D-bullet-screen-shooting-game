using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundControl : MonoBehaviour
{
    [Header("¼àÌý")]
    public SoundValueChange_SO SoundValueChangeEvent;
    public SoundValueChange_SO MainSound;
    public SoundValueChange_SO BGMSound;
    public SoundValueChange_SO FXSound;

    [Header("×é¼þ")]
    public Slider Main;
    public Slider BGM;
    public Slider FX;
    public AudioMixer mixer;
    private void Awake()
    {

    }

    private void OnEnable()
    {

        MainSound.action += SetValue;
        BGMSound.action += SetValue;
        FXSound.action += SetValue;
        Main.value = GetSoundValue("MasterVolume");
        BGM.value = GetSoundValue("BGMVolume");
        FX.value = GetSoundValue("FXVolume");
    }

    private void SetValue(string arg0, float arg1)
    {
        mixer.SetFloat(arg0, arg1 * 100 - 80);
    }

    private void OnDisable()
    {
        MainSound.action -= SetValue;
        BGMSound.action -= SetValue;
        FXSound.action -= SetValue;
    }

    public float GetSoundValue(string mixer_GroupName)
    {
        float amount;
        mixer.GetFloat(mixer_GroupName, out amount);
        Debug.Log(amount);
        return (amount + 80) / 100;
    }
    public void SetSoundValue(string mixer_GroupName,float amount)
    {
        mixer.SetFloat(mixer_GroupName, amount *100 - 80);
    }

}
