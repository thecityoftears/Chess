using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece
{
    public bool hasMoved = false;
    public override void Setup(Color pieceColor, PieceController newPieceController, Sprite pieceImage)
    {
        base.Setup(pieceColor, newPieceController, pieceImage);

        movement = new Vector3Int(7, 7, 0);
    }

    protected override void Move()
    {
        base.Move();
        hasMoved = true;
    }
}
