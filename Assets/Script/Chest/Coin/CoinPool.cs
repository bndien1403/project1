using System.Collections.Generic;
using UnityEngine;

public class CoinPool : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static CoinPool Instance;
 
    [Header("Coin Pool Settings")]
    public GameObject coinPrefab;
    public int coinPoolSize = 100;
    public Queue<GameObject> coinPool = new Queue<GameObject>();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Hủy nếu lỡ có 2 cái Pool
        }

    }

    void Start()
    {
        for(int i =0; i <= coinPoolSize; i++)
        {
            GameObject coin = Instantiate(coinPrefab);
            coin.SetActive(false);
            coinPool.Enqueue(coin);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetCoinFromPool()
    {
        if(coinPool.Count > 0)
        { 

            GameObject coin = coinPool.Dequeue();
            coin.SetActive(true);
            return coin;
        }
        else
        {
            GameObject coin = Instantiate(coinPrefab);
            return coin;
        }
    }
    public void ReturnCoinToPool(GameObject coin)
    {
        coin.SetActive(false);
        coinPool.Enqueue(coin);
    }

}
