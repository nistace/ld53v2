﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LD53.Data.Orders {
	[CreateAssetMenu]
	public class DeliveryOrder : ScriptableObject {
		[SerializeField] protected Meal[] _meals;
		[SerializeField] protected float  _creationTime;
		[SerializeField] protected float  _expectedPickUpTime;
		[SerializeField] protected float  _expectedDeliveryTime;
		[SerializeField] protected int    _baseScore;
		[SerializeField] protected bool   _pickedUp;

		public IReadOnlyList<Meal> meals => _meals;

		public float creationTime {
			get => _creationTime;
			set => _creationTime = value;
		}

		public float expectedPickUpTime {
			get => _expectedPickUpTime;
			set => _expectedPickUpTime = value;
		}

		public float expectedDeliveryTime {
			get => _expectedDeliveryTime;
			set => _expectedDeliveryTime = value;
		}

		public int baseScore {
			get => _baseScore;
			set => _baseScore = value;
		}

		public bool pickedUp {
			get => _pickedUp;
			set => _pickedUp = value;
		}

		public void KeepLessThanMeals(int count) {
			if (_meals.Length > count) _meals = _meals.Take(count).ToArray();
		}
	}
}