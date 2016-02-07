using UnityEngine;
using System.Collections;

public class JumpAnim : MonoBehaviour {

    NavMeshAgent navMeshAgent;
    Vector3 iniPos;
    Vector3 offset;
    public float bounceIntensity = 0.2f;
    public float bounceSpeed = 4f;
    float i = 1;

    void Start()
    {
        navMeshAgent = this.gameObject.GetComponentInParent<NavMeshAgent>();
        iniPos = transform.localPosition;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (navMeshAgent != null)
        {
            if (navMeshAgent.velocity.magnitude > 0.1f)
            {
                if (i >= 1)
                    i = 0;
            }
        }

        if (i < 1)
            Bounce();
        i += bounceSpeed * Time.deltaTime;

    }

    void Bounce()
    {
        float yOffset = Mathf.Sin(i * Mathf.PI);
        offset = new Vector3 (0, yOffset * bounceIntensity, 0);
        transform.localPosition = iniPos + offset;
    }
}
