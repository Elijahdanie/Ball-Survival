using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCounter : MonoBehaviour {

	Manager manager;


	void Start(){
		manager = FindObjectOfType<Manager> ();
		Manager.GameStarted += Manager_GameStarted;
		Manager.GameEnd += Manager_GameEnd;
		Manager.GameWon += Manager_GameWon;
		Manager.GoHome += Manager_GoHome;
	}

	void Manager_GoHome ()
	{
		Manager.GoHome -= Manager_GoHome;
		ResetParameters ();
		Manager.GoHome += Manager_GoHome;
	}

	void Manager_GameWon ()
	{
		Manager.GameWon -= Manager_GameWon;
		ResetParameters ();
		Manager.GameWon += Manager_GameWon;
	}

	void Manager_GameEnd ()
	{
		Manager.GameEnd -= Manager_GameEnd;
		ResetParameters ();
		Manager.GameEnd += Manager_GameEnd;
	}

	void ResetParameters(){
		isCounting = false;
		timedisplay.text = "";
		//time = 0f;
	}

	void Manager_GameStarted (float _timedesignated)
	{
		Manager.GameStarted -= Manager_GameStarted;
		if(isCounting == false)
			Counttime (_timedesignated);
		Manager.GameStarted += Manager_GameStarted;
	}
	public Text timedisplay;
	public void Counttime(float _value){
		time = _value;
		isCounting = true;
		StartCoroutine (_startTime (time));
	}


	float time;
	WaitForSeconds _second = new WaitForSeconds(1f);
	bool isCounting;
	IEnumerator _startTime(float _param){
		while (isCounting) {
			yield return _second;
			time--;
			timedisplay.text = time.ToString ();
			if (time <= 0f) {
				manager.GameEnded ();
			}
		}
	}
}
