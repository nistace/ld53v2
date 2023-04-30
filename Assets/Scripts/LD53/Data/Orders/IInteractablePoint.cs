using UnityEngine;

namespace LD53.Data.Orders {
	public interface IInteractablePoint {
		bool IsInteractable();

		Vector3 worldPosition { get; }
	}
}