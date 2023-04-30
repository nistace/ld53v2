using LD53.Data.Orders;
using UnityEngine;
using Utils.Extensions;

namespace LD53.Data.Environment {
	public class GameEnvironment : MonoBehaviour {
		[SerializeField] protected Transform[]     _spawnPositions;
		[SerializeField] protected DeliveryManager _deliveryManager;
		[SerializeField] protected Vector3         _mapMin = new Vector3(-75, 0, -75);
		[SerializeField] protected Vector3         _mapMax = new Vector3(75, 0, 75);

		public DeliveryManager deliveryManager => _deliveryManager;

		public Transform GetDefaultSpawnPosition() => _spawnPositions[0];
		public Transform GetClosestSpawnPosition(Vector3 position) => _spawnPositions.GetWithClosestScore(t => Vector3.SqrMagnitude(t.position - position), 0);

		public Vector2 GetPositionRelativeToBounds(Vector3 worldPosition) =>
			new Vector2((worldPosition.x - _mapMin.x) / (_mapMax.x - _mapMin.x), (worldPosition.z - _mapMin.z) / (_mapMax.z - _mapMin.z));

#if UNITY_EDITOR
		private void OnDrawGizmos() {
			Gizmos.color = new Color(1, .5f, 0);
			foreach (var spawnPosition in _spawnPositions) {
				Gizmos.DrawSphere(spawnPosition.position, .5f);
				Gizmos.DrawLine(spawnPosition.position, spawnPosition.position + spawnPosition.forward);
			}
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(_mapMin, new Vector3(_mapMin.x, 0, _mapMax.y));
			Gizmos.DrawLine(_mapMin, new Vector3(_mapMax.x, 0, _mapMin.y));
			Gizmos.DrawLine(_mapMax, new Vector3(_mapMin.x, 0, _mapMax.y));
			Gizmos.DrawLine(_mapMax, new Vector3(_mapMax.x, 0, _mapMin.y));
		}

#endif
	}
}