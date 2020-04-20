using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Fruits : MonoBehaviour {

	#region VARAIBLES
	public enum FruitTtype
	{
		Good, bad, Poisonous, Lethal
	}	

	public Text stateDisplay;

	public Vector2 _EffectValue = new Vector2();
	public FruitTtype fruittype;
	public Image fruitImage;

	RectTransform rect;
	public bool isEaten;
	Vector2 PlayerTarget;
	public Vector2 _activeTarget;
	Vector2 LaunchPosition;
	bool isActive;


	public List<Vector2> _targets = new List<Vector2>();
	Manager manager;
	#endregion

	void Start(){
		manager = FindObjectOfType<Manager> ();
		Manager.GameStarted += Manager_GameStarted;
		rect = GetComponent <RectTransform> ();
		fruitImage = GetComponent <Image> ();
		Manager.GameEnd += Manager_GameEnd;
		Manager.GameWon += Manager_GameWon;
		Manager.GoHome += Manager_GoHome;
	}

	void Manager_GoHome ()
	{
		Manager.GoHome -= Manager_GoHome;
		PauseGame ();
		Manager.GoHome += Manager_GoHome;
	}

	void Manager_GameWon ()
	{
		Manager.GameWon -= Manager_GameWon;
		PauseGame ();
		Manager.GameWon += Manager_GameWon;
	}

	void Manager_GameEnd ()
	{
		Manager.GameEnd -= Manager_GameEnd;
		PauseGame ();
		Manager.GameEnd += Manager_GameEnd;
	}

	void Manager_GameStarted (float _starttime)
	{
		Manager.GameStarted -= Manager_GameStarted;
		gameObject.SetActive (true);
		Invoke ("InitializeFruit", Random.Range (1f, 5f));
		ReshuffleType ();
		Manager.GameStarted += Manager_GameStarted;
	}

	public void ReshuffleType(){
		fruittype = (FruitTtype)Random.Range (0, 3);
		switch ((int)fruittype) {
		case (int)FruitTtype.Good:
			_EffectValue = new Vector2 (0.1f, 0.1f);
			stateDisplay.text = "+";
			fruitImage.color = Color.green;
			break;
		case (int)FruitTtype.bad:
			_EffectValue = new Vector2 (0f, 0f);
			fruitImage.color = Color.grey;
			stateDisplay.text = "0";
			break;
		case (int)FruitTtype.Poisonous:
			_EffectValue = new Vector2 (-0.02f, -0.02f);
			fruitImage.color = Color.red;
			stateDisplay.text = "-";
			break;
		case (int)FruitTtype.Lethal:
			_EffectValue = new Vector2 (-0.1f, -0.1f);
			fruitImage.color = Color.red;
			stateDisplay.text = "-";
			break;
		default:
			break;
		}
	}

	#region KEY_nAVIGATION_sWITCH_METHODS
	public void PickFruit(){
		manager.PickFruit (this);
		isEaten = true;
		//pool it
		//Debug.Log ("eatfruit");
		gameObject.SetActive (false);
		rect.anchoredPosition= LaunchPosition;
	}

	void PauseGame(){
		isEaten = true;
		//pool it
		gameObject.SetActive (false);
		rect.anchoredPosition= LaunchPosition;
	}

	public void InitializeFruit(){
		//initailize target and original
		LaunchPosition = rect.anchoredPosition;
		gameObject.SetActive (true);
		PlayerTarget = new Vector2(rect.anchoredPosition.x, manager.PlayerThreshold.anchoredPosition.y);
		_targets.Add (PlayerTarget);
		_targets.Add (LaunchPosition);
		_activeTarget = PlayerTarget;
		isActive = true;
		//Debug.Log ("Initialized");
		isEaten = false;
	}


	public void ReactivateFruit(){
		_toggler = 0;
		gameObject.SetActive (true);
		_activeTarget = PlayerTarget;
		isActive = true;
		//Debug.Log ("Initialized");
		isEaten = false;
	}

	#endregion

	public int _toggler;
	void Update()
	{
		if (!isEaten && isActive) {
			//Debug.Log (Vector3.Distance (transform.position, _activeTarget));
			if (Vector2.Distance (rect.anchoredPosition, _activeTarget) < 1f) {
				_toggler++;
				if (_toggler > 1) {
					_toggler = 0;
				}
				//Debug.Log ("detect");
				_activeTarget = _targets [_toggler];
				if (_toggler == 0) {
					ReshuffleType ();
				}
			} else {
				rect.anchoredPosition =  Vector2.MoveTowards (rect.anchoredPosition, _activeTarget, manager.LevelSpeed);
			}
		}
	}
}
