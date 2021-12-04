using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;

    public MapGenerator mapGenerator;

    public GameObject enemyLightPrefab;
    public GameObject enemiesContainer;

    public GameObject exitPrefab;
    private  GameObject exitGameObject;

    private Vector2 exitPosition;

    private int level = 1;

    public GameObject gameOverUI;
    public GameObject levelUI;

    // Start is called before the first frame update
    void Start()
    {
        gameOverUI.SetActive(false);
        levelUI.GetComponent<UnityEngine.UI.Text>().text = level.ToString();
        mapGenerator.Initialize();
        StartLevel();
    }

    public void NextLevel()
    {
        level++;
        levelUI.GetComponent<UnityEngine.UI.Text>().text = level.ToString();

        // Destroy current level
        Destroy(exitGameObject);
        foreach (Transform child in enemiesContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        // Generate next level
        mapGenerator.ReInitialize();

        StartLevel();
    }

    private void StartLevel()
    {
        Vector3 middleMap = new Vector3(Mathf.RoundToInt(mapGenerator.width / 2.0f), Mathf.RoundToInt(mapGenerator.height / 2.0f), -2);
        player.transform.position = middleMap;

        // Random HSV color
        Color colorMiddleLight = Color.HSVToRGB(Random.value, 1, 1);
        player.GetComponent<Light>().color = colorMiddleLight;
        player.GetComponent<SpriteRenderer>().color = colorMiddleLight;

        // Spawn Enemies Light
        Color complementaryColor = ColorUtils.GetComplementaryColorRGB(colorMiddleLight);
        for (int i = 0; i < 15 + level; i++)
        {
            Vector3 randomFloorLocation = mapGenerator.GetRandomFloorPosition();
            randomFloorLocation.z = -2;
            GameObject enemyLightGO = Instantiate(enemyLightPrefab, randomFloorLocation, Quaternion.identity);
            enemyLightGO.GetComponent<Light>().color = complementaryColor;
            enemyLightGO.GetComponent<SpriteRenderer>().color = complementaryColor;
            enemyLightGO.transform.parent = enemiesContainer.transform;
        }

        // Spawn Exit (TODO: check if no enemy)
        exitPosition = mapGenerator.GetExitPosition();
        exitGameObject = Instantiate(exitPrefab, new Vector3(exitPosition.x, exitPosition.y, -1), Quaternion.identity);
    }

    public void GameOver()
    {
        gameOverUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
