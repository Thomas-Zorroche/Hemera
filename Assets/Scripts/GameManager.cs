using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //public GameObject playerPrefab;
    
    public Player player;

    public MapGenerator mapGenerator;

    public GameObject enemyLightPrefab;

    // Start is called before the first frame update
    void Start()
    {
        mapGenerator.Initialize();

        Vector3 middleMap = new Vector3(Mathf.RoundToInt(mapGenerator.width / 2.0f), Mathf.RoundToInt(mapGenerator.height / 2.0f), -2);
        player.transform.position = middleMap;

        // Random HSV color
        Color colorMiddleLight = Color.HSVToRGB(Random.value, 1, 1);
        player.GetComponent<Light>().color = colorMiddleLight;
        player.GetComponent<SpriteRenderer>().color = colorMiddleLight;
            
        // Spawn Enemies Light
        Color complementaryColor = ColorUtils.GetComplementaryColorRGB(colorMiddleLight);

        for (int i = 0; i < 5; i++)
        {
            Vector3 randomFloorLocation = mapGenerator.GetRandomFloorPosition();
            randomFloorLocation.z = -2;
            GameObject enemyLightGO = Instantiate(enemyLightPrefab, randomFloorLocation, Quaternion.identity);
            enemyLightGO.GetComponent<Light>().color = complementaryColor;
            enemyLightGO.GetComponent<SpriteRenderer>().color = complementaryColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
