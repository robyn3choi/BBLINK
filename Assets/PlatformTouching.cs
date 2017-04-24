using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTouching : MonoBehaviour {

    PlayerController player;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("PlatformCheck"))
            player.isTouchingSideOfPlatform = true;
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("PlatformCheck"))
            player.isTouchingSideOfPlatform = false;
    }
}
