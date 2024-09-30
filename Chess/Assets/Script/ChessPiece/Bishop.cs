using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : ChessPiece
{
    public override bool IsValidMove(int x, int y)
    {
        return Mathf.Abs(x - currentX) == Mathf.Abs(y - currentY);
    }

    public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board)
    {
        throw new System.NotImplementedException();
    }
}
