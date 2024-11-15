using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Piece : MonoBehaviour
{
	private Pieces_Scriptable _piece_Scriptable;
	private Image _image;
	private Button _button;
	private Pieces_Scriptable.PieceType _pieceCategory;

	private void Awake()
	{
		_image = GetComponent<Image>();
		_button = GetComponent<Button>();
	}

	private void Start()
	{
		_button.onClick.AddListener(SetPiece);
	}

	public void SetPieceType(Pieces_Scriptable piece)
	{
		_piece_Scriptable = piece;
		_image.sprite = _piece_Scriptable.spritePiece;
		_pieceCategory = _piece_Scriptable.pieceType;
	}

	void SetPiece()
	{
		GameManager.Instance.SendPiece(_piece_Scriptable, this.gameObject);
	}



}
