using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVSComputerHandle: MonoBehaviour
{
    [SerializeField] private GameObject whitePromotionPiece;
    [SerializeField] private GameObject blackPromotionPiece;
    private ComputerHandle computerHandle;
    private PlayerHandle playerHandle;
    private List<GameObject> pieceOrder;
    private bool aPieceCanJump;
    private bool whiteTurn;

    public void OnEnable()
    {
        computerHandle = new ComputerHandle();
        playerHandle = new PlayerHandle();
        pieceOrder = new List<GameObject>(InstantiateBoard.pieceOrder);
        aPieceCanJump = false;
        whiteTurn = false;
    }

    private void Update()
    {
        
        if (whiteTurn)
        {
            computerHandle.ComputerTurn(ref pieceOrder, ref aPieceCanJump, whiteTurn, whitePromotionPiece, blackPromotionPiece);
        }
        else
        {
            playerHandle.PlayerTurn(ref pieceOrder, ref aPieceCanJump, whiteTurn, whitePromotionPiece, blackPromotionPiece);
        }

        if (playerHandle.isTurnOver || computerHandle.isTurnOver)
        {
            whiteTurn = !whiteTurn;
            aPieceCanJump = CalculateLegalMoves.APieceCanJump(pieceOrder, whiteTurn);
            playerHandle.ReassignFieldValues();
            computerHandle.ReassignFieldValues();
        }

        if (IsMatchOver())
        {
            GameState.GameOver();
        }
    }

    private bool IsMatchOver()
    {
        bool isMatchOver = true;

        if (CalculateLegalMoves.PlayerHasMoves(pieceOrder, whiteTurn))
        {
            isMatchOver = false;
        }

        return isMatchOver;
    }
}
