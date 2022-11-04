using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayController : MonoBehaviour
{
    [SerializeField]
    private GameObject fruit, bomb;

    private Text scoreText;

    private int scoreCount = 0;

    public static GameplayController Instance ;

    private float maxX = 4.25f, maxY =2.25f , posZ = 5.8f;

    void Awake()
    {
        createInstance();
        scoreText = GameObject.Find("Score").GetComponent<Text>();
        Invoke(nameof(Spawner), 0.5f);
    }

    void createInstance()
    {
        if (!Instance)
            Instance = this;
    }

    public void updateScore()
    {
        scoreCount++;
        scoreText.text = "SCORE : " + scoreCount;
    }

    void Spawner()
    {
        StartCoroutine(spawnPickUps());
    }

    IEnumerator spawnPickUps()
    {
        yield return new WaitForSeconds(Random.Range(1f, 1.5f));

        if (Random.Range(0f, 10f) > 2)
        {
            Instantiate(fruit, new Vector3(Random.Range(-maxX, maxX), Random.Range(-maxY, maxY), posZ), Quaternion.identity);
        }
        else Instantiate(bomb, new Vector3(Random.Range(-maxX, maxX), Random.Range(-maxY, maxY), posZ), Quaternion.identity);

        Invoke(nameof(Spawner), 0f);
    }


}
