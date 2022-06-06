using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceController : MonoBehaviour
{
    public GameObject piecePrefab;

    private List<Piece> whitePieces = null;
    private List<Piece> blackPieces = null;

    private string[] pieceOrder = new string[16]
    {
        "P", "P", "P", "P", "P", "P", "P", "P",
        "R", "N", "B", "Q", "K", "B", "N", "R"
    };

    private Dictionary<string, Type> pieceDict = new Dictionary<string, Type>()
    {
        {"P", typeof(Pawn) },
        {"N", typeof(Knight) },
        {"B", typeof(Bishop) },
        {"K", typeof(King) },
        {"Q", typeof(Queen) },
        {"R", typeof(Rook) },
    };

    public void Setup(Board board)
    {
        whitePieces = CreatePieces(Color.white, new Color32(255, 255, 255, 255), board);
        blackPieces = CreatePieces(Color.black, new Color32(0, 0, 0, 255), board);

        PlacePieces(1, 0, whitePieces, board);
        PlacePieces(6, 7, blackPieces, board);
    }

    private List<Piece> CreatePieces(Color color, Color32 spriteColor, Board board)
    {
        List<Piece> pieces = new List<Piece>();

        for (int i = 0; i < pieceOrder.Length; i++)
        {
            GameObject newPieceObject = Instantiate(piecePrefab);
            newPieceObject.transform.SetParent(transform);

            newPieceObject.transform.localScale = new Vector3(1, 1, 1);
            newPieceObject.transform.localRotation = Quaternion.identity;

            string key = pieceOrder[i];
            Type pieceType = pieceDict[key];

            Piece newPiece = (Piece)newPieceObject.AddComponent(pieceType);
            pieces.Add(newPiece);

            newPiece.Setup(color, spriteColor, this);
        }

        return pieces;
    }

    private void PlacePieces(int pawnRow, int royalRow, List<Piece> pieces, Board board)
    {
        for (int i = 0; i < 8; i++)
        {
            pieces[i].Place(board.allSquares[i, pawnRow]);
            pieces[i + 8].Place(board.allSquares[i, royalRow]);
        }
    }
}
