using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : ChessPiece
{
    public override bool IsValidMove(int x, int y)
    {
        return x == currentX || y == currentY || Mathf.Abs(x - currentX) == Mathf.Abs(y - currentY);
    }
    

}
