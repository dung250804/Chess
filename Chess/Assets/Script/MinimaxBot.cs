using System.Threading.Tasks;
using UnityEngine;

public class MinimaxBot : MonoBehaviour
{
    public MinimaxAlgorithm minimax = new MinimaxAlgorithm();

    void Update()
    {
        if (InteractSystem.Instance.isWhiteTurn) // Nếu là lượt của bot
        {
            InteractSystem.Instance.isWhiteTurn = false; // Khóa lượt trong khi bot đang suy nghĩ
            MakeAIMoveAsync(); // Thực hiện nước đi của AI trong nền
        }
    }

    // Thực hiện tìm nước đi tốt nhất bằng cách chạy Minimax trên luồng khác
    async void MakeAIMoveAsync()
    {
        var board = ChessBoard.Instance.ChessPieces;

        var (score, move) = await minimax.GetBestMoveAsync(ChessBoard.Instance.ChessPieces, 4, true);
        Debug.Log($"AI chọn nước đi: {move} với điểm: {score}");

        // Chuyển về luồng chính để di chuyển quân cờ
        ChessBoard.Instance.MoveTo(
            ChessBoard.Instance.ChessPieces[move.Item1, move.Item2], 
            move.Item3, move.Item4
        );
    }
}