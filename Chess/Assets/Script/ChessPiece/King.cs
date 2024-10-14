using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : ChessPiece
{
    private static Vector2Int[] directions = new Vector2Int[]
    {
        new Vector2Int(0, 1),   // Up
        new Vector2Int(0, -1),  // Down
        new Vector2Int(1, 0),   // Right
        new Vector2Int(-1, 0),  // Left
        new Vector2Int(1, 1),   // Up Right
        new Vector2Int(-1, 1),  // Up Left
        new Vector2Int(1, -1),  // Down Right
        new Vector2Int(-1, -1)  // Down Left
    };
    
    
    
    
    public override bool IsValidMove(int x, int y)
    {
        // Kiểm tra nhập thành nếu vua di chuyển 2 ô ngang
        if (y == currentY && (x == currentX + 2 || x == currentX - 2))
        {
            if (!hasMoved)  // Vua chưa di chuyển
            {
                ChessPiece leftRook = ChessBoard.Instance.ChessPieces[0, currentY];
                ChessPiece rightRook = ChessBoard.Instance.ChessPieces[7, currentY];

                if (x == currentX - 2 && CanCastle(leftRook, ref ChessBoard.Instance.ChessPieces))
                {
                    int newX = leftRook.team == PieceTeam.White ? leftRook.currentX + 3 : leftRook.currentX + 2;
                    ChessBoard.Instance.MoveTo(leftRook, newX, currentY);
                    Debug.Log("Nhập thành xa thành công!");
                    return true;
                }
                if (x == currentX + 2 && CanCastle(rightRook, ref ChessBoard.Instance.ChessPieces))
                {
                    int newX = leftRook.team == PieceTeam.White ? leftRook.currentX - 2 : leftRook.currentX - 3;
                    ChessBoard.Instance.MoveTo(rightRook, newX, currentY);
                    Debug.Log("Nhập thành gần thành công!");
                    return true;
                }
            }
        }

        // Kiểm tra các nước đi thông thường
        List<Vector2Int> availableMoves = GetAvailableMoves(ref ChessBoard.Instance.ChessPieces);
        foreach (Vector2Int move in availableMoves)
        {
            if (x == move.x && y == move.y)
            {
                return true;
            }
        }

        return false;
    }

    
    public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board)
    {
        List<Vector2Int> result = new List<Vector2Int>();

        // Duyệt các hướng đi hợp lệ của vua
        foreach (Vector2Int direction in directions)
        {
            int newX = currentX + direction.x;
            int newY = currentY + direction.y;

            if (IsInsideBoard(newX, newY))
            {
                ChessPiece targetPiece = board[newX, newY];
                if (targetPiece == null || targetPiece.team != team)
                {
                    result.Add(new Vector2Int(newX, newY));
                }
            }
        }

        // Kiểm tra khả năng nhập thành nếu vua chưa di chuyển
        if (!hasMoved)
        {
            ChessPiece leftRook = board[0, currentY];
            ChessPiece rightRook = board[7, currentY];

            if (CanCastle(leftRook, ref board))
            {
                result.Add(new Vector2Int(currentX - 2, currentY));  // Nhập thành xa
            }
            if (CanCastle(rightRook, ref board))
            {
                result.Add(new Vector2Int(currentX + 2, currentY));  // Nhập thành gần
            }
        }
        
        // (Thêm) Kiểm tra các ô không bị chiếu
        // ....................................

        return result;
    }
    
    public bool CanCastle(ChessPiece rook, ref ChessPiece[,] board)
    {
        // Kiểm tra xem quân xe có hợp lệ để nhập thành không
        if (rook == null || rook.team != team || rook.type != PieceType.Rook) return false;

        // Kiểm tra cả vua và xe chưa di chuyển
        if (hasMoved || rook.hasMoved) return false;

        // Vua và xe phải trên cùng một hàng
        if (currentY != rook.currentY) return false;

        // Không có quân nào chắn giữa vua và xe
        int startCol = Math.Min(currentX, rook.currentX) + 1;
        int endCol = Math.Max(currentX, rook.currentX);

        for (int col = startCol; col < endCol; col++)
        {
            if (board[col, currentY] != null) return false;
        }
        
        /*// Điều kiện 4: Các ô vua đi qua không bị tấn công
        int direction = (rook.Position.Col > king.Position.Col) ? 1 : -1;
        for (int i = 0; i <= 2; i++)
        {
            int checkCol = king.Position.Col + i * direction;
            if (isAttacked[king.Position.Row][checkCol])
                return false;
        }*/

        return true;
    }
}
