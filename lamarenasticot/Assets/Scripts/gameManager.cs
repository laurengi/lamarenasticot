using UnityEngine;
//using UnityEngine.UI;
//using System.Collections;

public class gameManager : MonoBehaviour
{

    public GameObject appleModel;
    public GameObject playerModel;
    public GameObject missileModel;
    public GameObject background;
    public GameObject playableArea;
    public GameObject wallModel;

    private GameObject[] walls;

    public int nbOfWalls = 8;

    Vector3 GetPlayableAreaMin()
    {
        return playableArea.transform.TransformPoint(playableArea.GetComponent<SpriteRenderer>().sprite.bounds.min);
    }
    Vector3 GetPlayableAreaMax()
    {
        return playableArea.transform.TransformPoint(playableArea.GetComponent<SpriteRenderer>().sprite.bounds.max);
    }

    void SpawnWalls()
    {
        Vector3 wallExtents = wallModel.GetComponent<SpriteRenderer>().sprite.bounds.extents;
        wallExtents.Scale(wallModel.transform.localScale); // for zoom
        float wallCollisionRadius = Mathf.Sqrt(wallExtents.x * wallExtents.x + wallExtents.y * wallExtents.y);

        //float playerCollisionRadius = 1.0f;
        Vector3 playerExtents = playerModel.GetComponent<SpriteRenderer>().sprite.bounds.extents;
        playerExtents.Scale(wallModel.transform.localScale); // for zoom
        float playerCollisionRadius = Mathf.Sqrt(playerExtents.x * playerExtents.x + playerExtents.y * playerExtents.y);

        float spawnCollisionRadius = wallCollisionRadius + playerCollisionRadius;

        Debug.Log(wallCollisionRadius);
        Debug.Log(playerCollisionRadius);
        Debug.Log(spawnCollisionRadius);

        Vector3 playableAreaMin = GetPlayableAreaMin() + new Vector3(spawnCollisionRadius, spawnCollisionRadius, 0.0f);
        Vector3 playableAreaMax = GetPlayableAreaMax() - new Vector3(spawnCollisionRadius, spawnCollisionRadius, 0.0f);

        for (int i = 0; i < nbOfWalls; i++)
        {
            int maxNbOfAttempts = 10;
            for (int nbOfAttempts = 0; nbOfAttempts < maxNbOfAttempts; nbOfAttempts++)
            {
                Vector2 newPosition = new Vector2(Random.Range(playableAreaMin.x, playableAreaMax.x), Random.Range(playableAreaMin.y, playableAreaMax.y));
                bool collisionFound = false;
                for(int j = 0; j < i; j++)
                {
                    Vector3 wallJPosition = walls[j].transform.position;
                    Vector3 wallsVector = wallJPosition - new Vector3(newPosition.x, newPosition.y, 0.0f);
                    if (wallsVector.magnitude < 2.0f * spawnCollisionRadius)
                    {
                        collisionFound = true;
                        break;
                    }
                }
                if (!collisionFound)
                {
                    Quaternion newRotation = Quaternion.identity;
                    float randomAngle = Random.Range(0.0f, 360.0f);
                    newRotation.eulerAngles = new Vector3(0.0f, 0.0f, randomAngle);
                    walls[i] = (GameObject)Instantiate(wallModel, newPosition, newRotation);
                    break;
                }
            }
        }


        for (int k = 0; k < nbOfWalls; k++)
        {
            Vector3 wallKPosition = walls[k].transform.position;
            Debug.Log(wallKPosition);
        }

    }

    void SpawnPlayers()
    {

    }

    void SpawnRandomApple()
    {

    }

    void SpawnObjects()
    {

        Vector3 playableAreaMin = GetPlayableAreaMin();
        Vector3 playableAreaMax = GetPlayableAreaMax();

        for(int i = 0; i < 100; i++)
        {
            Vector2 position = new Vector2(Random.Range(playableAreaMin.x, playableAreaMax.x), Random.Range(playableAreaMin.y, playableAreaMax.y));
            
            Instantiate(appleModel, position, Quaternion.identity);
        }

    }

    void Start()
    {
        walls = new GameObject[nbOfWalls];
        SpawnWalls();
        SpawnPlayers();
        SpawnObjects();
    }

    void FixedUpdate()
    {
    }


}