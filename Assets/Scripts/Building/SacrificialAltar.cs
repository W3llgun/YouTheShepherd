using UnityEngine;
using System.Collections;

public class SacrificialAltar : MonoBehaviour
{
    public int sacrificeNeeded = 2;
    int actual;
    public static float sacrificedAnimals;
    public GameObject animDeath;

    void Start()
    {
        actual = 0;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Animal")
        {
            GameObject.FindGameObjectWithTag("soundmanager").GetComponent<soundManagerScript>().sarifice.Play();
            Destroy(collider.gameObject);
            Instantiate(animDeath, collider.gameObject.transform.position, animDeath.transform.rotation);
            sacrificedAnimals++;
            actual++;
            if(actual>=sacrificeNeeded)
            {
                actual = 0;
                TerrainGenerator.Instance.spawnAnimaux(1);

                // UI FEEDBACK INVOC
            }
        }
    }
}