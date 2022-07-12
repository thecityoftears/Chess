using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Board;

public class Pawn : Piece
{
    public bool justDoubleMoved = false;
    private bool enPassant = false;

    public override void Setup(Color pieceColor, PieceController newPieceController, Sprite pieceImage)
    {
        base.Setup(pieceColor, newPieceController, pieceImage);
        movement = new Vector3Int(0, 2, 1);
    }

    protected override void Move()
    {
        pieceController.UpdateMovedPawns(color);

        if (targetSquare.piece == null && targetSquare.file != currentSquare.file)
        {
            enPassant = true;
        }
        targetSquare.RemovePiece();

        // removing special en passant pawn
        if (enPassant)
        {
            if (color == Color.black)
            {
                targetSquare.thisBoard.allSquares[targetSquare.mBoardPosition.x, targetSquare.mBoardPosition.y + 1].RemovePiece();
            }
            else
            {
                targetSquare.thisBoard.allSquares[targetSquare.mBoardPosition.x, targetSquare.mBoardPosition.y - 1].RemovePiece();
            }
            enPassant = false;
        }
        currentSquare.piece = null;

        // setting double move in case of moving 2 squares
        if (Math.Abs(targetSquare.rank - currentSquare.rank) == 2)
        {
            justDoubleMoved = true;
        }

        currentSquare = targetSquare;
        currentSquare.piece = this;
        transform.position = currentSquare.transform.position;
        targetSquare = null;
        movement = new Vector3Int(0, 1, 1);
        if (AtEndOfBoard())
        {
            pieceController.PawnPromoter(color, currentSquare);
        }
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

            // only allow the pawn to capture pieces in diagonal movement
            if (squareState == SquareState.Hostile)
            {
                if (x != 0)
                {
                    if (pieceController.CheckLegalMove(currentSquare, currentSquare.thisBoard.allSquares[currentX, currentY], this, color))
                    {
                        mHighlightedSquares.Add(currentSquare.thisBoard.allSquares[currentX, currentY]);
                    }
                }
                break;
            }

            // in the case that the square is free, only allow the pawn to move there if it is a regular forward move or en passant
            if (squareState == SquareState.Free)
            {
                if (x == 0)
                {
                    if (pieceController.CheckLegalMove(currentSquare, currentSquare.thisBoard.allSquares[currentX, currentY], this, color))
                    {
                        mHighlightedSquares.Add(currentSquare.thisBoard.allSquares[currentX, currentY]);
                    }
                }
                else
                {
                    if (color == Color.black)
                    {
                        if (currentSquare.thisBoard.allSquares[currentX, currentY + 1].piece != null)
                        {
                            if (typeof(Pawn) == currentSquare.thisBoard.allSquares[currentX, currentY + 1].piece.GetType())
                            {
                                if (((Pawn)currentSquare.thisBoard.allSquares[currentX, currentY + 1].piece).justDoubleMoved)
                                {
                                    if (pieceController.CheckLegalMove(currentSquare, currentSquare.thisBoard.allSquares[currentX, currentY], this, color))
                                    {
                                        mHighlightedSquares.Add(currentSquare.thisBoard.allSquares[currentX, currentY]);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (currentSquare.thisBoard.allSquares[currentX, currentY - 1].piece != null)
                        {
                            if (typeof(Pawn) == currentSquare.thisBoard.allSquares[currentX, currentY - 1].piece.GetType())
                            {
                                if (((Pawn)currentSquare.thisBoard.allSquares[currentX, currentY - 1].piece).justDoubleMoved)
                                {
                                    if (pieceController.CheckLegalMove(currentSquare, currentSquare.thisBoard.allSquares[currentX, currentY], this, color))
                                    {
                                        mHighlightedSquares.Add(currentSquare.thisBoard.allSquares[currentX, currentY]);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (squareState == SquareState.Friendly)
            {
                break;
            }
        }
    }

    private bool AtEndOfBoard()
    {
        if (color == Color.black)
        {
            if (currentSquare.rank == 1)
            {
                return true;
            }
        }
        else
        {
            if (currentSquare.rank == 8)
            {
                return true;
            }
        }
        return false;
    }

    public override void Kill()
    {
        base.Kill();
        justDoubleMoved = false;
        movement = new Vector3Int(0, 2, 1);
        enPassant = false;
    }
}
