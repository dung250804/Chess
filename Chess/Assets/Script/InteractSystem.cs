using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSystem : MonoBehaviour
{
    public static InteractSystem Instance { get; set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private King whiteKing;
    private King blackKing; 
    
    public ChessPiece currentChosenChessPiece;
    public Vector2Int previousPosition;
    public bool isWhiteTurn = true;

    void Update()
    {
        if (CanvasController.Instance.isOpen) return;
        if (!Input.GetMouseButtonDown(0)) return;
        RaycastHit hit;
        Ray ray = ChessBoard.Instance.gameCamera.ScreenPointToRay(Input.mousePosition);

        // Raycast to detect if mouse is hovering over a Tile and is not over UI
        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;

            // If the object hit is a Tile
            if (selectionTransform.gameObject.CompareTag("Tile"))
            {
                if (currentChosenChessPiece != null)
                {
                    Vector2Int chooseTargetPosition = ChessBoard.Instance.GetTileIndex(selectionTransform.gameObject);
                    if (currentChosenChessPiece.IsValidMove(chooseTargetPosition.x, chooseTargetPosition.y))
                    {
                        previousPosition = new Vector2Int(currentChosenChessPiece.currentX,
                            currentChosenChessPiece.currentY);
                        Debug.Log(currentChosenChessPiece);
                        ChessBoard.Instance.MoveTo(currentChosenChessPiece, chooseTargetPosition.x,
                            chooseTargetPosition.y);
                        currentChosenChessPiece = null;
                        ChessBoard.Instance.HideAllHighlightAvailableMoves();
                        isWhiteTurn = !isWhiteTurn;
                        if (isWhiteTurn)
                        {
                            if (whiteKing.IsCheckmate())
                            {
                                ChessBoard.Instance.CheckMate(PieceTeam.White);
                            }
                        }
                        else
                        {
                            if (blackKing.IsCheckmate())
                            {
                                ChessBoard.Instance.CheckMate(PieceTeam.Black);
                            }
                        }
                        return;
                    }
                    currentChosenChessPiece = null;
                    ChessBoard.Instance.HideAllHighlightAvailableMoves();
                }
                    
                Vector2Int choosePiecePosition = ChessBoard.Instance.GetTileIndex(selectionTransform.gameObject);
                if (ChessBoard.Instance.ChessPieces[choosePiecePosition.x, choosePiecePosition.y] != null)
                {
                    // if my turn
                    if ((ChessBoard.Instance.ChessPieces[choosePiecePosition.x, choosePiecePosition.y].team == PieceTeam.White && isWhiteTurn)
                        || (ChessBoard.Instance.ChessPieces[choosePiecePosition.x, choosePiecePosition.y].team == PieceTeam.Black && !isWhiteTurn))
                    {
                        List<Vector2Int> availableMoves = new List<Vector2Int>();
                        if (!whiteKing.IsCheck() && !blackKing.IsCheck())
                        {
                            currentChosenChessPiece =
                                ChessBoard.Instance.ChessPieces[choosePiecePosition.x, choosePiecePosition.y];
                            availableMoves = currentChosenChessPiece.GetAvailableMoves();
                            ChessBoard.Instance.ShowAllHighlightAvailableMoves(availableMoves);
                        }
                        else
                        {
                            if (isWhiteTurn)
                            {
                                if (whiteKing.IsCheck())
                                {
                                    foreach (var chessPiece in whiteKing.GetPiecesWithValidMoves())
                                    {
                                        if (chessPiece.Key == ChessBoard.Instance.ChessPieces[choosePiecePosition.x,
                                                choosePiecePosition.y])
                                        {
                                            currentChosenChessPiece =
                                                chessPiece.Key;
                                            availableMoves = chessPiece.Value;
                                            ChessBoard.Instance.ShowAllHighlightAvailableMoves(availableMoves);
                                            return;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (blackKing.IsCheck())
                                {
                                    foreach (var chessPiece in blackKing.GetPiecesWithValidMoves())
                                    {
                                        if (chessPiece.Key == ChessBoard.Instance.ChessPieces[choosePiecePosition.x,
                                                choosePiecePosition.y])
                                        {
                                            currentChosenChessPiece =
                                                chessPiece.Key;
                                            availableMoves = chessPiece.Value;
                                            ChessBoard.Instance.ShowAllHighlightAvailableMoves(availableMoves);
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void FindKings()
    {
        foreach (ChessPiece piece in ChessBoard.Instance.ChessPieces)
        {
            if (piece != null && piece.type == PieceType.King)
            {
                if (piece.team == PieceTeam.White)
                {
                    whiteKing = (King)piece;
                }
                else
                {
                    blackKing = (King)piece;
                }
            }
        }
    }
}