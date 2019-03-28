using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour {

    public BoxCollider boxCollider = null;
    public LayerMask layerMask;

	// Use this for initialization
	void Awake () {
        boxCollider = GetComponent<BoxCollider>();
        Debug.Log("무빙오브젝트");
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log("무빙오브젝트222");
        RaycastHit hit;
        // A지점, B지점 레이저
        // hit = Null;
        // hit = 방해물;

        Vector3 start = transform.position;  // A지점, 캐릭터의 현재 위치 값
        Vector3 end = start;    // B지점, 캐릭터가 이동하고자 하는 위치 값

        //hit = (bool)Physics.Linecast(start, end, layerMask);
	}

    public void up()
    {
        int i;
    }
}
