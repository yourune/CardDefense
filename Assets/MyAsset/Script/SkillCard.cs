using UnityEngine;

public class SkillCard : Card
{
    [SerializeField] private string ability;

    public string Ability
    {
        get { return ability; }
        set { ability = value; }
    }

    public override void UseCard()
    {
        // Implement skill card logic here
        Debug.Log($"Using Skill Card: {ability} (Cost: {energy} energy)");
    }
}
