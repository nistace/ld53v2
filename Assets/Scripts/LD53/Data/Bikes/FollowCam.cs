using UnityEngine;
using UnityEngine.Serialization;

namespace LD53.Data.Bikes {
	public class FollowCam : MonoBehaviour {
		public enum Strategy {
			FollowCam,
			WatchTarget,
		}

		[SerializeField] protected Transform _target;
		[SerializeField] protected Strategy  _strategy;
		[SerializeField] protected float     _followSmooth;
		[SerializeField] protected Vector3   _followVelocity;
		[SerializeField] protected Transform _cameraTransform;

		public Transform target {
			get => _target;
			set => _target = value;
		}

		public Strategy strategy {
			get => _strategy;
			set => _strategy = value;
		}

		private void FixedUpdate() {
			if (!target) return;
			if (strategy == Strategy.FollowCam) {
				_cameraTransform.localPosition = Vector3.zero;
				_cameraTransform.localRotation = Quaternion.identity;
				transform.position = Vector3.SmoothDamp(transform.position, _target.position, ref _followVelocity, _followSmooth);
				transform.forward = _target.forward;
			}
			else if (strategy == Strategy.WatchTarget) {
				_cameraTransform.forward = _target.position - _cameraTransform.position;
			}
		}

		public void Jump() {
			if (!target) return;
			transform.position = _target.position;
			transform.forward = _target.forward;
			_followVelocity = Vector3.zero;
		}
	}
}