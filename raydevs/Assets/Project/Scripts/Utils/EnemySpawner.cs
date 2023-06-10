using System.Collections;
using UnityEngine;

namespace Raydevs.Utils
{

    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private bool _spawnOnStart;
        public GameObject objectToSpawn; // The GameObject to spawn.
        public Vector3 spawnBounds = new Vector3(10, 10, 10); // The dimensions of the spawning area.
        public float spawnInterval = 1f; // The interval between spawns.

        private void Start()
        {
            // Start the spawn coroutine.
            if (_spawnOnStart)
                StartCoroutine(SpawnCoroutine());
        }

        public void SpawnEnemy()
        {
                Vector3 spawnPosition = new Vector3(
                    Random.Range(-spawnBounds.x / 2, spawnBounds.x / 2), transform.position.y, transform.position.z);

                // Add the spawn position to the position of the Spawner.
                spawnPosition += transform.position;

                // Instantiate the GameObject at the random position and with no rotation.
                Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);

        }

        private IEnumerator SpawnCoroutine()
        {
            // Infinite loop.
            for (int i = 0; i < 25; i++)
            {
                SpawnEnemy();

                // Wait for the specified interval before the next spawn.
                yield return new WaitForSeconds(spawnInterval);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position - new Vector3(spawnBounds.x, 0, 0),
                transform.position + new Vector3(spawnBounds.x, 0, 0));
        }
    }
}