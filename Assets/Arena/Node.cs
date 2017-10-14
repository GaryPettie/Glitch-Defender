using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour {

	[SerializeField] bool isBreakable;

	BuildManager buildManager;
	bool nodeOccupied;
	GameObject objectOnNode;
	Vector3 startPosition;
	Vector3 positionOffset;
	float yOffset;

	void Start () {  
		buildManager = BuildManager.instance;
		NodeOffsetSetup();
	}

	void OnMouseEnter () {
		if(EventSystem.current.IsPointerOverGameObject() == false) {
			if(!nodeOccupied && buildManager.IsTurretSelected() && buildManager.HasSufficientFundsToBuild()) {
				transform.position = startPosition + positionOffset;
			}
		}
	}

	void OnMouseExit () {
		transform.position = startPosition;
	}

	void OnMouseDown() {
		if(EventSystem.current.IsPointerOverGameObject() == false) {
			if (!nodeOccupied && buildManager != null) {
				if (buildManager.IsTurretSelected() && buildManager.HasSufficientFundsToBuild()) {
					buildManager.BuildTurretAt(this);
					transform.position = startPosition;
					nodeOccupied = true;
					objectOnNode = buildManager.GetSelectedTurret();
				}
				else {
					Debug.Log("MUST BUILD ADDITIONAL PYLONS! (You're actually out of money, or haven't yet selected a turret to build)");
				}
			}
		}
	}

	void NodeOffsetSetup() {
		yOffset = transform.localScale.y / 2;
		startPosition = transform.position;
		positionOffset = new Vector3(0, yOffset, 0);
	}

	public Vector3 GetStartPosition () {
		return startPosition;
	}
}
