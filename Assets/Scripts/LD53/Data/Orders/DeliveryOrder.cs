using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace LD53.Data.Orders {
	[CreateAssetMenu]
	public class DeliveryOrder : ScriptableObject {
		[SerializeField] protected Meal[] _meals;
		[SerializeField] protected float  _creationTime;
		[SerializeField] protected float  _maxScoreDeliveryTime;
		[SerializeField] protected int    _baseScore;
		[SerializeField] protected bool   _pickedUp;

		public IReadOnlyList<Meal> meals => _meals;

		public float creationTime {
			get => _creationTime;
			set => _creationTime = value;
		}

		public float maxScoreDeliveryTime {
			get => _maxScoreDeliveryTime;
			set => _maxScoreDeliveryTime = value;
		}

		public int baseScore {
			get => _baseScore;
			set => _baseScore = value;
		}

		public bool pickedUp {
			get => _pickedUp;
			private set => _pickedUp = value;
		}

		public void MarkAsPickedUp() {
			pickedUp = true;
			onPickedUp.Invoke();
		}

		public void KeepLessThanMeals(int count) {
			if (_meals.Length > count) _meals = _meals.Take(count).ToArray();
		}

		public UnityEvent onPickedUp { get; } = new UnityEvent();
	}
}