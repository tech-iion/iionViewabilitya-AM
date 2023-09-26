using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttentionSDK : MonoBehaviour
{
    public Camera mainCamera;

    public GameObject AdHolder;

    private AdloaderTest Adloader;

    Dictionary<GameObject, float> EntryTimes = new Dictionary<GameObject, float>();

    public List<float> Timeinview = new List<float>();

    // Start is called before the first frame update
    void Start()
    {
        Adloader = FindObjectOfType<AdloaderTest>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Adloader.Adloaded)
        {
            if (IsElementInViewport(AdHolder.transform.position))
            {
                if (!EntryTimes.ContainsKey(AdHolder))
                {
                    EntryTimes[AdHolder] = Time.time;
                    Debug.Log(EntryTimes[AdHolder]);
                }
            }
            else if (!IsElementInViewport(AdHolder.transform.position))
            {
                if (EntryTimes.ContainsKey(AdHolder))
                {
                    Timeinview.Add(Time.time - EntryTimes[AdHolder]);
                    EntryTimes.Clear();
                }

            }
        }
    }


    void Activeattention()
    {

    }


    private bool IsElementInViewport(Vector3 position)
    {
        Vector3 viewportPos = mainCamera.WorldToViewportPoint(position);
        
        return viewportPos.x >= 0 && viewportPos.x <= 1 && viewportPos.y >= 0 && viewportPos.y <= 1;
    }
}
