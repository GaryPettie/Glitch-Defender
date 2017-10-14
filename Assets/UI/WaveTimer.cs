using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveTimer : MonoBehaviour {

	EnemySpawner enemySpawner;
	Slider slider;

	// Use this for initialization
	void Start () {
		enemySpawner = FindObjectOfType<EnemySpawner>();
		slider = GetComponent<Slider>();
		slider.maxValue = enemySpawner.GetTimeBetweenWaves();
	}

	void Update () {
		slider.value = enemySpawner.GetCountdownTime();
	}
}
