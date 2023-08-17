using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdloaderTest : MonoBehaviour
{
    public Image Adholder;

    public Sprite iionad;

    Cooloftimer cooloftimer;

    [ContextMenu("LoadtheAD")]
    public void LoadAD()
    {
        Debug.Log("Checking the Cool Of timer ....");
        cooloftimer = FindObjectOfType<Cooloftimer>();
        if(!cooloftimer.timeractive)
        {
            Debug.Log("Loading the Ad .....");
            Adholder.sprite = iionad;
            Debug.Log("Ad Loaded");
            PixelRequiredcheck pixelcheck = new PixelRequiredcheck();
            pixelcheck.Pixelcheck(true);
            cooloftimer.Adloaded = true;
            cooloftimer.Starttimer();

        }
        else
        {
            Debug.Log("Cool of Timer is still active");
        }
       
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
