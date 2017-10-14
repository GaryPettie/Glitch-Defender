using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	[Header("Movement")]
	[SerializeField] float panSpeed;


	void Start () {
		
	}

	void Update () {
		Move();
	}

	void Move () {
		float horizontalTranslation = Input.GetAxis("Horizontal") * panSpeed * Time.deltaTime;
		float verticalTranslation = Input.GetAxis("Vertical") * panSpeed * Time.deltaTime;

		transform.Translate(new Vector3(horizontalTranslation, 0f, verticalTranslation), Space.World);
	}
}
