using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class PixelRequiredcheck
{
    public async void Pixelcheck(bool getresult)
    {
        bool pixelcheck,Timerequirement;
        // If the Ad is loaded that means 100% pixels are being loaded 
        pixelcheck = getresult;
        await Task.Delay(1000);
        Timerequirement = getresult;
        // Send Data to the server using json
        Debug.Log("Send Data to server : " + pixelcheck);

        

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
