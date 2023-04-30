using UnityEngine;

namespace LD53 {
	public class BikeHandlebar : MonoBehaviour {
		[SerializeField] protected float _handlebarMaxRotation;

		public void SetRotation(float angleWithForward) {
			transform.localRotation = Quaternion.Euler(0, Mathf.Clamp(angleWithForward, -_handlebarMaxRotation, _handlebarMaxRotation), 0);
		}
	}
}