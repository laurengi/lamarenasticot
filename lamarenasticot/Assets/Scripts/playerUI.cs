using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerUI : MonoBehaviour {

    public GameObject face;
    public GameObject apple1;
    public GameObject apple2;
    public GameObject apple3;
    public GameObject hat1;
    public GameObject hat2;
    public GameObject hat3;
    private int m_playerNumber;
    private int m_nApples = 0;
    private int m_nHats = 0;
    // Use this for initialization

    void Start () {
        float height = Camera.main.orthographicSize;
        float width = height * Camera.main.aspect;
        Color spriteColor;
        switch (m_playerNumber) {
            case 0:
                transform.position = new Vector3 (-width + 3, height - 4, 0.0f);
                spriteColor = Color.white;
                break;
            case 1:
                transform.position = new Vector3 (width - 4, height - 4, 0.0f);
                spriteColor = Color.red;
                break;
            case 2:
                transform.position = new Vector3 (-width + 3, -height + 2.5f, 0.0f);
                spriteColor = Color.green;
                break;
            case 3:
                transform.position = new Vector3 (width - 4, -height + 2.5f, 0.0f);
                spriteColor = Color.blue;
                break;
            default:
                spriteColor = Color.white;
                break;
        }
        face.GetComponent<SpriteRenderer>().color = spriteColor;
    }
    
    // Update is called once per frame
    void Update () {
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

    public void loseHat()
    {
        if (m_nHats > 0)
        {
            m_nHats--;
            switch (m_nHats)
            {
                case 0:
                    hat1.SetActive(false);
                    break;
                case 1:
                    hat2.SetActive(false);
                    break;
                case 2:
                    hat3.SetActive(false);
                    break;
                default:
                    break;
            }
        }
    }

    public void gainApple()
    {
        if (m_nApples < 3)
        {
            m_nApples++;
            switch (m_nApples)
            {
                case 1:
                    apple1.SetActive(true);
                    break;
                case 2:
                    apple2.SetActive(true);
                    break;
                case 3:
                    apple3.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }


    public void gainHat()
    {
        Debug.Log("gainHat");
        if (m_nHats < 3)
        {
            m_nHats++;
            switch (m_nHats)
            {
                case 1:
                    hat1.SetActive(true);
                    break;
                case 2:
                    hat2.SetActive(true);
                    break;
                case 3:
                    hat3.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }
}
