using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChessBoard : MonoBehaviour
{
    public static ChessBoard Instance { get; set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            TileArray = new GameObject[BOARD_SIZE, BOARD_SIZE];
            ChessPieces = new ChessPiece[BOARD_SIZE, BOARD_SIZE];
            GenerateChessBoard();
            InitAllChessPieces();
        }
    }

    public const int BOARD_SIZE = 8;

    [SerializeField] private float tileSize;
    [SerializeField] private GameObject tile;
    [SerializeField] public Camera gameCamera;

    public GameObject[,] TileArray;
    private GameObject _lastHoveredTile;

    public ChessPiece[,] ChessPieces;
    public List<Vector2Int> availableMoves = new List<Vector2Int>();

    void FixedUpdate()
    {
        RaycastHit hit;
        Ray ray = gameCamera.ScreenPointToRay(Input.mousePosition);

        // Raycast to detect if mouse is hovering over a Tile and is not over UI
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;

            // If the object hit is a Tile
            if (selectionTransform.gameObject.CompareTag("Tile"))
            {
                // If we are hovering a different tile than before
                if (_lastHoveredTile != null && _lastHoveredTile != selectionTransform.gameObject && !IsHighlightTile(_lastHoveredTile))
                {
                    // Reset the previous tile's alpha to 0 (transparent)
                    SetTileAlpha(_lastHoveredTile, 0);
                }

                // Set alpha of the current tile to 1 (opaque)
                SetTileAlpha(selectionTransform.gameObject, 1);

                // Store the current hovered tile
                _lastHoveredTile = selectionTransform.gameObject;
            }
        }
        else
        {
            // If raycast doesn't hit any tile, reset the last hovered tile
            if (_lastHoveredTile != null)
            {
                SetTileAlpha(_lastHoveredTile, 0);
                _lastHoveredTile = null; // Reset the last hovered tile
            }
        }
    }

    private void InitAllChessPieces()
    {
        //WHITE TEAM
        ChessPieces[0, 0] = SpawnSinglePiece(PieceTeam.White, PieceType.Rook, 0, 0);
        ChessPieces[1, 0] = SpawnSinglePiece(PieceTeam.White, PieceType.Knight, 1, 0);
        ChessPieces[2, 0] = SpawnSinglePiece(PieceTeam.White, PieceType.Bishop, 2, 0);
        ChessPieces[3, 0] = SpawnSinglePiece(PieceTeam.White, PieceType.Queen, 3, 0);
        ChessPieces[4, 0] = SpawnSinglePiece(PieceTeam.White, PieceType.King, 4, 0);
        ChessPieces[5, 0] = SpawnSinglePiece(PieceTeam.White, PieceType.Bishop, 5, 0);
        ChessPieces[6, 0] = SpawnSinglePiece(PieceTeam.White, PieceType.Knight, 6, 0);
        ChessPieces[7, 0] = SpawnSinglePiece(PieceTeam.White, PieceType.Rook, 7, 0);
        for (int x = 0; x < BOARD_SIZE; x++)
        {
            ChessPieces[x, 1] = SpawnSinglePiece(PieceTeam.White, PieceType.Pawn, x, 1);
        }
        //BLACK TEAM
        ChessPieces[0, 7] = SpawnSinglePiece(PieceTeam.Black, PieceType.Rook, 0, 7);
        ChessPieces[1, 7] = SpawnSinglePiece(PieceTeam.Black, PieceType.Knight, 1, 7);
        ChessPieces[2, 7] = SpawnSinglePiece(PieceTeam.Black, PieceType.Bishop, 2, 7);
        ChessPieces[3, 7] = SpawnSinglePiece(PieceTeam.Black, PieceType.King, 3, 7);
        ChessPieces[4, 7] = SpawnSinglePiece(PieceTeam.Black, PieceType.Queen, 4, 7);
        ChessPieces[5, 7] = SpawnSinglePiece(PieceTeam.Black, PieceType.Bishop, 5, 7);
        ChessPieces[6, 7] = SpawnSinglePiece(PieceTeam.Black, PieceType.Knight, 6, 7);
        ChessPieces[7, 7] = SpawnSinglePiece(PieceTeam.Black, PieceType.Rook, 7, 7);
        for (int x = 0; x < BOARD_SIZE; x++)
        {
            ChessPieces[x, 6] = SpawnSinglePiece(PieceTeam.Black, PieceType.Pawn, x, 6);
        }
    }

    public void ResetChessBoard()
    {
        //Delete All Remaining Pieces
        for (int x = 0; x < BOARD_SIZE; x++)
        {
            for (int y = 0; y < BOARD_SIZE; y++)
            {
                DestroyPieceOnTile(x,y);
            }
        }
        availableMoves.Clear();
        InitAllChessPieces();
    }

    public void DestroyPieceOnTile(int x, int y)
    {
        if (TileArray[x, y].transform.childCount > 0)
        {
            foreach (Transform child in TileArray[x, y].transform)
            {
                Destroy(child.gameObject);
            }
        }
        ChessPieces[x, y] = null;
    }

    public ChessPiece SpawnSinglePiece(PieceTeam team, PieceType type, int x, int y)
    {
        ChessPiece chessPiece = ChessAssets.i.SpawnChessPiecePrefab(team, type, TileArray[x, y].transform);
        PositioningChessPiece(chessPiece, TileArray[x, y].transform);
        return chessPiece;
    }
    
    public void PositioningChessPiece(ChessPiece chessPiece, Transform parent)
    {
        chessPiece.transform.position = parent.position;
        chessPiece.transform.parent = parent.transform;
        Vector2Int position = GetTileIndex(parent.gameObject);
        chessPiece.currentX = position.x;
        chessPiece.currentY = position.y;
    }

    public void MoveTo(ChessPiece chessPiece, int x, int y)
    {
        //Nếu có quân cờ ở đó
        if (ChessPieces[x, y] != null)
        {
            if (chessPiece.team == ChessPieces[x, y].team)
            {
                if (chessPiece.type != PieceType.King || ChessPieces[x, y].type != PieceType.Rook)
                {
                    Debug.Log("return");
                    return;
                }
            }
            
            //Checkmate
            if (ChessPieces[x, y].type == PieceType.King)
            {
                CheckMate(ChessPieces[x, y].team == PieceTeam.Black ? PieceTeam.Black : PieceTeam.White);
            }
            foreach (Transform child in TileArray[x,y].transform)
            {
                Destroy(child.gameObject);
            }
        }
        
        Vector2Int previousPosition = new Vector2Int(chessPiece.currentX, chessPiece.currentY);
        chessPiece.SetPosition(new Vector2Int(x, y));
        ChessPieces[x, y] = chessPiece;
        ChessPieces[previousPosition.x, previousPosition.y] = null;
        chessPiece.isMoving = true;
        chessPiece.hasMoved = true;
    }

    /**
     *  <param name="alpha">0:Default 1:Hover.</param>
     */
    private void SetTileAlpha(GameObject tile, float alpha)
    {
        Renderer tileRenderer = tile.GetComponent<Renderer>();
        if (tileRenderer != null)
        {
            Color tileColor = tileRenderer.material.color;
            tileColor.a = alpha;
            tileRenderer.material.color = tileColor;
        }
    }
    
    private void ShowHighlightTile(GameObject tile)
    {
        Renderer tileRenderer = tile.GetComponent<Renderer>();
        if (tileRenderer != null)
        {
            Color tileColor = new Color(0,255,0,0);
            tileRenderer.material.color = tileColor;
        }
        SetTileAlpha(tile, 1);
    }
    
    private void HideHighlightTile(GameObject tile)
    {
        Renderer tileRenderer = tile.GetComponent<Renderer>();
        if (tileRenderer != null)
        {
            Color tileColor = new Color(255,0,0,0);
            tileRenderer.material.color = tileColor;
        }
        SetTileAlpha(tile, 0);
    }

    private void GenerateChessBoard()
    {
        for (int x = 0; x < BOARD_SIZE; x++)
        {
            for (int y = 0; y < BOARD_SIZE; y++)
            {
                TileArray[x, y] = GenerateSingleTile(x, y);
            }
        }
    }

    private GameObject GenerateSingleTile(int x, int y)
    {
        GameObject tileObject = Instantiate(tile, transform);
        tileObject.name = $"X:{x}, Y:{y}";
        tileObject.transform.position = new Vector3(x * tileSize, 0.2402f, y * tileSize);
        Renderer tileObjectRenderer = tileObject.GetComponent<Renderer>();
        Color color = tileObjectRenderer.material.color;
        color.a = 0;
        tileObjectRenderer.material.color = color;
        return tileObject;
    }

    public Vector2Int GetTileIndex(GameObject currentTile)
    {
        for (int x = 0; x < BOARD_SIZE; x++)
        {
            for (int y = 0; y < BOARD_SIZE; y++)
            {
                if (currentTile.name == TileArray[x, y].name)
                {
                    return new Vector2Int(x, y);
                }
            }
        }
        return -Vector2Int.one;
    }

    public void ShowAllHighlightAvailableMoves(List<Vector2Int> availableMoves)
    {
        HideAllHighlightAvailableMoves();

        this.availableMoves = availableMoves;
        
        foreach (Vector2Int availableMoveTile in this.availableMoves)
        {
            ShowHighlightTile(TileArray[availableMoveTile.x, availableMoveTile.y]);
        }
    }
    
    public void HideAllHighlightAvailableMoves()
    {
        foreach (Vector2Int availableMoveTile in this.availableMoves)
        {
            HideHighlightTile(TileArray[availableMoveTile.x, availableMoveTile.y]);
        }
        availableMoves.Clear();
    }

    private bool IsHighlightTile(GameObject hoverTile)
    {
        foreach (Vector2Int availableMoveTile in this.availableMoves)
        {
            if (TileArray[availableMoveTile.x, availableMoveTile.y].name == hoverTile.name) return true;
        }

        return false;
    }

    private void CheckMate(PieceTeam team)
    {
        DisplayVictory(team);
    }
    
    private void DisplayVictory(PieceTeam team)
    {
        CanvasController.Instance.ShowEndGameMenu(team);
    }
}