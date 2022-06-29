using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece
{
    public override void Setup(Color pieceColor, PieceController newPieceController, Sprite pieceImage)
    {
        base.Setup(pieceColor, newPieceController, pieceImage);

        movement = new Vector3Int(7, 7, 7);
    }
}
