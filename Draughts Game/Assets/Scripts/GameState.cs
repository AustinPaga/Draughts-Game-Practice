using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    [SerializeField] GameObject serializedBoard;
    private static GameObject board;

    void Start()
    {
        board = serializedBoard;
        board.GetComponent<InstantiateBoard>().enabled = true;
        //board.GetComponent<PlayerHandle>().enabled = true;
        board.GetComponent<PlayerVSComputerHandle>().enabled = true;
    }

    public static void GameOver()
    {
        Debug.Log("Game Over");
        Debug.Break();
        board.GetComponent<PlayerVSComputerHandle>().enabled = false;
        board.GetComponent<InstantiateBoard>().enabled = false;
        foreach (GameObject board in GameObject.FindGameObjectsWithTag("Board"))
            Destroy(board);
        foreach (GameObject piece in GameObject.FindGameObjectsWithTag("Piece"))
            Destroy(piece);
        foreach (GameObject tile in GameObject.FindGameObjectsWithTag("Tile"))
            Destroy(tile);
        board.GetComponent<InstantiateBoard>().enabled = true;
        board.GetComponent<PlayerVSComputerHandle>().enabled = true;
    }
}
