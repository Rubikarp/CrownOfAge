using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WG.Common
{
    public class ScrollViewSectionSelector : MonoBehaviour
    {
        [Header("References")]
        [SerializeField, Required] private ScrollViewController scrollController;
        [SerializeField] private SimpleButton nextStepButton;
        [SerializeField] private SimpleButton previousStepButton;

        [Header("Colors")]
        [SerializeField] private Color neutralTextColor = Color.gray;
        [SerializeField] private Color neutralBackgroundColor = Color.white;
        [Space]
        [SerializeField] private Color selectedTextColor = Color.green;
        [SerializeField] private Color selectedBackgroundColor = Color.white;

        [Header("Steps Settings")]
        [SerializeField] private int stepCount = 5;
        [SerializeField, Required] private RectTransform stepsContainer;
        [SerializeField, Required] private SimpleButton stepButtonPrefab;
        [field: SerializeField] public List<SimpleButton> stepButtons { get; private set; } = new List<SimpleButton>();

        private void Awake()
        {
            InitializeStepsButtons();
        }

        private void OnEnable()
        {
            nextStepButton.onClick.AddListener(() => scrollController.MoveToPreviousVerticalStep());
            previousStepButton.onClick.AddListener(() => scrollController.MoveToNextVerticalStep());
        }
        private void OnDisable()
        {
            nextStepButton.onClick.RemoveListener(() => scrollController.MoveToPreviousVerticalStep());
            previousStepButton.onClick.RemoveListener(() => scrollController.MoveToNextVerticalStep());
        }


        [Button]
        private void InitializeStepsButtons()
        {
            scrollController.useStep = true;
            scrollController.stepYQuantity = stepCount;

            stepsContainer.DeleteChildrens();
            stepsContainer.name = "stepsContainer";
            stepButtons.Clear();
            for (int i = 0; i < stepCount; i++)
            {
#if UNITY_EDITOR
                SimpleButton stepButton = (SimpleButton)PrefabUtility.InstantiatePrefab(stepButtonPrefab, stepsContainer);
#else
                SimpleButton stepButton = Instantiate(stepButtonPrefab, stepsContainer);
#endif
                stepButton.name = $"StepButton_{i}";
                int bufferedIndex = i;
                stepButton.onClick.AddListener(() => OnButtonStepClicked(bufferedIndex));
                stepButton.UpdateText((bufferedIndex + 1).ToString());
                stepButtons.Add(stepButton);
            }
        }

        private void OnButtonStepClicked(int activeStep)
        {
            for (int i = 0; i < stepButtons.Count; i++)
            {
                if (i == activeStep)
                {
                    stepButtons[i].UpdateAppearance(selectedBackgroundColor, selectedTextColor);
                }
                else
                {
                    stepButtons[i].UpdateAppearance(neutralBackgroundColor, neutralTextColor);
                }
            }

            scrollController.MoveToSpecificVerticalStep(activeStep);
        }
    }
}
