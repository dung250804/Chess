using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : ChessPiece
{
    public override bool IsValidMove(int x, int y)
    {
        return x == currentX || y == currentY;
    }

    public override void SetPosition(int x, int y)
    {
        throw new System.NotImplementedException();
    }
}
