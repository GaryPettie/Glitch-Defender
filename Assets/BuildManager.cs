using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour {

	//Singleton
	public static BuildManager instance;
	void Awake () {
		if (instance == null) {
			instance = this;
		}
		else {
			Debug.LogWarning("Duplicate BuildManager destroyed on " + gameObject.name);
			Destroy(gameObject.GetComponent<BuildManager>());
		}
	}

	//TODO Break out all of the UI elements in each class and centralise it in a UIManager.
	[SerializeField] GameObject buildBoxPrefab;
	[SerializeField] GameObject[] turretPrefabs;
	[SerializeField] Text[] TurretCostTexts; 
	[SerializeField] float buildSpeed;
	string turretContainer = "TurretContainer";
	GameObject turretSelected;
	GameObject buildBox;
	GameObject builtTurret;

	void Start () {
		for (int i = 0; i < TurretCostTexts.Length; i++) {
			SetSelectedTurret(i);
			TurretCostTexts[i].text = "\u0243 " + string.Format("{0:0,0}", GetTurretCost(0));
		}
		turretSelected = null;
	}


	public void SetSelectedTurret (int turretIndex) {
		turretSelected = turretPrefabs[turretIndex];
	}

	public GameObject GetSelectedTurret() {
		return turretSelected;
	}

	public bool IsTurretSelected() {
		return turretSelected != null;
	}

	int GetTurretCost(int buildIndex) {
		return turretSelected.GetComponent<Turret>().GetBuildCost(buildIndex);
	}

	public bool HasSufficientFundsToBuild() {
		return PlayerManager.GetCurrentFunds() >= GetTurretCost(0);
	}

	public void BuildTurretAt (Node selectedNode) {
		PlayerManager.RemoveFunds(GetTurretCost(0));
		buildBox = (GameObject)Instantiate(buildBoxPrefab, selectedNode.GetStartPosition(), selectedNode.transform.rotation, GameObject.Find(turretContainer).transform);
		builtTurret = (GameObject)Instantiate(turretSelected, selectedNode.GetStartPosition(), selectedNode.transform.rotation, GameObject.Find(turretContainer).transform);
		builtTurret.GetComponent<Turret>().enabled = false;
		StartCoroutine(BuildTurret(buildBox, selectedNode, builtTurret));
	}

	/// <summary>
	/// Adds a box around the built turret, which slowly disolves based on the buildSpeed specified. 
	/// Turret is enabled after build process is complete.  
	/// </summary>
	/// <returns>The turret.</returns>
	/// <param name="box">Box.</param>
	/// <param name="selectedNode">Selected node.</param>
	/// <param name="builtTurret">Built turret.</param>
	IEnumerator BuildTurret (GameObject box, Node selectedNode, GameObject builtTurret) {
		Vector3 endPosition = new Vector3(box.transform.position.x, box.transform.position.y - 1f, box.transform.position.z);
		float speed = buildSpeed;
		while (box.transform.position.y > endPosition.y) {
			box.transform.position = Vector3.MoveTowards(box.transform.position, endPosition, speed * Time.deltaTime);
			yield return new WaitForEndOfFrame();
		}
		builtTurret.GetComponent<Turret>().enabled = true;
		Destroy(box.gameObject);
	}
}
