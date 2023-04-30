using System;
using TMPro;
using UnityEngine;

public class PhoneTopBarUi : MonoBehaviour {
	[SerializeField] protected TMP_Text _timeText;

	private void Update() {
		_timeText.text = $"{DateTime.Now:hh:mm}";
	}
}