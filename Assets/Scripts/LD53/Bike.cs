using LD53;
using LD53.Inputs;
using UnityEngine;
using Utils.Extensions;

public class Bike : MonoBehaviour {
	[Header("Pedalling"), SerializeField] protected Rigidbody       _rigidbody;
	[SerializeField]                      protected float           _minSpeed;
	[SerializeField]                      protected float           _maxSpeed;
	[SerializeField]                      protected float           _speedLerp;
	[SerializeField]                      protected float           _acceleration        = .1f;
	[SerializeField]                      protected float           _deceleration        = -.05f;
	[SerializeField]                      protected float           _brakingAcceleration = -.5f;
	[SerializeField]                      protected RotatingThing[] _wheels;
	[SerializeField]                      protected PedalMechanism  _pedalMechanism;
	[Header("Turning"), SerializeField]   protected Transform       _directionTransform;
	[SerializeField]                      protected BikeHandlebar   _handlebar;
	[SerializeField]                      protected float           _lookSpeed = 1;
	[SerializeField]                      protected float           _turnSpeed = 1;
	

	private void Start() {
		SetMovementEnabled(true);
		SetLookAroundEnabled(true);
	}

	private void OnCollisionEnter(Collision collision) {
		Debug.Log(collision.gameObject.name);
	}

	public void SetMovementEnabled(bool enabled) {
		GameInput.controls.Bike.LeftPedal.SetEnabled(enabled);
		GameInput.controls.Bike.RightPedal.SetEnabled(enabled);
		GameInput.controls.Bike.Brake.SetEnabled(enabled);
	}

	public void SetLookAroundEnabled(bool enabled) {
		GameInput.controls.Bike.Aim.SetEnabled(enabled);
		Cursor.visible = !enabled;
		Cursor.lockState = enabled ? CursorLockMode.Locked : CursorLockMode.Confined;
	}

	private void FixedUpdate() {
		_rigidbody.velocity = transform.forward * Mathf.Lerp(_minSpeed, _maxSpeed, _speedLerp);
	}

	private void Update() {
		UpdatePedalling();
		UpdateDirection();
		foreach (var wheel in _wheels) {
			wheel.lerpRotatingSpeed = _speedLerp;
		}
	}

	private void UpdateDirection() {
		var aimChange = GameInput.controls.Bike.Aim.ReadValue<float>();
		if (!Mathf.Approximately(aimChange, 0)) {
			Debug.Log(aimChange);
			_directionTransform.localRotation *= Quaternion.Euler(0, aimChange * _lookSpeed * Time.deltaTime, 0);
		}
		var angle = Vector3.SignedAngle(transform.forward, _directionTransform.forward, Vector3.up);
		_handlebar.SetRotation(angle);
		if (Mathf.Approximately(angle, 0)) return;
		var turnRotation = Quaternion.Euler(0, angle * _turnSpeed * _speedLerp * Time.deltaTime, 0);
		transform.rotation *= turnRotation;
		_directionTransform.localRotation *= Quaternion.Inverse(turnRotation);
	}

	private void UpdatePedalling() {
		if (GameInput.controls.Bike.Brake.inProgress) {
			_speedLerp = Mathf.Clamp01(_speedLerp + _brakingAcceleration * Time.deltaTime);
			_pedalMechanism.rotatingPart.lerpRotatingSpeed = 0;
		}
		else {
			var pedallingInput = GetPedallingInput();
			_speedLerp = Mathf.Clamp01(_speedLerp + (pedallingInput > 0 ? pedallingInput * _acceleration : _deceleration) * Time.deltaTime);
			_pedalMechanism.rotatingPart.lerpRotatingSpeed = pedallingInput * _speedLerp;
		}
	}

	private float GetPedallingInput() {
		if (GameInput.controls.Bike.LeftPedal.inProgress) {
			if (GameInput.controls.Bike.RightPedal.inProgress) return _pedalMechanism.leftPedalEfficiency * _pedalMechanism.rightPedalEfficiency;
			return _pedalMechanism.leftPedalEfficiency;
		}
		if (GameInput.controls.Bike.RightPedal.inProgress) return _pedalMechanism.rightPedalEfficiency;
		return 0;
	}
}