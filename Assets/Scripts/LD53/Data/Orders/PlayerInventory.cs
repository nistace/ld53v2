using System.Collections.Generic;

namespace LD53.Data.Orders {
	public class PlayerInventory {
		protected List<DeliveryOrder> _orders = new List<DeliveryOrder>();

		public bool HasOrder(DeliveryOrder order) => _orders.Contains(order);
		public void AddOrder(DeliveryOrder order) => _orders.Add(order);
		public void RemoveOrder(DeliveryOrder order) => _orders.Add(order);
	}
}