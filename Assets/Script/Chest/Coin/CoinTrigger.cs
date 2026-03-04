using UnityEngine;

public class CoinTrigger : MonoBehaviour
{
    [Header("Magnet Settings")]
    public float magnetSpeed = 10f; // Tốc độ bay về phía player
    public float pickupDistance = 0.5f; // Khoảng cách để coi là đã ăn được

    private Transform targetPlayer;
    private bool isMagnetized = false;

    private void OnEnable()
    {
        // Reset trạng thái khi được lấy ra từ Pool
        isMagnetized = false;
        targetPlayer = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Chỉ kích hoạt hút nếu chưa bị hút và đối tượng là Player
        if (!isMagnetized && other.CompareTag("Player"))
        {
            
            targetPlayer = other.transform;
            isMagnetized = true;
        }
    }

    private void Update()
    {
        if (isMagnetized && targetPlayer != null)
        {
            // Di chuyển về phía Player
            transform.position = Vector3.MoveTowards(transform.position, targetPlayer.position, magnetSpeed * Time.deltaTime);

            // Kiểm tra khoảng cách để "ăn" đồng tiền
            if (Vector3.Distance(transform.position, targetPlayer.position) < pickupDistance)
            {
                CollectCoin();
            }
        }
    }

    private void CollectCoin()
    {
        // Cộng tiền
        if (PlayerCoin.Instance != null)
        {
            PlayerCoin.Instance.AddCoins(1);
        }

        // Trả về Pool
        if (CoinPool.Instance != null)
        {
            CoinPool.Instance.ReturnCoinToPool(gameObject);
        }
        else
        {
            // Fallback nếu không dùng pool (hoặc test riêng lẻ)
            Destroy(gameObject);
        }
    }
}
