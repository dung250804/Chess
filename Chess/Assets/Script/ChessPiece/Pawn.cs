using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece
{
    private static Vector2Int _enPassantTarget = new Vector2Int(-1, -1); // Vị trí của quân Tốt có thể bị bắt qua sông
    private static ChessPiece currentEnPassantTarget = null;

    public override bool IsValidMove(int x, int y)
    {
        if (CanEnPassant(new Vector2Int(x, y)))
        {
            Destroy(currentEnPassantTarget.gameObject);
            _enPassantTarget = new Vector2Int(-1, -1);
            currentEnPassantTarget = null;
            return true;
        }

        if (_enPassantTarget != new Vector2Int(-1, -1) && currentEnPassantTarget != null)
        {
            _enPassantTarget = new Vector2Int(-1, -1);
            currentEnPassantTarget = null;
        }
        
        List<Vector2Int> availableMoves = GetAvailableMoves(ref ChessBoard.Instance.ChessPieces);
        int direction = team == PieceTeam.White ? 1 : -1;
        
        foreach (Vector2Int move in availableMoves)
        {
            if (x == move.x && y == move.y)
            {
                if (x == currentX && y == currentY + 2 * direction)
                {
                    _enPassantTarget = new Vector2Int(currentX, currentY + direction); // Đặt mục tiêu cho en passant
                    currentEnPassantTarget = this;
                }
                return true;
            }
        }
        

        return false;
    }

    public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board)
    {
        List<Vector2Int> result = new List<Vector2Int>();
        int direction = team == PieceTeam.White ? 1 : -1;

        // Di chuyển thẳng 1 ô
        if (IsInsideBoard(currentX, currentY + direction) && board[currentX, currentY + direction] == null)
        {
            result.Add(new Vector2Int(currentX, currentY + direction));
        }

        // Di chuyển thẳng 2 ô ở lượt đầu
        if (currentY == (team == PieceTeam.White ? 1 : 6) &&
            board[currentX, currentY + direction] == null && 
            board[currentX, currentY + 2 * direction] == null)
        {
            result.Add(new Vector2Int(currentX, currentY + 2 * direction));
        }

        // Ăn chéo
        if (IsInsideBoard(currentX + 1, currentY + direction) &&
            board[currentX + 1, currentY + direction] != null &&
            board[currentX + 1, currentY + direction].team != team)
        {
            result.Add(new Vector2Int(currentX + 1, currentY + direction));
        }
        if (IsInsideBoard(currentX - 1, currentY + direction) &&
            board[currentX - 1, currentY + direction] != null &&
            board[currentX - 1, currentY + direction].team != team)
        {
            result.Add(new Vector2Int(currentX - 1, currentY + direction));
        }

        // Kiểm tra nước đi en passant
        if (_enPassantTarget != new Vector2Int(-1, -1))
        {
            if (CanEnPassant(new Vector2Int(currentX + 1, currentY + direction)) || CanEnPassant(new Vector2Int(currentX - 1, currentY + direction))) 
            {
                result.Add(new Vector2Int(_enPassantTarget.x, _enPassantTarget.y));
            }
        }
        return result;
    }

    public bool CanEnPassant(Vector2Int position)
    {
        if (_enPassantTarget == position)
        {
            if (currentEnPassantTarget == null) return false;
            if (team != currentEnPassantTarget.team)
            {
                return true;
            }
        }

        return false;
    }
}