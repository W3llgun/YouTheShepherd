using UnityEngine;
using System.Collections;

public class Portal1 : MonoBehaviour {

    public GameObject portal2;

    void OnTriggerEnter(Collider other)
    {
        portal2 = GameObject.Find("Portal2");
        if (other.gameObject.tag == "Animal" && other.gameObject.GetComponent<Animal>().dispoTP == true)
        {
            other.gameObject.GetComponent<Animal>().dispoTP = false;
            other.gameObject.GetComponent<Animal>().WaitCDTP();
            other.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            Vector3 temps = portal2.transform.position;
            temps.y = 0.3f;
            other.gameObject.transform.position = temps;

            other.gameObject.GetComponent<NavMeshAgent>().enabled = true;
            other.gameObject.GetComponent<Animal>().recalculateDestination();
            GameObject.FindGameObjectWithTag("soundmanager").GetComponent<soundManagerScript>().teleport.Play();
        }
    }
}
