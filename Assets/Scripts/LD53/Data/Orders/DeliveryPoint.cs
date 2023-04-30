using LD53.Data.Orders;
using UnityEngine;
using UnityEngine.Events;
using Utils.Extensions;
using Utils.StaticUtils;

public class DeliveryPoint : MonoBehaviour, IInteractablePoint {
	public class Event : UnityEvent<DeliveryPoint> { }

	[SerializeField] protected BuildingInteractionArea _interactionArea;
	[SerializeField] protected DeliveryOrder           _expectedDelivery;

	public DeliveryOrder expectedDelivery => _expectedDelivery;
	public Vector3       worldPosition    => transform.position;

	private void Start() => RefreshInteractionArea();

	public void SetExpectedDelivery(DeliveryOrder order) {
		CancelOrder();
		_expectedDelivery = order;
		_expectedDelivery.onPickedUp.AddListenerOnce(HandleDeliveryPickedUp);
		RefreshInteractionArea();
	}

	private void HandleDeliveryPickedUp() => RefreshInteractionArea();

	public void CancelOrder() {
		if (!_expectedDelivery) return;
		_expectedDelivery.onPickedUp.RemoveListener(HandleDeliveryPickedUp);
		_expectedDelivery = null;
		RefreshInteractionArea();
	}

	private void RefreshInteractionArea() {
		_interactionArea.SetActive(_expectedDelivery);
		_interactionArea.SetInteractable(CheckInteractable(out var reason), reason);
	}

	private bool CheckInteractable(out string reason) {
		if (!_expectedDelivery) return "This place is not expecting any delivery.".False(out reason);
		if (!_expectedDelivery.pickedUp) return "You need to pick up the order before you can deliver it.".False(out reason);
		return string.Empty.True(out reason);
	}

	public bool IsInteractable() => _interactionArea.IsInteractable();
}