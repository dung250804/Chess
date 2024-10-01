using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : ChessPiece
{

    public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board)
    {
        List<Vector2Int> result = new List<Vector2Int>();
        
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
