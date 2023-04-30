using LD53.Data.Orders;
using UnityEngine;
using Utils.Extensions;

namespace LD53.Data.Environment {
	public class GameEnvironment : MonoBehaviour {
		[SerializeField] protected Transform[]     _spawnPositions;
		[SerializeField] protected DeliveryManager _deliveryManager;

		public DeliveryManager deliveryManager => _deliveryManager;

		public Transform GetDefaultSpawnPosition() => _spawnPositions[0];
		public Transform GetClosestSpawnPosition(Vector3 position) => _spawnPositions.GetWithClosestScore(t => Vector3.SqrMagnitude(t.position - position), 0);
	}
}