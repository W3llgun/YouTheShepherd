using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Animal : MonoBehaviour
{

    public string state = "GoToCenter";
    string old;
    public bool dispoTP;
    public float speed = 2.5f;
    

    TerrainGenerator terrain = null;

    public Type typetogo = Type.Herbe;

    static public int AllreadyInstance = 0;

    bool canBaby = false;

    // Use this for initialization
    void Start()
    {
        dispoTP = true;
        ObjectifsManager.nbAnimauxTot = ObjectifsManager.nbAnimauxTot + 1;
        //Debug.Log("Animaux Actuels = " + ObjectifsManager.nbAnimauxTot);
        old = state;

        terrain = GameObject.FindObjectOfType<TerrainGenerator>();

        if (state == "Fly")
        {
        }
        else if (state == "GoToCenter")
        {

            GameObject CaseActual = terrain.getCaseByPosition(transform.position);

            if (CaseActual.GetComponent<TerrainScript>().getType() == Type.Eau)
            {
                Destroy(gameObject);
            }

            else if (CaseActual.GetComponent<TerrainScript>().getType() == Type.Sable)
            {

            }
        }

        AllreadyInstance++;

        StartCoroutine(changeOrder());
    }

    static int order = 0;

    IEnumerator changeOrder()
    {
        yield return null;

        typetogo = transform.GetChild(0).FindChild("Head").GetComponent<whereToGo>().where;



        if(! GetComponent<UnityEngine.AI.NavMeshAgent>().isOnNavMesh)
        {
            UnityEngine.AI.NavMeshHit posTogo;
            UnityEngine.AI.NavMesh.SamplePosition(transform.position, out posTogo, 1000f, UnityEngine.AI.NavMesh.AllAreas);

            GetComponent<UnityEngine.AI.NavMeshAgent>().nextPosition = posTogo.position;
        }

        GetComponent<UnityEngine.AI.NavMeshAgent>().radius = 0.25f;
        GetComponent<UnityEngine.AI.NavMeshAgent>().speed = 1.5f;
        GetComponent<UnityEngine.AI.NavMeshAgent>().avoidancePriority = order;
        order += 2;

        //print("order = " + order);

        if (order > 99) 
            order = 0;

        transform.GetChild(0).localScale = new Vector3(0.5f, 0.5f, 0.5f);

        yield return new WaitForSeconds(5f);
        canBaby = true;

        transform.GetChild(0).localScale = new Vector3(1f, 1f, 1f);

        foreach (Transform e in transform.GetChild(0))
            e.localScale = new Vector3(1f, 1f, 1f);
    }
    
   public IEnumerator newAnimalOeuf()
    {
        yield return null;
        yield return null;
        FindObjectOfType<popAnimal>().showAnimal(gameObject);
    }

    void OnDestroy()
    {
        AllreadyInstance--;
    }


    Vector3 cible;

    float timeLast;

    float timerBaby = 0f;

    // Update is called once per frame
    void Update()
    {




        if (old != state)
        {
            if (state == "Fly")
            {
                //cible = new Vector3(Random.insideUnitCircle.x*10.0f, 0f, Random.insideUnitCircle.y*10.0f);
            }
        }


        old = state;


        GameObject CaseActual = terrain.getCaseByPosition(transform.position);

        if (CaseActual.GetComponent<TerrainScript>().getType() == Type.Eau)
        {
            Destroy(gameObject);
        }

        GetComponent<UnityEngine.AI.NavMeshAgent>().baseOffset = terrain.getTop(CaseActual.transform.position) + 0.5f;

        /*
		if(Input.GetKeyDown(KeyCode.B) ) {
			GameObject pose = terrain.getCaseByPosition(terrain.cursor);
			cible = new Vector3(pose.transform.position.x, 0f, pose.transform.position.z );

			GetComponent<NavMeshAgent>().SetDestination(cible);
		}
		*/


        if (Vector3.Distance(GetComponent<UnityEngine.AI.NavMeshAgent>().velocity, Vector3.zero) < 0.001f)
        {

            timeLast += Time.deltaTime;

            if (timeLast < 1.0f) return;



            // Debug.Log("new pos Calculated");

            if (CaseActual.GetComponent<TerrainScript>().getType() == typetogo)
            {

                Vector2 newPos = Vector3.left * 999f;
                Vector3 pos2 = Vector3.left * 999f;

                while (pos2.x > 20f || pos2.x < 0f || pos2.z > 20f || pos2.z < 0f)
                {
                    newPos = Random.insideUnitCircle * 4f;
                    pos2 = transform.position + new Vector3(newPos.x, 0f, newPos.y);
                }

                pos2.y = 0f;

                GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(pos2);
            }
            else
                recalculateDestination();

            timeLast = 0f;

        }


    }


    void OnTriggerEnter(Collider other)
    {

        


        Animal e = other.GetComponent<Animal>();
        if (e)
        {

            if (!canBaby)
                return;

            if (GameManager.Instance.timeOfEggs <= 0.0f)
            {
                animal1 = transform;
                animal2 = other.transform;
                fusion(other.ClosestPointOnBounds(transform.position));
                GameManager.Instance.timeOfEggs = 15.0f;
            }


        }


    }

    private Transform animal1;
    private Transform animal2;

    public void fusion(Vector3 pos)
    {

        if (AllreadyInstance > 29)
        {
            return;
        }

        Instantiate(GameObject.FindObjectOfType<TerrainGenerator>().animCoeur, pos, Quaternion.identity);
        GameObject.FindGameObjectWithTag("soundmanager").GetComponent<soundManagerScript>().kiss.Play();

        GameObject e = new GameObject("newAnimal");
        e.transform.position = Vector3.zero;
        Animal X = e.AddComponent<Animal>();
        //X.state = "Fly";

        e.SetActive(false);

        SphereCollider TP = e.AddComponent<SphereCollider>();
        TP.radius = 0.5f;
        TP.isTrigger = true;

        e.AddComponent<Rigidbody>().isKinematic = true;

        e.AddComponent<UnityEngine.AI.NavMeshAgent>();

        List<string> descriptionPart = new List<string>();

        // body choice; 
        GameObject newBody = null;
        Transform initialBody = null;

        if (Random.value > 0.5)
        {
            newBody = Instantiate(animal1.FindChild("Body").gameObject, Vector3.zero, Quaternion.identity) as GameObject;
            initialBody = animal1.FindChild("Body");
        }
        else
        {
            newBody = Instantiate(animal2.FindChild("Body").gameObject, Vector3.zero, Quaternion.identity) as GameObject;
            initialBody = animal2.FindChild("Body");
        }

        newBody.transform.parent = e.transform;
        newBody.transform.localPosition = Vector3.zero;
        newBody.transform.localRotation = initialBody.localRotation;

        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in newBody.transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));

        newBody.name = "Body";
        if (newBody.GetComponent<AnimalPart>())
            descriptionPart.Add(newBody.GetComponent<AnimalPart>().description);

        // head part : 
        Transform head = null;

        if (Random.value > 0.5)
        {
            head = Instantiate(animal1.FindChild("Body").FindChild("Head")) as Transform;
        }
        else
        {
            head = Instantiate(animal2.FindChild("Body").FindChild("Head")) as Transform;
        }

        head.name = "Head";

        head.parent = newBody.transform;
        head.localPosition = initialBody.FindChild("Head").localPosition;
        head.localRotation = initialBody.FindChild("Head").localRotation;

        if (head.GetComponent<AnimalPart>())
            descriptionPart.Add(head.GetComponent<AnimalPart>().description);


        // front 
        Transform frontA = null;
        Transform frontB = null;

        if (Random.value > 0.5)
        {
            frontA = Instantiate(animal1.FindChild("Body").FindChild("FrontR")) as Transform;
            frontB = Instantiate(animal1.FindChild("Body").FindChild("FrontL")) as Transform;
        }
        else
        {
            frontA = Instantiate(animal2.FindChild("Body").FindChild("FrontR")) as Transform;
            frontB = Instantiate(animal2.FindChild("Body").FindChild("FrontL")) as Transform;
        }

        frontA.parent = newBody.transform;
        frontA.localPosition = initialBody.FindChild("FrontR").localPosition;
        frontA.localRotation = initialBody.FindChild("FrontR").localRotation;

        frontB.parent = newBody.transform;
        frontB.localPosition = initialBody.FindChild("FrontL").localPosition;
        frontB.localRotation = initialBody.FindChild("FrontL").localRotation;

        frontA.name = "FrontR";
        frontB.name = "FrontL";

        if (frontA.GetComponent<AnimalPart>())
            descriptionPart.Add(frontA.GetComponent<AnimalPart>().description);


        // back
        Transform backA = null;
        Transform backB = null;

        if (Random.value > 0.5)
        {
            backA = Instantiate(animal1.FindChild("Body").FindChild("BackR")) as Transform;
            backB = Instantiate(animal1.FindChild("Body").FindChild("BackL")) as Transform;
        }
        else
        {
            backA = Instantiate(animal2.FindChild("Body").FindChild("BackR")) as Transform;
            backB = Instantiate(animal2.FindChild("Body").FindChild("BackL")) as Transform;
        }

        backA.parent = newBody.transform;
        backA.localPosition = initialBody.FindChild("BackR").localPosition;
        backA.localRotation = initialBody.FindChild("BackR").localRotation;

        backB.parent = newBody.transform;
        backB.localPosition = initialBody.FindChild("BackL").localPosition;
        backB.localRotation = initialBody.FindChild("BackL").localRotation;

        backA.name = "BackR";
        backB.name = "BackL";

        if (backA.GetComponent<AnimalPart>())
            descriptionPart.Add(backA.GetComponent<AnimalPart>().description);
        
        Egg oeuf = (Instantiate(Resources.Load("spawningEggs"), this.transform.position, Quaternion.identity) as GameObject).GetComponent<Egg>();
        oeuf.other = e;
        e.tag = "Animal";

        pos.y = 0f;

        e.transform.position = pos;
        
        e.SetActive(false);

        string legendaire = AnimalPart.compareTo(descriptionPart);

        if (legendaire == "Licorne")
        {
           GameObject P = Instantiate(FindObjectOfType<TerrainGenerator>().licorne, pos, Quaternion.identity) as GameObject;
            oeuf.other = P;
            Destroy(e);
        }

        else if (legendaire == "Dragon")
        {
            GameObject P = Instantiate(FindObjectOfType<TerrainGenerator>().Dragon, pos, Quaternion.identity) as GameObject;
            oeuf.other = P;
            Destroy(e);
        }
    }


    public void recalculateDestination()
    {

        if (state == "GoToCenter")
        {
            if (terrain == null)
                terrain = GameObject.FindObjectOfType<TerrainGenerator>();

            List<Vector3> e = terrain.getListCase(transform.position, typetogo);

            if (e.Count == 0)
            {
                Vector2 newPos = Vector3.left * 999f;
                Vector3 pos2 = Vector3.left * 999f;

                while (pos2.x > 20f || pos2.x < 0f || pos2.z > 20f || pos2.z < 0f)
                {
                    newPos = Random.insideUnitCircle * 4f;
                    pos2 = transform.position + new Vector3(newPos.x, 0f, newPos.y);
                }

                pos2.y = 0f;

                GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(pos2);
                return;
            }



            Vector3 test = new Vector3(500, 500, 500);

            foreach (Vector3 entry in e)
            {
                if (Vector3.Distance(transform.position, test) > Vector3.Distance(transform.position, entry))
                {
                    test = entry;
                }
            }


            cible = new Vector3(test.x, 0f, test.z);

            if (!GetComponent<UnityEngine.AI.NavMeshAgent>().isOnNavMesh)
            {
                UnityEngine.AI.NavMeshHit posTogo;
                UnityEngine.AI.NavMesh.SamplePosition(transform.position, out posTogo, 1000f, UnityEngine.AI.NavMesh.AllAreas);

                GetComponent<UnityEngine.AI.NavMeshAgent>().nextPosition = posTogo.position;

                return ;
            }

            GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(cible);

        }
    }

    public void WaitCDTP()
    {
        if (dispoTP == false)
        {
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(10);
        dispoTP = true;
    }
}
