using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartController : MonoBehaviour {

	[SerializeField] Transform full;
	[SerializeField] Transform left;
	[SerializeField] Transform right;

	public void ShowHeart() {
		full.gameObject.SetActive(true);
		left.gameObject.SetActive(false);
		right.gameObject.SetActive(false);
		left.localPosition = Vector3.zero;
		right.localPosition = Vector3.zero;
		left.transform.rotation = Quaternion.identity;
		right.transform.rotation = Quaternion.identity;
	}

	public void HideHeart() {
		full.gameObject.SetActive(false);
		left.gameObject.SetActive(false);
		right.gameObject.SetActive(false);
	}

	public void Break() {
		full.gameObject.SetActive(false);
		left.gameObject.SetActive(true);
		right.gameObject.SetActive(true);
		StartCoroutine(LaunchRoutine(left, Quaternion.Euler(0, 0, Random.Range(-60, 30)) * Vector3.left, Random.Range(20, 28), Random.Range(630, 1080)));
		StartCoroutine(LaunchRoutine(right, Quaternion.Euler(0, 0, Random.Range(30, 60)) * Vector3.right, Random.Range(20, 28), -Random.Range(630, 1080)));
	}

	IEnumerator LaunchRoutine(Transform heart, Vector3 direction, float speed, float rotationSpeed) {
		float gravity = 30;
		Vector3 dir = direction * speed;
		while (true) {
			dir += gravity * Time.deltaTime * Vector3.down;
			heart.position += dir * Time.deltaTime;
			heart.Rotate(0, 0, rotationSpeed * Time.deltaTime);
			yield return null;
		}
	}

}
