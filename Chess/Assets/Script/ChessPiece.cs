using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PieceTeam { White, Black }
public enum PieceType { King, Queen, Rook, Bishop, Knight, Pawn }

public abstract class ChessPiece : MonoBehaviour
{
    public PieceTeam team;
    public PieceType type;
    public int currentX;
    public int currentY;

    private Vector3 _positionCanMove;

    public abstract bool IsValidMove(int x, int y);
}
