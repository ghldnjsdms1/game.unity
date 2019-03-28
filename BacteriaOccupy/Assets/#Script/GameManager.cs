using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public BoardManager boardScript;
    public UIManager uiScript;
    public MovingManager movingScript;

    public int flagPlayerTurn = 1;
    public int flagOrder = 1;

    bool clickEvent = false;
    string answer = "";
    RaycastHit hitInfo, hitInfo2;
    bool flag = false;

    public int playerTurn = 1;      // 1 : 플레이어1 / 2 : 플레이어2
    public int totalScore = 49;
    public int currentScore = 0;
    public int score_1, score_2;

    public bool gameover = false;

    void Awake () {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        boardScript = GetComponent<BoardManager>();
        uiScript = GetComponent<UIManager>();
        movingScript = GetComponent<MovingManager>();
        InitGame();
        InitCurrentScore();
    }


    void InitGame() {
        boardScript.SetupScene();
        uiScript.SetupUI();
        uiScript.UpdateScore();
    }

    void InitCurrentScore()
    {
        score_1 = 0;
        score_2 = 0;
        currentScore = 0;

        for (int i = 0; i < boardScript.player_1.Length; ++i)
        {
            if (boardScript.player_1[i].activeSelf == true)
                score_1++;
        }
        for (int i = 0; i < boardScript.player_2.Length; ++i)
        {
            if (boardScript.player_2[i].activeSelf == true)
                score_2++;
        }
        currentScore = score_1 + score_2;
    }

    IEnumerator Start()
    {
        yield return StartCoroutine("ClickEvent");
        yield return StartCoroutine(answer);
    }

    IEnumerator ClickEvent()
    {
        clickEvent = true;
        do
        {
            yield return null;
        } while (answer == "");

        clickEvent = false;
    }
    IEnumerator Player_1()
    {
        flag = true;
        do
        {
            yield return null;
            if (playerTurn == 1)
            {
                boardScript.groundActive1.SetActive(true);
                boardScript.groundActive1.transform.position = hitInfo.transform.position;
                PlayerUpdate();
            }
            else
                flag = false;
        } while (flag);
        TotalScore();
        uiScript.UpdateScore();
        if (!MovePlayerCheck()) {
            switch (playerTurn) {
                case 1:
                    playerTurn = 2;
                    break;
                case 2:
                    playerTurn = 1;
                    break;
            }
        }
        StartCoroutine("Start");
    }

    IEnumerator Player_2()
    {
        flag = true;
        do
        {
            yield return null;
            if (playerTurn == 2)
            {
                boardScript.groundActive1.SetActive(true);
                boardScript.groundActive1.transform.position = hitInfo.transform.position;
                PlayerUpdate();
            }
            else
                flag = false;
        } while (flag);
        TotalScore();
        uiScript.UpdateScore();
        if (!MovePlayerCheck()) {
            switch (playerTurn) {
                case 1:
                    playerTurn = 2;
                    break;
                case 2:
                    playerTurn = 1;
                    break;
            }
        }
        StartCoroutine("Start");
    }

    IEnumerator Ground()
    {
        yield return null;
        StartCoroutine("Start");
    }

    void PlayerUpdate()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            hitInfo2 = Raycast();

            // (-3, -3) ~ (3, 3) 범위 밖을 선택했을 경우
            if (!bRangeClick(hitInfo2))
            {

            }
            // 같은 자리를 선택했을 경우(선택 초기화)
            else if (hitInfo.transform == hitInfo2.transform)
            {
                Debug.Log("같은자리");
                flag = false;
                clickEvent = true;
                answer = "";
                boardScript.groundActive1.SetActive(false);
            }
            // 상하좌우대각 1칸 선택했을 경우(플레이어 복사)
            else if (((hitInfo2.transform.position.x == hitInfo.transform.position.x - 1) || (hitInfo.transform.position.x + 1 == hitInfo2.transform.position.x) || (hitInfo.transform.position.x == hitInfo2.transform.position.x))
                && ((hitInfo2.transform.position.y == hitInfo.transform.position.y - 1) || (hitInfo.transform.position.y + 1 == hitInfo2.transform.position.y) || (hitInfo.transform.position.y == hitInfo2.transform.position.y)))
            {
                Debug.Log("한칸");
                if (hitInfo2.transform.gameObject.tag == "Ground")
                {
                    currentScore++;
                    if (playerTurn == 1)
                    {
                        for (int i = 0; i < boardScript.player_1.Length; ++i)
                        {
                            if (boardScript.player_1[i].transform.position == hitInfo2.transform.position)
                            {
                                boardScript.player_1[i].SetActive(true);
                                flag = false;
                                clickEvent = true;
                                answer = "";
                                boardScript.groundActive1.SetActive(false);
                                PlayerConvert(playerTurn, hitInfo2.transform.position);
                                playerTurn = 2;
                            }
                        }
                    }
                    else if(playerTurn == 2)
                    {
                        for (int i = 0; i < boardScript.player_2.Length; ++i)
                        {
                            if (boardScript.player_2[i].transform.position == hitInfo2.transform.position)
                            {
                                boardScript.player_2[i].SetActive(true);
                                flag = false;
                                clickEvent = true;
                                answer = "";
                                boardScript.groundActive1.SetActive(false);
                                PlayerConvert(playerTurn, hitInfo2.transform.position);
                                playerTurn = 1;
                            }
                        }
                    }
                }
                else if ((hitInfo2.transform.gameObject.tag == "Player_1") || (hitInfo2.transform.gameObject.tag == "Player_2"))
                {
                }
                else
                {

                }
            }
            // 상하좌우 2칸 선택했을 경우(플레이어 이동)
            else if ((((hitInfo.transform.position.x - 2 == hitInfo2.transform.position.x) || (hitInfo.transform.position.x + 2 == hitInfo2.transform.position.x)) && ((hitInfo.transform.position.y == hitInfo2.transform.position.y)))
                || ((hitInfo.transform.position.y - 2 == hitInfo2.transform.position.y) || (hitInfo.transform.position.y + 2 == hitInfo2.transform.position.y)) && ((hitInfo.transform.position.x == hitInfo2.transform.position.x)))
            {
                Debug.Log("두칸");
                if (hitInfo2.transform.gameObject.tag == "Ground") {
                    flag = false;
                    clickEvent = true;
                    answer = "";
                    boardScript.groundActive1.SetActive(false);
                    if (playerTurn == 1) {
                        for (int i = 0; i < boardScript.player_1.Length; ++i) {
                            if (boardScript.player_1[i].transform.position == hitInfo2.transform.position) {
                                boardScript.player_1[i].SetActive(true);
                            }
                            else if (boardScript.player_1[i].transform.position == hitInfo.transform.position) {
                                boardScript.player_1[i].SetActive(false);
                            }
                        }
                        PlayerConvert(playerTurn, hitInfo2.transform.position);
                        playerTurn = 2;
                    }
                    else if (playerTurn == 2) {
                        for (int i = 0; i < boardScript.player_2.Length; ++i) {
                            if (boardScript.player_2[i].transform.position == hitInfo2.transform.position) {
                                boardScript.player_2[i].SetActive(true);
                            }
                            else if (boardScript.player_2[i].transform.position == hitInfo.transform.position) {
                                boardScript.player_2[i].SetActive(false);
                            }
                        }
                        PlayerConvert(playerTurn, hitInfo2.transform.position);
                        playerTurn = 1;
                    }
                }
            }
            else
            {

            }
        }
    }

    void PlayerConvert(int player, Vector3 position)
    {
        Vector3 tmpPosition = position;

        switch (player)
        {
            case 1:
                for (int i = 0; i < boardScript.player_2.Length; ++i)
                {
                    if((boardScript.player_2[i].transform.position.x == position.x - 1) || (boardScript.player_2[i].transform.position.x == position.x) || (boardScript.player_2[i].transform.position.x == position.x + 1))
                    {
                        if((boardScript.player_2[i].transform.position.y == position.y - 1) || (boardScript.player_2[i].transform.position.y == position.y) || (boardScript.player_2[i].transform.position.y == position.y + 1))
                        {
                            if (boardScript.player_2[i].activeSelf == true)
                            {
                                boardScript.player_2[i].SetActive(false);
                                boardScript.player_1[i].SetActive(true);
                            }
                        }
                    }
                }
                    break;
            case 2:
                for (int i = 0; i < boardScript.player_1.Length; ++i)
                {
                    if ((boardScript.player_1[i].transform.position.x == position.x - 1) || (boardScript.player_1[i].transform.position.x == position.x) || (boardScript.player_1[i].transform.position.x == position.x + 1))
                    {
                        if ((boardScript.player_1[i].transform.position.y == position.y - 1) || (boardScript.player_1[i].transform.position.y == position.y) || (boardScript.player_1[i].transform.position.y == position.y + 1))
                        {
                            if (boardScript.player_1[i].activeSelf == true)
                            {
                                boardScript.player_1[i].SetActive(false);
                                boardScript.player_2[i].SetActive(true);
                            }
                        }
                    }
                }
                break;
        }
    }

    void TotalScore()
    {
        score_1 = 0;
        score_2 = 0;

        for (int i = 0; i < boardScript.player_1.Length; ++i)
        {
            if (boardScript.player_1[i].activeSelf == true)
                score_1++;
        }
        for (int i = 0; i < boardScript.player_2.Length; ++i)
        {
            if (boardScript.player_2[i].activeSelf == true)
                score_2++;
        }

        if (currentScore == totalScore || score_1 == 0 || score_2 == 0) {
            //SceneManager.LoadScene("PlayScene");
            gameover = true;
        }
    }

    void Update () {
        if (gameover) {
            uiScript.GameOverUI();
            Time.timeScale = 0;
        }

        if (clickEvent)
        {
            if (Input.GetMouseButtonUp(0))
            {
                hitInfo = Raycast();
                if (!bRangeClick(hitInfo))
                {
                    StartCoroutine("Start");
                }
            }
        }

        NextTurn();

        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene("PlayScene");
    }

    IEnumerator CorMouseEvent()
    {
        yield return new WaitForSeconds(1f);
        movingScript.MouseEvent();
    }

    bool MovePlayerCheck() {
        switch (playerTurn) {
            case 2:
                for(int i = 0; i < boardScript.player_1.Length; ++i) {
                    if(boardScript.player_1[i].activeSelf == true) {
                        for(int j = 0; j < boardScript.player_2.Length; ++j) {
                            if ((boardScript.player_1[i].transform.position.x - 1   == boardScript.player_2[j].transform.position.x) || 
                                (boardScript.player_1[i].transform.position.x       == boardScript.player_2[j].transform.position.x) || 
                                (boardScript.player_1[i].transform.position.x + 1   == boardScript.player_2[j].transform.position.x)) {
                                if ((boardScript.player_1[i].transform.position.y - 1   == boardScript.player_2[j].transform.position.y) || 
                                    (boardScript.player_1[i].transform.position.y       == boardScript.player_2[j].transform.position.y) || 
                                    (boardScript.player_1[i].transform.position.y + 1   == boardScript.player_2[j].transform.position.y)) {
                                    if ((boardScript.player_1[i].transform.position.x != boardScript.player_2[j].transform.position.x) && 
                                        (boardScript.player_1[i].transform.position.y != boardScript.player_2[j].transform.position.y)) {
                                        if (boardScript.player_2[j].activeSelf == false) {
                                            Debug.Log("111111111111");
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                break;
            case 1:
                for (int i = 0; i < boardScript.player_2.Length; ++i) {
                    if (boardScript.player_2[i].activeSelf == true) {
                        for (int j = 0; j < boardScript.player_1.Length; ++j) {
                            if ((boardScript.player_2[i].transform.position.x == boardScript.player_1[j].transform.position.x) &&
                                (boardScript.player_2[i].transform.position.y == boardScript.player_1[j].transform.position.y)) {

                            }
                            else if ((boardScript.player_2[i].transform.position.x - 1   == boardScript.player_1[j].transform.position.x) ||
                                     (boardScript.player_2[i].transform.position.x       == boardScript.player_1[j].transform.position.x) ||
                                     (boardScript.player_2[i].transform.position.x + 1   == boardScript.player_1[j].transform.position.x)) {
                                if ((boardScript.player_2[i].transform.position.y - 1   == boardScript.player_1[j].transform.position.y) ||
                                    (boardScript.player_2[i].transform.position.y       == boardScript.player_1[j].transform.position.y) ||
                                    (boardScript.player_2[i].transform.position.y + 1   == boardScript.player_1[j].transform.position.y)) {
                                    //if ((boardScript.player_2[i].transform.position.x != boardScript.player_1[j].transform.position.x) && 
                                    //    (boardScript.player_2[i].transform.position.y != boardScript.player_1[j].transform.position.y)) {
                                        if (boardScript.player_1[j].activeSelf == false) {
                                        Debug.Log("2222222222222");
                                        return true;
                                        }
                                    //}
                                }
                            }
                        }
                    }
                }
                break;
        }
        Debug.Log("333333333333");
        return false;
    }

    void NextTurn() {
        if (Input.GetKeyDown(KeyCode.T)) {
            switch (playerTurn) {
                case 1:
                    playerTurn = 2;
                    break;
                case 2:
                    playerTurn = 1;
                    break;
            }
            flag = false;
            clickEvent = true;
            answer = "";
            boardScript.groundActive1.SetActive(false);
            uiScript.UpdateScore();
            StartCoroutine("Start");
        }
    }
    
    RaycastHit Raycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            answer = hit.transform.gameObject.tag;
            Debug.Log(answer);
        }
        return hit;
    }

    bool bRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            answer = hit.transform.gameObject.tag;
            return true;
        }
        return false;
    }

    bool bRangeClick(RaycastHit hit)
    {
        if ((hit.transform.position.x < -3 || hit.transform.position.x > 3)
                    || (hit.transform.position.y < -3 || hit.transform.position.y > 3))
        {
            if (!flag)
            {
                clickEvent = true;
                answer = "";
                StartCoroutine("Start");
            }
            return false;
        }
        return true;
    }
}
