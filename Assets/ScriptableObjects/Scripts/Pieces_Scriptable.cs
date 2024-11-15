using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (fileName = "Piece", menuName = "Piece")]
public class Pieces_Scriptable : ScriptableObject
{
	public enum PieceType
	{
		Knight,
		Rook,
		Bishop
	}

	public PieceType pieceType;
	public Sprite spritePiece;
}
