using UnityEngine;
using System.Collections;

public class TotemMalusNat : MonoBehaviour {

    public GameObject meteorite;
    public GameObject particuleMalus;
    public GameObject particulePlace;
    public GameObject animDeath;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Animal")
        {
            Instantiate(animDeath, other.gameObject.transform.position, animDeath.transform.rotation);
            Destroy(other.gameObject);
            Instantiate(particuleMalus, particulePlace.transform.position, particuleMalus.transform.rotation);
            Vector3 position = new Vector3(Random.Range(0.0F, 20.0F), Random.Range(100.0F, 200.0F), Random.Range(0.0F, 20.0F));
            Instantiate(meteorite, position, Quaternion.identity);
        }
    }
}
