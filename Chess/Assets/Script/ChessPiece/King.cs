using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : ChessPiece
{
    public override bool IsValidMove(int x, int y)
    {
        return Mathf.Abs(x - currentX) <= 1 &&
               Mathf.Abs(y - currentY) <= 1;
    }
}
