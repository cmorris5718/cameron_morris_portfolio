using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour
{
    [SerializeField] float spawnRange;
	[SerializeField] GameObject enemyPrefab;

	int currentSpawnQuantity = 5;
	float currentSpawnDelay = 20f;

	void Start()
	{
		StartCoroutine(spawnEnemies(currentSpawnQuantity,currentSpawnDelay));
	}

	IEnumerator spawnEnemies(int spawnQuantity, float spawnDelay)
	{
		for(; ; )
		{
			Debug.Log("Spawning Enemies");
			for(int i = 0;i < spawnQuantity; i++)
			{
				Vector3 spawnLocation = new Vector3(Random.Range(-spawnRange, spawnRange), 0, Random.Range(-spawnRange, spawnRange));
				spawnLocation = transform.position - spawnLocation;
				Instantiate(enemyPrefab,spawnLocation,Quaternion.identity);
			}
			yield return new WaitForSecondsRealtime(currentSpawnDelay);
		}
	}

	private void OnDrawGizmos()
	{
        Gizmos.DrawWireSphere(transform.position, spawnRange);
	}
}
