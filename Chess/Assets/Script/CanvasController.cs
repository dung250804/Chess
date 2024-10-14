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
    [SerializeField] private GameObject endgameContainer;
    [SerializeField] private GameObject newGameButton;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject saveButton;
    [SerializeField] private GameObject exitButton;

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
        endgameContainer.SetActive(false);
        newGameButton.SetActive(true);
        continueButton.SetActive(true);
        saveButton.SetActive(true);
        exitButton.SetActive(true);
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
        endgameContainer.SetActive(true);
        newGameButton.SetActive(true);
        continueButton.SetActive(false);
        saveButton.SetActive(false);
        exitButton.SetActive(true);
        SetEndGameMenuButtonPosition();
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
    
}
