using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private float velocity;
    public float smoothTimeX;
    public GameObject player;
    public Transform spawnPoint;

    float nextTimeToSearch = 0;

	// Use this for initialization
	void Start () {
        transform.position = new Vector3(spawnPoint.position.x, 0, -40f);
	}
	
	// Update is called once per frame
	void Update () {
        if (player == null) {
            FindPlayer();
            return;
        }

            float posX = Mathf.SmoothDamp(transform.position.x, player.transform.position.x+4f, ref velocity, smoothTimeX);
            //float posY = Mathf.SmoothDamp(transform.position.y, player.transform.position.y, ref velocity, smoothTimeY);

            transform.position = new Vector3(posX, transform.position.y, transform.position.z);

	}

    void FindPlayer() {
        if (nextTimeToSearch <= Time.time) {
            GameObject searchResult = GameObject.FindGameObjectWithTag("Player");
            if (searchResult != null) {
                player = searchResult;
            }
            nextTimeToSearch = Time.time + 0.5f;
        }
    }
}
