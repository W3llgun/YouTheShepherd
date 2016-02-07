using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class TerrainGenerator : MonoBehaviour {
    [Header("Decors")]
    public GameObject[] Props;

    [Header("Batiment")]
    public GameObject Altar;
    public GameObject Portal1;
    public GameObject Portal2;
    public GameObject Totem;
    static Dictionary<GameObject, Vector3> buildingMap;

    [Header("Animaux")]
    public GameObject[] animaux;
    public int startingNumber = 3;

    public void SpawnProps(GameObject target)
    {
        GameObject temp;
        if (Props.Length > 0)
        {
            int random = Random.Range(0, Props.Length);

            temp = (GameObject)Instantiate(Props[random], new Vector3(target.transform.position.x, target.transform.position.y + yScale, target.transform.position.z), DEFAULT);
            temp.AddComponent<PropsScript>();
            if (temp.GetComponent<TreeCreator>() != null)
            {
                temp.GetComponent<TreeCreator>().setBiome(target.GetComponent<TerrainScript>().getType());
                temp.GetComponent<TreeCreator>().InstantiateTree();
            }
            else
            {
                switch (target.GetComponent<TerrainScript>().getType())
                {
                    case Type.Herbe:
                        temp.GetComponentInChildren<MeshRenderer>().material = (Material)Resources.Load("Plaine");
                        break;
                    case Type.Terre:
                        temp.GetComponentInChildren<MeshRenderer>().material = (Material)Resources.Load("Plaine");
                        break;
                    case Type.Montagne:
                        temp.GetComponentInChildren<MeshRenderer>().material = (Material)Resources.Load("Montagne");
                        break;
                    case Type.Sable:
                        temp.GetComponentInChildren<MeshRenderer>().material = (Material)Resources.Load("Plage");
                        break;
                    default:
                        break;
                }
            }



            temp.transform.parent = target.transform;


        }


    }

    void cleanColProps(Vector3 vec)
    {

    }

    public void CleanProps(GameObject target)
    {
        PropsScript[] props = target.GetComponentsInChildren<PropsScript>();
        foreach (PropsScript item in props)
        {
            Destroy(item.gameObject);
        }
    }

    public void SpawnBuilding(string name)
    {
        switch (name)
        {
            case "Altar":
                altar();
                break;
            case "Portal":
                portal();
                break;
            case "Totem":
                totem();
                break;
            default:
                break;
        }
    }

   public  void spawnAnimaux(int nombre)
    {
        GameObject[] currentSpawned = new GameObject[nombre];
        
        for (int i = 0; i < nombre; i++)
        {
            GameObject temp;
            GameObject rand = randomCase();
            int animalRand= Mathf.FloorToInt(Random.Range(0, animaux.Length));
            if (currentSpawned.Length<nombre&& currentSpawned.Length != animaux.Length)
            {
                bool test = true;
                while(test)
                {
                    test = false;
                    animalRand = Mathf.FloorToInt(Random.Range(0, animaux.Length));
                    for (int j = 0; j < currentSpawned.Length; j++)
                    {
                        if(animaux[animalRand] == currentSpawned[j])
                        {
                            test = true;
                            break;
                        }
                        
                    }
                    
                }
                currentSpawned[currentSpawned.Length] = animaux[animalRand];
            }
            while (rand.GetComponent<TerrainScript>().getType() == Type.Eau)
            {
                rand = randomCase();
            }
            Vector3 v = rand.transform.position;
            v.y =0.1f;
            temp = (GameObject)Instantiate(animaux[animalRand], v, DEFAULT);
            //temp.transform.parent = this.transform;
            temp.SetActive(true);
        }
    }

    void checkBuilding(Vector3 pos)
    {
        
        foreach (KeyValuePair<GameObject,Vector3> item in buildingMap)
        {
            pos.y = item.Value.y;
            if(pos == item.Value)
            {
                item.Key.transform.position = new Vector3(pos.x, getTop(pos),pos.z);
            }
        }
    }

    void altar()
    {
        GameObject temp;
        GameObject rand = randomCase();
        while(rand.GetComponent<TerrainScript>().getType()==Type.Eau)
        {
            rand = randomCase();
        }
        Vector3 v = rand.transform.position;
        v.y = rand.transform.position.y + 3*yScale;
        temp = (GameObject)Instantiate(Altar,v , DEFAULT);
        buildingMap.Add(temp,v);
    }

    void portal()
    {
        GameObject temp;
        GameObject rand = randomCase();
        while (rand.GetComponent<TerrainScript>().getType() == Type.Eau || rand.GetComponent<TerrainScript>().getType() == Type.Sable)
        {
            rand = randomCase();
        }
        Vector3 v = rand.transform.position;
        v.y = rand.transform.position.y + 3 * yScale;
        temp = (GameObject)Instantiate(Portal1, v, DEFAULT);
        temp.name = "Portal1";
        buildingMap.Add(temp, v);
        rand = randomCase();
        while (rand.GetComponent<TerrainScript>().getType() == Type.Eau || rand.GetComponent<TerrainScript>().getType() == Type.Sable)
        {
            rand = randomCase();
        }
        v = rand.transform.position;
        v.y = rand.transform.position.y + 3 * yScale;
        temp = (GameObject)Instantiate(Portal2, v, DEFAULT);
        temp.name = "Portal2";
        buildingMap.Add(temp, v);
    }

    void totem()
    {
        GameObject temp;
        GameObject rand = randomCase();
        while (rand.GetComponent<TerrainScript>().getType() == Type.Eau)
        {
            rand = randomCase();
        }
        Vector3 v = rand.transform.position;
        temp = (GameObject)Instantiate(Totem, v, DEFAULT);
        buildingMap.Add(temp, v);
    }
}
