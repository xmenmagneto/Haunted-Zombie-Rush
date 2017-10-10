﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Player : MonoBehaviour {

	[SerializeField] private float jumpForce = 100f;
	[SerializeField] private AudioClip sfxJump;
	[SerializeField] private AudioClip sfxDeath;
	[SerializeField] private AudioClip hitSound;

	private Animator anim;
	private Rigidbody rigidBody;
	private bool jump = false;
	private AudioSource audioSource;

	//before Start(){}
	void Awake() {
		Assert.IsNotNull (sfxJump);
		Assert.IsNotNull (sfxDeath);
		Assert.IsNotNull (hitSound);
	}

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		rigidBody = GetComponent<Rigidbody> ();
		audioSource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!GameManager.instance.GameOver () && GameManager.instance.GameStarted()) {
			if (Input.GetMouseButtonDown (0)) {
				GameManager.instance.PlayerStartedGame ();
				anim.Play ("Jump");
				audioSource.PlayOneShot (sfxJump);
				rigidBody.useGravity = true;
				jump = true;
			}
		}
	}

	void FixedUpdate() {
		if (jump) {
			jump = false;
			rigidBody.velocity = new Vector2 (0, 0);
			rigidBody.AddForce (new Vector2 (-5, jumpForce), ForceMode.Impulse);
		}
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "obstacle") {
			rigidBody.AddForce (new Vector2 (50, 20), ForceMode.Impulse);
			rigidBody.detectCollisions = false;
			audioSource.PlayOneShot (sfxDeath);
			GameManager.instance.PlayerCollided ();
		} else if (collision.gameObject.tag == "coin") {
			audioSource.PlayOneShot (hitSound);
		}
	}
}
