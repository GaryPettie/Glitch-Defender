using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

	[Header("Setup")]
	[SerializeField] GameObject projectileType;
	[SerializeField] GameObject turretHead;
	[SerializeField] GameObject firePoint;

	[Header("Properties")]
	[SerializeField] TurretType turretType;
	[SerializeField] int[] buildCost;
	[SerializeField] SelectionState selectionMethod;
	[SerializeField] float range;
	[SerializeField] float turnSpeed;
	[SerializeField] float attackRate;
	[SerializeField] float projectileSpeed;
	[SerializeField] float projectileDamage;
	[SerializeField] float damageRadius;

	enum SelectionState {FIRST, LAST, CLOSEST, WEAKEST, STRONGEST};
	enum TurretType {BULLET, LASER, MISSILE, FREEZE, POISON}

	Enemy[] targets;
	GameObject currentTarget;
	Vector3 turretRotationOffset = new Vector3(90, 0, 0);

	float distanceToEnemy;
	float closestDistance;
	float targetHitpoints;
	float attackCountdown;

	GameObject projectileContainer;


	public int GetBuildCost(int costIndex) {
		return buildCost[costIndex];
	}


	void Start () {
		projectileContainer = GameObject.Find("ProjectileContainer");
		attackCountdown = attackRate;
	}
	
	void Update () {
		//Forget Targets out of range.
		if (currentTarget != null && Vector3.Distance(currentTarget.transform.position, transform.position) > range) {
			currentTarget = null;
		}

		//Find new target
		LocateTarget();

		//Attack the current target
		if (currentTarget != null) {
			AttackTarget(currentTarget);
		}
	}

	void LocateTarget () {
		closestDistance = Mathf.Infinity;
		targetHitpoints = Mathf.Infinity;

		targets = FindObjectsOfType<Enemy>();

		if (targets != null) {
			foreach (Enemy target in targets) {
				float distance = Vector3.Distance(transform.position, target.transform.position);

				if (distance < range) {
					switch (selectionMethod) {
						case SelectionState.FIRST:
							if (currentTarget == null) {
								currentTarget = target.gameObject;
							}
							break;
						//TODO Needs to track last target in range.
						case SelectionState.LAST:
							currentTarget = target.gameObject;
							break;
					
						case SelectionState.CLOSEST:
							if (distance < closestDistance) {
								closestDistance = distance;
								currentTarget = target.gameObject;
							}
							break;
						case SelectionState.WEAKEST:
							if(target.GetHitPoints() < targetHitpoints) {
								targetHitpoints = target.GetHitPoints();
								currentTarget = target.gameObject;
							}
							break;
						case SelectionState.STRONGEST:
							if(target.GetHitPoints() > targetHitpoints) {
								targetHitpoints = target.GetHitPoints();
								currentTarget = target.gameObject;
							}
							break;
						default:
							Debug.LogError("Enemy selection method not set.");
							break;
					}
				}
			}
		}
	}

	void AttackTarget (GameObject currentTarget) {
		//Track Target
		Vector3 direction = currentTarget.transform.position - transform.position;
		Quaternion lookRotation = Quaternion.LookRotation(direction);
		Vector3 rotation = Quaternion.Lerp(turretHead.transform.rotation, lookRotation, turnSpeed * Time.deltaTime).eulerAngles;
		turretHead.transform.rotation = Quaternion.Euler (0f, rotation.y, 0f);

		//Shoot Target
		attackCountdown -= Time.deltaTime;

		if (attackCountdown <= 0) { 
			switch (turretType) {
				case TurretType.BULLET:
					GameObject projectile = (GameObject)Instantiate(projectileType, firePoint.transform.position, firePoint.transform.rotation, projectileContainer.transform);
					projectile.GetComponent<Projectile>().SeekTarget(currentTarget, projectileSpeed, projectileDamage, damageRadius);
					break;
				case TurretType.LASER:
					break;
				case TurretType.MISSILE:
					break;
				case TurretType.FREEZE:
					break;
				case TurretType.POISON:
					break;
				default:
					Debug.LogError("Turret type not selected.");
					break;
			}
			attackCountdown = attackRate;
		}
	}

	void OnDrawGizmos () {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, range);
	}
}
