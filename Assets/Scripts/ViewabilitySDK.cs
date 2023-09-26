using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewabilitySDK : MonoBehaviour
{
    public bool UseViewabilityPolling;
    bool pollingstarted = false;
    [HideInInspector]
    public bool timeractive = false;
    int timer = 20;

    [HideInInspector]
    public GameObject AdHolder;
    //[HideInInspector]
    //public bool Adloaded = false;

    public Camera mainCamera;
    private AdloaderTest Adloader;

    int Adtimecounter = 0;
    bool Adinview = false;

    Dictionary<GameObject, float> EntryTimesa = new Dictionary<GameObject, float>();

    public List<float> ActiveTimeinview = new List<float>();

    Dictionary<GameObject, float> EntryTimesp = new Dictionary<GameObject, float>();

    public List<float> PassiveTimeinview = new List<float>();

    private void Start()
    {
        Adloader = FindObjectOfType<AdloaderTest>();
        //StartCoroutine(viewabilitypolling());
    }

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
        if(Adloader.Adloaded && !UseViewabilityPolling)
        {
            if (IsElementInViewport(AdHolder.transform.position))
            {
                Adinview = true;
                if(CalculateADangle(AdHolder))
                {
                    if(VisibilityChecker.IsObjectVisible(AdHolder,mainCamera))
                    {
                        if (!EntryTimesa.ContainsKey(AdHolder))
                        {
                            EntryTimesa[AdHolder] = Time.time;
                            Debug.Log(EntryTimesa[AdHolder]);
                        }
                    }
                    else
                    {
                        Debug.LogWarning("There is an object in covering the AD");
                    }
                    
                }
                else if(!CalculateADangle(AdHolder))
                {
                    Debug.LogWarning("Ad Angle is more than 55 Degrees");
                }
                
            }
            else if (!IsElementInViewport(AdHolder.transform.position))
            {
                Adinview = false;
                if (EntryTimesa.ContainsKey(AdHolder))
                {
                    ActiveTimeinview.Add(Time.time - EntryTimesa[AdHolder]);
                    EntryTimesa.Clear();
                }

            }
        }
        else if(Adloader.Adloaded && UseViewabilityPolling && !pollingstarted)
        {
            pollingstarted = true;
            StartCoroutine(viewabilitypolling());
        }
       
    }

    void passiveattention()
    {
        if(Adinview)
        {
            if (!EntryTimesp.ContainsKey(AdHolder))
            {
                EntryTimesa[AdHolder] = Time.time;
                Debug.Log(EntryTimesa[AdHolder]);
            }
        }
        else if(!Adinview)
        {
            if (EntryTimesp.ContainsKey(AdHolder))
            {
                PassiveTimeinview.Add(Time.time - EntryTimesa[AdHolder]);
                EntryTimesp.Clear();
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

        //Debug.Log("Angle between object and camera: " + angle);
        return (angle >= 0 && angle <= 55);
        
        
    }

    private bool IsElementInViewport(Vector3 position)
    {
        Vector3 viewportPos = mainCamera.WorldToViewportPoint(position);
        //Debug.Log(viewportPos);
        return viewportPos.x >= 0 && viewportPos.x <= 1 && viewportPos.y >= 0 && viewportPos.y <= 1;
    }

    IEnumerator viewabilitypolling()
    {
        int i = 5;
        int Polling = 0;
        while (i != 0)
        {
            if (IsElementInViewport(AdHolder.transform.position))
            {
                if (CalculateADangle(AdHolder))
                {
                    if (VisibilityChecker.IsObjectVisible(AdHolder, mainCamera))
                    {
                         Polling++;
                            
                    }
                    else
                    {
                        Debug.LogWarning("There is an object in covering the AD");
                    }

                }
                else if (!CalculateADangle(AdHolder))
                {
                    Debug.LogWarning("Ad Angle is more than 55 Degrees");
                }

                
            }
            else
            {
                //Do nothing 
            }
            i--;
            Debug.Log(Polling);
            yield return new WaitForSeconds(0.2f);
        }
        pollingstarted = false;
        pollingData(Polling);
    }

    void pollingData(int result)
    {
        if (result >= 3)
        {
            //Send Data to Server
            Debug.LogWarning("Polling Result: " + result);
        }
        else
        {
            //Disqualified
            Debug.LogWarning("Polling Result is less than 3");
        }
    }


}
