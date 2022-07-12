using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceController : MonoBehaviour
{
    public Image whitePromoter;
    public Image blackPromoter;

    public bool kingsAlive = true;

    public GameObject piecePrefab;

    private List<Piece> whitePieces = null;
    private List<Piece> blackPieces = null;

    private Type promotionType = null;

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
        whitePieces = CreatePieces(Color.white, board);
        blackPieces = CreatePieces(Color.black, board);

        PlacePieces(1, 0, whitePieces, board);
        PlacePieces(6, 7, blackPieces, board);

        SwitchSides(Color.black);
    }

    private List<Piece> CreatePieces(Color color, Board board)
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

    private Piece CreatePiece(Color color, Type pieceType)
    {
        Sprite[] pieceImages = Resources.LoadAll<Sprite>("SpriteSheets/kindpng_355936");
        int pieceNum;

        GameObject newPieceObject = Instantiate(piecePrefab);
        newPieceObject.transform.SetParent(transform);

        newPieceObject.transform.localScale = new Vector3(1, 1, 1);
        newPieceObject.transform.localRotation = Quaternion.identity;

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
        newPiece.Setup(color, this, pieceImages[pieceNum]);

        return newPiece;
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

        SetInteractive(whitePieces, true);
        SetInteractive(blackPieces, false);
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

    public bool CheckLegalMove(Square originalSquare, Square newSquare, Piece piece, Color color)
    {
        // set piece locations in potential move, and then see if the king is in check
        bool legality = true;
        Piece tempPiece = newSquare.piece;
        newSquare.piece = piece;
        originalSquare.piece = null;
        piece.currentSquare = newSquare;

        if (color == Color.black)
        {
            foreach (Piece blackPiece in blackPieces)
            {
                if (typeof(King) == blackPiece.GetType())
                {
                    legality = !((King)blackPiece).IsInCheck();
                }
            }
        }
        else
        {
            foreach (Piece whitePiece in whitePieces)
            {
                if (typeof(King) == whitePiece.GetType())
                {
                    legality = !((King)whitePiece).IsInCheck();
                }
            }
        }

        newSquare.piece = tempPiece;
        originalSquare.piece = piece;
        piece.currentSquare = originalSquare;
        return legality;
    }

    public bool SquareUnderAttack(Square square, Color color)
    {
        int x = square.mBoardPosition[0];
        int y = square.mBoardPosition[1];
        // check right
        for (int i = 1; i <= 7; i++)
        {
            if (x + i > 7)
            {
                break;
            }
            if (square.thisBoard.allSquares[x + i, y].piece != null)
            {
                if (square.thisBoard.allSquares[x + i, y].piece.GetType() == typeof(Rook) && square.thisBoard.allSquares[x + i, y].piece.color != color)
                {
                    return true;
                }

                if (square.thisBoard.allSquares[x + i, y].piece.GetType() == typeof(Queen) && square.thisBoard.allSquares[x + i, y].piece.color != color)
                {
                    return true;
                }

                if (square.thisBoard.allSquares[x + i, y].piece.GetType() == typeof(King) && square.thisBoard.allSquares[x + i, y].piece.color != color && i == 1)
                {
                    return true;
                }

                break;
            }
        }

        //check left
        for (int i = 1; i <= 7; i++)
        {
            if (x - i < 0)
            {
                break;
            }
            if (square.thisBoard.allSquares[x - i, y].piece != null)
            {
                if (square.thisBoard.allSquares[x - i, y].piece.GetType() == typeof(Rook) && square.thisBoard.allSquares[x - i, y].piece.color != color)
                {
                    return true;
                }

                if (square.thisBoard.allSquares[x - i, y].piece.GetType() == typeof(Queen) && square.thisBoard.allSquares[x - i, y].piece.color != color)
                {
                    return true;
                }

                if (square.thisBoard.allSquares[x - i, y].piece.GetType() == typeof(King) && square.thisBoard.allSquares[x - i, y].piece.color != color && i == 1)
                {
                    return true;
                }

                break;
            }
        }

        //check up
        for (int i = 1; i <= 7; i++)
        {
            if (y + i > 7)
            {
                break;
            }
            if (square.thisBoard.allSquares[x, y + i].piece != null)
            {
                if (square.thisBoard.allSquares[x, y + i].piece.GetType() == typeof(Rook) && square.thisBoard.allSquares[x, y + i].piece.color != color)
                {
                    return true;
                }

                if (square.thisBoard.allSquares[x, y + i].piece.GetType() == typeof(Queen) && square.thisBoard.allSquares[x, y + i].piece.color != color)
                {
                    return true;
                }

                if (square.thisBoard.allSquares[x, y + i].piece.GetType() == typeof(King) && square.thisBoard.allSquares[x, y + i].piece.color != color && i == 1)
                {
                    return true;
                }

                break;
            }
        }

        // check down
        for (int i = 1; i <= 7; i++)
        {
            if (y - i < 0)
            {
                break;
            }
            if (square.thisBoard.allSquares[x, y - i].piece != null)
            {
                if (square.thisBoard.allSquares[x, y - i].piece.GetType() == typeof(Rook) && square.thisBoard.allSquares[x, y - i].piece.color != color)
                {
                    return true;
                }

                if (square.thisBoard.allSquares[x, y - i].piece.GetType() == typeof(Queen) && square.thisBoard.allSquares[x, y - i].piece.color != color)
                {
                    return true;
                }

                if (square.thisBoard.allSquares[x, y - i].piece.GetType() == typeof(King) && square.thisBoard.allSquares[x, y - i].piece.color != color && i == 1)
                {
                    return true;
                }

                break;
            }
        }

        // check diagonals

        for (int i = 1; i <= 7; i++)
        {
            if (y + i > 7 || x + i > 7)
            {
                break;
            }
            if (square.thisBoard.allSquares[x + i, y + i].piece != null)
            {
                if (square.thisBoard.allSquares[x + i, y + i].piece.GetType() == typeof(Bishop) && square.thisBoard.allSquares[x + i, y + i].piece.color != color)
                {
                    return true;
                }

                if (square.thisBoard.allSquares[x + i, y + i].piece.GetType() == typeof(Queen) && square.thisBoard.allSquares[x + i, y + i].piece.color != color)
                {
                    return true;
                }

                if (square.thisBoard.allSquares[x + i, y + i].piece.GetType() == typeof(King) && square.thisBoard.allSquares[x + i, y + i].piece.color != color && i == 1)
                {
                    return true;
                }

                if (square.thisBoard.allSquares[x + i, y + i].piece.GetType() == typeof(Pawn) && square.thisBoard.allSquares[x + i, y + i].piece.color != color && i == 1 && color == Color.white)
                {
                    return true;
                }

                break;
            }
        }

        for (int i = 1; i <= 7; i++)
        {
            if (y - i < 0 || x + i > 7)
            {
                break;
            }
            if (square.thisBoard.allSquares[x + i, y - i].piece != null)
            {
                if (square.thisBoard.allSquares[x + i, y - i].piece.GetType() == typeof(Bishop) && square.thisBoard.allSquares[x + i, y - i].piece.color != color)
                {
                    return true;
                }

                if (square.thisBoard.allSquares[x + i, y - i].piece.GetType() == typeof(Queen) && square.thisBoard.allSquares[x + i, y - i].piece.color != color)
                {
                    return true;
                }

                if (square.thisBoard.allSquares[x + i, y - i].piece.GetType() == typeof(King) && square.thisBoard.allSquares[x + i, y - i].piece.color != color && i == 1)
                {
                    return true;
                }

                if (square.thisBoard.allSquares[x + i, y - i].piece.GetType() == typeof(Pawn) && square.thisBoard.allSquares[x + i, y - i].piece.color != color && i == 1 && color == Color.black)
                {
                    return true;
                }

                break;
            }
        }

        for (int i = 1; i <= 7; i++)
        {
            if (y + i > 7 || x - i < 0)
            {
                break;
            }
            if (square.thisBoard.allSquares[x - i, y + i].piece != null)
            {
                if (square.thisBoard.allSquares[x - i, y + i].piece.GetType() == typeof(Bishop) && square.thisBoard.allSquares[x - i, y + i].piece.color != color)
                {
                    return true;
                }

                if (square.thisBoard.allSquares[x - i, y + i].piece.GetType() == typeof(Queen) && square.thisBoard.allSquares[x - i, y + i].piece.color != color)
                {
                    return true;
                }

                if (square.thisBoard.allSquares[x - i, y + i].piece.GetType() == typeof(King) && square.thisBoard.allSquares[x - i, y + i].piece.color != color && i == 1)
                {
                    return true;
                }

                if (square.thisBoard.allSquares[x - i, y + i].piece.GetType() == typeof(Pawn) && square.thisBoard.allSquares[x - i, y + i].piece.color != color && i == 1 && color == Color.white)
                {
                    return true;
                }

                break;
            }
        }

        for (int i = 1; i <= 7; i++)
        {
            if (y - i < 0 || x - i < 0)
            {
                break;
            }
            if (square.thisBoard.allSquares[x - i, y - i].piece != null)
            {
                if (square.thisBoard.allSquares[x - i, y - i].piece.GetType() == typeof(Bishop) && square.thisBoard.allSquares[x - i, y - i].piece.color != color)
                {
                    return true;
                }

                if (square.thisBoard.allSquares[x - i, y - i].piece.GetType() == typeof(Queen) && square.thisBoard.allSquares[x - i, y - i].piece.color != color)
                {
                    return true;
                }

                if (square.thisBoard.allSquares[x - i, y - i].piece.GetType() == typeof(King) && square.thisBoard.allSquares[x - i, y - i].piece.color != color && i == 1)
                {
                    return true;
                }

                if (square.thisBoard.allSquares[x - i, y - i].piece.GetType() == typeof(Pawn) && square.thisBoard.allSquares[x - i, y - i].piece.color != color && i == 1 && color == Color.black)
                {
                    return true;
                }

                break;
            }
        }

        // check knights
        if (x + 2 <= 7 && y + 1 <= 7)
        {
            if (square.thisBoard.allSquares[x + 2, y + 1].piece != null)
            {
                if (square.thisBoard.allSquares[x + 2, y + 1].piece.GetType() == typeof(Knight) && square.thisBoard.allSquares[x + 2, y + 1].piece.color != color)
                {
                    return true;
                }
            }
        }

        if (x + 2 <= 7 && y - 1 >= 0)
        {
            if (square.thisBoard.allSquares[x + 2, y - 1].piece != null)
            {
                if (square.thisBoard.allSquares[x + 2, y - 1].piece.GetType() == typeof(Knight) && square.thisBoard.allSquares[x + 2, y - 1].piece.color != color)
                {
                    return true;
                }
            }
        }

        if (x - 2 >= 0 && y + 1 <= 7)
        {
            if (square.thisBoard.allSquares[x - 2, y + 1].piece != null)
            {
                if (square.thisBoard.allSquares[x - 2, y + 1].piece.GetType() == typeof(Knight) && square.thisBoard.allSquares[x - 2, y + 1].piece.color != color)
                {
                    return true;
                }
            }
        }

        if (x - 2 >= 0 && y - 1 >= 0)
        {
            if (square.thisBoard.allSquares[x - 2, y - 1].piece != null)
            {
                if (square.thisBoard.allSquares[x - 2, y - 1].piece.GetType() == typeof(Knight) && square.thisBoard.allSquares[x - 2, y - 1].piece.color != color)
                {
                    return true;
                }
            }
        }

        if (x + 1 <= 7 && y + 2 <= 7)
        {
            if (square.thisBoard.allSquares[x + 1, y + 2].piece != null)
            {
                if (square.thisBoard.allSquares[x + 1, y + 2].piece.GetType() == typeof(Knight) && square.thisBoard.allSquares[x + 1, y + 2].piece.color != color)
                {
                    return true;
                }
            }
        }

        if (x - 1 >= 0 && y + 2 <= 7)
        {
            if (square.thisBoard.allSquares[x - 1, y + 2].piece != null)
            {
                if (square.thisBoard.allSquares[x - 1, y + 2].piece.GetType() == typeof(Knight) && square.thisBoard.allSquares[x - 1, y + 2].piece.color != color)
                {
                    return true;
                }
            }
        }

        if (x + 1 <= 7 && y - 2 >= 0)
        {
            if (square.thisBoard.allSquares[x + 1, y - 2].piece != null)
            {
                if (square.thisBoard.allSquares[x + 1, y - 2].piece.GetType() == typeof(Knight) && square.thisBoard.allSquares[x + 1, y - 2].piece.color != color)
                {
                    return true;
                }
            }
        }

        if (x - 1 >= 0 && y - 2 >= 0)
        {
            if (square.thisBoard.allSquares[x - 1, y - 2].piece != null)
            {
                if (square.thisBoard.allSquares[x - 1, y - 2].piece.GetType() == typeof(Knight) && square.thisBoard.allSquares[x - 1, y - 2].piece.color != color)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void PawnPromoter(Color color, Square square)
    {
        if (color == Color.black)
        {
            blackPromoter.gameObject.SetActive(true);
        }
        else
        {
            whitePromoter.gameObject.SetActive(true);
        }

        // start a coroutine so that no one can move before a promotion is chosen
        StartCoroutine(WaitForPromotionSelect(color, square));
    }

    public void SetPromotionType(string str)
    {
        if (str == "queen")
        {
            promotionType = typeof(Queen);
        }
        else if (str == "rook")
        {
            promotionType = typeof(Rook);
        }
        else if (str == "knight")
        {
            promotionType = typeof(Knight);
        }
        else
        {
            promotionType = typeof(Bishop);
        }
    }

    IEnumerator WaitForPromotionSelect(Color color, Square square)
    {
        while (promotionType == null)
        {
            yield return null;
        }

        square.RemovePiece();

        Piece promotedPiece;
        if (promotionType == typeof(Queen))
        {
            promotedPiece = CreatePiece(color, typeof(Queen));
        }
        else if (promotionType == typeof(Rook))
        {
            promotedPiece = CreatePiece(color, typeof(Rook));
        }
        else if (promotionType == typeof(Knight))
        {
            promotedPiece = CreatePiece(color, typeof(Knight));
        }
        else
        {
            promotedPiece = CreatePiece(color, typeof(Bishop));
        }

        promotedPiece.Place(square);
        if (color == Color.black)
        {
            blackPieces.Add(promotedPiece);
        }
        else
        {
            whitePieces.Add(promotedPiece);
        }

        promotionType = null;
        blackPromoter.gameObject.SetActive(false);
        whitePromoter.gameObject.SetActive(false);

        SwitchSides(color);
    }
}
