using Sirenix.OdinInspector;
using UnityEngine;
using System;

public class ScreenShoot : MonoBehaviour
{
    [SerializeField] string filename;

    [Button]
    public void MakeScreenShot()
    {
        ScreenCapture.CaptureScreenshot($"{filename}_{DateTime.Now.Ticks}.png");
    }
}