using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerUI : MonoBehaviour {

	public GameObject apple1;
	public GameObject apple2;
	public GameObject apple3;
	private int m_playerNumber;
	private int m_nApples = 0;
	private Camera m_camera;
	// Use this for initialization

	void Start () {
		m_camera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		float height = Camera.main.orthographicSize;
        float width = height * Camera.main.aspect;
        Debug.Log("height : " + height);
        Debug.Log("width : " + width);
        switch(m_playerNumber) {
        	case 0:
        		transform.position = new Vector3 (-width + 3, height - 2, 0.0f);
        		break;
        	case 1:
        		transform.position = new Vector3 (width - 4, height - 2, 0.0f);
        		break;
        	case 2:
        		transform.position = new Vector3 (-width + 3, -height + 2.5f, 0.0f);
        		break;
        	case 3:
        		transform.position = new Vector3 (width - 4, -height + 2.5f, 0.0f);
        		break;
        	default:
        		break;
        }
	}

	public void init(int playerNumber) {
		Debug.Log("init");
		m_playerNumber = playerNumber;
	}

	public void loseApple() {
		if (m_nApples > 0) {
			m_nApples--;
			switch(m_nApples) {
				case 0 :
					apple1.SetActive(false);
					break;
				case 1 :
					apple2.SetActive(false);
					break;
				case 2 :
					apple3.SetActive(false);
					break;
				default :
				break;
			}
		}
	}

	public void gainApple() {
		Debug.Log("MOAR APPLE !");
		if (m_nApples < 3) {
			m_nApples++;
			switch(m_nApples) {
				case 1 :
					apple1.SetActive(true);
					break;
				case 2 :
					apple2.SetActive(true);
					break;
				case 3 :
					apple3.SetActive(true);
					break;
				default :
				break;
			}
		}

	}
}
