using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public static class CalculateLegalMoves
{
    public static List<int> PieceLegalMoves(GameObject selectedPiece, List<GameObject> pieceOrder, bool whiteTurn)
    {
        if (selectedPiece.GetComponent<Checker>())
            return CheckerLegalMoves(selectedPiece, pieceOrder, whiteTurn);
        else 
            return KingLegalMoves(selectedPiece, pieceOrder, whiteTurn);
    }

    private static List<int> CheckerLegalMoves(GameObject selectedPiece, List<GameObject> pieceOrder, bool whiteTurn)
    {
        List<int> legalMoves;
        int pieceIndex = pieceOrder.IndexOf(selectedPiece);
        
        if (selectedPiece.GetComponent<White>())
            legalMoves = new List<int>(AscendingMoves(selectedPiece, pieceOrder, whiteTurn));
        else
            legalMoves = new List<int>(DescendingMoves(selectedPiece, pieceOrder, whiteTurn));
        
        return legalMoves;
    }

    private static List<int> KingLegalMoves(GameObject selectedPiece, List<GameObject> pieceOrder, bool whiteTurn)
    {
        List<int> legalMoves;
        int pieceIndex = pieceOrder.IndexOf(selectedPiece);

        if (whiteTurn)
            legalMoves = new List<int>(DescendingMoves(selectedPiece, pieceOrder, whiteTurn));
        else
            legalMoves = new List<int>(AscendingMoves(selectedPiece, pieceOrder, whiteTurn));
        foreach (int move in CheckerLegalMoves(selectedPiece, pieceOrder, whiteTurn))
            legalMoves.Add(move);
        
        return legalMoves;
    }

    private static List<int> AscendingMoves(GameObject selectedPiece, List<GameObject> pieceOrder, bool whiteTurn)
    {
        List<int> legalMoves = new List<int>();
        int pieceIndex = pieceOrder.IndexOf(selectedPiece);

        if (whiteTurn)
        {
            if (pieceIndex < 56 && pieceIndex % 8 != 0 && !pieceOrder[pieceIndex + 7])
                legalMoves.Add(pieceIndex + 7);
            if (pieceIndex < 49 && pieceIndex % 8 != 0 && (pieceIndex - 1) % 8 != 0 && pieceOrder[pieceIndex + 7] && !pieceOrder[pieceIndex + 14] && pieceOrder[pieceIndex + 7].GetComponent<Black>())
                legalMoves.Add(pieceIndex + 14);
            if (pieceIndex < 54 && (pieceIndex + 1) % 8 != 0 && !pieceOrder[pieceIndex + 9])
                legalMoves.Add(pieceIndex + 9);
            if (pieceIndex < 45 && (pieceIndex + 1) % 8 != 0 && (pieceIndex + 2) % 8 != 0 && pieceOrder[pieceIndex + 9] && !pieceOrder[pieceIndex + 18] && pieceOrder[pieceIndex + 9].GetComponent<Black>())
                legalMoves.Add(pieceIndex + 18);
        }
        else
        {
            if (pieceIndex < 56 && pieceIndex % 8 != 0 && !pieceOrder[pieceIndex + 7])
                legalMoves.Add(pieceIndex + 7);
            if (pieceIndex < 49 && pieceIndex % 8 != 0 && (pieceIndex - 1) % 8 != 0 && pieceOrder[pieceIndex + 7] && !pieceOrder[pieceIndex + 14] && pieceOrder[pieceIndex + 7].GetComponent<White>())
                legalMoves.Add(pieceIndex + 14);
            if (pieceIndex < 54 && (pieceIndex + 1) % 8 != 0 && !pieceOrder[pieceIndex + 9])
                legalMoves.Add(pieceIndex + 9);
            if (pieceIndex < 45 && (pieceIndex + 1) % 8 != 0 && (pieceIndex + 2) % 8 != 0 && pieceOrder[pieceIndex + 9] && !pieceOrder[pieceIndex + 18] && pieceOrder[pieceIndex + 9].GetComponent<White>())
                legalMoves.Add(pieceIndex + 18);
        }

        return legalMoves;
    }

    private static List<int> DescendingMoves(GameObject selectedPiece, List<GameObject> pieceOrder, bool whiteTurn)
    {
        List<int> legalMoves = new List<int>();
        int pieceIndex = pieceOrder.IndexOf(selectedPiece);

        if (whiteTurn)
        {
            if (pieceIndex > 7 && (pieceIndex + 1) % 8 != 0 && !pieceOrder[pieceIndex - 7])
                legalMoves.Add(pieceIndex - 7);
            if (pieceIndex > 14 && (pieceIndex + 1) % 8 != 0 && (pieceIndex + 2) % 8 != 0 && pieceOrder[pieceIndex - 7] && !pieceOrder[pieceIndex - 14] && pieceOrder[pieceIndex - 7].GetComponent<Black>())
                legalMoves.Add(pieceIndex - 14);
            if (pieceIndex > 9 && pieceIndex % 8 != 0 && !pieceOrder[pieceIndex - 9])
                legalMoves.Add(pieceIndex - 9);
            if (pieceIndex > 18 && pieceIndex % 8 != 0 && (pieceIndex - 1) % 8 != 0 && pieceOrder[pieceIndex - 9] && !pieceOrder[pieceIndex - 18] && pieceOrder[pieceIndex - 9].GetComponent<Black>())
                legalMoves.Add(pieceIndex - 18);
        }
        else
        {
            if (pieceIndex > 7 && (pieceIndex + 1) % 8 != 0 && !pieceOrder[pieceIndex - 7])
                legalMoves.Add(pieceIndex - 7);
            if (pieceIndex > 14 && (pieceIndex + 1) % 8 != 0 && (pieceIndex + 2) % 8 != 0 && pieceOrder[pieceIndex - 7] && !pieceOrder[pieceIndex - 14] && pieceOrder[pieceIndex - 7].GetComponent<White>())
                legalMoves.Add(pieceIndex - 14);
            if (pieceIndex > 9 && pieceIndex % 8 != 0 && !pieceOrder[pieceIndex - 9])
                legalMoves.Add(pieceIndex - 9);
            if (pieceIndex > 18 && pieceIndex % 8 != 0 && (pieceIndex - 1) % 8 != 0 && pieceOrder[pieceIndex - 9] && !pieceOrder[pieceIndex - 18] && pieceOrder[pieceIndex - 9].GetComponent<White>())
                legalMoves.Add(pieceIndex - 18);
        }

        return legalMoves;
    }

    public static bool APieceCanJump(List<GameObject> pieceOrder, bool whiteTurn)
    {
        bool aPieceCanJump = false;

        if (whiteTurn)
        {
            foreach (GameObject piece in pieceOrder.Where(n => n != null && n.GetComponent<White>()))
            {
                List<int> legalMoves = new List<int>(PieceLegalMoves(piece, pieceOrder, whiteTurn));
                int pieceIndex = pieceOrder.IndexOf(piece);
                
                foreach (int move in legalMoves)
                {
                    if (Mathf.Abs(move - pieceIndex) == 14 || Mathf.Abs(move - pieceIndex) == 18)
                    {
                        aPieceCanJump = true;
                        return aPieceCanJump;
                    }
                }
            }
        }
        else
        {
            foreach (GameObject piece in pieceOrder.Where(n => n != null && n.GetComponent<Black>()))
            {
                List<int> legalMoves = new List<int>(PieceLegalMoves(piece, pieceOrder, whiteTurn));
                int pieceIndex = pieceOrder.IndexOf(piece);
                
                foreach (int move in legalMoves)
                {
                    if (Mathf.Abs(move - pieceIndex) == 14 || Mathf.Abs(move - pieceIndex) == 18)
                    {
                        aPieceCanJump = true;
                        return aPieceCanJump;
                    }
                }
            }
        }

        return aPieceCanJump;
    }

    public static bool CanJump(GameObject selectedPiece, List<GameObject> pieceOrder, bool whiteTurn)
    {
        bool canJump = false;
        List<int> legalMoves = new List<int>(PieceLegalMoves(selectedPiece, pieceOrder, whiteTurn));
        int pieceIndex = pieceOrder.IndexOf(selectedPiece);

        foreach (int move in legalMoves)
        {
            if (Mathf.Abs(move - pieceIndex) == 14 || Mathf.Abs(move - pieceIndex) == 18)
            {
                //Debug.Log($"{selectedPiece}, {move}");
                canJump = true;
                break;
            }
        }

        return canJump;
    }

    public static bool PlayerHasMoves(List<GameObject> pieceOrder, bool whiteTurn)
    {
        bool playerHasMoves = false;

        if (whiteTurn)
        {
            foreach (GameObject piece in pieceOrder.Where(n => n != null && n.GetComponent<White>()))
            {
                if (PieceLegalMoves(piece, pieceOrder, whiteTurn).Any())
                {
                    playerHasMoves = true;
                    break;
                }
            }
        }
        else
        {
            foreach(GameObject piece in pieceOrder.Where(n => n != null && n.GetComponent<Black>()))
            {
                if (PieceLegalMoves(piece, pieceOrder, whiteTurn).Any())
                {
                    playerHasMoves = true;
                    break;
                }
            }
        }

        return playerHasMoves;
    }
}
