using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerHandle
{
    private bool canJump;
    private bool pieceJustPromoted;
    public bool isTurnOver;
    private int turnCount;
    public GameObject selectedPiece;
    private GameObject canJumpPiece;
    private List<int> legalMoves;

    public void ReassignFieldValues()
    {
        canJump = false;
        pieceJustPromoted = false;
        isTurnOver = false;
        selectedPiece = null;
        canJumpPiece = null;
        legalMoves = new List<int>();
    }

    public void PlayerTurn(ref List<GameObject> pieceOrder, ref bool aPieceCanJump, bool whiteTurn, GameObject whitePromotionPiece, GameObject blackPromotionPiece)
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    
        if (Input.GetMouseButtonDown(0))
        {
            SelectPiece(mousePosition, ref pieceOrder, ref aPieceCanJump, whiteTurn);
            Debug.Log("Clicked");
        }
        if (selectedPiece)
            DragPiece(mousePosition);
        
        if (Input.GetMouseButtonUp(0) && selectedPiece)
            DropPiece(mousePosition, ref pieceOrder, ref aPieceCanJump, whiteTurn, whitePromotionPiece, blackPromotionPiece);
    }

    public void SelectPiece(Vector3 mousePosition, ref List<GameObject> pieceOrder, ref bool aPieceCanJump, bool whiteTurn)
    {
        
        GameObject targetPieceTile = Physics2D.OverlapPoint(mousePosition).transform.gameObject;
        GameObject tileOccupant = pieceOrder[InstantiateBoard.tileOrder.IndexOf(targetPieceTile)];
        
        if (tileOccupant)
        {
            if (canJump && tileOccupant != canJumpPiece)
            {
                return;
            }
            selectedPiece = tileOccupant;
            if (selectedPiece.GetComponent<White>() && whiteTurn)
            {
                selectedPiece.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
            }
            else if (selectedPiece.GetComponent<Black>() && !whiteTurn)
            {
                selectedPiece.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
            }
            else selectedPiece = null;
        }
        else selectedPiece = null;

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
    }

    public void DragPiece(Vector3 mousePosition)
    {
        selectedPiece.GetComponent<SpriteRenderer>().sortingOrder = 3;
        selectedPiece.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0f);
    }

    public void DropPiece(Vector3 mousePosition, ref List<GameObject> pieceOrder, ref bool aPieceCanJump, bool whiteTurn, GameObject whitePromotionPiece, GameObject blackPrormotionPiece)
    {
        List<GameObject> pieceTempList = new List<GameObject>(pieceOrder);
        int previousIndex = pieceOrder.IndexOf(selectedPiece);
        int dropIndex = previousIndex;
        pieceTempList[dropIndex] = null;

        foreach (GameObject tile in InstantiateBoard.tileOrder)
        {
            if (tile.transform.position.x - 0.584f <= mousePosition.x && tile.transform.position.x + 0.584f > mousePosition.x)
            {
                if (tile.transform.position.y + 0.585f > mousePosition.y && tile.transform.position.y - 0.585f < mousePosition.y)
                {
                    dropIndex = InstantiateBoard.tileOrder.IndexOf(tile);
                    break;
                }
            }
        }

        if (legalMoves.Contains(dropIndex))
        {
            pieceTempList[dropIndex] = selectedPiece;
            int movedDistance = dropIndex - previousIndex;

            if (Mathf.Abs(movedDistance) == 14 || Mathf.Abs(movedDistance) == 18)
            {
                Capture.CapturePiece(previousIndex, movedDistance, pieceTempList);
                pieceOrder = new List<GameObject>(pieceTempList);
                canJump = CalculateLegalMoves.CanJump(selectedPiece, pieceOrder, whiteTurn);
                Promotion.PromoteToKing(ref pieceOrder, whiteTurn, pieceJustPromoted, selectedPiece, whitePromotionPiece, blackPrormotionPiece);
                if (pieceJustPromoted)
                    canJump = false;
                UpdateBoard(pieceOrder);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         
            }
            else
            {
                canJump = false;
                pieceOrder = new List<GameObject>(pieceTempList);
                Promotion.PromoteToKing(ref pieceOrder, whiteTurn, pieceJustPromoted, selectedPiece, whitePromotionPiece, blackPrormotionPiece);
                UpdateBoard(pieceOrder);
            }
            if (!canJump)
            {
                canJumpPiece = null;
                isTurnOver = true;     
            }
            else
            {
                canJumpPiece = selectedPiece;
            }
        }
        else
        {
            canJump = false;
            pieceTempList[previousIndex] = selectedPiece;
            pieceOrder = new List<GameObject>(pieceTempList);
            UpdateBoard(pieceOrder);
        }
        selectedPiece = null;
    }

    private void UpdateBoard(List<GameObject> pieceOrder)
    {
        foreach (GameObject piece in pieceOrder.Where(n => n != null))
            piece.transform.position = InstantiateBoard.tileOrder[pieceOrder.IndexOf(piece)].transform.position;
    }
}
