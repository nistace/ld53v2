using UnityEngine;
using Utils.Extensions;

public class FollowCam : MonoBehaviour {
	[SerializeField] protected Transform _follow;
	[SerializeField] protected float     _followSmooth;
	[SerializeField] protected Vector3   _followVelocity;
	[SerializeField] protected float     _forwardSmooth;
	[SerializeField] protected Vector3   _forwardVelocity;

	private void FixedUpdate() {
		transform.position = Vector3.SmoothDamp(transform.position, _follow.position, ref _followVelocity, _followSmooth);
		transform.forward = _follow.forward;
	}
}