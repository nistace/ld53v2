using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class MoveRandomly : MonoBehaviour {
	[SerializeField] protected Vector3 _min;
	[SerializeField] protected Vector3 _max;
	[SerializeField] protected Vector3 _next;
	[SerializeField] protected float   _speed = .23f;

	private void Start() {
		_next = Vector3.Lerp(_min, _max, Random.value);
	}

	private void Update() {
		transform.localPosition = Vector3.MoveTowards(transform.localPosition, _next, _speed * Time.deltaTime);
		if (transform.localPosition == _next) {
			_next = Vector3.Lerp(_min, _max, Random.value);
		}
	}

#if UNITY_EDITOR
	private void OnDrawGizmos() {
		Gizmos.color = Color.magenta;
		Gizmos.DrawSphere(transform.position, .2f);
	}
#endif
}