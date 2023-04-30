using UnityEngine;
using Utils.Behaviours;

namespace LD53.Data.Bikes {
	public class Deliverer : MonoBehaviour {
		[SerializeField] protected Animator    _animator;
		[SerializeField] protected Transform   _leftHandIkTarget;
		[SerializeField] protected Transform   _rightHandIkTarget;
		[SerializeField] protected Transform   _leftFootIkTarget;
		[SerializeField] protected Transform   _rightFootIkTarget;
		[SerializeField] protected Transform   _lookTowards;
		[SerializeField] protected Ragdoll     _ragdoll;
		[SerializeField] protected Rigidbody[] _crashDetachedComponents;
		[SerializeField] protected Vector3     _additionalCrashForce                   = Vector3.up;
		[SerializeField] protected float       _forceMultiplier                        = 300;
		[SerializeField] protected float       _crashDetachedComponentsForceMultiplier = .3f;
		[SerializeField] protected Transform   _headPosition;

		public Transform headPosition => _headPosition;

		public void EnableRagdoll(Vector3 force) {
			transform.SetParent(null);
			_animator.enabled = false;
			_ragdoll.enabled = true;
			_ragdoll.AddForce(string.Empty, _additionalCrashForce - force * _forceMultiplier, ForceMode.Impulse);
			foreach (var crashDetachedComponent in _crashDetachedComponents) {
				crashDetachedComponent.GetComponent<Collider>().enabled = true;
				crashDetachedComponent.isKinematic = false;
				crashDetachedComponent.AddForce(force * _crashDetachedComponentsForceMultiplier, ForceMode.Impulse);
				crashDetachedComponent.transform.SetParent(null);
			}
		}

		private void OnAnimatorIK(int layerIndex) {
			_animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
			_animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1);
			_animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
			_animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1);
			_animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
			_animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
			_animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
			_animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
			_animator.SetLookAtWeight(1);
			_animator.SetIKPosition(AvatarIKGoal.LeftFoot, _leftFootIkTarget.position);
			_animator.SetIKRotation(AvatarIKGoal.LeftFoot, _leftFootIkTarget.rotation);
			_animator.SetIKPosition(AvatarIKGoal.RightFoot, _rightFootIkTarget.position);
			_animator.SetIKRotation(AvatarIKGoal.RightFoot, _rightFootIkTarget.rotation);
			_animator.SetIKPosition(AvatarIKGoal.LeftHand, _leftHandIkTarget.position);
			_animator.SetIKRotation(AvatarIKGoal.LeftHand, _leftHandIkTarget.rotation);
			_animator.SetIKPosition(AvatarIKGoal.RightHand, _rightHandIkTarget.position);
			_animator.SetIKRotation(AvatarIKGoal.RightHand, _rightHandIkTarget.rotation);
			_animator.SetLookAtPosition(_lookTowards.position);
		}

		private void OnDestroy() {
			foreach (var crashDetachedComponent in _crashDetachedComponents) {
				if (crashDetachedComponent) Destroy(crashDetachedComponent.gameObject);
			}
		}
	}
}