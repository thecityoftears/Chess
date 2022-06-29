using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceController : MonoBehaviour
{
    public bool kingsAlive = true;

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

        SwitchSides(Color.black);
    }

    private List<Piece> CreatePieces(Color color, Color32 spriteColor, Board board)
    {
        List<Piece> pieces = new List<Piece>();
        Sprite[] pieceImages = Resources.LoadAll<Sprite>("SpriteSheets/kindpng_355936");
        int pieceNum;
        //Texture2D texture;

        for (int i = 0; i < pieceOrder.Length; i++)
        {
            GameObject newPieceObject = Instantiate(piecePrefab);
            newPieceObject.transform.SetParent(transform);

            newPieceObject.transform.localScale = new Vector3(1, 1, 1);
            newPieceObject.transform.localRotation = Quaternion.identity;
            //texture = Resources.Load<Texture2D>("SpriteSheets/Kindpng_355936_5");
            //pieceImage = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

            string key = pieceOrder[i];
            Type pieceType = pieceDict[key];

            //choosing the index for the sprite sheet
            if (pieceType == typeof(Pawn))
            {
                pieceNum = 5;
            }
            else if (pieceType == typeof(Knight))
            {
                pieceNum = 3;
            }
            else if (pieceType == typeof(Bishop))
            {
                pieceNum = 2;
            }
            else if (pieceType == typeof(Rook))
            {
                pieceNum = 4;
            }
            else if (pieceType == typeof(Queen))
            {
                pieceNum = 1;
            }
            else
            {
                pieceNum = 0;
            }

            if (color == Color.black)
            {
                pieceNum += 6;
            }

            Piece newPiece = (Piece)newPieceObject.AddComponent(pieceType);
            pieces.Add(newPiece);

            newPiece.Setup(color, this, pieceImages[pieceNum]);
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

    private void SetInteractive(List<Piece> allPieces, bool value)
    {
        foreach(Piece piece in allPieces)
        {
            piece.enabled = value;
        }
    }

    public void SwitchSides(Color color)
    {
        if (!kingsAlive)
        {
            ResetPieces();

            kingsAlive = true;

            color = Color.black;
        }

        bool isBlackTurn = color == Color.white ? true : false;

        SetInteractive(whitePieces, !isBlackTurn);
        SetInteractive(blackPieces, isBlackTurn);
    }

    public void ResetPieces()
    {
        foreach (Piece piece in blackPieces)
        {
            piece.Reset();
        }
        foreach (Piece piece in whitePieces)
        {
            piece.Reset();
        }
    }

    public void UpdateMovedPawns(Color color)
    {
        if (color == Color.black)
        {
            foreach (Piece piece in blackPieces)
            {
                if (typeof(Pawn) == piece.GetType()){
                    ((Pawn)piece).justDoubleMoved = false;
                }
            }
        }
        else
        {
            foreach (Piece piece in whitePieces)
            {
                if (typeof(Pawn) == piece.GetType())
                {
                    ((Pawn)piece).justDoubleMoved = false;
                }
            }
        }
    }
}
