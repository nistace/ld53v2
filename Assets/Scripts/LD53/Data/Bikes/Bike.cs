using LD53.Data.Common;
using LD53.Data.Orders;
using LD53.Inputs;
using UnityEngine;
using Utils.Events;
using Utils.Extensions;

namespace LD53.Data.Bikes {
	public class Bike : MonoBehaviour {
		[SerializeField]                      protected Deliverer       _deliverer;
		[Header("Pedalling"), SerializeField] protected Rigidbody       _rigidbody;
		[SerializeField]                      protected float           _minSpeed;
		[SerializeField]                      protected float           _maxSpeed = 6;
		[SerializeField]                      protected float           _speedLerp;
		[SerializeField]                      protected float           _acceleration        = .1f;
		[SerializeField]                      protected float           _deceleration        = -.05f;
		[SerializeField]                      protected float           _brakingAcceleration = -.5f;
		[SerializeField]                      protected RotatingThing[] _wheels;
		[SerializeField]                      protected PedalMechanism  _pedalMechanism;
		[Header("Turning"), SerializeField]   protected Transform       _directionTransform;
		[SerializeField]                      protected BikeHandlebar   _handlebar;
		[SerializeField]                      protected AnimationCurve  _lookSpeed            = AnimationCurve.Linear(0, 1, 1, .5f);
		[SerializeField]                      protected AnimationCurve  _turnSpeed            = AnimationCurve.Linear(0, 1, 1, .5f);
		[Header("Collision"), SerializeField] protected float           _crashMinSqrMagnitude = 1000;
		[SerializeField]                      protected bool            _crashed;

		public  Transform                     directionTransform         => _directionTransform;
		private BuildingInteractionArea       inInteractionArea          { get; set; }
		public  Vector3Event                  onCollisionHappened        { get; } = new Vector3Event();
		public  BuildingInteractionArea.Event onStoppedInInteractionArea { get; } = new BuildingInteractionArea.Event();
		public  Deliverer                     deliverer                  => _deliverer;
		public  PedalMechanism                pedalMechanism             => _pedalMechanism;
		public  bool                          crashed                    => _crashed;

		private void Start() {
			SetMovementEnabled(true);
			SetLookAroundEnabled(true);
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
			if (_crashed) return;
			transform.forward = transform.forward.With(y: 0);
			_rigidbody.velocity = transform.forward * Mathf.Lerp(_minSpeed, _maxSpeed, _speedLerp);
			_rigidbody.angularVelocity = Vector3.zero;
		}

		private void Update() {
			UpdatePedalling();
			UpdateDirection();
			foreach (var wheel in _wheels) {
				wheel.lerpRotatingSpeed = _speedLerp;
			}
			UpdateInteractionArea();
		}

		private void UpdateInteractionArea() {
			if (_crashed) return;
			if (!Mathf.Approximately(_speedLerp, 0)) return;
			if (!inInteractionArea) return;
			if (!inInteractionArea.IsInteractable()) return;
			onStoppedInInteractionArea.Invoke(inInteractionArea);
		}

		private void UpdateDirection() {
			var aimChange = GameInput.controls.Bike.Aim.ReadValue<float>();
			if (!Mathf.Approximately(aimChange, 0)) _directionTransform.localRotation *= Quaternion.Euler(0, aimChange * _lookSpeed.Evaluate(_speedLerp) * Time.deltaTime, 0);
			var angle = Vector3.SignedAngle(transform.forward, _directionTransform.forward, Vector3.up);
			_handlebar.SetRotation(angle);
			if (Mathf.Approximately(angle, 0)) return;
			var turnRotation = Quaternion.Euler(0, angle * _turnSpeed.Evaluate(_speedLerp) * Time.deltaTime, 0);
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

		public void Crash(Vector3 force) {
			_crashed = true;
			_speedLerp = 0;
			_deliverer.EnableRagdoll(force);
			_rigidbody.constraints = RigidbodyConstraints.None;
			_rigidbody.AddForce(force, ForceMode.Impulse);
		}

		private void OnDestroy() {
			if (_deliverer) Destroy(_deliverer.gameObject);
		}

		private void OnTriggerEnter(Collider other) {
			if (!other.gameObject.TryGetComponentInParent<BuildingInteractionArea>(out var interactionArea)) return;
			inInteractionArea = interactionArea;
		}

		private void OnTriggerExit(Collider other) {
			if (!other.gameObject.TryGetComponentInParent<BuildingInteractionArea>(out var interactionArea) && inInteractionArea == interactionArea) return;
			inInteractionArea = null;
		}

		private void OnCollisionEnter(Collision collision) {
			if (collision.impulse.sqrMagnitude > _crashMinSqrMagnitude) {
				onCollisionHappened.Invoke(collision.impulse);
			}
		}
	}
}