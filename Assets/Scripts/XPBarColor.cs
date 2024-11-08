using UnityEngine;
using UnityEngine.UI;

public class XPBarColor : MonoBehaviour
{
    public Slider xpSlider; // Le slider de l'XP
    public Image fillImage; // L'image de remplissage du slider
    public Color startColor = Color.red; // Couleur au début (0% XP)
    public Color endColor = Color.green; // Couleur à la fin (100% XP)

    void Update()
    {
        // Changer la couleur en fonction de la valeur actuelle de l'XP
        fillImage.color = Color.Lerp(startColor, endColor, xpSlider.value);
    }
}
