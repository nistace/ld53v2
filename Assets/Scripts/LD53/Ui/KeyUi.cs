using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.Extensions;

public class KeyUi : MonoBehaviour {
	[SerializeField] protected Image    _background;
	[SerializeField] protected TMP_Text _keyText;

	public string key {
		get => _keyText.text;
		set => _keyText.text = value;
	}

	public float opacity {
		set {
			_background.color = _background.color.With(a: value);
			_keyText.color = _keyText.color.With(a: value);
		}
	}
}