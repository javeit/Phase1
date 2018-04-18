using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControl : MonoBehaviour {
    public Light redLight;
    public Light yellowLight;
    public Light greenLight;


	// Use this for initialization
	void Start () {

    }
	
    void Awake() 
	{
        StartCoroutine(LightLoop());
    }

    IEnumerator LightLoop()
    {
       
        while (true)
        {
            Debug.Log("Red light On");
            //code for the red light = On
            //yellow and green = off
            redLight.enabled = true;
            yellowLight.enabled = false;
            greenLight.enabled = false;
            yield return new WaitForSeconds(10); //red will be on for 10 sec

            Debug.Log("Yellow light On");

            //code for the Yellow light = On
            //red and green = off
            redLight.enabled = false;
            yellowLight.enabled = true;
            greenLight.enabled = false;


            yield return new WaitForSeconds(2); //yellow will be on for 2 sec

            Debug.Log("Green light On");

            //code for the Green light = On
            //yellow and red = off
            redLight.enabled = false;
            yellowLight.enabled = false;
            greenLight.enabled = true;

            yield return new WaitForSeconds(10); //green will be on for 10 sec
        }
    }
}
