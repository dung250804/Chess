using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : ChessPiece
{

    public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board)
    {
        List<Vector2Int> result = new List<Vector2Int>();
        
        // Tạo các hướng di chuyển của Vua
        Vector2Int[] directions = new Vector2Int[]
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

        // (Thêm) Kiểm tra các ô không bị chiếu
        // ....................................

        return result;
    }
}
