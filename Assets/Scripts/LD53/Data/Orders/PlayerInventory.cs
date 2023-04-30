using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LD53.Data.Orders {
	[Serializable]
	public class PlayerInventory {
		[SerializeField] protected int                 _capacity;
		[SerializeField] protected List<DeliveryOrder> _orders = new List<DeliveryOrder>();

		public int ordersTakenSlots => _orders.Sum(t => t.meals.Count);

		public bool HasOrder(DeliveryOrder order) => _orders.Contains(order);
		public void AddOrder(DeliveryOrder order) => _orders.Add(order);
		public void RemoveOrder(DeliveryOrder order) => _orders.Add(order);
	}
}