using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : ChessPiece
{
    

    public override List<Vector2Int> GetAvailableMoves()
    {
        List<Vector2Int> result = new List<Vector2Int>();
        
        // Down
        for (int y = currentY - 1; y >= 0; y--)
        {
            if (ChessBoard.Instance.ChessPieces[currentX, y] == null)
            {
                result.Add(new Vector2Int(currentX,y));
            }
            else
            {
                if (ChessBoard.Instance.ChessPieces[currentX, y].team != team)
                {
                    result.Add(new Vector2Int(currentX,y));
                }
                break;
            }
        }
        
        // Up
        for (int y = currentY + 1; y < ChessBoard.BOARD_SIZE; y++)
        {
            if (ChessBoard.Instance.ChessPieces[currentX, y] == null)
            {
                result.Add(new Vector2Int(currentX,y));
            }
            else
            {
                if (ChessBoard.Instance.ChessPieces[currentX, y].team != team)
                {
                    result.Add(new Vector2Int(currentX,y));
                }
                break;
            }
        }
        
        // Left
        for (int x = currentX - 1; x >= 0; x--)
        {
            if (ChessBoard.Instance.ChessPieces[x, currentY] == null)
            {
                result.Add(new Vector2Int(x, currentY));
            }
            else
            {
                if (ChessBoard.Instance.ChessPieces[x, currentY].team != team)
                {
                    result.Add(new Vector2Int(x, currentY));
                }
                break;
            }
        }
        
        // Right
        for (int x = currentX + 1; x < ChessBoard.BOARD_SIZE; x++)
        {
            if (ChessBoard.Instance.ChessPieces[x, currentY] == null)
            {
                result.Add(new Vector2Int(x, currentY));
            }
            else
            {
                if (ChessBoard.Instance.ChessPieces[x, currentY].team != team)
                {
                    result.Add(new Vector2Int(x, currentY));
                }
                break;
            }
        }
         
        return result;
    }
    
    public override List<Vector2Int> GetAvailableAttacks()
    {
        List<Vector2Int> result = new List<Vector2Int>();
        
        // Down
        for (int y = currentY - 1; y >= 0; y--)
        {
            if (ChessBoard.Instance.ChessPieces[currentX, y] == null)
            {
                result.Add(new Vector2Int(currentX,y));
            }
            else
            {
                result.Add(new Vector2Int(currentX,y));
                break;
            }
        }
        
        // Up
        for (int y = currentY + 1; y < ChessBoard.BOARD_SIZE; y++)
        {
            if (ChessBoard.Instance.ChessPieces[currentX, y] == null)
            {
                result.Add(new Vector2Int(currentX,y));
            }
            else
            {
                result.Add(new Vector2Int(currentX,y));
                break;
            }
        }
        
        // Left
        for (int x = currentX - 1; x >= 0; x--)
        {
            if (ChessBoard.Instance.ChessPieces[x, currentY] == null)
            {
                result.Add(new Vector2Int(x, currentY));
            }
            else
            {
                result.Add(new Vector2Int(x, currentY));
                break;
            }
        }
        
        // Right
        for (int x = currentX + 1; x < ChessBoard.BOARD_SIZE; x++)
        {
            if (ChessBoard.Instance.ChessPieces[x, currentY] == null)
            {
                result.Add(new Vector2Int(x, currentY));
            }
            else
            {
                result.Add(new Vector2Int(x, currentY));
                break;
            }
        }
         
        return result;
    }
}
