using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class GhostSpawner : MonoBehaviour
{
    public GameObject ghostPrefab; // Pr�fabriqu� du fant�me
    public int maxGhosts = 3; // Nombre maximum de fant�mes
    public float spawnInterval = 5f; // Intervalle de temps entre chaque apparition
    public float spawnRadius = 2f; // Rayon du cercle d'apparition autour du joueur

    private List<GameObject> activeGhosts = new List<GameObject>(); // Liste des fant�mes actifs

    private void Start()
    {
        StartCoroutine(SpawnGhosts());
    }

    IEnumerator SpawnGhosts()
    {
        while (true)
        {
            if (activeGhosts.Count < maxGhosts)
            {
                Vector2 randomPoint = Random.insideUnitCircle.normalized * spawnRadius;
                Vector3 spawnPosition = new Vector3(randomPoint.x, 0, randomPoint.y) + Camera.main.transform.position;
                GameObject newGhost = Instantiate(ghostPrefab, spawnPosition, Quaternion.identity);
                activeGhosts.Add(newGhost);

                // Nettoyer la liste des fant�mes actifs pour supprimer les entr�es nulles
                activeGhosts.RemoveAll(ghost => ghost == null);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void RemoveGhost(GameObject ghost)
    {
        activeGhosts.Remove(ghost);
    }
}
