using UnityEngine;
using System.Collections;



public class TreeCreator : MonoBehaviour {

    public Type biome;

    public GameObject[] troncPlaine;
    public GameObject[] troncHot;
    public GameObject[] troncCold;

    public GameObject[] leafPlaine;
    public GameObject[] leafHot;
    public GameObject[] leafCold;

    

    public void setBiome(Type t)
    {
        biome = t;
    }

    public void InstantiateTree()
    {
        Transform child = transform.GetChild(0);
        if(child != null)
        Destroy(child.gameObject);
        GameObject tronc;
        int i;
        Transform[] leafSpawns = new Transform[0];

        switch (biome)
        {
            case Type.Herbe:
                i = Random.Range(0, troncPlaine.Length);
                (tronc = Instantiate(troncPlaine[i], this.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0)) as GameObject).transform.parent = this.transform;
                leafSpawns = tronc.GetComponentsInChildren<Transform>();
                break;
            case Type.Sable:
                i = Random.Range(0, troncHot.Length);
                (tronc = Instantiate(troncHot[i], this.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0)) as GameObject).transform.parent = this.transform;
                leafSpawns = tronc.GetComponentsInChildren<Transform>();
                break;
            case Type.Montagne:
                i = Random.Range(0, troncCold.Length);
                (tronc = Instantiate(troncCold[i], this.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0)) as GameObject).transform.parent = this.transform;
                leafSpawns = tronc.GetComponentsInChildren<Transform>();
                break;
        }

            foreach (Transform leafSpawn in leafSpawns)
            {
                if (leafSpawn.tag == "spawnLeaf")
                {
                    GameObject leaf;
                    switch (biome)
                    {
                        case Type.Herbe:
                            i = Random.Range(0, leafPlaine.Length);
                            (leaf = Instantiate(leafPlaine[i], leafSpawn.position, Quaternion.Euler(0, Random.Range(0, 360), 0)) as GameObject).transform.parent = leafSpawn.transform;
                            break;
                        case Type.Sable:
                            i = Random.Range(0, leafHot.Length);
                            (leaf = Instantiate(leafHot[i], leafSpawn.position, Quaternion.Euler(0, Random.Range(0, 360), 0)) as GameObject).transform.parent = leafSpawn.transform;
                            break;
                        case Type.Montagne:
                            i = Random.Range(0, leafCold.Length);
                            (leaf = Instantiate(leafCold[i], leafSpawn.position, Quaternion.Euler(0, Random.Range(0, 360), 0)) as GameObject).transform.parent = leafSpawn.transform;
                            break;
                    }
                }
            }
        }
    }
