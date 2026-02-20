using UnityEngine;

public class AbilityCard : Card
{
    [SerializeField] private string ability;

    public string Ability
    {
        get { return ability; }
        set { ability = value; }
    }

    public override void UseCard()
    {
        // Implement ability card logic here
        Debug.Log($"Using Ability Card: {ability} (Cost: {energy} energy)");
    }
}
