using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComputerHandle
{
    private bool canJump;
    private bool selectAgain;
    private bool pieceJustPromoted;
    public bool isTurnOver;
    private GameObject selectedPiece;
    private GameObject canJumpPiece;
    private List<int> legalMoves;
    private IEnumerable<GameObject> whitePieces;
    private IEnumerable<GameObject> blackPieces;

    public void ReassignFieldValues()
    {
        canJump = false;
        selectAgain = false;
        pieceJustPromoted = false;
        isTurnOver = false;
        selectedPiece = null;
        canJumpPiece = null;
        legalMoves = new List<int>();
    }

    public void ComputerTurn(ref List<GameObject> pieceOrder, ref bool aPieceCanJump, bool whiteTurn, GameObject whitePromotionPiece, GameObject blackPrormotionPiece)
    {
        if (!selectedPiece)
            SelectPiece(ref pieceOrder, ref aPieceCanJump, whiteTurn);
        else 
            DropPiece(ref pieceOrder, ref aPieceCanJump, whiteTurn, whitePromotionPiece, blackPrormotionPiece);

    }

    private void SelectPiece(ref List<GameObject> pieceOrder, ref bool aPieceCanJump, bool whiteTurn)
    {
        if (!selectAgain)
        {
            whitePieces = pieceOrder.Where(n => n != null && n.GetComponent<White>());
            blackPieces = pieceOrder.Where(n => n != null && n.GetComponent<Black>());
        }

        if (whiteTurn)
            selectedPiece = whitePieces.ElementAt(Random.Range(0, whitePieces.Count()));
        else   
            selectedPiece = blackPieces.ElementAt(Random.Range(0, blackPieces.Count()));
        
        //Check if there is a piece that must move and select again if it's not chosen as the selected piece
        //If the selected piece is wrong, remove it from its color's array
        if (canJump && selectedPiece != canJumpPiece)
        {
            if (whiteTurn)
                whitePieces = whitePieces.Where(n => n != selectedPiece);
            else
                blackPieces = blackPieces.Where(n => n != selectedPiece);
            
            selectedPiece = null;
        }

        //Check if the selected piece has legal moves
        //If it has none, remove it from its color's list and select again
        if (selectedPiece)
        {
            legalMoves = CalculateLegalMoves.PieceLegalMoves(selectedPiece, pieceOrder, whiteTurn);
        }

        List<int> tempLegalMoves = new List<int>(legalMoves);
        
        if (canJump || aPieceCanJump)
        {
            foreach (int index in legalMoves)
            {
                int movedDistance = Mathf.Abs(index - pieceOrder.IndexOf(selectedPiece));
                
                if (movedDistance != 14 && movedDistance != 18)
                {
                    tempLegalMoves.Remove(index);
                }
            }
        }

        legalMoves = new List<int>(tempLegalMoves);

        if (!legalMoves.Any() && whiteTurn)
        {
            whitePieces = whitePieces.Where(n => n != selectedPiece);
            selectedPiece = null;
            selectAgain = true;
            //Debug.Log("selectAgain white");
            //Debug.Log($"{canJump}, {canJumpPiece}, {whiteTurn}");
        }
        else if (!legalMoves.Any() && !whiteTurn)
        {
            blackPieces = blackPieces.Where(n => n != selectedPiece);
            selectedPiece = null;
            selectAgain = true;
            //Debug.Log("selectAgain black");
            //Debug.Log($"{canJump}, {canJumpPiece}, {whiteTurn}");
        }
        else
            selectAgain = false;
    }

    private void DropPiece(ref List<GameObject> pieceOrder, ref bool aPieceCanJump, bool whiteTurn, GameObject whitePromotionPiece, GameObject blackPrormotionPiece)
    {
        List<GameObject> pieceTempList = new List<GameObject>(pieceOrder);
        int previousIndex = pieceOrder.IndexOf(selectedPiece);
        int dropIndex = previousIndex;
        pieceTempList[dropIndex] = null;

        dropIndex = legalMoves[Random.Range(0, legalMoves.Count)];

        pieceTempList[dropIndex] = selectedPiece;
        int movedDistance = dropIndex - previousIndex;

        //Debug.Log($"{dropIndex}: {selectedPiece.name}");

        if (Mathf.Abs(movedDistance) == 14 || Mathf.Abs(movedDistance) == 18)
        {
            Capture.CapturePiece(previousIndex, movedDistance, pieceTempList);
            pieceOrder = new List<GameObject>(pieceTempList);
            canJump = CalculateLegalMoves.CanJump(selectedPiece, pieceOrder, whiteTurn);
            Promotion.PromoteToKing(ref pieceOrder, whiteTurn, pieceJustPromoted, selectedPiece, whitePromotionPiece, blackPrormotionPiece);
            if (pieceJustPromoted)
            {
                canJump = false;
            }
            UpdateBoard(pieceOrder);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         
        }
        else
        {
            canJump = false;
            pieceOrder = new List<GameObject>(pieceTempList);
            Promotion.PromoteToKing(ref pieceOrder, whiteTurn, pieceJustPromoted, selectedPiece, whitePromotionPiece, blackPrormotionPiece);
            if (pieceJustPromoted)
            {
                canJump = false;
            }
            UpdateBoard(pieceOrder);
        }
        if (!canJump)
        {
            isTurnOver = true;
            canJumpPiece = null;
        }
        else
        {
            canJumpPiece = selectedPiece;
        }

        selectedPiece = null;
    }

    private void UpdateBoard(List<GameObject> pieceOrder)
    {
        foreach (GameObject piece in pieceOrder.Where(n => n != null))
            piece.transform.position = InstantiateBoard.tileOrder[pieceOrder.IndexOf(piece)].transform.position;
    }

}
