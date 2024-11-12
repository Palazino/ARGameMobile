using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioSource audioSource; // Référence à l'AudioSource
    public string sceneToLoad = "03_ARStage_1"; // Nom de la scène à charger
    public float delay = 0.5f; // Délai avant de changer de scène

    public void StartGame()
    {
        StartCoroutine(PlaySoundAndLoadScene());
    }

    private IEnumerator PlaySoundAndLoadScene()
    {
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length + delay); // Attendre la fin du son et le délai
        SceneManager.LoadScene(sceneToLoad);
    }
}
