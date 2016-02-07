using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class TerrainGenerator : MonoBehaviour {

    #region Singleton
    static private TerrainGenerator s_Instance;
    static public TerrainGenerator Instance
    {
        get
        {
            return s_Instance;
        }
    }
    #endregion

    static Quaternion DEFAULT= Quaternion.identity;
    
    public GameObject basicCube;
    Vector3 startPosition = new Vector3(0,0,0);
    public GameObject cursorSprite;
    public GameObject obstacle;

    [Header("Option")]
    Vector3 []undestructible;
    public int zMax;
    public int xMax;
    public int yMax;
    public int seed = 0;
    float yScale;
    float xScale;
    float zScale;
    static Dictionary<Vector3, GameObject> obstacleMap;
    static Dictionary<Vector3, GameObject> terrainMap;

    public GameObject licorne;
    public GameObject Dragon;

    public GameObject animCoeur;


    void Awake () {
        if (s_Instance == null)
            s_Instance = this;
        undestructible = new Vector3[4];
        if (seed != 0)
        {
            Random.seed = seed;
        }

        terrainMap = new Dictionary<Vector3, GameObject>();
        obstacleMap = new Dictionary<Vector3, GameObject>();
        buildingMap = new Dictionary<GameObject, Vector3>();

        yScale = Mathf.Round(basicCube.GetComponent<Renderer>().bounds.size.y * 10) / 10;
        
        xScale = basicCube.GetComponent<Renderer>().bounds.size.x;
        zScale = basicCube.GetComponent<Renderer>().bounds.size.z;
        undestructible[0] = new Vector3(xMax / 2, 0, zMax / 2);
        undestructible[1] = new Vector3(xMax / 2 + 1, 0, zMax / 2);
        undestructible[2] = new Vector3(xMax / 2, 0, zMax / 2 + 1);
        undestructible[3] = new Vector3(xMax / 2 + 1, 0, zMax / 2 + 1);
        
        Generate();
        
        
    }
	
    void Generate()
    {
        Ground();
        
        Chunck();

        cleanNavMesh();

        SpawnBuilding("Portal");
        SpawnBuilding("Altar");
        spawnAnimaux(startingNumber);

    }

    void cleanNavMesh()
    {
        foreach (KeyValuePair<Vector3, GameObject> pair in obstacleMap)
        {
            bool condition = (terrainMap[new Vector3(pair.Key.x, getTop(pair.Key), pair.Key.z)].GetComponent<TerrainScript>().getType() == Type.Eau);
            
            pair.Value.SetActive(condition);

        }
    }

    private void Chunck()
    {
        sea();
        for (int i = 0; i < zMax/5; i++)
        {
            GameObject obj = randomCase();
            while (obj.GetComponent<TerrainScript>().getType() == Type.Eau || obj.GetComponent<TerrainScript>().getType() == Type.Sable)
            {
                obj = randomCase();
            }
            Vector3 pos = obj.transform.position;
            if (Random.Range(0, 10) > 6)
            {
                pos.y = Mathf.Round(pos.y * 10) / 10;
                for (int j = -Random.Range(2, 5); j < 0; j++)
                {
                    
                    mountain(pos);
                }
                
            }
            else
            {
                mountain(pos, 7);
            }

        }
    }

    void Ground()
    {
        Vector3 personalPos;
        
        for (int k = 0; k < yMax; k++)
        {
            for (int i = 0; i <= xMax; i++)
            {

                for (int j = 0; j <= zMax; j++)
                {
                    personalPos = new Vector3(startPosition.x + i*xScale, startPosition.y + k* yScale, startPosition.z + j * zScale);
                    createCube(personalPos,true);
                    
                }
            }
        }
    }

    void sea()
    {
        int number = zMax/5;// Random.Range(30,60);
        Vector3 temps;
        for (int i = 0; i < number; i++)
        {
            
                Vector3 vec = randomCase().transform.position;
                vec.y = Mathf.Round(vec.y * 10) / 10;
            int nb1 = Random.Range(2, 6);
                int nb2 = Random.Range(2, 6);
                for (int k= -nb1; k <= nb1; k++)
                {
                    for (int j = -nb2; j <= nb2; j++)
                    {
                        temps = vec;
                        temps.z = vec.z + j;
                        temps.x = vec.x + k;
                        if (Random.Range(0, 10) >= 2 && terrainMap.ContainsKey(temps) && terrainMap[temps].GetComponent<TerrainScript>().getType() != Type.Eau)
                        {

                            transformWater(temps);
                        }
                    }
                }
            
        }
    }

    bool isIndestructible(Vector3 pos)
    {
        bool test = false;
        pos.y = 0;
        for (int i = 0; i < undestructible.Length; i++)
        {
            if (undestructible[i] == pos) test = true;
        }
        return test;
    }

    void createCube(Vector3 personalPos, bool force = false)
    {
        GameObject cache;
        personalPos.y = Mathf.Round(personalPos.y * 10) / 10;
        if (force || !isIndestructible(personalPos))
        {
            if (!terrainMap.ContainsKey(personalPos) && personalPos.x <= xMax && personalPos.z <= zMax && personalPos.x >= 0 && personalPos.z >= 0)
            {
                cache = (GameObject)Instantiate(basicCube, personalPos, DEFAULT);
                cache.transform.parent = this.transform;
                terrainMap.Add(personalPos, cache);

                Vector3 vec = new Vector3(personalPos.x, Mathf.Max(0, personalPos.y - yScale), personalPos.z);
                coloring(personalPos);
                if (terrainMap.ContainsKey(vec))
                {
                    CleanProps(terrainMap[vec]);

                }
                if (Random.Range(0, 8) < 1)
                {
                    SpawnProps(cache);
                }

                Vector3 obs = new Vector3(personalPos.x, 0, personalPos.z);
                if(!obstacleMap.ContainsKey(obs))
                {
                    cache = (GameObject)Instantiate(obstacle, obs, DEFAULT);
                    cache.transform.parent = this.transform;
                    obstacleMap.Add(obs, cache);
                }
            }
        }
        
    }

    public GameObject getCaseByPosition(Vector3 pos)
    {
        Vector3 temps = pos;
        temps.x = Mathf.Abs( Mathf.RoundToInt(pos.x));
        temps.z = Mathf.Abs(Mathf.RoundToInt(pos.z));
        temps.y = getTop(temps);
        if (terrainMap.ContainsKey(temps))
            return terrainMap[temps];
        return null;
    }

    public List<Vector3> getListCase(Vector3 pos, Type t)
    {
        List<Vector3> temps = new List<Vector3>();

        Vector3 cache = pos;

        float normalY = getTop(pos);

        for (int i = 0; i < xMax; i++)
        {
            for (int j = 0; j < zMax; j++)
            {
                Vector3 TE = new Vector3((float)i, getTop(new Vector3((float)i, 0f, (float)j)), (float)j);
                GameObject found = terrainMap[TE];

                if (found.GetComponent<TerrainScript>().getType() == t)
                    temps.Add(TE);
            }
        }

        return temps;
    }

    void changeType(Vector3 pos, Type t)
    {
        
            for (int i = 0; i < getTop(pos); i++)
            {
                pos.y = customRound(i*yScale);
                if (terrainMap[pos].GetComponent<TerrainScript>().getType() == Type.Eau && pos.y < getTop(pos))
                {
                    terrainMap[pos].GetComponent<TerrainScript>().setType(t);
                }
            }
            
            
           
            
            
            Animal[] allAnimal = GameObject.FindObjectsOfType<Animal>();

            foreach (Animal e in allAnimal)
            {
                e.recalculateDestination();
            }
        
        
    }

    void mountain(Vector3 pos, int range=0)
    {
        Vector3 vec;
        int xMount = Random.Range(2,6);
        
        int yMount = Random.Range(2, 6);
        xMount = Mathf.Max(xMount, range);
        yMount = Mathf.Max(xMount, range);
        int zMount = xMount;
        pos.y = customRound(pos.y);
        if (terrainMap.ContainsKey(pos) && getTop(pos)<12)
        {
            for (int i = -xMount; i <= xMount; i++)
            {

                for (int j = -zMount; j <= zMount; j++)
                {
                    vec = new Vector3(pos.x + i, 0, pos.z + j);
                    vec.y = getTop(vec);
                    int adapt = proche(0, i) + proche(0, j);
                    if(terrainMap.ContainsKey(vec) )
                        changeType(vec,Type.Terre);

                    //createCol(vec, ((int)(getAverageHeight(vec)+Random.Range(0,2)) - adapt ));
                    createCol(vec, ((int)yMount + Random.Range(0, 2) - adapt));
                }
            }
        }
        //cursorDecolor();
        cursorTop();
    }

    int proche(int v1, int v2)
    {
        int v = v1-v2;
        return Mathf.Abs(v);
    }

    void createCol(Vector3 vec, int height)
    {
        vec.y = customRound(vec.y);
        for (int i =0; i< height; i++)
        {
            Vector3 v = new Vector3(vec.x, vec.y + (i - 1) * yScale, vec.z);
            if (terrainMap.ContainsKey(v))
                CleanProps(terrainMap[v]);
            v = new Vector3(vec.x, vec.y + i * yScale, vec.z);
            createCube(new Vector3(vec.x, vec.y+i * yScale, vec.z));
        }
    }

    void deleteCol(Vector3 vec, int height)
    {
        vec.y = customRound(vec.y);
        for (int i = 0; i < height; i++)
        {
            delete(new Vector3(vec.x, vec.y-i * yScale, vec.z));
        }
    }

    float getAverageHeight(Vector3 vec)
    {
        int count = 0;
        float value = 0;
        for (int i = -1; i <= 1; i++)
        {

            for (int j = -1; j <= 1; j++)
            {
                if(j==0&&i==0)
                {

                }
                else
                {
                    value += getTop(new Vector3(vec.x + i, 0, vec.z + j));
                    count++;
                }
                
            }
        }

        value = value / count;
       return value;
    }

    public void impact(Vector3 pos, int range=0)
    {
        Vector3 vec;
        pos.y = customRound(pos.y);
        int xMount = Random.Range(2, 6);
        int zMount = xMount;
        int yMount = Random.Range(2, 6);
        if (range==0)
        {
            xMount = Random.Range(2, 4);
            yMount = Random.Range(1, 5);
        }
        else
        {
            xMount = range;
           yMount = Random.Range(2, 4);
        }
        zMount = xMount;
        if (terrainMap.ContainsKey(pos))
        {
            for (int i = -xMount; i <= xMount; i++)
            {

                for (int j = -zMount; j <= zMount; j++)
                {
                    vec = new Vector3(pos.x + i, 0, pos.z + j);
                    vec.y = getTop(vec);
                    int adapt = proche(0, i) + proche(0, j);


                    //deleteCol(vec, ((int)(getAverageHeight(vec)+Random.Range(0,2)) - adapt ));
                    deleteCol(vec, ((int)yMount + Random.Range(0, 2) - adapt));
                }
            }
        }
        cleanNavMesh();
    }

    void coloring(Vector3 personalPos)
    {
        GameObject obj = terrainMap[personalPos];
        float height = obj.transform.position.y;
        if (height <= 0*yScale)
        {
            obj.GetComponent<TerrainScript>().setType(Type.Terre);
        }
        else if(height <= 1 * yScale)
        {
            obj.GetComponent<TerrainScript>().setType(Type.Terre);
        }
        else if(height <= 5* yScale)
        {
            obj.GetComponent<TerrainScript>().setType(Type.Herbe);
        }
        else if(height <= 10 * yScale)
        {
            obj.GetComponent<TerrainScript>().setType(Type.Terre);
        }
        else
        {
            obj.GetComponent<TerrainScript>().setType(Type.Montagne);
        }
        Animal[] allAnimal = GameObject.FindObjectsOfType<Animal>();

        foreach (Animal e in allAnimal)
        {
            e.recalculateDestination();
        }
    }
	
}