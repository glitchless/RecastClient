using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeMenu : MonoBehaviour {

    public Canvas canvas;
    public GameObject Camera;
    bool Paused = false;

    void Start() {
        canvas = gameObject.GetComponent<Canvas>();
        canvas.gameObject.SetActive(false);
    }

    void Update() {
        if (Input.GetKeyDown("escape")) {
            Debug.Log("Escape");
            if (Paused == true) {
                Time.timeScale = 1.0f;
                canvas.gameObject.SetActive(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                Paused = false;
            }
            else {
                Time.timeScale = 0.0f;
                canvas.gameObject.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Paused = true;
            }
        }
    }

    public void Resume() {
        Time.timeScale = 1.0f;
        canvas.gameObject.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}