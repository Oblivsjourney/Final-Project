using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	public GameObject[] hazards;
	public Vector3 spawnValues;
	public int hazardCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;

	public Text scoreText;
	public Text restartText;
	public Text gameOverText;
	public Text winText;
	public Text nextLevelText;
	public Text newGameText;
	public Text hardModeText;

	public AudioSource gameMusic;
	public AudioSource winMusic;
	public AudioSource loseMusic;

	private bool gameOver;
	private bool HardMode;
	private bool restart;
	private bool nextLevel;
	private bool newGame;
	private int score;


	void Start ()
	{
	    gameOver = false;
	    restart = false;
		nextLevel = false;
		HardMode = false;
		restartText.text = "";
		gameOverText.text = "";
		winText.text = "";
		nextLevelText.text = "";
		newGameText.text = "";
		hardModeText.text = "Press 'H' to Play Hard Mode";
	    score = 0;
		gameMusic.Play();
		UpdateScore();
		StartCoroutine (SpawnWaves ());
	}

	void Update ()
	{
		if (restart)
		{
			if (Input.GetKeyDown (KeyCode.T))
			{
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}
		}

		if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

		if (nextLevel)
		{
			if (Input.GetKeyDown(KeyCode.N))
		    {
			    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		    }
		}

		if (newGame)
		{
			if (Input.GetKeyDown(KeyCode.R))
			{
			    SceneManager.LoadScene("Level1");
			}
		}


	    if (Input.GetKeyDown(KeyCode.H))
	    {
			SceneManager.LoadScene("Hard Mode");
		}

		if (SceneManager.GetActiveScene () == SceneManager.GetSceneByName ("Hard Mode")) 
		{
			hardModeText.text = "You're Playing Hard Mode!";
			HardMode = true;
		}

		if (SceneManager.GetActiveScene () == SceneManager.GetSceneByName ("Level3")) 
		{
			nextLevel = false;
			nextLevelText.text = "";
		}

		if (gameOver)
		{
			restartText.text = "Press 'T' to Retry Level";
			restart = true;
		}

	}

	IEnumerator SpawnWaves ()
	{
	    yield return new WaitForSeconds (startWait);
		while (true)
		{
			for (int i = 0; i < hazardCount; i++)
			{
			    GameObject hazard = hazards[Random.Range (0, hazards.Length)];
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (hazard, spawnPosition, spawnRotation);
				yield return new WaitForSeconds (spawnWait);
			}
			yield return new WaitForSeconds (waveWait);


			if (nextLevel)
			{
				break;
			}


		}
	}

	public void AddScore(int newScoreValue)
	{
		score += newScoreValue;
		UpdateScore();
	}

	void UpdateScore()
	{
		scoreText.text = "Points: " + score;
		 if (score >= 200)
          {
            winText.text = "You Win! Can You Survive the Next Level?";
            nextLevel = true;
			newGame = true;
			gameMusic.Stop();
			winMusic.Play();
			gameOverText.text = "";
			restartText.text = "";
			nextLevelText.text = "Press 'N' for Next Level";
			newGameText.text = "Press 'R' to Restart Game";
           }

		   if (score >= 200 && SceneManager.GetActiveScene () == SceneManager.GetSceneByName ("Level3"))
		   {
		   	   winText.text = "Game Over, Congrats! You've Won the Game!";
		   }

		     if (score >= 200 && SceneManager.GetActiveScene () == SceneManager.GetSceneByName ("Hard Mode"))
		   {
		   	   winText.text = "Game Over, Wow! You've Survived Hard Mode!";
			   nextLevelText.text = "";
		   }
	}

	public void GameOver ()
	{
		gameOverText.text = "Game Over! Game Created by Richard Barnes";
		gameOver = true;
		gameMusic.Stop();
		loseMusic.Play();
		newGame = true;
		newGameText.text = "Press 'R' to Restart Game";
	}

}