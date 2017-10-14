using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {

	//Singleton
	public static PlayerManager instance;
	void Awake () {
		if (instance == null) {
			instance = this;
		}
		else {
			Debug.LogWarning("Duplicate PlayerManager destroyed on " + gameObject.name);
			Destroy(gameObject.GetComponent<PlayerManager>());
		}
	}


	[Header("Setup")]
	[SerializeField] int startingFunds;
    [SerializeField] int StartingLives;

    [Header("UI")]
    [SerializeField] Text fundsText;
    [SerializeField] Text livesText;

	private static int currentFunds;
	private static int currentLives;

	public static int GetCurrentFunds () {
		return currentFunds;
	}

	public static void AddFunds (int anAmount) {
		currentFunds += anAmount;
		instance.UpdateFundsText();
	}

	public static void RemoveFunds (int anAmount) {
		currentFunds -= anAmount;
		instance.UpdateFundsText();
	}

	void UpdateFundsText () {
		fundsText.text = "\u0243 " + string.Format("{0:0,0}", currentFunds);
	}

    public static int GetLives () {
		return currentLives;
    }

	public static void AddLives (int anAmount) {
		currentLives += anAmount;
		instance.UpdateLivesText();
	}

	public static void RemoveLives (int anAmount) {
		if (currentLives >= anAmount) {
			currentLives -= anAmount;
		}
		else {
			currentLives = 0;
		}
		instance.UpdateLivesText();
	}

	public static bool HasLives () {
		return currentLives > 0;
	}

	void UpdateLivesText () {
		livesText.text = currentLives.ToString();
	}

	void Start () {
		currentFunds = startingFunds;
		currentLives = StartingLives;
		UpdateFundsText();
		UpdateLivesText();
	}
}
