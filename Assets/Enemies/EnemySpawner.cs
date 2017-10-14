using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	[SerializeField] GameObject enemyPrefab;
	[SerializeField] float timebetweenWaves;
	[SerializeField] float enemyInterval;

	float countdown;
	int waveIndex = 1;

	public float GetTimeBetweenWaves () {
		return timebetweenWaves;
	}

	public float GetCountdownTime () {
		return countdown;
	}
	
	void Start () {
		countdown = timebetweenWaves;
	}
	
	void Update () {
		if (countdown <= 0) {
			StartCoroutine(SpawnWave());
			countdown = timebetweenWaves;
		}

		countdown -= Time.deltaTime;
		countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);
	}

	IEnumerator SpawnWave() {
		for (int i = 0; i < waveIndex; i++) {
			GameObject enemy = (GameObject)Instantiate(enemyPrefab, Waypoints.GetFirstWaypoint().position, transform.rotation, transform);
			yield return new WaitForSeconds(enemyInterval);
		}
		waveIndex++;
	}
}
