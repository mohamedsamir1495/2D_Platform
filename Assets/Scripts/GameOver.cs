using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

    int score = 0;

	void Start () {
        score = PlayerPrefs.GetInt("Score");
	}

    private void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2 - 40, 50, 80, 30), "Game Over");
        GUI.Label(new Rect(Screen.width / 2 - 40, 300, 80, 30), "Score "+score);

        if (GUI.Button(new Rect(Screen.width / 2 - 30, 350, 40, 30), "Retry"))
            Application.LoadLevel(0);
    }
}
