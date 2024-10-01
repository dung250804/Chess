using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : ChessPiece
{

    public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board)
    {
        List<Vector2Int> result = new List<Vector2Int>();
        
        // Down
        for (int y = currentY - 1; y >= 0; y--)
        {
            if (board[currentX, y] == null)
            {
                result.Add(new Vector2Int(currentX,y));
            }
            else
            {
                if (board[currentX, y].team != team)
                {
                    result.Add(new Vector2Int(currentX,y));
                }
                break;
            }
        }
        
        // Up
        for (int y = currentY + 1; y < ChessBoard.BOARD_SIZE; y++)
        {
            if (board[currentX, y] == null)
            {
                result.Add(new Vector2Int(currentX,y));
            }
            else
            {
                if (board[currentX, y].team != team)
                {
                    result.Add(new Vector2Int(currentX,y));
                }
                break;
            }
        }
        
        // Left
        for (int x = currentX - 1; x >= 0; x--)
        {
            if (board[x, currentY] == null)
            {
                result.Add(new Vector2Int(x, currentY));
            }
            else
            {
                if (board[x, currentY].team != team)
                {
                    result.Add(new Vector2Int(x, currentY));
                }
                break;
            }
        }
        
        // Right
        for (int x = currentX + 1; x < ChessBoard.BOARD_SIZE; x++)
        {
            if (board[x, currentY] == null)
            {
                result.Add(new Vector2Int(x, currentY));
            }
            else
            {
                if (board[x, currentY].team != team)
                {
                    result.Add(new Vector2Int(x, currentY));
                }
                break;
            }
        }
        
        //Up Right
        for (int x = currentX + 1, y = currentY + 1; IsInsideBoard(x,y); x++, y++)
        {
            if (board[x, y] == null)
            {
                result.Add(new Vector2Int(x,y));
            }
            else
            {
                if (board[x, y].team != team)
                {
                    result.Add(new Vector2Int(x,y));
                }
                break;
            }
        }
        
        //Up Left
        for (int x = currentX - 1, y = currentY + 1; IsInsideBoard(x,y); x--, y++)
        {
            if (board[x, y] == null)
            {
                result.Add(new Vector2Int(x,y));
            }
            else
            {
                if (board[x, y].team != team)
                {
                    result.Add(new Vector2Int(x,y));
                }
                break;
            }
        }
        
        //Down Right
        for (int x = currentX + 1, y = currentY - 1; IsInsideBoard(x,y); x++, y--)
        {
            if (board[x, y] == null)
            {
                result.Add(new Vector2Int(x,y));
            }
            else
            {
                if (board[x, y].team != team)
                {
                    result.Add(new Vector2Int(x,y));
                }
                break;
            }
        }
        
        //Down Left
        for (int x = currentX - 1, y = currentY - 1; IsInsideBoard(x,y); x--, y--)
        {
            if (board[x, y] == null)
            {
                result.Add(new Vector2Int(x,y));
            }
            else
            {
                if (board[x, y].team != team)
                {
                    result.Add(new Vector2Int(x,y));
                }
                break;
            }
        }
        
        return result;
    }
}
