﻿using UnityEngine;
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
    private GameObject[] players;
    private GameObject[] apples;

    public int maxNbOfWalls = 8;
    public int maxNbOfPlayers = 4;
    public int maxNbOfApples = 10;

    public float playerSpawnCollisionRadius = 0.75f;

    public float missileSpeed = 0.2f;

    private Vector3 playableAreaMin;
    private Vector3 playableAreaMax;
    
    float GetCollisionRadius(GameObject i_object)
    {
        Vector3 objectExtents = i_object.GetComponent<SpriteRenderer>().sprite.bounds.extents;
        objectExtents.Scale(wallModel.transform.localScale); // for zoom
        float objectCollisionRadius = Mathf.Sqrt(objectExtents.x * objectExtents.x + objectExtents.y * objectExtents.y);
        return objectCollisionRadius;
    }

    void SpawnWalls()
    {
        float wallCollisionRadius = GetCollisionRadius(wallModel);
        float playerCollisionRadius = playerSpawnCollisionRadius;
        float spawnCollisionRadius = wallCollisionRadius + playerCollisionRadius;
        
        Vector3 spawnAreaMin = playableAreaMin + new Vector3(spawnCollisionRadius, spawnCollisionRadius, 0.0f);
        Vector3 spawnAreaMax = playableAreaMax - new Vector3(spawnCollisionRadius, spawnCollisionRadius, 0.0f);
        
        for (int i = 0; i < maxNbOfWalls; i++)
        {
            int maxNbOfAttempts = 10;
            bool wallSpawned = false;
            for (int nbOfAttempts = 0; nbOfAttempts < maxNbOfAttempts; nbOfAttempts++)
            {
                Vector2 newPosition = new Vector2(Random.Range(spawnAreaMin.x, spawnAreaMax.x), Random.Range(spawnAreaMin.y, spawnAreaMax.y));
                bool collisionFound = false;
                for(int j = 0; j < i; j++)
                {
                    if (walls[j] == null)
                        continue;
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
                    wallSpawned = true;
                    break;
                }
            }
            if (!wallSpawned)
                break;
        }

    }
    
    GameObject SpawnRandomApple()
    {
        float wallCollisionRadius = GetCollisionRadius(wallModel);
        float playerCollisionRadius = playerSpawnCollisionRadius;
        float appleCollisionRadius = GetCollisionRadius(appleModel);
        float playerExtraCollisionDistance = playerCollisionRadius * 3;

        float spawnWallCollisionRadius = wallCollisionRadius + appleCollisionRadius;
        float spawnPlayerCollisionRadius = playerCollisionRadius + appleCollisionRadius + playerExtraCollisionDistance * 0.5f;
        float spawnAppleCollisionRadius = 2 * appleCollisionRadius;
        
        Vector3 spawnAreaMin = playableAreaMin + new Vector3(appleCollisionRadius, appleCollisionRadius, 0.0f);
        Vector3 spawnAreaMax = playableAreaMax - new Vector3(appleCollisionRadius, appleCollisionRadius, 0.0f);
        
        int maxNbOfAttempts = 10;
        for (int nbOfAttempts = 0; nbOfAttempts < maxNbOfAttempts; nbOfAttempts++)
        {
            Vector2 newPosition = new Vector2(Random.Range(spawnAreaMin.x, spawnAreaMax.x), Random.Range(spawnAreaMin.y, spawnAreaMax.y));
            bool collisionFound = false;
            // collision with walls
            for (int j = 0; j < maxNbOfWalls; j++)
            {
                if (walls[j] == null)
                    continue;
                Vector3 wallJPosition = walls[j].transform.position;
                Vector3 vector = wallJPosition - new Vector3(newPosition.x, newPosition.y, 0.0f);
                if (vector.magnitude < 2.0f * spawnWallCollisionRadius)
                {
                    collisionFound = true;
                    break;
                }
            }
            if (collisionFound)
                continue;
            // collision with players
            for (int j = 0; j < maxNbOfPlayers; j++)
            {
                if (players[j] == null)
                    continue;
                Vector3 playerJPosition = players[j].transform.position;
                Vector3 vector = playerJPosition - new Vector3(newPosition.x, newPosition.y, 0.0f);
                if (vector.magnitude < 2.0f * spawnPlayerCollisionRadius)
                {
                    collisionFound = true;
                    break;
                }
            }
            if (collisionFound)
                continue;
            // collision with apples
            for (int j = 0; j < maxNbOfApples; j++)
            {
                if (apples[j] == null)
                    continue;
                Vector3 appleJPosition = apples[j].transform.position;
                Vector3 vector = appleJPosition - new Vector3(newPosition.x, newPosition.y, 0.0f);
                if (vector.magnitude < 2.0f * spawnAppleCollisionRadius)
                {
                    collisionFound = true;
                    break;
                }
            }
            if (!collisionFound)
            {
                GameObject newApple = (GameObject)Instantiate(appleModel, newPosition, Quaternion.identity);
                return newApple;
            }
        }
        return null;
    }
    
    void SpawnObjects()
    {
        for (int i = 0; i < maxNbOfApples; i++)
        {
            if (apples[i] != null)
                continue;
            apples[i] = SpawnRandomApple();
        }
    }

    GameObject SpawnRandomPlayer()
    {
        float wallCollisionRadius = GetCollisionRadius(wallModel);
        float playerCollisionRadius = playerSpawnCollisionRadius;
        float appleCollisionRadius = GetCollisionRadius(appleModel);
        float playerExtraCollisionDistance = playerCollisionRadius * 3;

        float spawnWallCollisionRadius = wallCollisionRadius + playerCollisionRadius;
        float spawnPlayerCollisionRadius = playerCollisionRadius + playerCollisionRadius + playerExtraCollisionDistance * 0.5f;
        float spawnAppleCollisionRadius = appleCollisionRadius + playerCollisionRadius;

        Vector3 spawnAreaMin = playableAreaMin + new Vector3(playerCollisionRadius, playerCollisionRadius, 0.0f);
        Vector3 spawnAreaMax = playableAreaMax - new Vector3(playerCollisionRadius, playerCollisionRadius, 0.0f);

        int maxNbOfAttempts = 100;
        for (int nbOfAttempts = 0; nbOfAttempts < maxNbOfAttempts; nbOfAttempts++)
        {
            Vector2 newPosition = new Vector2(Random.Range(spawnAreaMin.x, spawnAreaMax.x), Random.Range(spawnAreaMin.y, spawnAreaMax.y));
            bool collisionFound = false;
            // collision with walls
            for (int j = 0; j < maxNbOfWalls; j++)
            {
                if (walls[j] == null)
                    continue;
                Vector3 wallJPosition = walls[j].transform.position;
                Vector3 vector = wallJPosition - new Vector3(newPosition.x, newPosition.y, 0.0f);
                if (vector.magnitude < 2.0f * spawnWallCollisionRadius)
                {
                    collisionFound = true;
                    break;
                }
            }
            if (collisionFound)
                continue;
            // collision with players
            for (int j = 0; j < maxNbOfPlayers; j++)
            {
                if (players[j] == null)
                    continue;
                Vector3 playerJPosition = players[j].transform.position;
                Vector3 vector = playerJPosition - new Vector3(newPosition.x, newPosition.y, 0.0f);
                if (vector.magnitude < 2.0f * spawnPlayerCollisionRadius)
                {
                    collisionFound = true;
                    break;
                }
            }
            if (collisionFound)
                continue;
            // collision with apples
            for (int j = 0; j < maxNbOfApples; j++)
            {
                if (apples[j] == null)
                    continue;
                Vector3 appleJPosition = apples[j].transform.position;
                Vector3 vector = appleJPosition - new Vector3(newPosition.x, newPosition.y, 0.0f);
                if (vector.magnitude < 2.0f * spawnAppleCollisionRadius)
                {
                    collisionFound = true;
                    break;
                }
            }
            if (!collisionFound)
            {
                GameObject newApple = (GameObject)Instantiate(playerModel, newPosition, Quaternion.identity);
                return newApple;
            }
        }
        return null;
    }

    void SpawnPlayers()
    {
        for (int i = 0; i < maxNbOfPlayers; i++)
        {
            if (players[i] != null)
                continue;
            players[i] = SpawnRandomPlayer();
            players[i].GetComponent<playerController>().playerId = i;

            // HACK TMP TEST
            Shoot(players[i]);
            // HACK TMP TEST END
        }
    }

    void Start()
    {
        walls = new GameObject[maxNbOfWalls];
        players = new GameObject[maxNbOfPlayers];
        apples = new GameObject[maxNbOfApples];
        playableAreaMin = playableArea.transform.TransformPoint(playableArea.GetComponent<SpriteRenderer>().sprite.bounds.min);
        playableAreaMax = playableArea.transform.TransformPoint(playableArea.GetComponent<SpriteRenderer>().sprite.bounds.max);
        SpawnWalls();
        SpawnPlayers();
        SpawnObjects();
    }

    void FixedUpdate()
    {
    }

    public void Collect(GameObject i_collectedObject, int i_playerId)
    {
        if (i_collectedObject.CompareTag("Apple"))
        {
            for(int i = 0; i < maxNbOfApples; i++)
            {
                if(i_collectedObject == apples[i])
                {
                    Destroy(i_collectedObject);
                    apples[i] = null;
                    break;
                }
            }
        }
    }

    public void DamagePlayer(int i_playerId)
    {
        // TODO stunned ? invulnerable ? lose hat ?
    }

    public void Shoot(GameObject i_shooter)
    {
        Vector3 shooterDirectionNormalized = i_shooter.transform.right;
        shooterDirectionNormalized.Normalize();
        Vector3 shootingPosition = i_shooter.transform.position + shooterDirectionNormalized * playerSpawnCollisionRadius * 2.0f;
        GameObject missile = (GameObject)Instantiate(missileModel, shootingPosition, i_shooter.transform.rotation);
        missile.GetComponent<Rigidbody2D>().velocity = shooterDirectionNormalized * missileSpeed;
        missile.GetComponent<missileController>().shooterId = i_shooter.GetComponent<playerController>().playerId;
    }


}