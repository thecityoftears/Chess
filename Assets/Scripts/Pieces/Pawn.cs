using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Board;

public class Pawn : Piece
{
    //finish this
    public bool justDoubleMoved = false;

    public override void Setup(Color pieceColor, PieceController newPieceController, Sprite pieceImage)
    {
        base.Setup(pieceColor, newPieceController, pieceImage);

        movement = new Vector3Int(0, 2, 1);
    }

    protected override void Move()
    {
        pieceController.UpdateMovedPawns(color);
        targetSquare.RemovePiece();
        currentSquare.piece = null;
        if (Math.Abs(targetSquare.rank - currentSquare.rank) == 2)
        {
            justDoubleMoved = true;
        }
        currentSquare = targetSquare;
        currentSquare.piece = this;
        transform.position = currentSquare.transform.position;
        targetSquare = null;
        movement = new Vector3Int(0, 1, 1);
    }

    protected override void CheckSquarePathing()
    {
        if (originalSquare.rank == 2)
        {
            CreateSquarePath(0, 1, movement.y);
            CreateSquarePath(1, 1, movement.z);
            CreateSquarePath(-1, 1, movement.z);
        }
        else
        {
            CreateSquarePath(0, -1, movement.y);
            CreateSquarePath(1, -1, movement.z);
            CreateSquarePath(-1, -1, movement.z);
        }
    }

    protected override void CreateSquarePath(int x, int y, int movement)
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
                if (x != 0)
                {
                    mHighlightedSquares.Add(currentSquare.thisBoard.allSquares[currentX, currentY]);
                }
                break;
            }

            if (squareState == SquareState.Free)
            {
                if (x == 0)
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
}
