using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update

    public Board gameBoard;
    public PieceController pieceController;
    void Start()
    {
        gameBoard.Create();
        pieceController.Setup(gameBoard);
    }

}
