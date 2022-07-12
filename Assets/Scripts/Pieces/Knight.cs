using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Board;

public class Knight : Piece
{
    protected override void CheckSquarePathing()
    {
        int currentX = currentSquare.mBoardPosition.x;
        int currentY = currentSquare.mBoardPosition.y;
        SquareState squareState = SquareState.None;

        int x = currentX + 2;
        int y = currentY + 1;
        squareState = currentSquare.thisBoard.ValidateSquare(x, y, this);
        AddHighlightedSquare(squareState, x, y);

        x = currentX + 2;
        y = currentY - 1;
        squareState = currentSquare.thisBoard.ValidateSquare(x, y, this);
        AddHighlightedSquare(squareState, x, y);

        x = currentX - 2;
        y = currentY - 1;
        squareState = currentSquare.thisBoard.ValidateSquare(x, y, this);
        AddHighlightedSquare(squareState, x, y);

        x = currentX - 2;
        y = currentY + 1;
        squareState = currentSquare.thisBoard.ValidateSquare(x, y, this);
        AddHighlightedSquare(squareState, x, y);

        x = currentX + 1;
        y = currentY + 2;
        squareState = currentSquare.thisBoard.ValidateSquare(x, y, this);
        AddHighlightedSquare(squareState, x, y);

        x = currentX - 1;
        y = currentY + 2;
        squareState = currentSquare.thisBoard.ValidateSquare(x, y, this);
        AddHighlightedSquare(squareState, x, y);

        x = currentX + 1;
        y = currentY - 2;
        squareState = currentSquare.thisBoard.ValidateSquare(x, y, this);
        AddHighlightedSquare(squareState, x, y);

        x = currentX - 1;
        y = currentY - 2;
        squareState = currentSquare.thisBoard.ValidateSquare(x, y, this);
        AddHighlightedSquare(squareState, x, y);
    }

    public override bool HasLegalMoves()
    {
        int currentX = currentSquare.mBoardPosition.x;
        int currentY = currentSquare.mBoardPosition.y;
        SquareState squareState = SquareState.None;

        int x = currentX + 2;
        int y = currentY + 1;
        squareState = currentSquare.thisBoard.ValidateSquare(x, y, this);
        if (CheckLegality(squareState, x, y))
        {
            return true;
        }

        x = currentX + 2;
        y = currentY - 1;
        squareState = currentSquare.thisBoard.ValidateSquare(x, y, this);
        if (CheckLegality(squareState, x, y))
        {
            return true;
        }

        x = currentX - 2;
        y = currentY - 1;
        squareState = currentSquare.thisBoard.ValidateSquare(x, y, this);
        if (CheckLegality(squareState, x, y))
        {
            return true;
        }

        x = currentX - 2;
        y = currentY + 1;
        squareState = currentSquare.thisBoard.ValidateSquare(x, y, this);
        if (CheckLegality(squareState, x, y))
        {
            return true;
        }

        x = currentX + 1;
        y = currentY + 2;
        squareState = currentSquare.thisBoard.ValidateSquare(x, y, this);
        if (CheckLegality(squareState, x, y))
        {
            return true;
        }

        x = currentX - 1;
        y = currentY + 2;
        squareState = currentSquare.thisBoard.ValidateSquare(x, y, this);
        if (CheckLegality(squareState, x, y))
        {
            return true;
        }

        x = currentX + 1;
        y = currentY - 2;
        squareState = currentSquare.thisBoard.ValidateSquare(x, y, this);
        if (CheckLegality(squareState, x, y))
        {
            return true;
        }

        x = currentX - 1;
        y = currentY - 2;
        squareState = currentSquare.thisBoard.ValidateSquare(x, y, this);
        if (CheckLegality(squareState, x, y))
        {
            return true;
        }

        return false;
    }

    private void AddHighlightedSquare(SquareState squareState, int x, int y)
    {
        if (squareState == SquareState.Hostile)
        {
            if (pieceController.CheckLegalMove(currentSquare, currentSquare.thisBoard.allSquares[x, y], this, color))
            {
                mHighlightedSquares.Add(currentSquare.thisBoard.allSquares[x, y]);
            }
        }

        if (squareState == SquareState.Free)
        {
            if (pieceController.CheckLegalMove(currentSquare, currentSquare.thisBoard.allSquares[x, y], this, color))
            {
                mHighlightedSquares.Add(currentSquare.thisBoard.allSquares[x, y]);
            }
        }

        if (squareState == SquareState.Friendly)
        {
        }
    }

    private bool CheckLegality(SquareState squareState, int x, int y)
    {
        if (squareState == SquareState.Hostile)
        {
            if (pieceController.CheckLegalMove(currentSquare, currentSquare.thisBoard.allSquares[x, y], this, color))
            {
                return true;
            }
        }

        if (squareState == SquareState.Free)
        {
            if (pieceController.CheckLegalMove(currentSquare, currentSquare.thisBoard.allSquares[x, y], this, color))
            {
                return true;
            }
        }

        if (squareState == SquareState.Friendly)
        {
        }

        return false;
    }
}
