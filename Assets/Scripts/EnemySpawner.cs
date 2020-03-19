using UnityEngine;

namespace BarkyBoys.Game
{
	public class EnemySpawner : MonoBehaviour	//This class spawns the enemy objects
	{
		[SerializeField]
		private float baseSpawnFrequency;	//How often we spawn the enemies

		[SerializeField]
		private GameObject[] enemyPrefabs;	//An array of the possible enemy types

		[SerializeField]
		private BoxCollider spawnArea;	//We use a box collider to define our spawn area

		private void Start()
		{
			InvokeRepeating("SpawnEnemy", baseSpawnFrequency, baseSpawnFrequency);	//We start spawning enemies on the set interval. TODO: Make the enemies spawn more and more often instead
		}

		void SpawnEnemy()
		{
			Vector3 spawnPosition = transform.position;		//We initialize the spawn position as the center of the collider
			spawnPosition.y = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);	//Then we set the spawn location's Y coordinate randomly between the bottom and top of the box collider
			Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], spawnPosition, Quaternion.identity);	//Finally, we spawn the enemy object
		}
	}
}
