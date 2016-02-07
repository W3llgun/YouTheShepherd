using UnityEngine;
using System.Collections;

public class Meteorite : MonoBehaviour
{

    private GameObject target;
    public GameObject explosion;
    bool
    explo = false;
    void Start()
    {
        StartCoroutine(WaitDestroy());
    }

    void Update()
    {
        // DrawRay
        Vector3 forward = transform.TransformDirection(Vector3.down) * 200;
        Debug.DrawRay(transform.position, forward, Color.red);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Animal")
        {
            Destroy(col.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            GameObject.FindGameObjectWithTag("soundmanager").GetComponent<soundManagerScript>().impact.Play();
            // CamShake
            target = GameObject.FindWithTag("MainCamera");
            target.GetComponent<CamShake>().shake = 1.5f;
            target.GetComponent<CamShake>().shakeIt = true;
            // Destroy Ground
            if (!explo)
            {
                Instantiate(explosion, this.transform.position, explosion.transform.rotation);
                explo = true;
            }
            
             TerrainGenerator.Instance.impact(collision.gameObject.transform.position, 10);
            // Destroy
            Destroy(gameObject);
        }

    }

    IEnumerator WaitDestroy()
    {    
        yield return new WaitForSeconds(10);
        Debug.Log("Destroy Meteorite Lost...");
        Destroy(gameObject);
    }
}
