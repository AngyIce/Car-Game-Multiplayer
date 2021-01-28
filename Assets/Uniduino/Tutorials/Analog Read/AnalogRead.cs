
using UnityEngine;
using System.Collections;
using Uniduino;


	public class AnalogRead : MonoBehaviour {

    [Header("Arduino Variables")]

        public Arduino arduino;	
		private GameObject player;

		public int pin = 0;
		public float pinValue;
        public float mappedPot;

    [Header("Player Variables")]

        public float leftEdge;
        public float rightEdge;

    void Start () {
		
			arduino = Arduino.global;
			arduino.Log = (s) => Debug.Log("Arduino: " +s);
			arduino.Setup(ConfigurePins);
			
			player = GameObject.Find("Player");
		}
		
		void ConfigurePins( )
		{
			arduino.pinMode(pin, PinMode.ANALOG);
			arduino.reportAnalog(pin, 1);
		}
		
		void Update ()
        {
            pinValue = arduino.analogRead(pin);
            mappedPot = pinValue.Remap(1023, 0, leftEdge, rightEdge);
            player.transform.position = new Vector3(mappedPot, transform.position.y, transform.position.z);
            
    }
}


    
   
