using UnityEngine;
using UnityEngine.Events;

namespace LD53.Data.Orders {
	public class DeliveryOrder {
		private float creationTime         { get; }
		private float maxScoreDeliveryTime { get; }
		private int   baseScore            { get; }
		public  bool  pickedUp             { get; private set; }

		public UnityEvent onPickedUp { get; } = new UnityEvent();

		public DeliveryOrder(float distanceToTravel) {
			creationTime = Time.time;
			baseScore = Mathf.FloorToInt(5 + distanceToTravel * 100);
			maxScoreDeliveryTime = distanceToTravel + 10;
		}

		public void MarkAsPickedUp() {
			pickedUp = true;
			onPickedUp.Invoke();
		}

		public int GetScoreNow() {
			if (Time.time < creationTime + maxScoreDeliveryTime) return baseScore;
			if (Time.time < creationTime + 2 * maxScoreDeliveryTime)
				return Mathf.FloorToInt(Mathf.Lerp(baseScore * .25f, baseScore * .5f, Time.time - (creationTime + maxScoreDeliveryTime) / (maxScoreDeliveryTime)));
			return Mathf.FloorToInt(baseScore * .1f);
		}
	}
}