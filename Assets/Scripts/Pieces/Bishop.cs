using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    public override void Setup(Color pieceColor, PieceController newPieceController, Sprite pieceImage)
    {
        base.Setup(pieceColor, newPieceController, pieceImage);

        movement = new Vector3Int(0, 0, 7);
    }

}
