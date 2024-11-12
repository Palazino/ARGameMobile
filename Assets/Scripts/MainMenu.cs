using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioSource audioSource; // R�f�rence � l'AudioSource
    public string sceneToLoad = "03_ARStage_1"; // Nom de la sc�ne � charger
    public float delay = 0.5f; // D�lai avant de changer de sc�ne

    public void StartGame()
    {
        StartCoroutine(PlaySoundAndLoadScene());
    }

    private IEnumerator PlaySoundAndLoadScene()
    {
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length + delay); // Attendre la fin du son et le d�lai
        SceneManager.LoadScene(sceneToLoad);
    }
}
