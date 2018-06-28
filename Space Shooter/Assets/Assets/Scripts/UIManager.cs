using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

	public void StartGame() {
		//Start the game
		SceneManager.LoadScene ("Main", LoadSceneMode.Single);
	}

	public void QuitGame() {
		//Release mode
		Application.Quit();
		//Quit the game, preview mode
		//UnityEditor.EditorApplication.isPlaying = false;
	}
}
