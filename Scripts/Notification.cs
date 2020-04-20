using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Notification : MonoBehaviour {

	public Button _clickButton;
	public GameObject panel;
	public Text Message;
	public Text Title;
	public Text btnText;


	Manager manager;
	void Start(){
		manager = FindObjectOfType<Manager> ();
		//Manager.GameEnd += Manager_GameEnd;
	}


	public void GetMessage(string _title, string _message, string _btnText ="", UnityAction _onclick = null)
	{
		Title.text = _title;
		Message.text = _message;
		panel.SetActive (true);
		_clickButton.onClick.RemoveAllListeners ();
		if (_onclick != null) {
			_clickButton.onClick.AddListener (_onclick);
		}
		_clickButton.onClick.AddListener (()=>{
			panel.SetActive (false);
		});
		if (_btnText == "") {
			btnText.text = "Back";
		} else {
			btnText.text = _btnText;
		}
	}

}
