using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooloftimer : MonoBehaviour
{
    public bool timeractive = false;
    int timer = 20;

    public GameObject AdHolder;
    public bool Adloaded = false;
    public Camera mainCamera;

    public int Adtimecounter = 0;
    bool Adinview = false;

    public Dictionary<GameObject, float> EntryTimes = new Dictionary<GameObject, float>();

    public List<float> Timeinview = new List<float>();

    public void Starttimer()
    {
        StartCoroutine(CooloffTimer());
        //mainCamera = FindObjectOfType<Camera>();
    }
    IEnumerator CooloffTimer()
    {
        if(!timeractive)
        {
            //StartCoroutine(CheckifAdinview());
            while (timer != 0)
            {
                timeractive = true;
                yield return new WaitForSeconds(1);
                timer--;
                //Debug.Log(timer);

            }
            timeractive = false;
            timer = 20;
        }
        else
        {
            //Timer is already active

        }
        
    }

    private void Update()
    {
        if(Adloaded)
        {
            if (IsElementInViewport(AdHolder.transform.position))
            {
                Adinview = true;
                if(CalculateADangle(AdHolder))
                {
                    if (!EntryTimes.ContainsKey(AdHolder))
                    {
                        EntryTimes[AdHolder] = Time.time;
                        Debug.Log(EntryTimes[AdHolder]);
                    }
                }
                else if(!CalculateADangle(AdHolder))
                {
                    Debug.Log("Ad Angle is more than 55 Degrees");
                }
                
            }
            else if (!IsElementInViewport(AdHolder.transform.position))
            {
                Adinview = false;
                if (EntryTimes.ContainsKey(AdHolder))
                {
                    Timeinview.Add(Time.time - EntryTimes[AdHolder]);
                    EntryTimes.Clear();
                }

            }
        }
       
    }

    private bool CalculateADangle(GameObject Ad)
    {

        // Calculate the object's forward direction based on its rotation
        Vector3 objectForward = Ad.transform.forward;

        Vector3 cameraForward = mainCamera.transform.forward;

        // Calculate the angle between object's forward direction and vector to camera
        float angle = Vector3.Angle(cameraForward, objectForward);

        Debug.Log("Angle between object and camera: " + angle);
        return (angle >= 0 && angle <= 55);
        
        
    }

    private bool IsElementInViewport(Vector3 position)
    {
        Vector3 viewportPos = mainCamera.WorldToViewportPoint(position);
        //Debug.Log(viewportPos);
        return viewportPos.x >= 0 && viewportPos.x <= 1 && viewportPos.y >= 0 && viewportPos.y <= 1;
    }


}
