using UnityEngine;

namespace LD53.Data.Common {
	public class RotatingThing : MonoBehaviour {
		[SerializeField] protected float   _minRotatingThing;
		[SerializeField] protected float   _maxRotatingThing  = 30;
		[SerializeField] protected float   _lerpRotatingSpeed = 0;
		[SerializeField] protected Vector3 _rotatingAxis      = Vector3.right;

		public float lerpRotatingSpeed {
			get => _lerpRotatingSpeed;
			set => _lerpRotatingSpeed = value;
		}

		private void Update() => transform.localRotation *= Quaternion.Euler(_rotatingAxis * (Time.deltaTime * Mathf.Lerp(_minRotatingThing, _maxRotatingThing, _lerpRotatingSpeed)));
	}
}