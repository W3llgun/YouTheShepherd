using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class popAnimal : MonoBehaviour {

	public Vector3 offset;
	public RuntimeAnimatorController rac;
	public ParticleSystem showParticle;
	public List <AudioSource> AlpacaSounds = new List<AudioSource>();
	public List <AudioSource> BirdSounds = new List<AudioSource>();
	public List <AudioSource> HorseSounds = new List<AudioSource>();
	public List <AudioSource> ScorpionSounds = new List<AudioSource>();
	public List <AudioSource> ElephantSounds = new List<AudioSource>();
	public List <AudioSource> RhinoSounds = new List<AudioSource>();
	public List <AudioSource> TurtleSounds = new List<AudioSource>();
	public List <AudioSource> UnicornSounds = new List<AudioSource>();
	public List <AudioSource> DragonSounds = new List<AudioSource>();


	public void showAnimal(GameObject animal)
	{
		GameObject animalInstance;
		animalInstance = Instantiate (animal, transform.position, Quaternion.identity) as GameObject;
        if (animalInstance.GetComponent<Animal>() != false)
            animalInstance.GetComponent<Animal>().enabled = false;
        if (animalInstance.GetComponent<NavMeshAgent>() != false)
            animalInstance.GetComponent<NavMeshAgent>().enabled = false;
        animalInstance.transform.GetChild(0).localScale = new Vector3(1f, 1f, 1f);
        foreach (Transform e in animalInstance.transform.GetChild(0))
            e.localScale = new Vector3(1f, 1f, 1f);
        animalInstance.transform.SetParent (transform);
		animalInstance.AddComponent<rotationScript>();
		Animator anim = animalInstance.AddComponent<Animator> ();
		anim.runtimeAnimatorController = rac;
		Destroy (animalInstance, 5);
		ParticleSystem particleInstance;
		particleInstance = Instantiate (showParticle, animalInstance.transform.position + offset*0.04f, Quaternion.identity) as ParticleSystem;
		particleInstance.transform.LookAt (transform);
		Destroy (particleInstance, 3);
		if (animalInstance.transform.Find("Body/Head/horse_head") != null) {
			int i = Random.Range (0, HorseSounds.Count);
			AudioSource audio;
			audio = Instantiate (HorseSounds [i], transform.position, Quaternion.identity) as AudioSource;
			Destroy (audio, 3);
		}
		if (animalInstance.transform.Find("Body/Head/alpaca_head") != null) {
			int i = Random.Range (0, AlpacaSounds.Count);
			AudioSource audio;
			audio = Instantiate (AlpacaSounds [i], transform.position, Quaternion.identity) as AudioSource;
			Destroy (audio, 3);
		}
		if (animalInstance.transform.Find("Body/Head/bird_head") != null) {
			int i = Random.Range (0, BirdSounds.Count);
			AudioSource audio;
			audio = Instantiate (BirdSounds [i], transform.position, Quaternion.identity) as AudioSource;
			Destroy (audio, 3);
		}
		if (animalInstance.transform.Find("Body/Head/elephant_head") != null) {
			int i = Random.Range (0, ElephantSounds.Count);
			AudioSource audio;
			audio = Instantiate (ElephantSounds [i], transform.position, Quaternion.identity) as AudioSource;
			Destroy (audio, 3);
		}
		if (animalInstance.transform.Find("Body/Head/unicorn_head") != null) {
			int i = Random.Range (0, UnicornSounds.Count);
			AudioSource audio;
			audio = Instantiate (UnicornSounds [i], transform.position, Quaternion.identity) as AudioSource;
			Destroy (audio, 3);
		}
		if (animalInstance.transform.Find("Body/Head/rhino_head") != null) {
			int i = Random.Range (0, RhinoSounds.Count);
			AudioSource audio;
			audio = Instantiate (RhinoSounds [i], transform.position, Quaternion.identity) as AudioSource;
			Destroy (audio, 3);
		}
		if (animalInstance.transform.Find("Body/Head/turtle_head") != null) {
			int i = Random.Range (0, TurtleSounds.Count);
			AudioSource audio;
			audio = Instantiate (TurtleSounds [i], transform.position, Quaternion.identity) as AudioSource;
			Destroy (audio, 3);
		}
		if (animalInstance.transform.Find("Body/Head/dragon_head") != null) {
			int i = Random.Range (0, DragonSounds.Count);
			AudioSource audio;
			audio = Instantiate (DragonSounds [i], transform.position, Quaternion.identity) as AudioSource;
			Destroy (audio, 3);
		}
		if (animalInstance.transform.Find("Body/Head/scorpion_head") != null) {
			int i = Random.Range (0, ScorpionSounds.Count);
			AudioSource audio;
			audio = Instantiate (ScorpionSounds [i], transform.position, Quaternion.identity) as AudioSource;
			Destroy (audio, 3);
		}
	}
}
