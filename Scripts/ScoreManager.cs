using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

	public int SCORE;

	public Text scoredisplay;
	public Text Highscoredisplay;

	public int Highscore;

	Manager manager;
	void Start(){
		manager = FindObjectOfType<Manager> ();
		Manager.GameStarted += Manager_GameStarted;
		Highscore = GetSavedHighscore ();
		Manager.GameEnd += Manager_GameEnd;
		Manager.FruitPicked += Manager_FruitPicked;
		scoredisplay.text = "";
		Highscoredisplay.text = Highscore.ToString ();
	}

	void Manager_FruitPicked (Fruits _fruitpick)
	{
		Manager.FruitPicked -= Manager_FruitPicked;
		AddScore (10);
		Manager.FruitPicked += Manager_FruitPicked;
	}

	void Manager_GameEnd ()
	{
		Manager.GameEnd -= Manager_GameEnd;
		if (_detectedHighscore) {
			//do some animation stuff
			SaveHighscore ();
		}
		Manager.GameEnd += Manager_GameEnd;
	}

	void Manager_GameStarted (float _starttime)
	{
		Manager.GameStarted -= Manager_GameStarted;
		scoredisplay.text = "";
		SCORE = 0;
		Highscore = GetSavedHighscore ();
		Manager.GameStarted += Manager_GameStarted;
	}

	bool _detectedHighscore;
	public void AddScore(int _score){
		SCORE += _score;
		scoredisplay.text = SCORE.ToString ();
		if (Highscore <SCORE) {
			_detectedHighscore = true;
		}
	}


	public int GetSavedHighscore(){
		return PlayerPrefs.GetInt ("Highscore");
	}

	void SaveHighscore(){
		PlayerPrefs.SetInt ("Highscore", SCORE);
	}
}
