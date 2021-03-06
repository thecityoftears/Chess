using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Board;

public abstract class Piece : EventTrigger
{
    public bool active = true;
    public Color color = Color.clear;
    public bool extra = false;

    protected Square originalSquare = null;
    public Square currentSquare = null;

    protected RectTransform mRectTransform = null;
    protected PieceController pieceController;

    protected Vector3Int movement = Vector3Int.one;
    protected List<Square> mHighlightedSquares = new List<Square>();

    protected Square targetSquare = null;

    public virtual void Setup(Color pieceColor, PieceController newPieceController, Sprite pieceImage)
    {
        pieceController = newPieceController;
        color = pieceColor;
        mRectTransform = GetComponent<RectTransform>();
        GetComponent<Image>().sprite = pieceImage;
    }

    public void Place(Square square)
    {
        originalSquare = square;
        currentSquare = square;
        currentSquare.piece = this;

        transform.position = square.transform.position;
        gameObject.SetActive(true);
    }

    protected virtual void CreateSquarePath(int x, int y, int movement)
    {
        int currentX = currentSquare.mBoardPosition.x;
        int currentY = currentSquare.mBoardPosition.y;

        for (int i = 1; i <= movement; i++)
        {
            currentX += x;
            currentY += y;

            SquareState squareState = SquareState.None;
            squareState = currentSquare.thisBoard.ValidateSquare(currentX, currentY, this);

            if (squareState == SquareState.Hostile)
            {
                if (pieceController.CheckLegalMove(currentSquare, currentSquare.thisBoard.allSquares[currentX, currentY], this, color))
                {
                    mHighlightedSquares.Add(currentSquare.thisBoard.allSquares[currentX, currentY]);
                }
                break;
            }

            if (squareState == SquareState.Free)
            {
                if (pieceController.CheckLegalMove(currentSquare, currentSquare.thisBoard.allSquares[currentX, currentY], this, color))
                {
                    mHighlightedSquares.Add(currentSquare.thisBoard.allSquares[currentX, currentY]);
                }
            }

            if (squareState == SquareState.Friendly)
            {
                break;
            }
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

    // these two functions calculate checkmate/stalemate
    // iterate through all of the pieces of the current player, if there are no legal moves, decide if it is a checkmate or a stalemate
    public virtual bool HasLegalMoves()
    {
        if (CheckLegalMoves(1, 0, movement.x)
            || CheckLegalMoves(-1, 0, movement.x)
            || CheckLegalMoves(0, 1, movement.y)
            || CheckLegalMoves(0, -1, movement.y)
            || CheckLegalMoves(1, 1, movement.z)
            || CheckLegalMoves(-1, 1, movement.z)
            || CheckLegalMoves(1, -1, movement.z)
            || CheckLegalMoves(-1, -1, movement.z))
        {
            return true;
        }
        return false;
    }

    protected virtual bool CheckLegalMoves(int x, int y, int movement)
    {
        int currentX = currentSquare.mBoardPosition.x;
        int currentY = currentSquare.mBoardPosition.y;

        for (int i = 1; i <= movement; i++)
        {
            currentX += x;
            currentY += y;

            SquareState squareState = SquareState.None;
            squareState = currentSquare.thisBoard.ValidateSquare(currentX, currentY, this);

            if (squareState == SquareState.Hostile)
            {
                if (pieceController.CheckLegalMove(currentSquare, currentSquare.thisBoard.allSquares[currentX, currentY], this, color))
                {
                    return true;
                }
                break;
            }

            if (squareState == SquareState.Free)
            {
                if (pieceController.CheckLegalMove(currentSquare, currentSquare.thisBoard.allSquares[currentX, currentY], this, color))
                {
                    return true;
                }
            }

            if (squareState == SquareState.Friendly)
            {
                break;
            }
        }

        return false;
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

        foreach (Square square in mHighlightedSquares)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(square.mRectTransform, Input.mousePosition))
            {
                targetSquare = square;
                break;
            }
            targetSquare = null;
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        ClearSquares();

        if (!targetSquare)
        {
            transform.position = currentSquare.gameObject.transform.position;
        }

        Move();

        pieceController.SwitchSides(color);
        pieceController.CheckEndOfGame(color);
    }

    public void Reset()
    {
        Kill();
        Place(originalSquare);
        active = true;
    }

    public virtual void Kill()
    {
        active = false;
        currentSquare.piece = null;
        gameObject.SetActive(false);
    }

    protected virtual void Move()
    {
        targetSquare.RemovePiece();
        currentSquare.piece = null;
        currentSquare = targetSquare;
        currentSquare.piece = this;
        transform.position = currentSquare.transform.position;
        targetSquare = null;
        pieceController.UpdateMovedPawns(color);
    }
}
