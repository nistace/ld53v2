using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Utils.Extensions;
using Utils.Types;

namespace LD53.Data.Orders {
	public class DeliveryManager : MonoBehaviour {
		public class OrderEvent : UnityEvent<DeliveryOrder, DeliveryPoint, PickUpPoint> { }

		public static int maxOrderSize { get; set; }

		[SerializeField] protected PlayerInventory _playerInventory;
		[SerializeField] protected DeliveryPoint[] _deliveryPoints;
		[SerializeField] protected PickUpPoint[]   _pickUpPoints;
		[SerializeField] protected FloatRange      _delayBetweenNewOrders = (5, 10);
		[SerializeField] protected int             _maxOrders             = 4;
		[SerializeField] protected float           _nextNewOrderTime;

		private static Dictionary<DeliveryOrder, (PickUpPoint pickUp, DeliveryPoint delivery)> orderProps { get; } = new Dictionary<DeliveryOrder, (PickUpPoint, DeliveryPoint)>();

		public static OrderEvent onOrderCreated  { get; } = new OrderEvent();
		public static OrderEvent onOrderRemoved  { get; } = new OrderEvent();
		public static OrderEvent onOrderPickedUp { get; } = new OrderEvent();

		public void Setup() {
			if (orderProps.Count == 0) {
				CreateNewOrder();
			}
		}

		private static bool TryGetProps(DeliveryOrder order, out PickUpPoint pickUp, out DeliveryPoint delivery) {
			pickUp = default;
			delivery = default;
			if (!orderProps.ContainsKey(order)) return false;
			(pickUp, delivery) = orderProps[order];
			return true;
		}

		private void RemoveOrder(DeliveryOrder order) {
			if (!TryGetProps(order, out var pickUp, out var delivery)) return;
			pickUp.CancelOrder();
			delivery.CancelOrder();
			_playerInventory.RemoveOrder(order);
			orderProps.Remove(order);
			onOrderRemoved.Invoke(order, delivery, pickUp);
		}

		private void PickUp(DeliveryOrder order) {
			if (!TryGetProps(order, out var pickUp, out var delivery)) return;
			order.MarkAsPickedUp();
			_playerInventory.AddOrder(order);
			onOrderPickedUp.Invoke(order, delivery, pickUp);
		}

		private void Deliver(DeliveryOrder order) {
			RemoveOrder(order);
			// TODO Handle delivery
		}

		private void Update() {
			if (Time.time < _nextNewOrderTime) return;
			if (orderProps.Count < _maxOrders) CreateNewOrder();
			_nextNewOrderTime = _delayBetweenNewOrders.Random();
		}

		private void CreateNewOrder() {
			var randomDeliveryPoint = _deliveryPoints.Where(t => !t.expectedDelivery).RandomOrDefault();
			if (!randomDeliveryPoint) return;
			var randomPickUpPoint = _pickUpPoints.Where(t => !t.currentOrder).RandomOrDefault();
			if (!randomPickUpPoint) return;
			var newOrder = randomPickUpPoint.GenerateOrder(maxOrderSize, randomDeliveryPoint);
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