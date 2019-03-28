using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public GameManager gameScript = null;
    public Text scoreText = null;
    private int score_1 = 0, score_2 = 0;

    public GameObject prefabArrow = null;
    public GameObject Arrow = null;

    public Text winText = null;

	public void SetupUI () {
        gameScript = GetComponent<GameManager>();

        prefabArrow = Resources.Load<GameObject>("Prefab/Arrow");

        if (prefabArrow == null)
            return;

        Arrow = GameObject.Instantiate<GameObject>(prefabArrow);
        Arrow.transform.position = new Vector3(-6f, 2.5f, 0);
        gameScript.score_1 = 4;
        gameScript.score_2 = 4;
    }

	public void UpdateScore () {
        score_1 = gameScript.score_1;
        score_2 = gameScript.score_2;
        scoreText.text = score_1.ToString() + " : " + score_2.ToString();

        switch (gameScript.playerTurn) {
            case 1:
                Arrow.transform.position = new Vector3(-6f, 2.5f, 0);
                break;
            case 2:
                Arrow.transform.position = new Vector3(6f, 2.5f, 0);
                break;
        }
    }

    public void GameOverUI() {
        if (score_1 > score_2) {
            winText.text = "1P WIN!!";
        }
        else if (score_1 < score_2) {
            winText.text = "2P WIN!!";
        }
        else {
            winText.text = "DRAW!!";
        }
        GameObject.Find("Canvas").transform.Find("WinText").gameObject.SetActive(true);
    }
}
