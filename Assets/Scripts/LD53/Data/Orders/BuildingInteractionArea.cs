using UnityEngine;
using UnityEngine.Events;

namespace LD53.Data.Orders {
	public class BuildingInteractionArea : MonoBehaviour {
		public class Event : UnityEvent<BuildingInteractionArea> { }

		[SerializeField] protected bool         _interactable;
		[SerializeField] protected MeshRenderer _areaRenderer;
		[SerializeField] protected MeshRenderer _iconRenderer;

		public void SetActive(bool active) {
			enabled = active;
			gameObject.SetActive(active);
		}

		public bool IsInteractable() => enabled && _interactable;

		public void SetInteractable(bool interactable) {
			_interactable = interactable;
			_areaRenderer.material.color = _interactable ? Color.green : Color.red;
		}
	}
}