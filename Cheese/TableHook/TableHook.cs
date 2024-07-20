﻿using System;
using UdonSharp;
using UnityEngine;
using VRC.Udon;
using UnityEngine.UI;
using VRC.SDKBase;
using TMPro;

public class TableHook : UdonSharpBehaviour
{
    //Slider
    [SerializeField] public UdonBehaviour TableColorSlider;
    [SerializeField] public UdonBehaviour TableColorLightnessSlider;
    [HideInInspector] public float TableColor;
    [HideInInspector] public float TableColorLightness;

    [HideInInspector] public int inOwner;
    [HideInInspector]public int outCanUse;
    private int outCanUseTmp = 0;
    [SerializeField] private BilliardsModule[] table;
    public int DefaultCue;
    public bool keepRotating = false;
    private int isRotating;
    [NonSerialized] private int maxRotation=120;
    private Renderer renderer;
    void Start()
    {
        outCanUse = 0;
        outCanUseTmp = DefaultCue;
        isRotating = maxRotation;
        keepRotating = false;
        //BilliardsModule[] table =UnityEngine.Object.FindObjectsOfType<BilliardsModule>();
        renderer = this.transform.Find("body/render").GetComponent<Renderer>();
    }

    public void _CanUseCueSkin()
    {
            outCanUse = outCanUseTmp;
    }

    public void _ChangeKeepRotating()
    { 
        keepRotating = !keepRotating;
    }
    private void ChangeMaterial()
    {
        if (table != null)
        {
            for (int i = 0; i < table.Length; i++)
            {
                renderer.materials[1].SetTexture("_MainTex", table[i].cueSkins[outCanUseTmp]);
            }
        }
        isRotating = 0;

    }
    void Update()
    {
        if (isRotating < maxRotation || keepRotating)
        {
            renderer.transform.Rotate(new Vector3(1, 0.05f, 0.05f), Mathf.Clamp(maxRotation-isRotating,0,3), Space.Self);
            isRotating++;
        }
        TableColor=GetTableColor();
        TableColorLightness=GetTableLightness();
    }

    public float GetTableColor()
    {
        return (float)TableColorSlider.GetProgramVariable("localValue");
    }
    public float GetTableLightness()
    {
        return (float)TableColorLightnessSlider.GetProgramVariable("localValue");
    }
    public void _Cue0()
    {
        outCanUseTmp = 0;
        ChangeMaterial();
    }

    public void _Cue1()
    {
        outCanUseTmp = 1;
        ChangeMaterial();
    }
    public void _Cue2()
    {
        outCanUseTmp = 2;
        ChangeMaterial();
    }

    public void _Cue3()
    {
        outCanUseTmp = 3;
        ChangeMaterial();
    }
    public void _Cue4()
    {
        outCanUseTmp = 4;
        ChangeMaterial();
    }

    public void _Cue5()
    {
        outCanUseTmp = 5;
        ChangeMaterial();
    }
    public void _Cue6()
    {
        outCanUseTmp = 6;
        ChangeMaterial();
    }

    public void _Cue7()
    {
        outCanUseTmp = 7;
        ChangeMaterial();
    }
    public void _Cue8()
    {
        outCanUseTmp = 8;
        ChangeMaterial();
    }
    public void _Cue9()
    {
        outCanUseTmp = 9;
        ChangeMaterial();
    }

    public void _Cue10()
    {
        outCanUseTmp = 10;
        ChangeMaterial();
    }
}