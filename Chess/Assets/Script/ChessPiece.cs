using System;
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

    private Vector2Int _targetPosition = -Vector2Int.one;

    public bool isMoving = false;
    public bool hasMoved = false;

    private void Update()
    {
        if (isMoving && transform.parent.position == transform.position)
        {
            isMoving = false;
        }
        if(_targetPosition == -Vector2Int.one || transform.parent.position == transform.position) return;
        if (isMoving)
        {
            transform.position = Vector3.Lerp(transform.position, transform.parent.position, Time.deltaTime * 10);
        }
    }

    public virtual bool IsValidMove(int x, int y)
    {
        List<Vector2Int> availableMoves = GetAvailableMoves();
        
        foreach (Vector2Int move in availableMoves)
        {
            if (x == move.x && y == move.y)
            {
                return true;
            }
        }

        return false;
    }

    public void SetPosition(Vector2Int position, bool force = false)
    {
        _targetPosition = position;
        transform.parent = ChessBoard.Instance.TileArray[position.x, position.y].transform;
        currentX = position.x;
        currentY = position.y;
        if (force)
        {
            transform.position = ChessBoard.Instance.TileArray[position.x, position.y].transform.position;
        }
    }

    public abstract List<Vector2Int> GetAvailableMoves();

    public virtual List<Vector2Int> GetAvailableAttacks()
    {
        return GetAvailableMoves();
    }
    public static bool IsInsideBoard(int x, int y)
    {
        return x is >= 0 and < 8 && y is >= 0 and < 8;
    }
}
