using System.Xml.Serialization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CombatantView : MonoBehaviour
{
    //serializefield for healthbar
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private SpriteRenderer spriteRenderer;
    public int MaxHealth { get; private set; }
    public int currentHealth { get; private set; }

    protected void SetupBase(int health, Sprite sprite)
    {
        MaxHealth = health;
        currentHealth = health;
        spriteRenderer.sprite = sprite;
        UpdateHealthText();
    }

    private void UpdateHealthText()
    {
        healthText.text = "HP: " + currentHealth.ToString();
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        UpdateHealthText();
    }
    
    public void IncreaseMaxHealth(int amount)
    {
        MaxHealth += amount;
        currentHealth += amount;
        UpdateHealthText();
    }
}
