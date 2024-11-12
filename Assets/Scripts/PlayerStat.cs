using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public int currentXP = 0;
    public int currentLevel = 1;
    public int xpToNextLevel = 100;
    public int coins = 0; // Nombre de pièces du joueur
    public Slider xpSlider; // Barre de progression pour l'XP
    public TextMeshProUGUI levelText; // Texte pour afficher le niveau
    public TextMeshProUGUI coinText; // Texte pour afficher le nombre de pièces

    void Start()
    {
        UpdateXPSlider();
        UpdateLevelText(); // Mettre à jour le texte du niveau au démarrage
        UpdateCoinText(); // Mettre à jour le texte des pièces au démarrage
    }

    public void AddXP(int amount)
    {
        currentXP += amount;
        Debug.Log("XP actuel : " + currentXP);
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
        UpdateLevelText(); // Mettre à jour le texte du niveau lors de la montée de niveau
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateCoinText();
    }

    void UpdateXPSlider()
    {
        if (xpSlider != null)
        {
            xpSlider.value = (float)currentXP / xpToNextLevel;
            Debug.Log("XP Slider mis à jour : " + xpSlider.value);
        }
    }

    void UpdateLevelText()
    {
        if (levelText != null)
        {
            levelText.text = "Level " + currentLevel;
        }
    }

    void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = "Coins: " + coins;
        }
    }
}
