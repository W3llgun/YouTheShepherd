using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class InputManager : MonoBehaviour {
    Regex rgx;
    Vector3 LEFT = Vector3.left, RIGHT = Vector3.right, UP = Vector3.forward, DOWN = Vector3.back;
    int altar, totem;
    float timeDelayVote = 250;
    float timeVote = 30;
    float timeStart = 0f;
    bool voteOpen;
    public GameObject aText;
    public GameObject tText;
    public IRC twitch;

    #region Singleton
    static private InputManager s_Instance;
    static public InputManager Instance
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

    public GameObject altarIm, totemIm;

    void Start () {
        timeStart = 0f;
        resetVote();
        rgx = new Regex("[^a-zA-Z0-9 -]");
        if (twitch == null)
            twitch = gameObject.GetComponent<IRC>();
    }

    void resetVote()
    {
        voteOpen = false;
        altar = 0;
        totem = 0;
        timeStart = 0;
        altarIm.SetActive(false);
        totemIm.SetActive(false);
        aText.SetActive(false);
        tText.SetActive(false);
    }

    public void OpenVote()
    {
        voteOpen = true;
        
    }

    IEnumerator voteEnCour()
    {
        yield return new WaitForSeconds(timeVote);
        if (altar >= totem)
        {
            TerrainGenerator.Instance.SpawnBuilding("Altar");
            
        }
        else if(totem > altar)
        {
            TerrainGenerator.Instance.SpawnBuilding("Totem");
        }
        resetVote();
    }

    public void command(string user, string com)
    {
        
        com = rgx.Replace(com, "");
        com = com.ToLower();
        switch (com)
        {
            case "up":
            case "u":
                
                TerrainGenerator.Instance.move(UP);
                break;
            case "down":
            case "d":
                TerrainGenerator.Instance.move(DOWN);
                break;
            case "left":
            case "l":
                TerrainGenerator.Instance.move(LEFT);
                break;
            case "right":
            case "r":
                TerrainGenerator.Instance.move(RIGHT);
                break;
            case "elevate":
            case "e":
                TerrainGenerator.Instance.commandMountain();
                break;
            case "pit":
            case "p":
                TerrainGenerator.Instance.commandPit();
                break;
            case "water":
            case "w":
                TerrainGenerator.Instance.commandWater();
                break;
            case "totem":
            case "t":
                voteTotem();
                break;
            case "altar":
            case "a":
                voteAltar();
                break;
            case "ritual":
                ObjectifsManager.Instance.invokeRitual();
                break;
            default:
                //Debug.Log(com);
                if(user == "youtheshepherd" && com=="restart")
                {
                    ObjectifsManager.Instance.restart();
                    if (twitch != null)
                        twitch.say("GAME RESTARTING...");
                }
                break;
        }

        
        
    }

    void voteAltar()
    {
        if (voteOpen)
        {
            // Altar +1
            altar++;

        }
    }
    void voteTotem()
    {
        if (voteOpen)
        {
            // Totem +1
            totem++;

        }
    }

    // Update is called once per frame
    void Update () {
        timeStart += Time.deltaTime;
        if(timeStart > timeDelayVote&&!voteOpen)
        {
            altarIm.SetActive(true);
            totemIm.SetActive(true);
            aText.SetActive(true);
            tText.SetActive(true);
            voteOpen = true;
            StartCoroutine(voteEnCour());
        }        
    }   
}
