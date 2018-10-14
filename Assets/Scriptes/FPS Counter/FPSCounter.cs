using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    
    public int frameRange = 60;

    public int AverageFPS { get; private set; }      // to be get from a diffrent scriptes
    public int HighestFPS { get; private set; }     // highest FPS in each frameRate
    public int LowestFPS { get; private set; }      // lowest FPS in each frameRate

    int[] bufferFPS;
    int indexFPSbuffer;   

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
            else if (fps < lowest)
            {
                lowest = fps;
            }
        }
        AverageFPS = sum / frameRange;
        HighestFPS = highest;
        LowestFPS = lowest;
    }
}
