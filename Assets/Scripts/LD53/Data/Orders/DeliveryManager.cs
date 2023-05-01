using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Utils.Extensions;
using Utils.Types;

namespace LD53.Data.Orders {
	public class DeliveryManager : MonoBehaviour {
		public class OrderEvent : UnityEvent<DeliveryOrder, DeliveryPoint, PickUpPoint> { }

		[SerializeField] protected DeliveryPoint[] _deliveryPoints;
		[SerializeField] protected PickUpPoint[]   _pickUpPoints;
		[SerializeField] protected FloatRange      _delayBetweenNewOrders = (5, 10);
		[SerializeField] protected int             _maxOrders             = 4;
		[SerializeField] protected float           _newOrderCooldown;

		private static Dictionary<DeliveryOrder, (PickUpPoint pickUp, DeliveryPoint delivery)> orderProps { get; } = new Dictionary<DeliveryOrder, (PickUpPoint, DeliveryPoint)>();

		private static PlayerInventory playerInventory { get; set; }

		public static OrderEvent onOrderCreated   { get; } = new OrderEvent();
		public static OrderEvent onOrderDelivered { get; } = new OrderEvent();
		public static OrderEvent onOrderRemoved   { get; } = new OrderEvent();
		public static OrderEvent onOrderPickedUp  { get; } = new OrderEvent();
		public static bool       creationEnabled  { get; set; }

		public void Setup() {
			orderProps.Clear();
			foreach (var deliveryPoint in _deliveryPoints) deliveryPoint.CancelOrder();
			foreach (var pickUpPoint in _pickUpPoints) pickUpPoint.CancelOrder();
			playerInventory = new PlayerInventory();
		}

		private static bool TryGetProps(DeliveryOrder order, out PickUpPoint pickUp, out DeliveryPoint delivery) {
			pickUp = default;
			delivery = default;
			if (!orderProps.ContainsKey(order)) return false;
			(pickUp, delivery) = orderProps[order];
			return true;
		}

		private static void RemoveOrder(DeliveryOrder order) {
			if (!TryGetProps(order, out var pickUp, out var delivery)) return;
			pickUp.CancelOrder();
			delivery.CancelOrder();
			playerInventory.RemoveOrder(order);
			orderProps.Remove(order);
			onOrderRemoved.Invoke(order, delivery, pickUp);
		}

		private static void PickUp(DeliveryOrder order) {
			if (!TryGetProps(order, out var pickUp, out var delivery)) return;
			order.MarkAsPickedUp();
			playerInventory.AddOrder(order);
			onOrderPickedUp.Invoke(order, delivery, pickUp);
		}

		private static void Deliver(DeliveryOrder order) {
			if (!TryGetProps(order, out var pickUp, out var delivery)) return;
			RemoveOrder(order);
			onOrderDelivered.Invoke(order, delivery, pickUp);
		}

		private void Update() {
			if (!creationEnabled) return;
			if (orderProps.Count != 0) {
				_newOrderCooldown -= Time.deltaTime;
				if (_newOrderCooldown > 0) return;
			}
			if (orderProps.Count < _maxOrders) CreateNewOrder();
			_newOrderCooldown = _delayBetweenNewOrders.Random();
		}

		private void CreateNewOrder() {
			var randomDeliveryPoint = _deliveryPoints.Where(t => t.canOrder).RandomOrDefault();
			if (!randomDeliveryPoint) return;
			var randomPickUpPoint = _pickUpPoints.Where(t => t.canGenerateOrder).RandomOrDefault();
			if (!randomPickUpPoint) return;
			var newOrder = randomPickUpPoint.GenerateOrder(randomDeliveryPoint);
			orderProps.Add(newOrder, (randomPickUpPoint, randomDeliveryPoint));
			onOrderCreated.Invoke(newOrder, randomDeliveryPoint, randomPickUpPoint);
		}

		[ContextMenu("Find all points")]
		private void FindAllPoints() {
			_deliveryPoints = GetComponentsInChildren<DeliveryPoint>();
			_pickUpPoints = GetComponentsInChildren<PickUpPoint>();
		}

		public void HandleInteraction(BuildingInteractionArea interactionArea) {
			if (!interactionArea.IsInteractable()) return;
			if (interactionArea.gameObject.TryGetComponentInParent<PickUpPoint>(out var pickUpPoint)) PickUp(pickUpPoint.currentOrder);
			else if (interactionArea.gameObject.TryGetComponentInParent<DeliveryPoint>(out var deliveryPoint)) Deliver(deliveryPoint.expectedDelivery);
		}
	}
}