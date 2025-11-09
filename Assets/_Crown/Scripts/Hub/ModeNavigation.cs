using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

public enum EScreenCarousel
{
    AuraFarm,
    Tinder,
    Roucool,
}

public class ModeNavigation : SerializedMonoBehaviour
{
    public RectTransform RectTransform => rectTransform;
    private RectTransform rectTransform;

    [Header("Info")]
    public EScreenCarousel currentCenterScreen = EScreenCarousel.Tinder;

    [Header("Coponents")]
    [Space(10)]
    [SerializeField]
    private Dictionary<EScreenCarousel, ScreenMovement> screenModeMap = new Dictionary<EScreenCarousel, ScreenMovement>()
    {
        { EScreenCarousel.AuraFarm, null },
        { EScreenCarousel.Tinder, null },
        { EScreenCarousel.Roucool, null },
    };

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    private void OnValidate()
    {
        if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
        if(screenModeMap.Count != Enum.GetValues(typeof(EScreenCarousel)).Length)
        {
            var newMap = new Dictionary<EScreenCarousel, ScreenMovement>()
            {
                { EScreenCarousel.AuraFarm, null },
                { EScreenCarousel.Tinder, null },
                { EScreenCarousel.Roucool, null },
            };
            foreach (var key in screenModeMap.Keys)
            {
                if (newMap.ContainsKey(key))
                {
                    newMap[key] = screenModeMap[key];
                }
            }
            screenModeMap = newMap;
        }
    }

    [Button] public void MoveToPreviousScreen() => MoveToScreen(-1);
    [Button] public void MoveToNextScreen() => MoveToScreen(1);

    public void MoveToScreen(int move)
    {
        var currentPos = (int)currentCenterScreen + move;
        currentPos = (currentPos + 3) % 3; // Loop around 0-2
        currentCenterScreen = (EScreenCarousel)(currentPos);
        foreach (var screen in screenModeMap)
        {
            screen.Value.Move(-move);
        }
    }
}
