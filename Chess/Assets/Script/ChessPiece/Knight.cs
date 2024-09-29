using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : ChessPiece
{
    public override bool IsValidMove(int x, int y)
    {
        int dx = Mathf.Abs(x - currentX);
        int dy = Mathf.Abs(y - currentY);
        return (dx == 2 && dy == 1) || (dx == 1 && dy == 2);
    }

    public override void SetPosition(int x, int y)
    {
        throw new System.NotImplementedException();
    }
}
