using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	[SerializeField] GameObject impactEffectPrefab;

	string projectileContainer = "ProjectileContainer";
	GameObject target;
	float speed;
	float damage;
	float damageRadius;

	void Update () {
		if (target == null) {
			DestroyProjectile();
			return;
		}

		MoveProjectile();
	}

	public void SeekTarget (GameObject aTarget, float aSpeed, float someDamage, float aDamageRadius) {
		target = aTarget;
		speed = aSpeed;
		damage = someDamage;
		damageRadius = aDamageRadius;
	}

	void MoveProjectile () {
		Vector3 direction = target.transform.position - transform.position;
		float distanceThisFrame = speed * Time.deltaTime;
		if (direction.magnitude <= distanceThisFrame) {
			HitTarget();
		}
		transform.LookAt(target.transform);
		transform.Translate(direction.normalized * distanceThisFrame, Space.World);
	}

	void HitTarget () {
		if (damageRadius == 0) {
			DamageTarget(target);
		}
		else {
			DamageArea();
		}
		DestroyProjectile();
	}

	void DestroyProjectile() {
		if (target == null) {
			//TODO some effect that differentiates a hit with a dud fire (they currently explode in the same way).
		}
		else {
			GameObject impactEffect = (GameObject)Instantiate(impactEffectPrefab, transform.position, transform.rotation, GameObject.Find(projectileContainer).transform);
			ParticleSystem particles = impactEffect.GetComponent<ParticleSystem>();
			Destroy(impactEffect, particles.startLifetime);
		}
		Destroy(gameObject);
	}

	void DamageTarget(GameObject aTarget) {
		Enemy enemyTarget = aTarget.GetComponent<Enemy>();
		if (enemyTarget != null) {
			enemyTarget.TakeDamage(damage);
		}
	}

	void DamageArea() {
		int enemyLayer = 1 << 8;
		Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius, enemyLayer);
		foreach (Collider col in colliders) {
			DamageTarget(col.gameObject);
		}
	}

	//Shows the projectile radius
	void OnDrawGizmos () {
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, damageRadius);
	}
}
