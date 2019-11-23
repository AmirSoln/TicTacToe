using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Player
{
    public Image panel;
    public Text text;
    public Button button;
}

[System.Serializable]
public class PlayerColor
{
    public Color panelColor;
    public Color textColor;
}

public class GameController : MonoBehaviour
{
    public const string DRAW = "It's a draw! FFS =(";
    public const string WIN = " Wins! =3";

    public Text[] buttonsList;
    private string playerSide;
    public GameObject gameOverPanel;
    public Text gameOverText;
    private int moveCount;
    public GameObject restartButton;
    public Player playerX;
    public Player playerO;
    public PlayerColor activePlayerColor;
    public PlayerColor inactivePlayerColor;
    public GameObject startInfo;

    public void SetPlayerButtons(bool isInteractable)
    {
        playerX.button.interactable = isInteractable;
        playerO.button.interactable = isInteractable;
    }

    public void SetPlayerColors(Player newPlayer, Player oldPlayer)
    {
        newPlayer.panel.color = activePlayerColor.panelColor;
        newPlayer.text.color = activePlayerColor.textColor;
        oldPlayer.panel.color = inactivePlayerColor.panelColor;
        oldPlayer.text.color = inactivePlayerColor.textColor;
    }
    public void SetGameControllerForButtons()
    {
        foreach (Text txt in buttonsList)
        {
            txt.GetComponentInParent<GridSpace>().SetGameController(this);
        }
    }

    public void Awake()
    {
        this.moveCount = 0;
        SetGameControllerForButtons();
        gameOverPanel.SetActive(false);
        restartButton.SetActive(false);
    }

    public string GetPlayerSide()
    {
        return this.playerSide;
    }

    public void EndTurn()
    {
        moveCount++;
        if (CheckRows() || CheckCols() || CheckDiagonals())
        {
            GameOver(playerSide + WIN);
        }
        else if (moveCount >= 9)
        {
            GameOver(DRAW);
            SetPlayerColorsInactive();
        }
        else
        {
            ChangeSides();
        }
    }

    public bool CheckRows()
    {
        for (int i = 0; i < buttonsList.Length; i += 3)
        {
            if (buttonsList[i].text == buttonsList[i + 1].text && buttonsList[i].text == buttonsList[i + 2].text && buttonsList[i].text == playerSide)
            {
                return true;
            }
        }
        return false;
    }
    public bool CheckCols()
    {
        for (int i = 0; i < buttonsList.Length / 3; i++)
        {
            if (buttonsList[i].text == buttonsList[i + 3].text && buttonsList[i].text == buttonsList[i + 6].text && buttonsList[i].text == playerSide)
            {
                return true;
            }
        }
        return false;
    }
    public bool CheckDiagonals()
    {
        if (buttonsList[0].text == buttonsList[4].text && buttonsList[0].text == buttonsList[8].text && buttonsList[0].text == playerSide)
        {
            return true;
        }
        else if (buttonsList[2].text == buttonsList[4].text && buttonsList[2].text == buttonsList[6].text && buttonsList[2].text == playerSide)
        {
            return true;
        }
        return false;
    }

    public void ChangeSides()
    {
        playerSide = (playerSide == "X") ? "O" : "X";
        if (playerSide == "X")
        {
            SetPlayerColors(playerX, playerO);
        }
        else
        {
            SetPlayerColors(playerO, playerX);
        }
    }

    public void GameOver(string msg)
    {
        SetBoardInteractable(false);
        SetGameOverVisually(msg);
        restartButton.SetActive(true);
    }

    public void RestartGame()
    {
        moveCount = 0;
        gameOverPanel.SetActive(false);
        foreach (Text txt in buttonsList)
        {
            txt.text = "";
        }
        restartButton.SetActive(false);
        SetPlayerButtons(true);
        SetPlayerColorsInactive();
        startInfo.SetActive(true);
    }

    public void SetPlayerColorsInactive()
    {
        playerX.panel.color = inactivePlayerColor.panelColor;
        playerX.text.color = inactivePlayerColor.textColor;
        playerO.panel.color = inactivePlayerColor.panelColor;
        playerO.text.color = inactivePlayerColor.textColor;
    }

    public void StartGame()
    {
        SetBoardInteractable(true);
        SetPlayerButtons(false);
        startInfo.SetActive(false);
    }

    public void SetBoardInteractable(bool isInteractable)
    {
        foreach (Text txt in buttonsList)
        {
            txt.GetComponentInParent<Button>().interactable = isInteractable;
        }
    }

    /// <summary>
    /// set's the text and show the panel with GameOverText
    /// </summary>
    /// <param name="text"> the text to show</param>
    public void SetGameOverVisually(string text)
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = text;
    }

    public void SetStartingSide(string startingSide)
    {
        playerSide = startingSide;
        if (playerSide == "X")
        {
            SetPlayerColors(playerX, playerO);
        }
        else
        {
            SetPlayerColors(playerO, playerX);
        }
        StartGame();
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}