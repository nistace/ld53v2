using UnityEngine;
using Utils.Extensions;
using Utils.StaticUtils;

namespace LD53.Data.Orders {
	public class PickUpPoint : MonoBehaviour, IInteractablePoint {
		[SerializeField] protected BuildingInteractionArea _interactionArea;

		public DeliveryOrder currentOrder     { get; private set; }
		public bool          canGenerateOrder => currentOrder == null;
		public Vector3       worldPosition    => transform.position;

		private void Start() {
			_interactionArea.SetActive(false);
		}

		public DeliveryOrder GenerateOrder(DeliveryPoint deliveryPoint) {
			CancelOrder();
			currentOrder = new DeliveryOrder(Vector3.Magnitude(deliveryPoint.worldPosition - worldPosition));
			currentOrder.onPickedUp.AddListenerOnce(HandleCurrentOrderPickedUp);
			deliveryPoint.SetExpectedDelivery(currentOrder);
			RefreshInteractionArea();
			return currentOrder;
		}

		private void HandleCurrentOrderPickedUp() => RefreshInteractionArea();

		public void CancelOrder() {
			if (currentOrder == null) return;
			currentOrder.onPickedUp.RemoveListener(HandleCurrentOrderPickedUp);
			currentOrder = null;
			RefreshInteractionArea();
		}

		private void RefreshInteractionArea() {
			var activeAndInteractable = CheckInteractable(out var reason);
			_interactionArea.SetActive(activeAndInteractable);
			_interactionArea.SetInteractable(activeAndInteractable, reason);
		}

		private bool CheckInteractable(out string reason) {
			if (currentOrder == null) return "This place is not delivering anything now.".False(out reason);
			if (currentOrder.pickedUp) return "This order has already been picked up.".False(out reason);
			return string.Empty.True(out reason);
		}

		public bool IsInteractable() => _interactionArea.IsInteractable();
	}
}