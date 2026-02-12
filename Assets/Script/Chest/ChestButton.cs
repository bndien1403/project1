using UnityEngine;
using UnityEngine.UI ;

public class ChestButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private OpenChest _openChest;
  
   
    void Start()
    {
        _openChest = FindAnyObjectByType<OpenChest>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
   
    public void OnClickGold()
    {
      _openChest.chestPanel.SetActive(false);
        _openChest.isChoose = true;
        PlayerCoin.Instance.AddCoins(_openChest.GoldReward);
        Debug.Log("Bạn đã nhận 1000 vàng");
        Time.timeScale = 1f;
    }
    public void OnClickItem()
    { 
        _openChest.chestPanel.SetActive(false);
        _openChest.isChoose = true;
        Debug.Log("Bạn đã nhận 1 item");
        Time.timeScale = 1f;

    }
    public void OnClickClose()
    {
        _openChest.chestPanel.SetActive(false);
        Debug.Log("Đóng bảng chọn");
        Time.timeScale = 1f;
    }
}
