using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class Piece : EventTrigger
{
    public Color color = Color.clear;

    protected Square originalSquare = null;
    protected Square currentSquare = null;

    protected RectTransform mRectTransform = null;
    protected PieceController pieceController;

    protected Vector3Int movement = Vector3Int.one;
    protected List<Square> mHighlightedSquares = new List<Square>();

    public virtual void Setup(Color pieceColor, Color32 spriteColor, PieceController newPieceController)
    {
        pieceController = newPieceController;
        color = pieceColor;
        GetComponent<Image>().color = spriteColor;
        mRectTransform = GetComponent<RectTransform>();
    }

    public void Place(Square square)
    {
        originalSquare = square;
        currentSquare = square;
        currentSquare.piece = this;

        transform.position = square.transform.position;
        gameObject.SetActive(true);
    }

    private void CreateSquarePath(int x, int y, int movement)
    {
        int currentX = currentSquare.mBoardPosition.x;
        int currentY = currentSquare.mBoardPosition.y;

        for (int i = 1; i <= movement; i++)
        {
            currentX += x;
            currentY += y;

            mHighlightedSquares.Add(currentSquare.thisBoard.allSquares[currentX, currentY]);
        }
    }

    protected virtual void CheckSquarePathing()
    {
        CreateSquarePath(1, 0, movement.x);
        CreateSquarePath(-1, 0, movement.x);

        CreateSquarePath(0, 1, movement.y);
        CreateSquarePath(0, -1, movement.y);

        CreateSquarePath(1, 1, movement.z);
        CreateSquarePath(-1, 1, movement.z);
        CreateSquarePath(1, -1, movement.z);
        CreateSquarePath(-1, -1, movement.z);
    }

    protected void ShowSquares()
    {
        foreach (Square square in mHighlightedSquares)
        {
            square.outlineImage.enabled = true;
        }
    }

    protected void ClearSquares()
    {
        foreach (Square square in mHighlightedSquares)
        {
            square.outlineImage.enabled = false;
        }
        mHighlightedSquares.Clear();
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        CheckSquarePathing();
        ShowSquares();
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        transform.position += (Vector3)eventData.delta;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        ClearSquares();
    }
}
