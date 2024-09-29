using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessAssets : MonoBehaviour
{
    private static ChessAssets _i;

    public static ChessAssets i
    {
        get
        {
            if (_i == null) _i = Instantiate(Resources.Load("ChessAssets") as GameObject).GetComponent<ChessAssets>();
            return _i;
        }
    }

    // BLACK CHESS PIECE
    public GameObject kingBlack;
    public GameObject queenBlack;
    public GameObject bishopBlack;
    public GameObject knightBlack;
    public GameObject rookBlack;
    public GameObject pawnBlack;
    
    // WHITE CHESS PIECE
    public GameObject kingWhite;
    public GameObject queenWhite;
    public GameObject bishopWhite;
    public GameObject knightWhite;
    public GameObject rookWhite;
    public GameObject pawnWhite;
    
    //Chess piece data
    public List<GameObject> chessPiecePrefab;

    private void Awake()
    {
        chessPiecePrefab.Add(kingBlack);
        chessPiecePrefab.Add(queenBlack);
        chessPiecePrefab.Add(bishopBlack);
        chessPiecePrefab.Add(knightBlack);
        chessPiecePrefab.Add(rookBlack);
        chessPiecePrefab.Add(pawnBlack);
        chessPiecePrefab.Add(kingWhite);
        chessPiecePrefab.Add(queenWhite);
        chessPiecePrefab.Add(bishopWhite);
        chessPiecePrefab.Add(knightWhite);
        chessPiecePrefab.Add(rookWhite);
        chessPiecePrefab.Add(pawnWhite);
    }

    public ChessPiece SpawnChessPiecePrefab(PieceTeam team, PieceType type, Transform parent)
    {
        foreach (GameObject prefab in chessPiecePrefab)
        {
            ChessPiece chessPiece = prefab.GetComponent<ChessPiece>();
            if (chessPiece.team == team && chessPiece.type == type)
            {
                GameObject spawnChessPiece = Instantiate(prefab);
                spawnChessPiece.transform.position = parent.position;
                spawnChessPiece.transform.parent = parent;
                ChessPiece result = spawnChessPiece.GetComponent<ChessPiece>();
                return result;
            }
        }

        return null;
    }
}
