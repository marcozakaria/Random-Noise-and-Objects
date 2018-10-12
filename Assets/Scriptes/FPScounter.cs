using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPScounter : MonoBehaviour
{
    public Text AverageFPSCounterText , LowestFPSCounterText , HighestFPSCounterText;
    public int frameRange = 60;

    public int AverageFPS { get; private set; }      // to be get from diffrent scriptes
    public int HighestFPS { get; private set; }
    public int LowestFPS { get; private set; }

    int[] bufferFPS;
    int indexFPSbuffer;

    static string[] stringsFrom00To99 =         // TO AVOID CRETING STRING FOR FPS EVERY FRAME
        {
            "00", "01", "02", "03", "04", "05", "06", "07", "08", "09",
            "10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
            "20", "21", "22", "23", "24", "25", "26", "27", "28", "29",
            "30", "31", "32", "33", "34", "35", "36", "37", "38", "39",
            "40", "41", "42", "43", "44", "45", "46", "47", "48", "49",
            "50", "51", "52", "53", "54", "55", "56", "57", "58", "59",
            "60", "61", "62", "63", "64", "65", "66", "67", "68", "69",
            "70", "71", "72", "73", "74", "75", "76", "77", "78", "79",
            "80", "81", "82", "83", "84", "85", "86", "87", "88", "89",
            "90", "91", "92", "93", "94", "95", "96", "97", "98", "99"
         };

    void InitializeBuffer()
    {
        if (frameRange <= 0)
        {
            frameRange = 1;
        }
        bufferFPS = new int[frameRange];
        indexFPSbuffer = 0;
    }

    private void Update()
    {
        if (bufferFPS == null || bufferFPS.Length!= frameRange)  // incase we changed the frame range inside playmode
        {
            InitializeBuffer();
        }
        UpdateBuffer();
        CalculateFPS();
        AverageFPSCounterText.text = stringsFrom00To99[Mathf.Clamp(AverageFPS, 0, 99)];
        LowestFPSCounterText.text = stringsFrom00To99[Mathf.Clamp(LowestFPS, 0, 99)];
        HighestFPSCounterText.text = stringsFrom00To99[Mathf.Clamp(HighestFPS, 0, 99)];
    }

    void UpdateBuffer()
    {
        bufferFPS[indexFPSbuffer++] = (int)(1f / Time.unscaledDeltaTime);
        if (indexFPSbuffer >= frameRange)
        {
            indexFPSbuffer = 0;
        }
    }

    void CalculateFPS()
    {
        int sum = 0;
        int highest = 0;
        int lowest = 144;
        for (int i = 0; i < frameRange; i++)
        {
            int fps = bufferFPS[i];
            sum += fps;
            if (fps > highest)
            {
                highest = fps;
            }
            if (fps < lowest)
            {
                lowest = fps;
            }
        }
        AverageFPS = sum / frameRange;
        HighestFPS = highest;
        LowestFPS = lowest;
    }
}
