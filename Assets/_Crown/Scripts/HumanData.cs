using UnityEngine;

[CreateAssetMenu(fileName = "New Human Data", menuName = "Crown/Human Data", order = 1)]
public class HumanData : ScriptableObject
{
    public string HumanName;
    public Sprite HumanPortrait;
    [Multiline(3)]
    public string Description;
    [Space]
    public TextAsset dateDialogue;
    [Space]
    public HumanStats Stats;
}