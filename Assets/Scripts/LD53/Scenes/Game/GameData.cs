using System;
using LD53.Data.Bikes;
using LD53.Data.Environment;
using LD53.Data.Orders;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LD53.Scenes.Game {
	[Serializable]
	public class GameData {
		public static GameData        instance        { get; set; }
		public static Bike            currentBike     { get; private set; }
		public static GameEnvironment gameEnvironment => instance._gameEnvironment;
		public static DeliveryManager deliveryManager => gameEnvironment.deliveryManager;
		public static FollowCam       camera          => instance._camera;
		public static Inventory       inventory       => instance._inventory;

		[SerializeField] protected Bike            _bikePrefab;
		[SerializeField] protected GameEnvironment _gameEnvironment;
		[SerializeField] protected FollowCam       _camera;
		[SerializeField] protected Inventory       _inventory;

		public static void InstantiateBike(Transform position) {
			if (currentBike) Object.Destroy(currentBike.gameObject);
			currentBike = Object.Instantiate(instance._bikePrefab, position.position, position.rotation);
			camera.target = currentBike.directionTransform;
			camera.strategy = FollowCam.Strategy.FollowCam;
			camera.Jump();
		}
	}
}