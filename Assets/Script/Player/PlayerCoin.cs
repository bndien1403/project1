using UnityEngine;
using TMPro;

public class PlayerCoin : MonoBehaviour
{
    public int TotalCoins = 0;
    public TextMeshProUGUI coinText;
    public static PlayerCoin Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Coin"))
        {
           
            CoinPool.Instance.ReturnCoinToPool(hit.gameObject);
             AddCoins(1);


        }

    }
    public void AddCoins(int amount)
    {
        TotalCoins += amount;
        coinText.text = TotalCoins.ToString();
    }
     void Update()
    {
       
    }
}
