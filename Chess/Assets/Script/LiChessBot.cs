using System;
using System.Collections;
using UnityEngine;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class LiChessBot : MonoBehaviour
{
    public string
        pythonPath = @"C:\Users\Tuan Dung\IdeaProjects\CSCHTTT\venv\Scripts\python.exe"; // Path to Python interpreter

    public string
        scriptPath = @"C:\Users\Tuan Dung\IdeaProjects\CSCHTTT\.idea\Chess\Model3-Evaluation\Predict.py"; // Path to the Python script

    public bool whiteTeam = false;

    private bool isPredicting = false;

    private Process pythonProcess;

    private void Update()
    {
        if (!isPredicting && InteractSystem.Instance.isWhiteTurn == whiteTeam)
        {
            Debug.Log("predict");
            PredictAndMove();
        }
        // Đọc output lỗi từ Python
        pythonProcess.ErrorDataReceived += (sender, args) =>
        {
            if (!string.IsNullOrEmpty(args.Data))
            {
                Debug.LogError($"Python Error: {args.Data}");
            }
        };
    }

    void Start()
    {
        // Khởi động Python khi Unity bắt đầu
        pythonProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = pythonPath,
                Arguments = $"\"{scriptPath}\"",
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
            }
        };
        
        pythonProcess.Start();
        
        
    }


    public string GetPredictedMove(string fen)
    {
        if (pythonProcess == null || pythonProcess.HasExited)
        {
            if (pythonProcess == null) UnityEngine.Debug.LogError("Python process == null.");
            if (pythonProcess.HasExited) UnityEngine.Debug.LogError("Python process has Exited.");
            
            return null;
        }
        // Gửi FEN đến Python script qua Standard Input
        pythonProcess.StandardInput.WriteLine(fen);
        pythonProcess.StandardInput.Flush();
        
        // Đợi output mới từ Python script
        string result = pythonProcess.StandardOutput.ReadLine();

        return result;
    }
    
    
    
    void OnApplicationQuit()
    {
        // Đóng Python khi Unity dừng
        if (pythonProcess != null && !pythonProcess.HasExited)
        {
            pythonProcess.Kill();
            pythonProcess.Dispose();
        }
    }
    
    // Hàm gọi Python script và đợi output mới
    public async Task<string> GetPredictedMoveAsync(string fen)
    {
        if (pythonProcess == null || pythonProcess.HasExited)
        {
            if (pythonProcess == null)
            {
                Debug.LogError("Python process null");
            }
            else
            {
                Debug.LogError("Python process đã thoát.");
            }   
            return null;
        }

        // Gửi FEN đến Python script qua Standard Input
        await pythonProcess.StandardInput.WriteLineAsync(fen);
        pythonProcess.StandardInput.Flush();

        // Đợi output mới từ Python script
        string result = await pythonProcess.StandardOutput.ReadLineAsync();
        return result?.Trim();
    }
    
    public async void PredictAndMove()
    {
        isPredicting = true;

        string fen = ConvertBoardToFen(ChessBoard.Instance.ChessPieces,
            InteractSystem.Instance.isWhiteTurn,
            InteractSystem.Instance.whiteKing.CanCastle(ChessBoard.Instance.ChessPieces[7, 0]),
            InteractSystem.Instance.whiteKing.CanCastle(ChessBoard.Instance.ChessPieces[0, 0]),
            InteractSystem.Instance.whiteKing.CanCastle(ChessBoard.Instance.ChessPieces[7, 7]),
            InteractSystem.Instance.whiteKing.CanCastle(ChessBoard.Instance.ChessPieces[0, 7]),
            Vector2IntToString(Pawn._enPassantTarget),
            0, 1);

        // Đợi kết quả từ GetPredictedMoveAsync
        string moveInString = await GetPredictedMoveAsync(fen);
        if (moveInString != null)
        {
            (int, int, int, int) move = StringMoveToInt(moveInString);
            ChessBoard.Instance.BotMoveTo(
                ChessBoard.Instance.ChessPieces[move.Item1, move.Item2],
                move.Item3, move.Item4);
            InteractSystem.Instance.isWhiteTurn = !whiteTeam;
        }
        
        isPredicting = false;
    }

    public string ConvertBoardToFen(ChessPiece[,] board, bool isWhiteTurn, bool whiteCanCastleKingSide,
        bool whiteCanCastleQueenSide, bool blackCanCastleKingSide, bool blackCanCastleQueenSide, string enPassantTarget,
        int halfmoveClock, int fullmoveNumber)
    {
        string fen = "";

        // 1. Piece Placement (8x8 board)
        for (int row = 7; row >= 0; row--) // Traverse from top rank (8) to bottom rank (1)
        {
            int emptyCount = 0;

            for (int col = 0; col < 8; col++) // Traverse each file from left to right
            {
                ChessPiece piece = board[col, row]; // Accessing board[col, row]

                if (piece == null || string.IsNullOrEmpty(GetNameFromType(piece.team, piece.type))) // Empty square
                {
                    emptyCount++;
                }
                else // Non-empty square
                {
                    if (emptyCount > 0)
                    {
                        fen += emptyCount.ToString();
                        emptyCount = 0;
                    }

                    fen += GetNameFromType(piece.team, piece.type);
                }
            }

            if (emptyCount > 0) fen += emptyCount.ToString(); // Add remaining empty squares
            if (row > 0) fen += "/"; // Add '/' after each row except the last
        }

        // 2. Active Color
        fen += " " + (isWhiteTurn ? "w" : "b");

        // 3. Castling Availability
        string castling = "";
        if (whiteCanCastleKingSide) castling += "K";
        if (whiteCanCastleQueenSide) castling += "Q";
        if (blackCanCastleKingSide) castling += "k";
        if (blackCanCastleQueenSide) castling += "q";
        fen += " " + (string.IsNullOrEmpty(castling) ? "-" : castling);

        // 4. En Passant Target
        fen += " " + (enPassantTarget ?? "-");

        // 5. Halfmove Clock
        fen += " " + halfmoveClock;

        // 6. Fullmove Number
        fen += " " + fullmoveNumber;
        Debug.Log(fen);
        return fen;
    }


    private string Vector2IntToString(Vector2Int vector2Int)
    {
        if (vector2Int == -Vector2Int.one) return "-"; // Không có En Passant Target hợp lệ

        int targetRow = vector2Int.y; // Tính hàng trung gian
        char targetCol = (char)('a' + vector2Int.x); // Tính cột (a-h)
        return $"{targetCol}{targetRow + 1}"; // Trả về tọa độ En Passant (FEN sử dụng hàng từ 1-8)
    }

    private (int, int, int, int) StringMoveToInt(string move)
    {
        Debug.Log(move);
        if (move.Length != 4 && move.Length != 5) {return (-1, -1, -1, -1);}
        (int, int, int, int) res = (-1, -1, -1, -1);
        res.Item1 = (int)(move[0] - 'a');
        res.Item2 = (int)(move[1] - '1');
        res.Item3 = (int)(move[2] - 'a');
        res.Item4 = (int)(move[3] - '1');
        if (move.Length == 5)
        {
            Pawn.currentPromotePawn = ChessBoard.Instance.ChessPieces[res.Item1, res.Item2];
            if (move[4] == 'q' || move[4] == 'Q')
            {
                CanvasController.Instance.OnQueenButton();
            }
            else if (move[4] == 'r' || move[4] == 'R')
            {
                CanvasController.Instance.OnRookButton();
            }
            else if (move[4] == 'n' || move[4] == 'N')
            {
                CanvasController.Instance.OnKnightButton();
            }
            else if (move[4] == 'b' || move[4] == 'B')
            {
                CanvasController.Instance.OnBishopButton();
            }
        }
        return res;
    }

    public static string GetNameFromType(PieceTeam team, PieceType type)
    {
        switch (type)
        {
            case PieceType.King:
                return team == PieceTeam.White ? "K" : "k";
            case PieceType.Queen:
                return team == PieceTeam.White ? "Q" : "q";
            case PieceType.Rook:
                return team == PieceTeam.White ? "R" : "r";
            case PieceType.Bishop:
                return team == PieceTeam.White ? "B" : "b";
            case PieceType.Knight:
                return team == PieceTeam.White ? "N" : "n";
            case PieceType.Pawn:
                return team == PieceTeam.White ? "P" : "p";
            default:
                return "";
        }
    }
}