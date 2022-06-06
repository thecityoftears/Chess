using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Square : MonoBehaviour
{
    public string coordinate;
    public Vector2Int mBoardPosition = Vector2Int.zero;
    public Image outlineImage;
    public bool occupiedByWhite;
    public bool occupiedByBlack;
    public Piece piece;
    public RectTransform mRectTransform = null;
    public Board thisBoard;

    public void Setup(Vector2Int position, Board newBoard)
    {
        mBoardPosition = position;
        mRectTransform = GetComponent<RectTransform>();
        thisBoard = newBoard;
    }
}
