using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour {

	public Rigidbody2D rb;
	public ParticleSystem effects;
	Manager manager;

	public enum BallSize
	{
		Small, Medium, Large, Extra_Large
	}

	[Tooltip("Used to control the speed of the character")]
	[Range(5f, 30f)]
	public float movespeed;

	[Tooltip("Control the upwards force")]
	public float JumpForce;
	float Gravity;

	bool jumped;
	// Use this for initialization
	void Start () {
		rb = GetComponent <Rigidbody2D> ();	
		manager = FindObjectOfType<Manager> ();
	}

	bool TakenFruit;
	void OnCollisionEnter2D(Collision2D _object){
		if (_object.collider.CompareTag ("Ground")) {
			isGrounded = true;
			jumped = false;
			Gravity = 0f;
			if (!TakenFruit) {
				Boost (false, new Vector2(0.1f, 0.1f));
			} else {
				TakenFruit = false;
			}
		}
		if (_object.collider.CompareTag ("Fruit"))
		{
			//Debug.Log ("Hit Fruit");
			Fruits _fruits = _object.collider.GetComponent <Fruits> ();
			_fruits.PickFruit ();
			Boost (true, _fruits._EffectValue);
			TakenFruit = true;
			effects.Play ();
		}
	}

	public Text BallsizeDisplay;
	public BallSize size;
	public void ParseBallSize(){
		Vector2 _state = new Vector2 (transform.localScale.x, transform.localScale.y);
		if (_state.x >=0.4f && _state.x< 0.8f) {
			size = BallSize.Small;
		}
		if (_state.x >=0.8f && _state.x< 1f) {
			size = BallSize.Medium;
		}
		if (_state.x >=1f && _state.x< 1.4f) {
			size = BallSize.Large;
		}
		if (_state.x >1.4f && _state.x< 1.6f) {
			size = BallSize.Extra_Large;
		}
		BallsizeDisplay.text = size.ToString ();
	}


	void OnCollisionExit2D(Collision2D _object){
		if (_object.collider.CompareTag ("Ground")) {
			isGrounded = false;
		}
	}


	public void Boost(bool _isAdd, Vector2 _deductScale)
	{
		float _horizontalscale = 0f;
		float _verticalScale = 0f;
		if (!_isAdd) {
			_horizontalscale = transform.localScale.x - _deductScale.x;
			_verticalScale = transform.localScale.y - _deductScale.y;
		} else {
			_horizontalscale = transform.localScale.x + _deductScale.x;
			_verticalScale = transform.localScale.y + _deductScale.y;
		}
		if (_horizontalscale > 0.3f) {
			transform.localScale = new Vector3 (_horizontalscale, _verticalScale, 1f);
			if (isBelowThreshHold ()) {
				//Debug.Log ("Game Over");
				//manager.GameEnded ();
			}
		}
		ParseBallSize ();
	}


	bool isBelowThreshHold(){
		if (transform.localScale.y < 0.4f) {
			return true;
		} else {
			return false;
		}
	}

	public bool isGrounded;
	// Update is called once per frame
	void Update () {
		float x = Input.GetAxis ("Horizontal");
		float y = Input.GetAxis ("Vertical");
		Vector2 _movedir = new Vector2 (x, Gravity);
		rb.velocity = _movedir * movespeed;

		if (Input.GetKey (KeyCode.Space) && !jumped) {
			jumped = true;
			Gravity = JumpForce;
		}
		if (!isGrounded) {
			Gravity -= 0.1f;
		}
	}
}
