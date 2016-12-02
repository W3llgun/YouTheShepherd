using UnityEngine;
using System.Collections;

public class Portal2 : MonoBehaviour {

    public GameObject portal1;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Animal" && other.gameObject.GetComponent<Animal>().dispoTP == true)
        {
            portal1 = GameObject.Find("Portal1");
            other.gameObject.GetComponent<Animal>().dispoTP = false;
            other.gameObject.GetComponent<Animal>().WaitCDTP();
            other.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
            Vector3 temps = portal1.transform.position;
            temps.y = 0.3f;
            other.gameObject.transform.position = temps;

            other.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
            other.gameObject.GetComponent<Animal>().recalculateDestination();
            GameObject.FindGameObjectWithTag("soundmanager").GetComponent<soundManagerScript>().teleport.Play();

        }
    }
}
