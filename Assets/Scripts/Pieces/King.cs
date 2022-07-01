using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    public bool hasMoved = false;
   public bool IsInCheck()
    {
        return true;
    }
    protected override void CreateSquarePath(int x, int y, int movement)
    {
        base.CreateSquarePath(x, y, movement);

        // TODO check for attacked square in castle
        if (!hasMoved)
        {
            // check kingside castle
            if (currentSquare.thisBoard.allSquares[currentSquare.mBoardPosition[0] + 1, currentSquare.mBoardPosition[1]].piece == null && currentSquare.thisBoard.allSquares[currentSquare.mBoardPosition[0] + 2, currentSquare.mBoardPosition[1]].piece == null && currentSquare.thisBoard.allSquares[currentSquare.mBoardPosition[0] + 3, currentSquare.mBoardPosition[1]].piece != null)
            {
                if (currentSquare.thisBoard.allSquares[currentSquare.mBoardPosition[0] + 3, currentSquare.mBoardPosition[1]].piece.GetType() == typeof(Rook))
                {
                    if (((Rook)currentSquare.thisBoard.allSquares[currentSquare.mBoardPosition[0] + 3, currentSquare.mBoardPosition[1]].piece).hasMoved == false) {
                        mHighlightedSquares.Add(currentSquare.thisBoard.allSquares[currentSquare.mBoardPosition[0] + 2, currentSquare.mBoardPosition[1]]);
                    }
                }
            }

            // check queenside castle
            if (currentSquare.thisBoard.allSquares[currentSquare.mBoardPosition[0] - 1, currentSquare.mBoardPosition[1]].piece == null && currentSquare.thisBoard.allSquares[currentSquare.mBoardPosition[0] - 2, currentSquare.mBoardPosition[1]].piece == null && currentSquare.thisBoard.allSquares[currentSquare.mBoardPosition[0] - 3, currentSquare.mBoardPosition[1]].piece == null && currentSquare.thisBoard.allSquares[currentSquare.mBoardPosition[0] - 4, currentSquare.mBoardPosition[1]].piece != null)
            {
                if (currentSquare.thisBoard.allSquares[currentSquare.mBoardPosition[0] - 4, currentSquare.mBoardPosition[1]].piece.GetType() == typeof(Rook))
                {
                    if (((Rook)currentSquare.thisBoard.allSquares[currentSquare.mBoardPosition[0] - 4, currentSquare.mBoardPosition[1]].piece).hasMoved == false)
                    {
                        mHighlightedSquares.Add(currentSquare.thisBoard.allSquares[currentSquare.mBoardPosition[0] - 2, currentSquare.mBoardPosition[1]]);
                    }
                }
            }
        }
    }

    protected override void Move()
    {
        targetSquare.RemovePiece();
        currentSquare.piece = null;

        // castling update rook
        if (Math.Abs(targetSquare.mBoardPosition[0] - currentSquare.mBoardPosition[0]) == 2)
        {
            if (targetSquare.mBoardPosition[0] - currentSquare.mBoardPosition[0] == -2) {
                // move the rook
                targetSquare.thisBoard.allSquares[targetSquare.mBoardPosition[0] + 1, targetSquare.mBoardPosition[1]].piece = targetSquare.thisBoard.allSquares[targetSquare.mBoardPosition[0] - 2, targetSquare.mBoardPosition[1]].piece;
                // set old rook square to null
                targetSquare.thisBoard.allSquares[targetSquare.mBoardPosition[0] - 2, targetSquare.mBoardPosition[1]].piece = null;
                // set the currentSquare for the rook
                targetSquare.thisBoard.allSquares[targetSquare.mBoardPosition[0] + 1, targetSquare.mBoardPosition[1]].piece.currentSquare = targetSquare.thisBoard.allSquares[targetSquare.mBoardPosition[0] + 1, targetSquare.mBoardPosition[1]];
                // set the transform for the rook
                targetSquare.thisBoard.allSquares[targetSquare.mBoardPosition[0] + 1, targetSquare.mBoardPosition[1]].piece.transform.position = targetSquare.thisBoard.allSquares[targetSquare.mBoardPosition[0] + 1, targetSquare.mBoardPosition[1]].piece.currentSquare.transform.position;
            }
            else
            {
                targetSquare.thisBoard.allSquares[targetSquare.mBoardPosition[0] - 1, targetSquare.mBoardPosition[1]].piece = targetSquare.thisBoard.allSquares[targetSquare.mBoardPosition[0] + 1, targetSquare.mBoardPosition[1]].piece;
                targetSquare.thisBoard.allSquares[targetSquare.mBoardPosition[0] + 1, targetSquare.mBoardPosition[1]].piece = null;
                targetSquare.thisBoard.allSquares[targetSquare.mBoardPosition[0] - 1, targetSquare.mBoardPosition[1]].piece.currentSquare = targetSquare.thisBoard.allSquares[targetSquare.mBoardPosition[0] - 1, targetSquare.mBoardPosition[1]];
                targetSquare.thisBoard.allSquares[targetSquare.mBoardPosition[0] - 1, targetSquare.mBoardPosition[1]].piece.transform.position = targetSquare.thisBoard.allSquares[targetSquare.mBoardPosition[0] - 1, targetSquare.mBoardPosition[1]].piece.currentSquare.transform.position;
            }
        }
        currentSquare = targetSquare;
        currentSquare.piece = this;
        transform.position = currentSquare.transform.position;
        targetSquare = null;
        pieceController.UpdateMovedPawns(color);
        hasMoved = true;
    }
}
