using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capture : MonoBehaviour
{
    public static void CapturePiece(int previousIndex, int movedDistance, List<GameObject> pieceTempList)
    {
        int capturedPieceIndex = previousIndex + (movedDistance / 2);
        GameObject capturedPiece = pieceTempList[capturedPieceIndex];
        pieceTempList[capturedPieceIndex] = null;
        Destroy(capturedPiece);
    }
}
