using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public int currentXP = 0;
    public int currentLevel = 1;
    public int xpToNextLevel = 500;
    public Slider xpSlider; // Barre de progression pour l'XP

    void Start()
    {
        UpdateXPSlider();
    }

    public void AddXP(int amount)
    {
        currentXP += amount;
        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
        UpdateXPSlider();
    }

    void LevelUp()
    {
        currentLevel++;
        currentXP = currentXP - xpToNextLevel; // Conserver l'excédent d'XP
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.5f); // Augmenter le seuil d'XP pour le prochain niveau
        UpdateXPSlider();
    }

    void UpdateXPSlider()
    {
        xpSlider.value = (float)currentXP / xpToNextLevel;
    }
}
