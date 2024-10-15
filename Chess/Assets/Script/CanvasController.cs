using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public static CanvasController Instance { get; set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            HideInGameMenu();
        }
    }
    
    [SerializeField] private GameObject fullInGameMenu;
    [SerializeField] private GameObject menuContainer;
    [SerializeField] private GameObject promotionContainer;
    [SerializeField] private GameObject endgameContainer;
    [SerializeField] private GameObject newGameButton;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject saveButton;
    [SerializeField] private GameObject exitButton;
    
    [SerializeField] private List<Image> promotePieceImg;

    public bool isOpen = false;

    private void Update()
    {
        if (isOpen && Input.GetKeyDown(KeyCode.Alpha2))
        {
            HideInGameMenu();
        }
        else if (!isOpen && Input.GetKeyDown(KeyCode.Alpha2))
        {
            ShowInGameMenu();
        }
    }

    public void ShowInGameMenu()
    {
        isOpen = true;
        fullInGameMenu.SetActive(true);
        menuContainer.SetActive(true);
        endgameContainer.SetActive(false);
        newGameButton.SetActive(true);
        continueButton.SetActive(true);
        saveButton.SetActive(true);
        exitButton.SetActive(true);
        promotionContainer.SetActive(false);
        SetInGameMenuButtonPosition();
    }
    
    private void HideInGameMenu()
    {
        isOpen = false;
        fullInGameMenu.SetActive(false);
    }
    
    public void ShowEndGameMenu(PieceTeam team)
    {
        SetImageAndTextEndGame(team);
        isOpen = true;
        fullInGameMenu.SetActive(true);
        menuContainer.SetActive(true);
        endgameContainer.SetActive(true);
        newGameButton.SetActive(true);
        continueButton.SetActive(false);
        saveButton.SetActive(false);
        exitButton.SetActive(true);
        promotionContainer.SetActive(false);
        SetEndGameMenuButtonPosition();
    }
    
    public void ShowPromoteChoice()
    {
        if(Pawn.currentPromotePawn == null) return;
        PieceTeam team = Pawn.currentPromotePawn.team;
        isOpen = true;
        if (team == PieceTeam.Black)
        {
            int i = 0;
            foreach (Image image in promotePieceImg)
            {
                image.sprite = ChessAssets.i.blackPieceUI[i];
                i++;
            }
        } 
        else if (team == PieceTeam.White)
        {
            int i = 0;
            foreach (Image image in promotePieceImg)
            {
                image.sprite = ChessAssets.i.whitePieceUI[i];
                i++;
            }
        }
        fullInGameMenu.SetActive(true);
        promotionContainer.SetActive(true);
        menuContainer.SetActive(false);
        endgameContainer.SetActive(false);
    }

    private void SetImageAndTextEndGame(PieceTeam team)
    {
        Image image = endgameContainer.GetComponentInChildren<Image>();
        TextMeshProUGUI text = endgameContainer.GetComponentInChildren<TextMeshProUGUI>(); 
        if (team == PieceTeam.White)
        {
            image.sprite = ChessAssets.i.loseTrophy;
            text.SetText("You Lose!");
        }
        else
        {
            image.sprite = ChessAssets.i.winTrophy;
            text.SetText("You Win!");
        }
    }

    private void SetInGameMenuButtonPosition()
    {
        exitButton.transform.localPosition = new Vector3(0, -110, 0);
        newGameButton.transform.localPosition = new Vector3(0, 130, 0);
    }
    
    private void SetEndGameMenuButtonPosition()
    {
        exitButton.transform.localPosition = new Vector3(0, -40, 0);
        newGameButton.transform.localPosition = new Vector3(0, 80, 0);
    }

    public void OnNewGameButton()
    {
        HideInGameMenu();
        ChessBoard.Instance.ResetChessBoard();
    }
    
    public void OnContinueButton()
    {
        HideInGameMenu();
    }
    
    public void OnSaveButton()
    {
        HideInGameMenu();
    }

    public void OnExitButton()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    
    public void OnQueenButton()
    {
        int x = Pawn.currentPromotePawn.currentX;
        int y = Pawn.currentPromotePawn.currentY;
        PieceTeam team = Pawn.currentPromotePawn.team;
        ChessBoard.Instance.DestroyPieceOnTile(x,y);
        ChessBoard.Instance.ChessPieces[x, y] = ChessBoard.Instance.SpawnSinglePiece(team, PieceType.Queen, x, y);
        HideInGameMenu();
    }
    
    public void OnKnightButton()
    {
        int x = Pawn.currentPromotePawn.currentX;
        int y = Pawn.currentPromotePawn.currentY;
        PieceTeam team = Pawn.currentPromotePawn.team;
        ChessBoard.Instance.DestroyPieceOnTile(x,y);
        ChessBoard.Instance.ChessPieces[x, y] = ChessBoard.Instance.SpawnSinglePiece(team, PieceType.Knight, x, y);
        HideInGameMenu();
    }
    
    public void OnRookButton()
    {
        int x = Pawn.currentPromotePawn.currentX;
        int y = Pawn.currentPromotePawn.currentY;
        PieceTeam team = Pawn.currentPromotePawn.team;
        ChessBoard.Instance.DestroyPieceOnTile(x,y);
        ChessBoard.Instance.ChessPieces[x, y] = ChessBoard.Instance.SpawnSinglePiece(team, PieceType.Rook, x, y);
        HideInGameMenu();
        HideInGameMenu();
    }
    
    public void OnBishopButton()
    {
        int x = Pawn.currentPromotePawn.currentX;
        int y = Pawn.currentPromotePawn.currentY;
        PieceTeam team = Pawn.currentPromotePawn.team;
        ChessBoard.Instance.DestroyPieceOnTile(x,y);
        ChessBoard.Instance.ChessPieces[x, y] = ChessBoard.Instance.SpawnSinglePiece(team, PieceType.Bishop, x, y);
        HideInGameMenu();
        HideInGameMenu();
    }
    
}
