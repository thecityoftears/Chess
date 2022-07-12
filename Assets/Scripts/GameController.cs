using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    public Image endGameScreen;
    public Board gameBoard;
    public PieceController pieceController;
    public Text endGameText;
    void Start()
    {
        gameBoard.Create();
        pieceController.Setup(gameBoard, this);
    }

    public void EndOfGame(string str, Color color)
    {
        if (str == "checkmate")
        {
            if (color == Color.black)
            {
                endGameText.text = "Checkmate! Black has won.";
            }
            else
            {
                endGameText.text = "Checkmate! White has won.";
            }
        }
        else
        {
            endGameText.text = "Stalemate! It's a draw!";
        }
        endGameScreen.gameObject.SetActive(true);
    }

    public void DisableEndOfGame()
    {
        endGameScreen.gameObject.SetActive(false);
    }
}
