using LD53.Scenes.Game.Data;
using UnityEngine;
using GameState = Utils.GameStates.GameState;

namespace LD53.Scenes.Game {
	public class GameSceneController : MonoBehaviour {
		[SerializeField] protected GameData _gameData;

		private void Start() {
			GameData.instance = _gameData;
			GameState.ChangeState(InitGameState.state);
		}
	}
}