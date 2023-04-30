using LD53.Data.Orders;
using UnityEngine;
using UnityEngine.Events;

public class DeliveryPoint : MonoBehaviour {
	public class Event : UnityEvent<DeliveryPoint> { }

	[SerializeField] protected BuildingInteractionArea _interactionArea;
	[SerializeField] protected DeliveryOrder           _expectedDelivery;

	public DeliveryOrder expectedDelivery => _expectedDelivery;

	private void Start() => RefreshInteractionArea();

	public void SetExpectedDelivery(DeliveryOrder order) {
		_expectedDelivery = order;
		RefreshInteractionArea();
	}

	public void OnDeliveryPickedUp() => RefreshInteractionArea();

	public void CancelOrder() {
		_expectedDelivery = null;
		RefreshInteractionArea();
	}

	private void RefreshInteractionArea() {
		_interactionArea.SetActive(_expectedDelivery);
		_interactionArea.SetInteractable(_expectedDelivery && _expectedDelivery.pickedUp);
	}
}