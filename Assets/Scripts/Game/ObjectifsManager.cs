using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ObjectifsManager : MonoBehaviour {
    
    #region Singleton
    static private ObjectifsManager s_Instance;
    static public ObjectifsManager Instance
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

    public GameObject rune1Default;
    public GameObject rune2Default;
    public GameObject rune3Default;

    public bool objectif1 = false;
    public bool objectif2 = false;
    public bool objectif3 = false;

    public GameObject meteorite;

    private int i = 0;
    public int nbMeteoriteApo = 10;

    public static int nbAnimauxTot = 0;

    public bool licorne = false;
    public bool dragon = false;


    public GameObject panelSwitch;
    void Start ()
    {
        nbAnimauxTot = 0;
        panelSwitch = GameObject.Find("PanelGO");
        StartCoroutine(fadeOut());
    }

    void reset()
    {
        
        rune1Default.GetComponent<Animator>().SetBool("Active", false);
        objectif1 = false;
        rune2Default.GetComponent<Animator>().SetBool("Active", false);
        objectif2 = false;
        rune3Default.GetComponent<Animator>().SetBool("Active", false);
        objectif3 = false;
        nbAnimauxTot = 0;
        i = 0;
        licorne = false;
        dragon = false;
        SacrificialAltar.sacrificedAnimals = 0;
        TerrainGenerator.Instance.spawnAnimaux(2);
    }

    IEnumerator fadeOut()
    {
        yield return new WaitForSeconds(0.1f);
        panelSwitch.GetComponent<Image>().color = new Color(0, 0, 0, 0.8f);
        yield return new WaitForSeconds(0.1f);
        panelSwitch.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
        yield return new WaitForSeconds(0.1f);
        panelSwitch.GetComponent<Image>().color = new Color(0, 0, 0, 0.2f);
        yield return new WaitForSeconds(0.1f);
        panelSwitch.GetComponent<Image>().color = new Color(0, 0, 0, 0f);
    }

    IEnumerator fadeIn()
    {
        yield return new WaitForSeconds(0.1f);
        panelSwitch.GetComponent<Image>().color = new Color(0, 0, 0, 0.2f);
        yield return new WaitForSeconds(0.1f);
        panelSwitch.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
        yield return new WaitForSeconds(0.1f);
        panelSwitch.GetComponent<Image>().color = new Color(0, 0, 0, 0.8f);
        yield return new WaitForSeconds(0.1f);
        panelSwitch.GetComponent<Image>().color = new Color(0, 0, 0, 1f);
        Application.LoadLevel(0);
    }

        void Update ()
    {

        // GAME OVER
        if(GameObject.FindGameObjectsWithTag("Animal").Length <= 1)
        {
            restart();
        }

        // Objectif 1 : 2 Animaux Légendaires
        if (licorne == true || dragon == true) {
            rune1Default.GetComponent<Animator>().SetBool("Active", true);
            objectif1 = true;
        }
        else
        {
            objectif1 = false;
        }

        // Objectif 2 : 10 Sacrifices
        if (SacrificialAltar.sacrificedAnimals >= 10&&!objectif2)
        {
            rune2Default.GetComponent<Animator>().SetBool("Active", true);
            objectif2 = true;
        }

        // Objectif 3 : 25 Animaux
        if (nbAnimauxTot >= 25&& !objectif3)
        {
            rune3Default.GetComponent<Animator>().SetBool("Active", true);
            objectif3 = true;
        }
        
        // Apocalypse
        // if (objectif1 == true ) 
        
    }

    public void invokeRitual()
    {
        if (objectif1 == true && objectif2 == true && objectif3 == true)
        {
            GameObject.FindGameObjectWithTag("soundmanager").GetComponent<soundManagerScript>().apocalypse.Play();
            while (i < nbMeteoriteApo)
            {
                Vector3 position = new Vector3(Random.Range(0.0F, 20.0F), Random.Range(100.0F, 200.0F), Random.Range(0.0F, 20.0F));
                Instantiate(meteorite, position, Quaternion.identity);
                i++;
            }
            reset();
         }
        
    }

    public void restart()
    {
        rune1Default.GetComponent<Animator>().SetBool("Active", false);
        objectif1 = false;
        rune2Default.GetComponent<Animator>().SetBool("Active", false);
        objectif2 = false;
        rune3Default.GetComponent<Animator>().SetBool("Active", false);
        objectif3 = false;
        nbAnimauxTot = 0;
        i = 0;
        licorne = false;
        dragon = false;
        SacrificialAltar.sacrificedAnimals = 0;
        StartCoroutine(fadeIn());
    }
}
