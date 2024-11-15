# Tatedrez

The game consists of several playable pieces, a chessboard, and logic to check for victories. The pieces are ScriptableObject objects that define their type and appearance. To organize the code, we opted to separate the game logic into different scripts that control the board, the pieces, and user interaction.

Pieces as ScriptableObject
The pieces in the game are represented by a ScriptableObject, a Unity class that allows creating reusable and easily editable data objects in the editor. Each piece has a type (Knight, Rook, Bishop) and a sprite associated for visual representation. This decision allows the pieces to be easily managed and modular.

Chessboard and Turn Control
The chessboard is made up of a grid of cells (3x3 grid), where each cell can contain a piece. At the beginning of the game, the 6 pieces (3 white and 3 black) are placed in predetermined positions. The variable _isWhiteTurn is used to control whose turn it is, alternating between true (white) and false (black). This turn alternation ensures that each player makes one move at a time.

Victory Check: Tic-Tac-Toe
The logic to check if a player has won is based on an implementation of tic-tac-toe. After each move, the system checks if the pieces of the same type (either white or black) are aligned in any of the rows, columns, or diagonals on the board. This check is done in the CheckWin method.

User Interaction
The user can interact with the pieces through buttons that represent each piece on the interface. Unity UI is used to manage the interaction, enabling and disabling buttons based on the turn of each player. By clicking on a piece, it moves to a new cell on the board, and the piece's position is updated visually.

The code to alternate between turns and enable the corresponding buttons for white or black pieces is handled in the GameManager script, where the SetPieceTurn method is called, alternating the value of _isWhiteTurn.






