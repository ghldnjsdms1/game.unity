using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingManager : MonoBehaviour {
    public int clickLayer = 8;
    public int blockLayer = 9;
    public bool flag = false;

    public void MouseEvent () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 100f))
        {
            if (hitInfo.transform.gameObject.tag == "Player_1") {
                Debug.Log(" hit object : " + hitInfo.collider.name);
            }
            else if (hitInfo.transform.gameObject.tag == "Player_2") {
                Debug.Log(" hit object : " + hitInfo.collider.name);
            }
            flag = true;
            StartCoroutine(CorMouseEvent(hitInfo));
        }
        else {

        }
    }

    IEnumerator CorMouseEvent(RaycastHit hitInfo)
    {
        if (!flag)
            yield return null;

        if (Input.GetMouseButtonUp(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo2;

            if (Physics.Raycast(ray, out hitInfo2, 100f)) {
                if (hitInfo.transform.position == hitInfo2.transform.position) {
                    yield return null;
                }
                else if (hitInfo.transform.gameObject.tag == "Player_2") {

                }
                StartCoroutine(CorMouseEvent(hitInfo));
            }
            else {
                
            }
        }
    }
}
