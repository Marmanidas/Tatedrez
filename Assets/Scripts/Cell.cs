using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    private Button _button;
	private string _id;

	private void Awake()
	{
		_button = GetComponent<Button>();
	}

	private void Start()
	{
		_button.onClick.AddListener(SelectCell);
	}

	void SelectCell()
	{
		if (GameManager.Instance.TotalCount < 6)
		{
			GameManager.Instance.SetPieceTurn(this.transform);
		}
		else
		{
			GameManager.Instance.MovePiece(this.transform);
		}
	}
}
