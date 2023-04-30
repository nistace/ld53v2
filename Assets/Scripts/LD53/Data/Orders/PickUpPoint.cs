using UnityEngine;
using Utils.Extensions;

namespace LD53.Data.Orders {
	public class PickUpPoint : MonoBehaviour {
		[SerializeField] protected BuildingInteractionArea _interactionArea;
		[SerializeField] protected DeliveryOrder[]         _orderPrefabs;

		public DeliveryOrder currentOrder     { get; private set; }
		public bool          canGenerateOrder => !currentOrder;

		private void Start() {
			_interactionArea.SetActive(false);
		}

		public DeliveryOrder GenerateOrder(int mealCountMax, DeliveryPoint deliveryPoint) {
			currentOrder = Instantiate(_orderPrefabs.Random());
			currentOrder.KeepLessThanMeals(mealCountMax);
			currentOrder.creationTime = Time.time;
			currentOrder.expectedDeliveryTime = Time.time + Vector3.SqrMagnitude(deliveryPoint.transform.position - transform.position) / 2;
			deliveryPoint.SetExpectedDelivery(currentOrder);
			RefreshInteractionArea();
			return currentOrder;
		}

		public void OnPickedUp() => RefreshInteractionArea();

		public void CancelOrder() {
			currentOrder = null;
			RefreshInteractionArea();
		}

		private void RefreshInteractionArea() {
			_interactionArea.SetActive(currentOrder && !currentOrder.pickedUp);
			_interactionArea.SetInteractable(currentOrder && !currentOrder.pickedUp);
		}
	}
}