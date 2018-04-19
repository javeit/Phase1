using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControlReverse : MonoBehaviour {
    public Light redLight;
    public Light yellowLight;
    public Light greenLight;
    public static bool redStop = false;

    //Traffic Light Control for NS
    void Awake()
    {
        StartCoroutine(LightLoop());
        redStop = getRedStatus();
    }

    IEnumerator LightLoop()
    {

        while (true)
        {

            //Debug.Log("Green light On");

            //code for the Green light = On
            //yellow and red = off
            redLight.enabled = false;
            yellowLight.enabled = false;
            greenLight.enabled = true;
            redStop = false;
            getRedStatus();


            yield return new WaitForSeconds(10); //green will be on for 10 sec

            //Debug.Log("Yellow light On");

            //code for the Yellow light = On
            //red and green = off
            redLight.enabled = false;
            yellowLight.enabled = true;
            greenLight.enabled = false;
            redStop = true;
            getRedStatus();

            yield return new WaitForSeconds(2); //yellow will be on for 1 sec

            //Debug.Log("Red light On");
            //code for the red light = On
            //yellow and green = off


            redLight.enabled = true;
            yellowLight.enabled = false;
            greenLight.enabled = false;
            redStop = true;
            getRedStatus();

            yield return new WaitForSeconds(10); //red will be on for 10 sec
            yield return new WaitForSeconds(2); //wait for opposite traffic to turn red from yellow

        }
    }
         //function that returns true when redlight
        public bool getRedStatus()
        {

            return redStop;
        }
}

