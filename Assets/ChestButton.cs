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
        Debug.Log("Bạn đã nhận 1000 vàng");
    }
    public void OnClickItem()
    { 
        //_openChest.chestPanel.SetActive(false);
        Debug.Log("Bạn đã nhận 1 item");
        
    }
}
