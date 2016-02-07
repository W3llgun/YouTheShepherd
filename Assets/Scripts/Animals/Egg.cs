using UnityEngine;
using System.Collections;

public class Egg : MonoBehaviour {

	public GameObject other; 

	// Use this for initialization
	void Start () {
		Invoke ("spawn", 10f);
	}

	void spawn() {
		other.SetActive (true);
        other.gameObject.GetComponent<Animal>().StartCoroutine(other.gameObject.GetComponent<Animal>().newAnimalOeuf());
		Destroy (gameObject);
	}

}
