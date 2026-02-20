using UnityEngine;

public abstract class Card : MonoBehaviour
{
    [SerializeField] protected int energy;

    public int Energy
    {
        get { return energy; }
        set { energy = value; }
    }

    public abstract void UseCard();
}
