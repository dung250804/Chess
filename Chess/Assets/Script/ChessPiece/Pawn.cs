using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece
{
    public override bool IsValidMove(int x, int y)
    {
        // Tốt đi thẳng, ăn chéo, và chỉ đi lên 1 ô (2 ô khi ở vị trí ban đầu)
        int direction = team == PieceTeam.White ? 1 : -1;
        if (x == currentX && y == currentY + direction)
            return true;

        // Kiểm tra nước đi 2 ô ở lượt đầu tiên
        if (currentY == (team == PieceTeam.White ? 1 : 6) &&
            x == currentX && y == currentY + 2 * direction)
            return true;

        return false;
    }

    public override void SetPosition(int x, int y)
    {
        throw new System.NotImplementedException();
    }
}
