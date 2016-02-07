using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class TerrainGenerator : MonoBehaviour {

    Vector3 LEFT=Vector3.left, RIGHT=Vector3.right, UP=Vector3.forward, DOWN=Vector3.back;
    int reload = 0;
    public Vector3 cursor;
    Color cacheColor;
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            move(LEFT);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            move(RIGHT);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            move(UP);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            move(DOWN);
        }
        

        if (Input.GetKeyDown(KeyCode.D))
        {
            commandPit();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            commandWater();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            commandMountain();
        }
    }

    public void commandPit()
    {
        impact(cursor, 5);
        checkBuilding(cursor);
        GameObject.FindGameObjectWithTag("soundmanager").GetComponent<soundManagerScript>().dig.Play();

    }

    public void commandWater()
    {
        transformWater(cursor);
        cleanNavMesh();
        GameObject.FindGameObjectWithTag("soundmanager").GetComponent<soundManagerScript>().water.Play();

    }

    public void commandMountain()
    {
        mountain(cursor);
        cleanNavMesh();
        checkBuilding(cursor);

        RaycastHit[] P = Physics.SphereCastAll(new Ray(cursor, Vector3.up * 0.01f), 7f);

        foreach (RaycastHit e in P)
        {
            if (e.transform.tag == "Animal")
            {
                Vector3 newPos = cursor;
                newPos.y = 0.1f;
                e.transform.GetComponent<NavMeshAgent>().SetDestination(newPos);
                break;
            }
        }
        GameObject.FindGameObjectWithTag("soundmanager").GetComponent<soundManagerScript>().mountain.Play();

        
    }

    void reset()
    {
        foreach (KeyValuePair<Vector3, GameObject> obj in terrainMap)
        {
            Destroy(obj.Value);
            
        }
        terrainMap.Clear();
        Generate();
    }

    float customRound(float f)
    {
        return Mathf.Round(f * 10) / 10;
    }

    void delete(Vector3 vec)
    {
        vec.y = customRound(vec.y);
        if(vec.y>0&& !isIndestructible(vec))
        {
            
            //cursorDecolor();
            Destroy(terrainMap[vec]);
            terrainMap.Remove(vec);
            cursorTop();
        }
        
    }

    GameObject randomCase()
    {
        Vector3 vec = new Vector3(Random.Range(1, xMax), 0, Random.Range(1, zMax));
        //vec.y = getTop(vec);

        return getCaseByPosition(vec);
    }

    void transformWater(Vector3 pos)
    {
        pos.y = getTop(pos);
        Vector3 vec = pos;
        Vector3 temps = vec;
        terrainMap[vec].GetComponent<TerrainScript>().setType(Type.Eau);
        CleanProps(terrainMap[vec]);
        cacheColor = terrainMap[vec].GetComponent<Renderer>().material.color;
        for (int i =-1; i<=1;i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                temps.z = vec.z + j;
                temps.x = vec.x + i;
                if (i == 0 && j == 0)
                {
                }
                else 
                if (terrainMap.ContainsKey(temps)&& terrainMap[temps].GetComponent<TerrainScript>().getType()!= Type.Eau)
                {
                    
                    terrainMap[temps].GetComponent<TerrainScript>().setType(Type.Sable);
                    checkSable(temps);
                }
            }
        }
        

    }

    void checkSable(Vector3 pos)
    {
        Vector3 vec = pos;
        Vector3 temps = pos;
        int ile = 0;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                temps.z = vec.z + j;
                temps.x = vec.x + i;
                if (i == 0 && j == 0)
                {
                }
                else
                { 
                    if (terrainMap.ContainsKey(temps) && terrainMap[temps].GetComponent<TerrainScript>().getType() != Type.Eau)
                    {
                        ile ++;
                    }
                }
                
            }
        }
        if (ile<=2)
        {
            transformWater(pos);
        }
    }

    void create(Vector3 vec)
    {
        vec.y = customRound(vec.y);
        if(vec.y <12f)
        {
            //cursorDecolor();
            cursor.y += yScale;

            createCube(cursor);
            cursor.y = 0;
            cursorTop();
        }
       
    }

    public void move(Vector3 vec)
    {
        
        Vector3 temp = new Vector3(cursor.x + vec.x, 0, cursor.z + vec.z);
        if (terrainMap.ContainsKey(temp))
        {
            //cursorDecolor();
            cursor = temp;
            cursorTop();
        }
        
        
    }
    

    public float getTop(Vector3 vec)
    {
        Vector3 custom;
        int height = 0;
        custom = new Vector3(vec.x, height, vec.z);
        while (terrainMap.ContainsKey(custom))
        {
            vec.y = custom.y;
            height++;
            custom.y = height * yScale;
            custom.y = Mathf.Round(custom.y * 10) / 10;
        }
        
        return vec.y;
    }

    public void cursorTop()
    {
        Vector3 custom;
        int height = 0;
        custom = new Vector3(cursor.x, height, cursor.z);
        
        while (terrainMap.ContainsKey(custom))
        {
            cursor.y = custom.y;
            height++;
            custom.y = height*yScale;
            custom.y = Mathf.Round(custom.y * 10) / 10;
        }
        cursorColoring();
    }

    void cursorColoring()
    {

        //cacheColor = terrainMap[cursor].GetComponent<Renderer>().material.color;
        cursorSprite.transform.position = new Vector3(cursor.x, cursor.y+yScale+0.01f, cursor.z);

    }

    //void cursorDecolor()
    //{
    //    terrainMap[cursor].GetComponentInChildren<Renderer>().material.color = cacheColor;
    //}
}
