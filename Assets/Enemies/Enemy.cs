using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	[SerializeField] float hitPoints;
	[SerializeField] int rewardAmount;
	[SerializeField] float moveSpeed;
	[SerializeField] float targetAccuracy;
	[SerializeField] GameObject deathEffectPrefab;
	Transform moveTarget;
	int currentTargetIndex = -1;
	int counter = 0;

	void Start () {
		moveTarget = transform;
	}

	void Update () {
		MoveToTarget();
	}

	public float GetHitPoints() {
		return hitPoints;
	}

	public void TakeDamage (float anAmount) {
		hitPoints -= anAmount;

		if (hitPoints <= 0) {
			Die();
		}
	}

	void Die () {
		GameObject deathEffect = (GameObject)Instantiate(deathEffectPrefab,transform.position, transform.rotation, transform.parent);
		PlayerManager.AddFunds(rewardAmount);
		ParticleSystem particles = deathEffectPrefab.GetComponent<ParticleSystem>();
		Destroy(deathEffect, particles.startLifetime);
		Destroy(gameObject);
	}

	void MoveToTarget () {
		if (Vector3.Distance(transform.position, moveTarget.position) >= targetAccuracy) {
			Vector3 nomalizedDiection = (moveTarget.position - transform.position).normalized;
			transform.Translate(nomalizedDiection * moveSpeed * Time.deltaTime);
		}
		else {
			FindNextTarget ();
		}
		Debug.Log(moveTarget.name);
	}

	void FindNextTarget () {;
		//FIXME This incremental counter breaks when waypoints are removed from the list. 
		//need to come up with a way of this staying in sync with the main list. 
		//Since an enemy may already be on the path to a node being removed, it should only affect new units, 
		//so will probably have to change the architecture to pass in a complete path when the enemy is created. 
		currentTargetIndex++;
		Debug.Log(currentTargetIndex);
		if (Waypoints.IsLastWaypoint(currentTargetIndex + 1) == true) {
			EndPoint();
		}
		else {
			moveTarget = Waypoints.GetNextWaypoint(currentTargetIndex);
		}
	}

	void EndPoint () {
		PlayerManager.RemoveLives(1);
		Destroy(gameObject);
	}
}
