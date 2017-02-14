using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public GameObject playerInstance;
    public GameObject shadowInstance;
    public Transform spawnPoint;
    public int spawnDelay = 2;
    GameObject camera;
    public bool hasKey = false;
    public int levelDelay = 3;
    public Text levelCompleteText;
    public Text instructions;

    void Awake() {
        if (instance == null) {
            instance = this;
        }    
        else if (instance != this) {
            Destroy(gameObject);
        }

        camera = GameObject.Find("Main Camera");
    }

    void Start() {
        levelCompleteText.enabled = false;
    }

    void Update() {
        if (instructions.name == "BlinkInstructions" && Input.GetButtonDown("Blink")) {
            instructions.enabled = false;
        }

        if (instructions.name == "MomentumInstructions" && hasKey) {
            instructions.enabled = false;
        }
    }

    public IEnumerator RespawnPlayer() {
        yield return new WaitForSeconds(spawnDelay);
        RestartLevel();
    }

    public void RestartLevel() {
//        camera.transform.position = new Vector3(spawnPoint.position.x,0,-40f);
//        playerInstance.transform.position = spawnPoint.position;
//        shadowInstance.transform.position = spawnPoint.position;
//        shadowInstance.GetComponent<ShadowRecording>().RestartQueues();
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }

    public static void KillPlayer(PlayerController player, ShadowRecording shadow) {
        player.gameObject.transform.position = new Vector3(player.gameObject.transform.position.x, 150f);
        shadow.gameObject.transform.position = new Vector2(shadow.gameObject.transform.position.x, 150f);
        instance.StartCoroutine("RespawnPlayer");
    }

    public void WinLevel() {
        Destroy(playerInstance);
        Destroy(shadowInstance);
        levelCompleteText.enabled = true;
        StartCoroutine("LoadNextLevel");
    }

    public IEnumerator LoadNextLevel() {
        yield return new WaitForSeconds(levelDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
