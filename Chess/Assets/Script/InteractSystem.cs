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

    public ChessPiece currentChosenChessPiece;
    public Vector2Int previousPosition;
    
    void FixedUpdate()
    {
        RaycastHit hit;
        Ray ray = ChessBoard.Instance.gameCamera.ScreenPointToRay(Input.mousePosition);

        // Raycast to detect if mouse is hovering over a Tile and is not over UI
        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;

            // If the object hit is a Tile
            if (selectionTransform.gameObject.CompareTag("Tile"))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Vector2Int choosePiecePosition = ChessBoard.Instance.GetTileIndex(selectionTransform.gameObject);
                    if (ChessBoard.Instance.ChessPieces[choosePiecePosition.x, choosePiecePosition.y] != null)
                    {
                        // if my turn
                        if (true)
                        {
                            currentChosenChessPiece = ChessBoard.Instance.ChessPieces[choosePiecePosition.x, choosePiecePosition.y];
                        }
                    }
                }

                if (currentChosenChessPiece != null && Input.GetMouseButtonUp(0))
                {
                    Vector2Int chooseTargetPosition = ChessBoard.Instance.GetTileIndex(selectionTransform.gameObject);
                    if (currentChosenChessPiece.IsValidMove(chooseTargetPosition.x, chooseTargetPosition.y))
                    {
                        previousPosition = new Vector2Int(currentChosenChessPiece.currentX,
                            currentChosenChessPiece.currentY);
                        
                    }
                }
            }
        }
        
    }
}
