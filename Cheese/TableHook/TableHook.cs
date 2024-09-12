﻿using System;
using UdonSharp;
using UnityEngine;
using VRC.Udon;
using UnityEngine.UI;
using VRC.SDKBase;
using TMPro;

public class TableHook : UdonSharpBehaviour
{
    [SerializeField] public Texture2D[] cueSkins;
    //Slider
    [SerializeField] public UdonBehaviour TableColorSlider;
    [SerializeField] public UdonBehaviour TableColorLightnessSlider;
    [HideInInspector] public float TableColor;
    [HideInInspector] public float TableColorLightness;

    // Cue Skin & Ball Skin
    [HideInInspector] public int inOwner;
    [HideInInspector] public byte outCanUse;
    private byte outCanUseTmp = 0;
    [HideInInspector] public byte ball;
    public byte DefaultCue;
    [SerializeField] public bool keepCueRotating;
    private int isRotating;
    [NonSerialized] private int maxRotation=130;
    private Renderer renderer;

    //Save & Load
    public InputField inputField;

    void Start()
    {
        outCanUse = 0;
        ball = 0;
        outCanUseTmp = DefaultCue;
        isRotating = maxRotation;
        renderer = this.transform.Find("body/render").GetComponent<Renderer>();

        //Load PlayerSettings from my server
        //string LocalData = EncodeLocalData();

    }

    public void _CanUseCueSkin()
    {
            outCanUse = outCanUseTmp;
    }

    //public void _ChangeKeepRotating()
    //{ 
    //    keepCueRotating = !keepCueRotating;
    //}
    private void ChangeMaterial()
    {
        if (cueSkins[outCanUseTmp] != null)
        {
                renderer.materials[1].SetTexture("_MainTex", cueSkins[outCanUseTmp]);
        }
        isRotating = 0;
    }
    void Update()
    {
        if ((isRotating < maxRotation) || keepCueRotating)
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

    //Sava and load system
    #region ConvertFunction
    private void floatToBytes(byte[] data, int pos, float v)
    {
        byte[] bytes = BitConverter.GetBytes(v);
        Array.Copy(bytes, 0, data, pos, 4);
    }

    public float bytesToFloat(byte[] data, int pos)
    {
        byte[] floatBytes = new byte[4];
        Array.Copy(data, pos, floatBytes, 0, 4);
        return BitConverter.ToSingle(floatBytes, 0);
    }

    private bool isInvalidBase64Char(char value)
    {
        var intValue = (int)value;

        // 1 - 9
        if (intValue >= 48 && intValue <= 57)
            return false;

        // A - Z
        if (intValue >= 65 && intValue <= 90)
            return false;

        // a - z
        if (intValue >= 97 && intValue <= 122)
            return false;

        // + or /
        return intValue != 43 && intValue != 47;
    }

    private bool isValidBase64(string value)
    {
        if (value == null || value.Length == 0 || value.Length % 4 != 0
            || value.Contains(" ") || value.Contains("\t") || value.Contains("\r") || value.Contains("\n"))
            return false;
        var index = value.Length - 1;

        if (value[index] == '=')
            index--;

        if (value[index] == '=')
            index--;

        for (var i = 0; i <= index; i++)
            if (isInvalidBase64Char(value[i]))
                return false;

        return true;
    }
    #endregion

    #region Save & Load
    // I Call it : Cheese Version ,for short CV,rewrite from "NetworingManagers" 
    uint LocalDataLength = 11;
    private string EncodeLocalData()
    {
        byte[] gameState = new byte[LocalDataLength];
        int encodePos = 0;
        gameState[encodePos] = outCanUseTmp;
        encodePos += 1;
        gameState[encodePos] = ball;
        encodePos += 1;
        floatToBytes(gameState, encodePos,TableColor);
        encodePos += 4;
        floatToBytes(gameState, encodePos, TableColorLightness);
        encodePos += 4;

        // find gameStateLength
        //Debug.Log("gameStateLength = " + (encodePos + 1));
        
        return "CV:"+Convert.ToBase64String(gameState);

        //Debug.Log("CV:" + Convert.ToBase64String(gameState));
    }

    private void LoadLocalData(string gameStateStr)
    {
        if (!isValidBase64(gameStateStr)) return;

        byte[] gameState = Convert.FromBase64String(gameStateStr);
        if (gameState.Length != LocalDataLength) return;

        int encoodePos = 0;

        outCanUseTmp = gameState[encoodePos];
        encoodePos += 1;
        ball = gameState[encoodePos];
        encoodePos += 1;
        TableColor = bytesToFloat(gameState, encoodePos);
        encoodePos += 4;
        TableColorLightness = bytesToFloat(gameState,encoodePos);
        encoodePos += 4;

        ChangeMaterial();
    }

    public void OnSaveButtonPushed()
    {

        if (ReferenceEquals(null, inputField))
        {
            Debug.Log("Table Hook::OnSaveButtonPushed() inputField property is not set !");
            return;
        }

        inputField.text = EncodeLocalData();
    }

    public void OnLoadButtonPushed()
    {

        if (ReferenceEquals(null, inputField))
        {
            Debug.Log("Table Hook::OnSaveButtonPushed() inputField property is not set !");
            return;
        }

        if (string.IsNullOrEmpty(inputField.text))
        {
            return;
        }


        LoadLocalData(inputField.text);

    }
    #endregion

    #region Cue & Ball
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
    public void _Cue11()
    {
        outCanUseTmp = 11;
        ChangeMaterial();
    }
    public void _Cue12()
    {
        outCanUseTmp = 12;
        ChangeMaterial();
    }

    public void _Cue13()
    {
        outCanUseTmp = 13;
        ChangeMaterial();
    }

    public void _Ball0()
    {
        ball = 0;
    }
    public void _Ball1()
    {
        ball = 4;
    }
    public void _Ball2()
    {
        ball = 5;
    }
    public void _Ball3()
    {
        ball = 6;
    }
    #endregion
}