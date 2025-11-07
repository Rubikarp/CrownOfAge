using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using TMPro;

[RequireComponent(typeof(RectTransform))]
public class SimpleButton : Selectable, IPointerClickHandler, ISubmitHandler
{
    public RectTransform rectTransform => (RectTransform)transform;

    [Header("Button Elements")]
    [SerializeField, Required] protected TextMeshProUGUI primaryTextSlot;
    [field: SerializeField] public bool HasSecondaryText { get; set; } = false;
    [ShowIf("HasSecondaryText"), SerializeField] protected TextMeshProUGUI secondaryTextSlot;
    [Space]
    [SerializeField, Required] protected MaskableGraphic[] coloredFont;
    [SerializeField, Required] protected MaskableGraphic[] coloredBackground;
    [Space]
    [SerializeField] protected Color fontColor = Color.black;
    [SerializeField] protected Color backgroundColor = Color.white;

    public UnityEvent onClick;

    protected override void Awake()
    {
        base.Awake();
    }
    protected void OnValidate() 
    {
        targetGraphic = null;
        transition = Selectable.Transition.None;

        foreach (var txt in coloredFont) txt.color = fontColor;
        foreach (var img in coloredBackground) img.color = backgroundColor;
    }

    public void UpdateText(string textContent)
    {
        primaryTextSlot.text = textContent;
    }
    public void UpdateSecondaryText(string textContent)
    {
        if(!HasSecondaryText) return;
        if(secondaryTextSlot == null)
        {
            Debug.LogError("Try to update secondary text, but the slot is not assigned.", this); 
            return;
        }

        secondaryTextSlot.text = textContent;
    }

    private void Press()
    {
        if (!IsActive() || !IsInteractable()) return;

        UISystemProfilerApi.AddMarker("Button.onClick", this);
        onClick?.Invoke();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        Press();
    }
    public void OnSubmit(BaseEventData eventData) => Press();

    public void UpdateAppearance(Color imgColor, Color textColor)
    {
        this.fontColor = textColor;
        this.backgroundColor = imgColor;

        foreach (var txt in coloredFont) txt.color = this.fontColor;
        foreach (var img in coloredBackground) img.color = backgroundColor;
    }
    public Tween DoUpdateAppearance(Color imgColor, Color textColor, float duration)
    {
        this.fontColor = textColor;
        this.backgroundColor = imgColor;

        var seq = DOTween.Sequence();
        foreach (var txt in coloredFont) seq.Join(txt.DOColor(this.fontColor, duration));
        foreach (var img in coloredBackground) seq.Join(img.DOColor(backgroundColor, duration));
        return seq;
    }
}