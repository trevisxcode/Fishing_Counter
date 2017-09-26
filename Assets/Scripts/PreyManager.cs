﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreyManager : MonoBehaviour {

	public GameObject player;
	public GameObject fish;
	private float ySpeed;
	private float timeDelay;
	private bool isReeling;
	private bool isReelingUp;
	private bool isFirstTriggered;
	private Vector2 pointStart;
	private Vector2 pointEnd;
	private Vector3 target;
	private bool fishAttached;

	// Use this for initialization
	void Start () {
		ySpeed = 0f;
		timeDelay = 1.0f;
		GetComponent<Renderer> ().enabled = false;
		isReeling = false;
		isFirstTriggered = true;
		isReelingUp = false;
		target = new Vector3 ();
		fishAttached = false;
	}
	
	// Update is called once per frame
	void Update () {
		// wait for animation "player-cast"
		if (IsDelayed() || !isReeling)
			return;

		// set active
		GetComponent<Renderer> ().enabled = true;

		target = transform.position;
		target.x = player.transform.position.x;
		target.x = GetXDistFromPlayer (target.x);

		// start moving
		MovePrey ();

		if (!fishAttached)
			CheckCollideFish ();
	}

	private void MovePrey() {
		MoveX ();
		MoveY ();

		transform.position = GetPositionAtLayer (-8.0f);
	}

	private void MoveX() {
		// travel-x 
		if (isFirstTriggered) {
			transform.position = target;
			isFirstTriggered = false;
		}

		float xDistance = target.x - transform.position.x;
		float xSpeed = Mathf.Abs (xDistance) * 0.5f;
		if (xDistance > 0.2)
			transform.position += Vector3.right * xSpeed * Time.deltaTime;
		else if (xDistance < -0.8)
			transform.position += Vector3.left * xSpeed * Time.deltaTime;

		pointStart.x = target.x;
		pointEnd.x = transform.position.x;
	}

	private void MoveY() {
		// travel-y
		if (isReelingUp) {
			float yDistance = player.transform.position.y - transform.position.y;
			float ySpeed = Mathf.Abs (yDistance) * 5f;
			transform.position += Vector3.up * ySpeed * Time.deltaTime;
			if (transform.position.y > 2.8) {
				// finishing up fish[i]
				Start ();
				fish.GetComponent<FishManager> ().Reeled ();
			}
		} else if (transform.position.y > -4.5) {
			transform.position += Vector3.down * ySpeed * Time.deltaTime;
		}

		pointStart.y = player.transform.position.y + 1;
		pointEnd.y = transform.position.y + 0.5f;
	}

	public void Reel() {
		timeDelay = 1.0f;
		ySpeed = 1.0f;
		isReeling = true;
	}

	public void ReelUp() { isReelingUp = true; }
	public void StopReel() { ySpeed = 0f; }

	private float GetXDistFromPlayer(float x) {
		PlayerManager playerManager = player.GetComponent<PlayerManager> ();
		bool facingRight = playerManager.GetFacingRight ();
		return (float) (facingRight ? (x + 1.5f) : (x - 1.8f));
	}

	private bool IsDelayed() {
		timeDelay -= Time.deltaTime;
		return (timeDelay > 0);
	}

	public Vector2[] GetPoints() {
		Vector2[] points = { pointStart, pointEnd };
		return points;
	}

	private void CheckCollideFish() {
		float r = 0.6f;
		float x = transform.position.x;
		float y = transform.position.y;
		float fX = fish.transform.position.x;
		float fY = fish.transform.position.y;
		if ((x > fX - r && x < fX + r) && (y > fY - r && y < fY + r)) {
			fish.GetComponent<FishManager> ().SetGotHooked(true);
		}
	}

	private Vector3 GetPositionAtLayer(float z) {
		return (new Vector3 (transform.position.x, transform.position.y, z));
	}
}
