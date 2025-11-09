[System.Serializable]
public class VampireStats
{
    public int Charisma { get; set; }
    public int Manipulation { get; set; }
    public int Knowledge { get; set; }
    public VampireStats(int charisma, int manipulation, int knowledge)
    {
        Charisma = charisma;
        Manipulation = manipulation;
        Knowledge = knowledge;
    }
}
