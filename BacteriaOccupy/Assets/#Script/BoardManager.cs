using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

    public int columns = 9;
    public int rows = 9;
    public GameObject[] wallTiles = null;
    public GameObject[] groundTiles = null;

    public Transform boardObject = null;
    public List<Vector3> gridPositions = new List<Vector3>();
    public GameObject[] arrayGridPosition = null;
    public GameObject vacant = null;

    public Transform playerObject = null;
    public GameObject[] player_1 = null;
    public GameObject[] player_2 = null;

    public GameObject prefabPlayer_1 = null;
    public GameObject prefabPlayer_2 = null;

    public Transform groundActiveObject = null;
    public GameObject[] groundActive = null;
    public GameObject prefabGroundActive = null;
    public GameObject groundActive1 = null;

    public Transform imageBackgroundObject = null;
    public GameObject imageBackground = null;
    public GameObject imagePlayer_1 = null;
    public GameObject imagePlayer_2 = null;

    public GameObject prefabImageBackground = null;

    void InitializeList() {
        gridPositions.Clear();

        for(int i = 0; i < columns; ++i) {
            for(int j = 0; j < rows; ++j) {
                gridPositions.Add(new Vector3(i, j, 0));
            }
        }
    }

    void InitializeBoard() {
        boardObject = new GameObject("BoardObject").transform;

        for (int i = 0; i < columns; ++i) {
            for (int j = 0; j < rows; ++j) {
                GameObject toInstantiate = groundTiles[Random.Range(0, groundTiles.Length)];
                if (i == 0 || i == columns - 1 || j == 0 || j == rows - 1)
                    toInstantiate = wallTiles[Random.Range(0, wallTiles.Length)];

                GameObject instance = Instantiate(toInstantiate, new Vector3(i - 4, j - 4, 0), Quaternion.identity);
                instance.transform.SetParent(boardObject);
            }
        }
    }

    void InitializePlayer() {
        imageBackgroundObject = new GameObject("ImageBackgroundObject").transform;
        playerObject = new GameObject("PlayerObject").transform;

        prefabImageBackground = Resources.Load<GameObject>("Prefab/BackGround");
        prefabPlayer_1 = Resources.Load<GameObject>("Prefab/Bacteria_1");
        prefabPlayer_2 = Resources.Load<GameObject>("Prefab/Bacteria_2");

        if (prefabPlayer_1 == null)
            return;
        if (prefabPlayer_2 == null)
            return;

        imageBackground = GameObject.Instantiate<GameObject>(prefabImageBackground);
        imageBackground.transform.SetParent(imageBackgroundObject);
        imagePlayer_1 = GameObject.Instantiate<GameObject>(prefabPlayer_1);
        imagePlayer_1.transform.SetParent(imageBackgroundObject);
        imagePlayer_1.transform.position = new Vector3(-6, 0, 0);
        imagePlayer_1.transform.localScale = new Vector3(4, 4, 0);
        imagePlayer_2 = GameObject.Instantiate<GameObject>(prefabPlayer_2);
        imagePlayer_2.transform.SetParent(imageBackgroundObject);
        imagePlayer_2.transform.position = new Vector3(6, 0, 0);
        imagePlayer_2.transform.localScale = new Vector3(4, 4, 0);

        player_1 = new GameObject[(columns - 2) * (rows - 2)];
        player_2 = new GameObject[(columns - 2) * (rows - 2)];

        int index = 0;

        for (int i = 1; i < columns - 1; ++i) {
            for (int j = 1; j < rows - 1; ++j) {
                player_1[index] = GameObject.Instantiate<GameObject>(prefabPlayer_1);
                player_1[index].transform.SetParent(playerObject);
                player_1[index].SetActive(false);
                player_1[index].transform.position = new Vector3(i - 4, j - 4, 0);

                if ((i == 1 && j == 1) || (i == 1 && j == 2) || (i == 2 && j == 1) || (i == 2 && j == 2))
                    player_1[index].SetActive(true);

                player_2[index] = GameObject.Instantiate<GameObject>(prefabPlayer_2);
                player_2[index].transform.SetParent(playerObject);
                player_2[index].SetActive(false);
                player_2[index].transform.position = new Vector3(i - 4, j - 4, 0);

                if ((i == columns - 2 && j == rows - 2) || (i == columns - 3 && j == rows - 2) || (i == columns - 2 && j == rows - 3) || (i == columns - 3 && j == rows - 3))
                    player_2[index].SetActive(true);

                index++;
            }
        }
    }

    void InitializeGroundActive()
    {
        prefabGroundActive = Resources.Load<GameObject>("Prefab/GroundActive");

        if (prefabGroundActive == null)
            return;

        groundActive1 = Instantiate(prefabGroundActive, new Vector3(0, 0, 0), Quaternion.identity);
        groundActive1.SetActive(false);
    }

    public void SetupScene() {
        InitializeBoard();
        InitializePlayer();
        InitializeList();
        InitializeGroundActive();
    }
}
