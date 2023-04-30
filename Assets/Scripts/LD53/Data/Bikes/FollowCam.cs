using System.Linq;
using UnityEngine;

namespace LD53.Data.Bikes {
	public class FollowCam : MonoBehaviour {
		public enum Strategy {
			FollowCam,
			WatchTarget,
		}

		private static RaycastHit[] nonAllocHits { get; } = new RaycastHit[8];

		[SerializeField]                              protected Transform _target;
		[SerializeField]                              protected Strategy  _strategy;
		[SerializeField]                              protected float     _followSmooth;
		[SerializeField]                              protected Vector3   _followVelocity;
		[SerializeField]                              protected Transform _cameraTransform;
		[Header("Smart Positioning"), SerializeField] protected Transform _keepDistanceFrom;
		[SerializeField]                              protected Transform _cameraDistanceTransform;
		[SerializeField]                              protected float     _cameraDistanceMinDistance      = .5f;
		[SerializeField]                              protected float     _preferredCameraDistance        = 8;
		[SerializeField]                              protected float     _cameraDistanceSphereCastRadius = .2f;
		[SerializeField]                              protected LayerMask _cameraDistanceCheckMask;

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

			if (_keepDistanceFrom) {
				var hits = Physics.SphereCastNonAlloc(_keepDistanceFrom.position, _cameraDistanceSphereCastRadius, -_cameraDistanceTransform.forward, nonAllocHits, _preferredCameraDistance,
					_cameraDistanceCheckMask, QueryTriggerInteraction.Collide);
				if (hits == 0) {
					_cameraDistanceTransform.localPosition = Vector3.back * _preferredCameraDistance;
				}
				else {
					_cameraDistanceTransform.localPosition = Vector3.back * Mathf.Max(_cameraDistanceMinDistance, nonAllocHits.Take(hits).Min(t => t.distance));
				}
			}
		}

		public void Jump() {
			if (!target) return;
			transform.position = _target.position;
			transform.forward = _target.forward;
			_followVelocity = Vector3.zero;
		}

#if UNITY_EDITOR
		private void OnDrawGizmos() {
			if (!_keepDistanceFrom) return;
			Gizmos.color = new Color(.8f, .8f, .3f, .5f);
			var castDirection = (_cameraDistanceTransform.position - _keepDistanceFrom.position).normalized;
			for (var i = 0; i < _preferredCameraDistance / _cameraDistanceSphereCastRadius; ++i) {
				Gizmos.DrawSphere(_keepDistanceFrom.position + castDirection * i * _cameraDistanceSphereCastRadius, _cameraDistanceSphereCastRadius);
			}
		}
#endif
	}
}