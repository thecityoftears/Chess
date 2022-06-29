using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public GameObject squarePrefab;
    public Square[,] allSquares = new Square[8,8];

    public Dictionary<int, string> squaresDictionary = new Dictionary<int, string>()
    {
        {0, "a" },
        {1, "b" },
        {2, "c" },
        {3, "d" },
        {4, "e" },
        {5, "f" },
        {6, "g" },
        {7, "h" },
    };

    public enum SquareState
    {
        None, Friendly, Hostile, Free, OutOfBounds
    }

    public void Create()
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                GameObject newSquare = Instantiate(squarePrefab, this.transform);

                RectTransform rectTransform = newSquare.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(x*100+50, y*100+50);

                allSquares[x, y] = newSquare.GetComponent<Square>();
                allSquares[x, y].Setup(new Vector2Int(x, y), this);
            }
        }

        bool prevBlack = false;
        //set square colour
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                if (prevBlack)
                {
                    allSquares[x, y].GetComponent<Image>().color = new Color32(129, 84, 56, 255);
                    prevBlack = false;
                }
                else
                {
                    allSquares[x, y].GetComponent<Image>().color = new Color32(248, 223, 161, 255);
                    prevBlack = true;
                }
            }

            prevBlack = !prevBlack;
        }
    }

    public SquareState ValidateSquare(int x, int y, Piece checkingPiece)
    {
        if (x < 0 || x > 7 || y < 0 || y > 7)
        {
            return SquareState.OutOfBounds;
        }

        Square target = allSquares[x, y];

        if (target.piece != null)
        {
            if (checkingPiece.color == target.piece.color)
            {
                return SquareState.Friendly;
            }
            else
            {
                return SquareState.Hostile;
            }
        }

        return SquareState.Free;
    }
}
