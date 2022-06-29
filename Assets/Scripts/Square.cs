using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Square : MonoBehaviour
{
    public string coordinate;
    public Vector2Int mBoardPosition = Vector2Int.zero;
    public Image outlineImage;
    public Piece piece;
    public RectTransform mRectTransform = null;
    public Board thisBoard;
    public string file;
    public int rank;

    public void Setup(Vector2Int position, Board newBoard)
    {
        mBoardPosition = position;
        mRectTransform = GetComponent<RectTransform>();
        thisBoard = newBoard;
        rank = position.y + 1;
        file = newBoard.squaresDictionary[position.x];
    } 
    
    public void RemovePiece()
    {
        if (piece != null)
        {
            piece.Kill();
        }
    }
}
