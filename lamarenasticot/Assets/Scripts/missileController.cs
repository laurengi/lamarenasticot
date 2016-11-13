using UnityEngine;
using System.Collections;

public class missileController : MonoBehaviour {

    private gameManager gm;
    public int shooterId;

    // Use this for initialization
    void Start ()
    {
        gm = FindObjectOfType<gameManager>();
    }
    
    // Update is called once per frame
    void Update ()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("missile.OnTriggerEnter2D");
        // TODO : store gm in private member and fill it in Start()
        if (other.gameObject.CompareTag("Wall"))
        {
            Debug.Log("missile.OnTriggerEnter2D # Wall");
            Destroy(gameObject);
            return;
        }
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("missile.OnTriggerEnter2D # Player");
            int collidedPlayerId = other.GetComponent<playerController>().playerId;
            if(shooterId != collidedPlayerId)
            {
                gm.DamagePlayer(collidedPlayerId);
                Destroy(gameObject);
                return;
            }
        }
    }
}
