[System.Serializable]
public class HumanStats
{
    public int Richness { get; set; }
    public int Influence { get; set; }
    public int Power { get; set; }

    public HumanStats(int richness, int influence, int power)
    {
        Richness = richness;
        Influence = influence;
        Power = power;
    }
}
