using System.Collections;
using LD53.Scenes.Game.Data;
using LD53.Scenes.Game.Ui;
using Utils.Extensions;
using Utils.GameStates;
using GameState = Utils.GameStates.GameState;

namespace LD53.Scenes.Game {
	public class TutorialGameState : GameState {
		public static bool alreadySawTutorial { get; private set; }

		public static GameState state { get; } = new TutorialGameState();
		private TutorialGameState() { }
		private int page;

		protected override void Enable() {
			page = 0;
			TutorialUi.Show(page);
		}

		protected override void Disable() => TutorialUi.Hide();

		protected override IEnumerator Continue() {
			yield break;
		}

		protected override void SetListenersEnabled(bool enabled) {
			TutorialUi.onEndTutorialClicked.AddListenerOnce(HandleContinueButtonClicked);
		}

		private void HandleContinueButtonClicked() {
			page++;
			if (page >= TutorialUi.pageCount) {
				alreadySawTutorial = true;
				GameContext.StartTimer();
				ChangeState(CyclingGameState.state);
			}
			else {
				TutorialUi.Show(page);
			}
		}
	}
}