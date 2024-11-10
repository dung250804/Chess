using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : ChessPiece
{
    private static Vector2Int[] directions = new Vector2Int[]
    {
        new Vector2Int(0, 1),   // Up
        new Vector2Int(0, -1),  // Down
        new Vector2Int(1, 0),   // Right
        new Vector2Int(-1, 0),  // Left
        new Vector2Int(1, 1),   // Up Right
        new Vector2Int(-1, 1),  // Up Left
        new Vector2Int(1, -1),  // Down Right
        new Vector2Int(-1, -1)  // Down Left
    };

    public override bool IsValidMove(int x, int y)
    {
        // Castling check if moving two squares horizontally
        if (y == currentY && (x == currentX + 2 || x == currentX - 2))
        {
            if (!hasMoved)  // King has not moved yet
            {
                ChessPiece leftRook = ChessBoard.Instance.ChessPieces[0, currentY];
                ChessPiece rightRook = ChessBoard.Instance.ChessPieces[7, currentY];

                if (x == currentX - 2 && CanCastle(leftRook))
                {
                    int newX = leftRook.currentX + 3;
                    ChessBoard.Instance.MoveTo(leftRook, newX, currentY);
                    Debug.Log("Long castle successful!");
                    return true;
                }
                if (x == currentX + 2 && CanCastle(rightRook))
                {
                    int newX = rightRook.currentX - 2;
                    ChessBoard.Instance.MoveTo(rightRook, newX, currentY);
                    Debug.Log("Short castle successful!");
                    return true;
                }
            }
        }

        // Standard move validation
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

    public override List<Vector2Int> GetAvailableMoves()
    {
        List<Vector2Int> result = new List<Vector2Int>();

        // Check all valid king moves
        foreach (Vector2Int direction in directions)
        {
            int newX = currentX + direction.x;
            int newY = currentY + direction.y;

            if (IsInsideBoard(newX, newY))
            {
                ChessPiece targetPiece = ChessBoard.Instance.ChessPieces[newX, newY];
                if ((targetPiece == null || targetPiece.team != team) &&
                    !IsSquareUnderAttack(newX, newY, team))
                {
                    result.Add(new Vector2Int(newX, newY));
                }
            }
        }

        // Check for castling
        if (!hasMoved)
        {
            ChessPiece leftRook = ChessBoard.Instance.ChessPieces[0, currentY];
            ChessPiece rightRook = ChessBoard.Instance.ChessPieces[7, currentY];

            if (CanCastle(leftRook))
            {
                if(!IsSquareUnderAttack(currentX - 2, currentY, team)) 
                {
                    result.Add(new Vector2Int(currentX - 2, currentY));  // Long castle
                }
            }
            if (CanCastle(rightRook))
            {
                if(!IsSquareUnderAttack(currentX + 2, currentY, team)) 
                {
                    result.Add(new Vector2Int(currentX + 2, currentY));  // Short castle
                }
            }
        }

        return result;
    }

    public bool CanCastle(ChessPiece rook)
    {
        if (rook == null || rook.team != team || rook.type != PieceType.Rook) return false;
        if (hasMoved || rook.hasMoved) return false;
        if (currentY != rook.currentY) return false;

        int startCol = Math.Min(currentX, rook.currentX) + 1;
        int endCol = Math.Max(currentX, rook.currentX);

        // Check for pieces between king and rook
        for (int col = startCol; col < endCol; col++)
        {
            if (ChessBoard.Instance.ChessPieces[col, currentY] != null) return false;
        }
        
        // Mate
        if (IsSquareUnderAttack(currentX, currentY, team)) return false;
        

        return true;
    }

    public override List<Vector2Int>  GetAvailableAttacks()
    {
        List<Vector2Int> result = new List<Vector2Int>();
        foreach (Vector2Int direction in directions)
        {
            int newX = currentX + direction.x;
            int newY = currentY + direction.y;

            if (IsInsideBoard(newX, newY))
            {
                result.Add(new Vector2Int(newX, newY));
            }
        }

        return result;
    }

    private bool IsSquareUnderAttack(int x, int y, PieceTeam team)
    {
        foreach (ChessPiece piece in ChessBoard.Instance.ChessPieces)
        {
            if (piece != null && piece.team != team)
            {
                List<Vector2Int> opponentAttacks = piece.GetAvailableAttacks();
                foreach (Vector2Int move in opponentAttacks)
                {
                    if (move.x == x && move.y == y)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    
    private bool IsSquareUnderAttack(int x, int y, PieceTeam team, ChessPiece[,] board)
    {
        foreach (ChessPiece piece in board)
        {
            if (piece != null && piece.team != team && piece.type != PieceType.King)
            {
                List<Vector2Int> opponentAttacks = piece.GetAvailableAttacks();
                foreach (Vector2Int move in opponentAttacks)
                {
                    if (move.x == x && move.y == y)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    
    public bool IsCheck()
    {
        // Kiểm tra xem Vua của đội này có bị chiếu không
        return IsSquareUnderAttack(currentX, currentY, team);
    }
    
    public bool IsCheck(ChessPiece[,] board)
    {
        // Kiểm tra xem Vua của đội này có bị chiếu không
        return IsSquareUnderAttack(currentX, currentY, team, board);
    }

    private List<Vector2Int> GetValidMovesForEachValidPiece(ChessPiece piece)
    {
        List<Vector2Int> validMoves = new List<Vector2Int>();
        List<Vector2Int> potentialMoves = piece.GetAvailableMoves();

        foreach (Vector2Int move in potentialMoves)
        {
            // Deep clone the board to create an independent copy for each move
            ChessPiece[,] tempBoard = ChessBoard.Instance.ChessPieces;

            // Move the piece temporarily in the cloned board
            int originalX = piece.currentX;
            int originalY = piece.currentY;
            ChessPiece posPiece = tempBoard[move.x, move.y];
            tempBoard[move.x, move.y] = piece;
            tempBoard[originalX, originalY] = null;

            // Update the cloned piece’s position
            piece.currentX = move.x;
            piece.currentY = move.y;
            
            // Check if the move prevents check
            if (!IsCheck(tempBoard))
            {
                validMoves.Add(move);
                Debug.Log(piece + " " + "hehe");
            }

            // Revert the piece's position
            piece.currentX = originalX;
            piece.currentY = originalY;
            
            tempBoard[move.x, move.y] = posPiece;
            tempBoard[originalX, originalY] = piece;
        }

        return validMoves;
    }


    // Lấy danh sách các quân cờ với nước đi hợp lệ
    public Dictionary<ChessPiece, List<Vector2Int>> GetPiecesWithValidMoves()
    {
        Dictionary<ChessPiece, List<Vector2Int>> validPieces = new Dictionary<ChessPiece,List<Vector2Int>>();

        // Duyệt qua toàn bộ quân cờ của đội đang đi
        for (int i = 0; i < ChessBoard.BOARD_SIZE; i++)
        {
            for (int j = 0; j < ChessBoard.BOARD_SIZE; j++)
            {
                ChessPiece piece = ChessBoard.Instance.ChessPieces[i, j];
                if (piece != null && piece.team == team)
                {
                    List<Vector2Int> validMoves = GetValidMovesForEachValidPiece(piece);
                    Debug.Log(piece + " " + validMoves.Count);
                    if (validMoves.Count > 0)
                        validPieces[piece] = validMoves;
                }
            }
        }

        return validPieces;
    }

    // Kiểm tra Checkmate
    public bool IsCheckmate()
    {
        if (!IsCheck()) return false; // Không bị chiếu thì không phải chiếu hết

        // Kiểm tra có quân cờ nào có nước đi hợp lệ để thoát chiếu không
        Dictionary<ChessPiece, List<Vector2Int>> piecesWithMoves = GetPiecesWithValidMoves();
        return piecesWithMoves.Count == 0; // Nếu không có quân nào, đây là checkmate
    }

}
