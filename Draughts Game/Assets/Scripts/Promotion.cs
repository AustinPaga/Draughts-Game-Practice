using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Promotion : MonoBehaviour
{
    public static void PromoteToKing(ref List<GameObject> pieceOrder, bool whiteTurn, bool pieceJustPromoted, GameObject selectedPiece, GameObject whitePromotionPiece, GameObject blackPromotionPiece)
    {
        int landedIndex = pieceOrder.IndexOf(selectedPiece);
        pieceJustPromoted = false;
        if (selectedPiece.GetComponent<Checker>())
        {
            if (whiteTurn && landedIndex > 55)
            {
                pieceOrder[landedIndex] = Instantiate(whitePromotionPiece, InstantiateBoard.tileOrder[landedIndex].transform.position, Quaternion.identity);
                Destroy(selectedPiece);
                pieceJustPromoted = true;
            }
            else if (!whiteTurn && landedIndex < 8)
            {
                pieceOrder[landedIndex] = Instantiate(blackPromotionPiece, InstantiateBoard.tileOrder[landedIndex].transform.position, Quaternion.identity);
                Destroy(selectedPiece);
                pieceJustPromoted = true;
            }
        }
    }
    
}
