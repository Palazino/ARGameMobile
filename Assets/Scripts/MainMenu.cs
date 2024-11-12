using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public AudioSource audioSource; // Référence à l'AudioSource
    public Button startButton; // Référence au bouton Start
    public string sceneToLoad = "03_ARStage_1"; // Nom de la scène à charger
    public float delay = 1f; // Délai avant de changer de scène, réduit à 0.5 seconde

    void Start()
    {
        startButton.interactable = true; // Assurer que le bouton est actif au départ
    }

    public void StartGame()
    {
        startButton.interactable = false; // Désactiver le bouton Start pour éviter les doubles clics
        StartCoroutine(PlaySoundAndLoadScene());
    }

    private IEnumerator PlaySoundAndLoadScene()
    {
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length - (audioSource.clip.length - delay)); // Attendre la fin du son et le délai
        SceneManager.LoadScene(sceneToLoad);
    }
}
