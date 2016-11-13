using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerUI : MonoBehaviour {

	public GameObject apple1;
	public GameObject apple2;
	public GameObject apple3;

	private int nApples = 3;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void loseApple() {
		if (nApples > 0) {
			nApples--;
			switch(nApples) {
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
		if (nApples < 3) {
			nApples++;
			switch(nApples) {
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
