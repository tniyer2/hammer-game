using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamZoomer : MonoBehaviour {

    [HideInInspector]
    public static CamZoomer instance;       //Singleton
    Camera cam;                             //gameObject's Camera component
    const float zoomRate = 0.01f;           //Smaller float will give smoother zoom
    float prevFOW;                          //Field of view before zoomInOut
    Vector3 prevRotation;                   //Previous Rotation of Camera

    void Awake()
    {
        instance = this;    //Establish singleton
        cam = gameObject.GetComponent<Camera>();
    }

    void Start()
    {
        prevFOW = cam.fieldOfView;
        prevRotation = transform.rotation.eulerAngles;
    }

    public IEnumerator zoomInOut(Vector3 newRotation, float newFOW, float outtime, float intime, float delay)
    {
        for (int i = 0; i * zoomRate <= outtime; i++)
        {
            lerpValues(newRotation, prevRotation, newFOW, prevFOW, i * zoomRate / outtime);
            yield return new WaitForSeconds(zoomRate);
        }

        yield return new WaitForSeconds(delay);

        for (int i = 0; i * zoomRate <= intime; i++)
        {
            lerpValues(prevRotation, newRotation, prevFOW, newFOW, i * zoomRate / intime);
            yield return new WaitForSeconds(zoomRate);
        }
    }

    void lerpValues(Vector3 _newRot, Vector3 _oldRot, float _new, float _old, float time)
    {
        cam.fieldOfView = Mathf.Lerp(_old, _new, time);
        transform.eulerAngles = Vector3.Lerp(_oldRot, _newRot, time);
    }
}
