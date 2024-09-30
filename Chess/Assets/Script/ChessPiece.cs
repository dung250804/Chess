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

    private void Update()
    {
        if (isMoving && transform.parent.position == transform.position)
        {
            
        }
        if(_targetPosition == -Vector2Int.one || transform.parent.position == transform.position) return;
        if (isMoving)
        {
            transform.position = Vector3.Lerp(transform.position, transform.parent.position, Time.deltaTime * 10);
        }
    }

    public abstract bool IsValidMove(int x, int y);

    public virtual void SetPosition(Vector2Int position, bool force = false)
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

    public abstract List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board);
}
