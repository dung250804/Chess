using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : ChessPiece
{

    public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board)
    {
        List<Vector2Int> result = new List<Vector2Int>();
        int x, y;
        
        
        // Top Right
        x = currentX + 1;
        y = currentY + 2;
        if (IsInsideBoard(x,y) && (board[x,y] == null || board[x,y].team != team))
        {
            result.Add(new Vector2Int(x,y));
        }
        
        x = currentX + 2;
        y = currentY + 1;
        if (IsInsideBoard(x,y) && (board[x,y] == null || board[x,y].team != team))
        {
            result.Add(new Vector2Int(x,y));
        }
        
        // Top left
        x = currentX - 1;
        y = currentY + 2;
        if (IsInsideBoard(x,y) && (board[x,y] == null || board[x,y].team != team))
        {
            result.Add(new Vector2Int(x,y));
        }
        
        x = currentX - 2;
        y = currentY + 1;
        if (IsInsideBoard(x,y) && (board[x,y] == null || board[x,y].team != team))
        {
            result.Add(new Vector2Int(x,y));
        }
        
        // Bottom Right
        x = currentX + 1;
        y = currentY - 2;
        if (IsInsideBoard(x,y) && (board[x,y] == null || board[x,y].team != team))
        {
            result.Add(new Vector2Int(x,y));
        }
        
        x = currentX + 2;
        y = currentY - 1;
        if (IsInsideBoard(x,y) && (board[x,y] == null || board[x,y].team != team))
        {
            result.Add(new Vector2Int(x,y));
        }
        
        // Bottom Left
        x = currentX - 1;
        y = currentY - 2;
        if (IsInsideBoard(x,y) && (board[x,y] == null || board[x,y].team != team))
        {
            result.Add(new Vector2Int(x,y));
        }
        
        x = currentX - 2;
        y = currentY - 1;
        if (IsInsideBoard(x,y) && (board[x,y] == null || board[x,y].team != team))
        {
            result.Add(new Vector2Int(x,y));
        }
        
        return result;
    }
}
