using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils.Extensions;

namespace LD53.Data.Orders {
	public class DeliveryManager : MonoBehaviour {
		public static int maxOrderSize { get; set; }

		[SerializeField] protected DeliveryPoint[] _deliveryPoints;
		[SerializeField] protected PickUpPoint[]   _pickUpPoints;

		public static List<DeliveryOrder> activeOrders { get; } = new List<DeliveryOrder>();

		public void Setup() {
			if (activeOrders.Count == 0) {
				CreateNewOrder();
			}
		}

		private void TimeOut(int deliveryIndex) {
			foreach (var pickUpPoint in _pickUpPoints.Where(t => t.currentOrder == activeOrders[deliveryIndex])) {
				pickUpPoint.CancelOrder();
			}
			foreach (var deliveryPoint in _deliveryPoints.Where(t => t.expectedDelivery == activeOrders[deliveryIndex])) {
				deliveryPoint.CancelOrder();
			}
			activeOrders.RemoveAt(deliveryIndex);
			// TODO Handle Timeout
		}

		public void PickUp(PickUpPoint from, Inventory to) {
			from.currentOrder.pickedUp = true;
			to.AddOrder(from.currentOrder);
			from.OnPickedUp();
			foreach (var deliveryPoint in _deliveryPoints.Where(t => t.expectedDelivery == from.currentOrder)) {
				deliveryPoint.OnDeliveryPickedUp();
			}
			Debug.Log("Picked Up");
		}

		public void Deliver(Inventory from, DeliveryPoint to) {
			from.RemoveOrder(to.expectedDelivery);
			foreach (var pickUpPoint in _pickUpPoints.Where(t => t.currentOrder == to.expectedDelivery)) {
				pickUpPoint.CancelOrder();
			}
			to.CancelOrder();
			Debug.Log("Delivered");
			// TODO Handle delivery
		}

		private void Update() {
			for (var index = 0; index < activeOrders.Count; index++) {
				if (activeOrders[index].pickedUp) continue;
				if (Time.time <= activeOrders[index].expectedDeliveryTime) continue;
				TimeOut(index);
				index--;
			}
		}

		private bool CreateNewOrder() {
			var randomDeliveryPoint = _deliveryPoints.Where(t => !t.expectedDelivery).RandomOrDefault();
			if (!randomDeliveryPoint) return false;
			var randomPickUpPoint = _pickUpPoints.Where(t => !t.currentOrder).RandomOrDefault();
			if (!randomPickUpPoint) return false;
			activeOrders.Add(randomPickUpPoint.GenerateOrder(maxOrderSize, randomDeliveryPoint));
			return true;
		}

		[ContextMenu("Find all points")]
		private void FindAllPoints() {
			_deliveryPoints = GetComponentsInChildren<DeliveryPoint>();
			_pickUpPoints = GetComponentsInChildren<PickUpPoint>();
		}

		public void HandleInteraction(BuildingInteractionArea interactionArea, Inventory inventory) {
			if (!interactionArea.IsInteractable()) return;
			if (interactionArea.gameObject.TryGetComponentInParent<PickUpPoint>(out var pickUpPoint)) PickUp(pickUpPoint, inventory);
			else if (interactionArea.gameObject.TryGetComponentInParent<DeliveryPoint>(out var deliveryPoint)) Deliver(inventory, deliveryPoint);
		}
	}
}