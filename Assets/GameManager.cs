using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	//Singleton
	public static GameManager instance;
	void Awake () {
		if (instance == null) {
			instance = this;
		}
		else {
			Debug.LogWarning("Duplicate GameManager destroyed on " + gameObject.name);
			Destroy(gameObject.GetComponent<GameManager>());
		}
	}

	bool hasEnded = false;

	void Update () {
		if (!hasEnded && PlayerManager.HasLives() == false) {
			EndGame();
		}	
	}

	void EndGame () {
		Debug.Log("You dead son!");
		hasEnded = true;
	}
}
