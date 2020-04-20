using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelScript : MonoBehaviour{
	public float [] _LevelTimes;
	public float [] speed;
	public PlayerController.BallSize [] sizes;
	public int _currentLevelCounter;

	public PlayerController.BallSize currentsizeTarget;

	public UnityEngine.UI.Text _targetSizeDisplay;

	public UnityEngine.UI.Text _Leveltext;
	void Start(){
		PlayerPrefs.SetInt ("Level", 0);
		_currentLevelCounter = GetSavedLevel ();
		currentsizeTarget = sizes [_currentLevelCounter];
		_Leveltext.text = "Level" + Levelvalue.ToString ();
	}


	int Levelvalue;
	public int GetSavedLevel(){
		_currentLevelCounter = PlayerPrefs.GetInt ("Level");
		_targetSizeDisplay.text = sizes [_currentLevelCounter].ToString ();
		return _currentLevelCounter;
	}

	public void SetNextTime(){
		_currentLevelCounter++;
		_targetSizeDisplay.text = sizes [_currentLevelCounter].ToString ();
		PlayerPrefs.SetInt ("Level", _currentLevelCounter);
	}

	public void ResetLevel(){
		PlayerPrefs.SetInt ("Level", 0);
	}


	void Update(){
		Levelvalue = _currentLevelCounter + 1;
		_Leveltext.text = "Level " + Levelvalue.ToString ();
	}
}
