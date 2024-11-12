using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class GhostSpawner : MonoBehaviour
{
    public GameObject ghostPrefab; // Préfabriqué du fantôme
    public int maxGhosts = 3; // Nombre maximum de fantômes
    public float spawnInterval = 5f; // Intervalle de temps entre chaque apparition
    public float spawnRadius = 2f; // Rayon du cercle d'apparition autour du joueur

    private List<GameObject> activeGhosts = new List<GameObject>(); // Liste des fantômes actifs

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

                // Nettoyer la liste des fantômes actifs pour supprimer les entrées nulles
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
