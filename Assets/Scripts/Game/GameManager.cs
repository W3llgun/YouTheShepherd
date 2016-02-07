using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    #region Singleton
    static private GameManager s_Instance;
    static public GameManager Instance
    {
        get
        {
            return s_Instance;
        }
    }
    void Awake()
    {
        if (s_Instance == null)
            s_Instance = this;
        
    }
    #endregion

    public float timeOfEggs = 0.0f;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if (timeOfEggs > 0.0f)
            timeOfEggs -= Time.deltaTime;
    }
}
