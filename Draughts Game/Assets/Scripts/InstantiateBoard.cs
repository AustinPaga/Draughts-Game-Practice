using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InstantiateBoard: MonoBehaviour
{
    [SerializeField] private GameObject tile;
    [SerializeField] private GameObject board;
    [SerializeField] private GameObject whitePiece;
    [SerializeField] private GameObject blackPiece;
    public static List<GameObject> tileOrder;
    public static List<GameObject> pieceOrder;

    private void OnEnable()
    { 
        tileOrder = new List<GameObject>();
        pieceOrder = new List<GameObject>();
        CreateBoard();
    }

    private void CreateBoard()
    {
        Instantiate(board, board.transform.position, board.transform.rotation);
        Vector3 tilePosition = tile.transform.position;
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                tileOrder.Add(Instantiate(tile, tilePosition + new Vector3(1.166515f * x, -1.169895f * y, 0f) , Quaternion.identity));
                if (x + (y * 8) < 24)
                {
                    if ((y % 2 == 0 && (x + y * 8) % 2 != 0) || (y % 2 != 0 && (x + y * 8) % 2 == 0))
                    {
                        pieceOrder.Add(Instantiate(whitePiece, tileOrder[x + (y * 8)].transform.position, Quaternion.identity));
                    }
                    else
                        pieceOrder.Add(null);
                }
                else if (x + (y * 8) >= 40)
                {
                    if ((y % 2 == 0 && (x + y * 8) % 2 != 0) || (y % 2 != 0 && (x + y * 8) % 2 == 0))
                    {
                        pieceOrder.Add(Instantiate(blackPiece, tileOrder[x + (y * 8)].transform.position, Quaternion.identity));
                    }
                    else
                        pieceOrder.Add(null);  
                }
                else
                {
                    pieceOrder.Add(null);
                }
            }
        }
    }
}
