using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public int currentXP = 0;
    public int currentLevel = 1;
    public int xpToNextLevel = 100;
    public Slider xpSlider; // Barre de progression pour l'XP
    public TextMeshProUGUI levelText; // Texte pour afficher le niveau

    void Start()
    {
        UpdateXPSlider();
        UpdateLevelText(); // Mettre � jour le texte du niveau au d�marrage
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
        currentXP = currentXP - xpToNextLevel; // Conserver l'exc�dent d'XP
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.5f); // Augmenter le seuil d'XP pour le prochain niveau
        UpdateXPSlider();
        UpdateLevelText(); // Mettre � jour le texte du niveau lors de la mont�e de niveau
    }

    void UpdateXPSlider()
    {
        xpSlider.value = (float)currentXP / xpToNextLevel;
    }

    void UpdateLevelText()
    {
        levelText.text = "Level " + currentLevel;
    }
}
