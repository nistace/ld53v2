using UnityEngine;
using UnityEngine.Events;

namespace LD53.Data.Orders {
	public class BuildingInteractionArea : MonoBehaviour {
		public class Event : UnityEvent<BuildingInteractionArea> { }

		[SerializeField] protected bool         _interactable;
		[SerializeField] protected MeshRenderer _areaRenderer;
		[SerializeField] protected GameObject   _disabledIcon;
		[SerializeField] protected string       _notInteractableReason;

		public string notInteractableReason => _notInteractableReason;

		public void SetActive(bool active) {
			enabled = active;
			gameObject.SetActive(active);
		}

		public bool IsInteractable() => enabled && _interactable;

		public void SetInteractable(bool interactable, string reason = null) {
			_interactable = interactable;
			_areaRenderer.material.color = _interactable ? Color.green : Color.red;
			_disabledIcon.SetActive(!interactable);
			_notInteractableReason = reason;
		}
	}
}