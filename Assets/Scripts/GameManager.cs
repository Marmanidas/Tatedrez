using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	[SerializeField] CanvasManager canvasManager;

	[SerializeField] Pieces_Scriptable[] _whitePieces;
	[SerializeField] Pieces_Scriptable[] _blackPieces;	
	[SerializeField] GameObject _objectPiece;
	[SerializeField] List<GameObject> _whitepiecesList = new List<GameObject>();
	[SerializeField] List<GameObject> _blackpiecesList = new List<GameObject>();

	private string[,] _board = new string[3, 3];
	private int _whiteCount;
	private int _blackCount;
	private bool _isWhiteTurn = true;
	private bool win;
	private Pieces_Scriptable.PieceType _pieceCategory;

	private GameObject selectedPiece;
	private int selectedPieceOldX;
	private int selectedPieceOldY;

	public int TotalCount { get; set; }
	public bool PieceSelected { get; set; }


	public static GameManager Instance { get; private set; }

	[SerializeField] private List<Button> _boardCells;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			return;
		}
		Instance = this;
	}

	private void Start()
	{
		canvasManager.MessageText("White Turn");
	}

	#region SetPieces

	public void SetPieceTurn(Transform cellPos)
	{
		if (TotalCount < 6 && !win)
		{
			GameObject piece = Instantiate(_objectPiece, cellPos);
			TotalCount++;

			string pieceType;
			if (_isWhiteTurn && _whiteCount < 3)
			{
				piece.GetComponent<Piece>().SetPieceType(_whitePieces[_whiteCount]);
				_whitepiecesList.Add(piece);
				_whiteCount++;
				_isWhiteTurn = false;
				pieceType = "white";
			}
			else if (!_isWhiteTurn && _blackCount < 3)
			{
				piece.GetComponent<Piece>().SetPieceType(_blackPieces[_blackCount]);
				_blackpiecesList.Add(piece);
				_blackCount++;
				_isWhiteTurn = true;
				pieceType = "black";
			}
			else
			{
				return;
			}

			string cellId = cellPos.name;
			string[] coordinates = cellId.Split('_');
			if (coordinates.Length == 2 && int.TryParse(coordinates[0], out int x) && int.TryParse(coordinates[1], out int y))
			{
				x -= 1;
				y -= 1;

				_board[x, y] = pieceType;

				if (CheckWin(pieceType))
				{
					canvasManager.MessageText($"{pieceType} wins!");
					win = true;
					ButtonsAndBoardControl(false);
				}
			}
			else
			{
				Debug.LogError("Invalid format");
			}
		}

		if (TotalCount == 6)
		{
			Debug.Log("Start movement");
			ButtonsAndBoardControl(false);
			_isWhiteTurn = true;
			UpdateButtonStates();
		}
	}

	private void ButtonsAndBoardControl(bool boardIsEnabled)
	{
		foreach (Button cellButton in _boardCells)
		{
			cellButton.interactable = boardIsEnabled; 
		}
	}

	private void UpdateButtonStates()
	{
		foreach (var button in _whitepiecesList)
		{
			button.GetComponent<Button>().interactable = _isWhiteTurn;
		}

		foreach (var button in _blackpiecesList)
		{
			button.GetComponent<Button>().interactable = !_isWhiteTurn;
		}
	}

	public void DisableAllButtons(bool isEnabled)
	{
		foreach (var button in _whitepiecesList)
		{
			button.GetComponent<Button>().interactable = isEnabled;
		}

		foreach (var button in _blackpiecesList)
		{
			button.GetComponent<Button>().interactable = isEnabled;
		}
	}

	private bool CheckWin(string pieceType)
	{
		for (int i = 0; i < 3; i++)
		{
			// Check rows and columns
			if ((_board[i, 0] == pieceType && _board[i, 1] == pieceType && _board[i, 2] == pieceType) ||
				(_board[0, i] == pieceType && _board[1, i] == pieceType && _board[2, i] == pieceType))
			{
				return true;
			}
		}

		// Check diagonals
		if ((_board[0, 0] == pieceType && _board[1, 1] == pieceType && _board[2, 2] == pieceType) ||
			(_board[0, 2] == pieceType && _board[1, 1] == pieceType && _board[2, 0] == pieceType))
		{
			return true;
		}

		return false;
	}




	#endregion

	#region MovePieces

	public void SendPiece(Pieces_Scriptable piece, GameObject pieceObject)
	{
		_pieceCategory = piece.pieceType;
		selectedPiece = pieceObject;

		string[] coordinates = pieceObject.transform.parent.name.Split('_');
		if (coordinates.Length == 2 && int.TryParse(coordinates[0], out int x) && int.TryParse(coordinates[1], out int y))
		{
			selectedPieceOldX = x - 1;
			selectedPieceOldY = y - 1;
		}

		PieceSelected = true;
	}

	public void MovePiece(Transform movePosition)
	{
		if (PieceSelected && !win)
		{
			string selectedPieceCellId = selectedPiece.transform.parent.name;
			string[] selectedCoordinates = selectedPieceCellId.Split('_');

			if (selectedCoordinates.Length != 2 ||
				!int.TryParse(selectedCoordinates[0], out int selectedPieceOldX) ||
				!int.TryParse(selectedCoordinates[1], out int selectedPieceOldY))
			{
				Debug.LogError("Error getting coordinates for selected part.");
				PieceSelected = false;
				return;
			}

			selectedPieceOldX -= 1;
			selectedPieceOldY -= 1;

			// Get the coordinates of the moving position
			string moveCellId = movePosition.name;
			string[] moveCoordinates = moveCellId.Split('_');

			if (moveCoordinates.Length != 2 ||
				!int.TryParse(moveCoordinates[0], out int x) ||
				!int.TryParse(moveCoordinates[1], out int y))
			{
				Debug.LogError("Error getting move coordinates.");
				PieceSelected = false;
				return;
			}

			x -= 1;
			y -= 1;

			if (IsValidMove(selectedPieceOldX, selectedPieceOldY, x, y, _pieceCategory))
			{
				selectedPiece.transform.SetParent(movePosition, false);
				UpdateBoard(selectedPieceOldX, selectedPieceOldY, x, y, _pieceCategory);
				_isWhiteTurn = !_isWhiteTurn;
				UpdateButtonStates();
			}

			else
			{
				canvasManager.MessageText("Invalid movement");
			}

			PieceSelected = false;
		}
	}


	private bool IsValidMove(int currentX, int currentY, int targetX, int targetY, Pieces_Scriptable.PieceType pieceType)
	{
		switch (pieceType)
		{
			case Pieces_Scriptable.PieceType.Knight:
				return (Mathf.Abs(currentX - targetX) == 2 && Mathf.Abs(currentY - targetY) == 1) ||
					   (Mathf.Abs(currentX - targetX) == 1 && Mathf.Abs(currentY - targetY) == 2);

			case Pieces_Scriptable.PieceType.Rook:
				return currentX == targetX || currentY == targetY;

			case Pieces_Scriptable.PieceType.Bishop:
				return Mathf.Abs(currentX - targetX) == Mathf.Abs(currentY - targetY);

			default:
				return false;
		}
	}


	private bool IsPathClear(int startX, int startY, int endX, int endY)
	{
		int dx = endX > startX ? 1 : (endX < startX ? -1 : 0);
		int dy = endY > startY ? 1 : (endY < startY ? -1 : 0);

		int x = startX + dx, y = startY + dy;
		while (x != endX || y != endY)
		{
			if (_board[x, y] != null)
			{
				canvasManager.MessageText("Blocked");
				return false; 
			}
			x += dx;
			y += dy;
		}

		return true;
	}

	private void UpdateBoard(int oldX, int oldY, int newX, int newY, Pieces_Scriptable.PieceType pieceType)
	{
		_board[oldX, oldY] = null;
		string pieceTypeString = _isWhiteTurn ? "white" : "black";
		_board[newX, newY] = pieceTypeString;

		if (CheckWin(pieceTypeString))
		{
			canvasManager.MessageText($"{pieceTypeString} wins!");
			DisableAllButtons(false);
			ButtonsAndBoardControl(false);

		}
	}




}




#endregion


