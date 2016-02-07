using UnityEngine;
using System.Collections;

public enum Type
{
    Eau,
    Terre,
    Herbe,
    Sable,
    Montagne
}

public class TerrainScript : MonoBehaviour {
    NavMeshObstacle e = null;
    Type type;
	// Use this for initialization
	void Start () {
        if (e == null)
            e = gameObject.AddComponent<NavMeshObstacle>();
        e.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public Type getType()
    {
        return type;
    }

    public void setType(Type t)
    {
        
        type = t;
        InitType();
        changeChild();
    }

    void changeChild()
    {
        if(GetComponentInChildren<TreeCreator>() != null)
        {
            GetComponentInChildren<TreeCreator>().setBiome(type);
            GetComponentInChildren<TreeCreator>().InstantiateTree();
        }
    }

    void InitType()
    {
        switch (type)
        {
            case Type.Eau:
                eau();
                break;
            case Type.Terre:
                terre();
                break;
            case Type.Herbe:
                herbe();
                break;
            case Type.Sable:
                sable();
                break;
            case Type.Montagne:
                Montagne();
                break;
            default:
                break;
        }
    }

    void Montagne()
    {
        GetComponent<Renderer>().material = (Material)Resources.Load("Montagne");
       
    }

    void terre()
    {
        
        GetComponent<Renderer>().material = (Material)Resources.Load("Terre");


    }

    void herbe()
    {
        GetComponent<Renderer>().material = (Material)Resources.Load("Plaine"); 
       
    }

    void sable()
    {
        GetComponent<Renderer>().material = (Material)Resources.Load("Plage"); 
       
    }

    void eau()
    {
        GetComponent<Renderer>().material = (Material)Resources.Load("Mer");


    }

    public IEnumerator nextframe()
    {
        yield return null;

        if (e == null)
            e = gameObject.AddComponent<NavMeshObstacle>();
        e.enabled = true;
        e.carving = true;
        e.size = new Vector3(1, 2, 1);
    }

}
