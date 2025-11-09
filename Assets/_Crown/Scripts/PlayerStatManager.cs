using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStatManager : Singleton<PlayerStatManager>
{
    [Header("Vampire Stats")]
    public VampireStats VampireStats { get; set; } = new VampireStats(20, 10, 10);

    [Header("Human Stats")]
    public List<HumanData> Followers { get; set; } = new List<HumanData>();
    public int GetTotalRichness() => Followers.Sum(follower => follower.Stats.Richness);
    public int GetTotalInfluence() => Followers.Sum(follower => follower.Stats.Influence);
    public int GetTotalPower() => Followers.Sum(follower => follower.Stats.Power);

}
