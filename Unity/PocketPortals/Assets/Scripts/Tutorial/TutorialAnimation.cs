using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAnimation : MonoBehaviour {

	[SerializeField]
	ParticleSystem particles;

	float timePlaying = 0f;
	float particleLifeTime;

	void Update() {
		if (particles.isPlaying) {
			timePlaying += Time.deltaTime;
			if (timePlaying > particleLifeTime) {
				particles.Stop ();
				timePlaying = 0f;
			}
		}
	}

	public void ToggleParticles() {
		particleLifeTime = particles.main.duration;
		particles.Play ();
	}

}
