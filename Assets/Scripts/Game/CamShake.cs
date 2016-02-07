using UnityEngine;
using System.Collections;

public class CamShake : MonoBehaviour
{
    public Transform camTransform;

    public float shake = 1.5f;

    public float shakeAmount = 2f;
    public float decreaseFactor = 1.0f;

    Vector3 originalPos;

    public bool shakeIt = false;

    void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    void Update()
    {
        if (shakeIt == true) {
            if (shake > 0)
            {
                camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

                shake -= Time.deltaTime * decreaseFactor;
            }
            else
            {
                shake = 0f;
                camTransform.localPosition = originalPos;
                shakeIt = false;
            }
        }       
    }
}
