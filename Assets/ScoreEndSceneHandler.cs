﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreEndSceneHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Text> ().text = "" + ScoreValue.score;
	}
}
