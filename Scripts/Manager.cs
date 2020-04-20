using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Manager : MonoBehaviour {

	public delegate void OnFruitPick(Fruits _fruitpick);
	public static event OnFruitPick FruitPicked;

	public List<Fruits> FruitsEaten = new List<Fruits> ();
	public float[] Thresholds;
	public Notification notification;
	public ScoreManager scoremanager;
	public float LevelSpeed;
	public LevelScript level;
	public PlayerController ball;

	[Tooltip("This threshold parameter value on the Y AXIS")]
	public RectTransform PlayerThreshold;

	void Start(){
		level = FindObjectOfType<LevelScript> ();
		FruitsEaten = FindObjectsOfType <Fruits> ().ToList ();
		GameStarted += Manager_GameStarted;
		notification = FindObjectOfType<Notification> ();
	}

	void Manager_GameStarted (float _starttime)
	{
		GameStarted -= Manager_GameStarted;
		_searching = true;
		StartCoroutine (ReactivateInActiveFruits ());
		GameStarted += Manager_GameStarted;
	}


	int _radomizeType;
	public void PickFruit(Fruits _fruit){
		if (FruitPicked != null) {
			FruitPicked (_fruit);
		}
		if (!FruitsEaten.Contains (_fruit)) {
			_fruit.fruittype = (Fruits.FruitTtype)Random.Range (0, 2);
			FruitsEaten.Add (_fruit);
		}
	}


	public void DisplayInfo(string _info){
		string[] _temp = _info.Split ('|');
		notification.GetMessage (_temp[0], _temp[1], "");
	}

	public GameObject homepanel;
	public delegate void OnHome();
	public static event  OnHome GoHome;
	public void Home(){
		if (GoHome != null) {
			GoHome();
			homepanel.SetActive (true);
		}
	}

	public delegate void OnGameEnd();
	public static event  OnGameEnd GameEnd;
	public void GameEnded(){
		if (GameEnd != null) {
			GameEnd ();
			_searching = false;
			if (level.currentsizeTarget == ball.size) {
				PlayerWonLevel ();
			} else {
				notification.GetMessage ("Game Failed!", "You score is "+ scoremanager.scoredisplay.text, "Go Again", StartGame);
			}
		}
	}

	public void PlayGame(){
		notification.GetMessage ("Briefing", "" +
			"You mistakenly kicked your ball into a mysterious hole, " +
			"as water drops on the ball, it either increases in size or reduces in size, " +
			"Set the ball to the target size by colliding with the water droplets, the ones with plus sign, increases the size" +
			" the ones with minuses sign reduces the ball, the ones with 0 has no effect, Every jump without collision results in the ball reducing in size, the target size is indicated on screen, REACH AND MAINTAIN THE TARGET BEFORE THE TIME COUNTS DOWN Goodluck!",
			"Start!", StartGame);
	}


	public delegate void OnGameStart(float _time);
	public static event OnGameStart GameStarted;
	public void StartGame(){
		if (GameStarted != null) {
			GameStarted (level._LevelTimes[level.GetSavedLevel ()]);
			LevelSpeed = level.speed [level.GetSavedLevel ()];
		}
	}

	public delegate void OnGameWon();
	public static event OnGameWon GameWon;
	public void PlayerWonLevel(){
		if (GameWon != null) {
			GameWon ();
			level.SetNextTime ();
			LevelSpeed = level.speed [level._currentLevelCounter];
			if(level.GetSavedLevel () == 9){
				notification.GetMessage ("Congratulations", "You have completed the Game!", "Play Again", ()=>{
					level.ResetLevel ();
					Home ();
				});
			}
			notification.GetMessage ("Congratulations", "You have won this level " + level._currentLevelCounter, "Next", StartGame);
		}
	}

	bool _searching;
	IEnumerator ReactivateInActiveFruits(){
		while (_searching) {
			yield return new WaitForSeconds (2f);
			if (FruitsEaten.Count != 0) {
				Fruits _fruit = FruitsEaten [Random.Range (0, FruitsEaten.Count)];
				FruitsEaten.Remove (_fruit);
				_fruit.ReactivateFruit ();
			}
		}
	}


}
