using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public AudioSource audioSource; // R�f�rence � l'AudioSource
    public Button startButton; // R�f�rence au bouton Start
    public string sceneToLoad = "03_ARStage_1"; // Nom de la sc�ne � charger
    public float delay = 1f; // D�lai avant de changer de sc�ne, r�duit � 0.5 seconde

    void Start()
    {
        startButton.interactable = true; // Assurer que le bouton est actif au d�part
    }

    public void StartGame()
    {
        startButton.interactable = false; // D�sactiver le bouton Start pour �viter les doubles clics
        StartCoroutine(PlaySoundAndLoadScene());
    }

    private IEnumerator PlaySoundAndLoadScene()
    {
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length - (audioSource.clip.length - delay)); // Attendre la fin du son et le d�lai
        SceneManager.LoadScene(sceneToLoad);
    }
}
