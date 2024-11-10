using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MinimaxAlgorithm
{
    int recursionNumber = 0;
    // Giá trị các quân cờ
    private static readonly Dictionary<PieceType, int> PieceValues = new Dictionary<PieceType, int>()
    {
        { PieceType.Pawn, 10 }, { PieceType.Knight, 30 }, { PieceType.Bishop, 30 }, { PieceType.Rook, 50 }, { PieceType.Queen, 90 }, { PieceType.King, 900 }
    };

    // Hàm lấy Best Move với xử lý đa luồng
    public async Task<(int, (int, int, int, int))> GetBestMoveAsync(ChessPiece[,] board, int depth, bool isWhite)
    {
        List<Task<(int, (int, int, int, int))>> tasks = new List<Task<(int, (int, int, int, int))>>();
        List<(int, int, int, int)> moves = GetAllPossibleMoves(board, isWhite);

        // Chia từng nhánh ở root-level vào các Task riêng
        foreach (var move in moves)
        {
            ChessPiece[,] newBoard = MakeMove(board, move);
            tasks.Add(Task.Run(() => (Minimax(newBoard, depth - 1, !isWhite, int.MinValue, int.MaxValue), move)));
        }

        // Chờ tất cả các Task hoàn thành
        var results = await Task.WhenAll(tasks);

        // Chọn Best Move dựa trên kết quả trả về
        (int bestScore, (int, int, int, int) bestMove) = isWhite
            ? results.MaxBy(r => r.Item1) // Chọn score lớn nhất cho quân trắng
            : results.MinBy(r => r.Item1); // Chọn score nhỏ nhất cho quân đen

        return (bestScore, bestMove);
    }

    private int Minimax(ChessPiece[,] board, int depth, bool isMaximizing, int alpha, int beta)
    {
        if (depth == 0 || IsGameOver(board))
            return EvaluateBoard(board);

        if (isMaximizing)
        {
            int maxEval = int.MinValue;
            foreach (var move in GetAllPossibleMoves(board, true))
            {
                ChessPiece[,] newBoard = MakeMove(board, move);
                int eval = Minimax(newBoard, depth - 1, false, alpha, beta);
                maxEval = Math.Max(maxEval, eval);
                alpha = Math.Max(alpha, eval);
                if (alpha >= beta) break;
            }
            return maxEval;
        }
        else
        {
            int minEval = int.MaxValue;
            foreach (var move in GetAllPossibleMoves(board, false))
            {
                ChessPiece[,] newBoard = MakeMove(board, move);
                int eval = Minimax(newBoard, depth - 1, true, alpha, beta);
                minEval = Math.Min(minEval, eval);
                beta = Math.Min(beta, eval);
                if (beta <= alpha) break;
            }
            return minEval;
        }
    }


    private int EvaluateBoard(ChessPiece[,] board)
    {
        int score = 0;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                ChessPiece piece = board[i, j];
                if (piece != null)
                {
                    int pieceValue = PieceValues[piece.type];
                    score += piece.team == PieceTeam.White ? pieceValue : -pieceValue;

                    // Cộng thêm điểm cho quân ở vị trí quan trọng (ví dụ: trung tâm bàn cờ)
                    if ((i >= 2 && i <= 5) && (j >= 2 && j <= 5))
                    {
                        score += piece.team == PieceTeam.White ? 1 : -1;
                    }
                }
            }
        }
        return score;
    }


    private List<(int, int, int, int)> GetAllPossibleMoves(ChessPiece[,] board, bool isWhite)
    {
        List<(int, int, int, int)> moves = new List<(int, int, int, int)>();
        King kingWhite = null;
        King kingBlack = null;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                ChessPiece piece = board[i, j];
                if (piece != null && piece.type == PieceType.King)
                {
                    King king = (King)piece;
                    if (king.team == PieceTeam.Black)
                    {
                        kingBlack = king;
                    }
                    else
                    {
                        kingWhite = king;
                    }
                }
            }
        }
        
        if (kingBlack != null && kingBlack.IsCheck() && !isWhite)
        {
            List<(int, int, int, int)> temp = new List<(int, int, int, int)>();
            foreach (var chessPiece in kingBlack.GetPiecesWithValidMoves())
            {
                foreach (var availableMove in chessPiece.Value)
                {
                    temp.Add((chessPiece.Key.currentX, chessPiece.Key.currentY, availableMove.x, availableMove.y)) ;
                }
            }
            moves.AddRange(temp);
            return moves;
        }
        
        if (kingWhite != null && kingWhite.IsCheck() && isWhite)
        {
            List<(int, int, int, int)> temp = new List<(int, int, int, int)>();
            foreach (var chessPiece in kingWhite.GetPiecesWithValidMoves())
            {
                foreach (var availableMove in chessPiece.Value)
                {
                    temp.Add((chessPiece.Key.currentX, chessPiece.Key.currentY, availableMove.x, availableMove.y)) ;
                }
            }
            moves.AddRange(temp);
            return moves;
        }

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                ChessPiece piece = board[i, j];
                
                if (piece != null && ((piece.team == PieceTeam.White && isWhite) || (piece.team == PieceTeam.Black && !isWhite)))
                    moves.AddRange(GetMovesForPiece(piece));
            }
        }
        return moves;
    }

    private List<(int, int, int, int)> GetMovesForPiece(ChessPiece piece)
    {
        List<(int, int, int, int)> moves = new List<(int, int, int, int)>();

        List<Vector2Int> availableMoves = piece.GetAvailableMoves();

        foreach (Vector2Int availableMove in availableMoves)
        {
            moves.Add((piece.currentX, piece.currentY, availableMove.x, availableMove.y));
            
        }

        return moves;
    }

    public ChessPiece[,] MakeMove(ChessPiece[,] board, (int, int, int, int) move)
    {
        if (!ChessPiece.IsInsideBoard(move.Item3, move.Item4) ||
            !ChessPiece.IsInsideBoard(move.Item1, move.Item2)) return board;
        ChessPiece[,] newBoard = (ChessPiece[,])board.Clone();
        newBoard[move.Item3, move.Item4] = newBoard[move.Item1, move.Item2];
        newBoard[move.Item1, move.Item2] = null;
        return newBoard;
    }

    private bool IsGameOver(ChessPiece[,] board)
    {
        bool whiteKing = false, blackKing = false;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (board[i, j] == null) continue;
                if (board[i, j].type == PieceType.King && board[i, j].team == PieceTeam.White) whiteKing = true;
                if (board[i, j].type == PieceType.King && board[i, j].team == PieceTeam.Black) blackKing = true;
            }
        }
        return !(whiteKing && blackKing);
    }
}

public static class LinqExtensions
{
    public static T MaxBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector) where TKey : IComparable<TKey>
    {
        using var enumerator = source.GetEnumerator();
        if (!enumerator.MoveNext())
            throw new InvalidOperationException("Sequence contains no elements.");

        T maxElement = enumerator.Current;
        TKey maxKey = selector(maxElement);

        while (enumerator.MoveNext())
        {
            T currentElement = enumerator.Current;
            TKey currentKey = selector(currentElement);

            if (currentKey.CompareTo(maxKey) > 0)
            {
                maxElement = currentElement;
                maxKey = currentKey;
            }
        }
        return maxElement;
    }

    public static T MinBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector) where TKey : IComparable<TKey>
    {
        using var enumerator = source.GetEnumerator();
        if (!enumerator.MoveNext())
            throw new InvalidOperationException("Sequence contains no elements.");

        T minElement = enumerator.Current;
        TKey minKey = selector(minElement);

        while (enumerator.MoveNext())
        {
            T currentElement = enumerator.Current;
            TKey currentKey = selector(currentElement);

            if (currentKey.CompareTo(minKey) < 0)
            {
                minElement = currentElement;
                minKey = currentKey;
            }
        }
        return minElement;
    }
}