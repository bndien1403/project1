using System.Collections;

using UnityEngine;

public class OpenChest : MonoBehaviour
{
    TriggerChest _chest;
    private Animator animator;
    public int GoldReward = 1000;
    // Biến lưu trạng thái cũ để so sánh
    private bool _lastState;
    [Header("Panel Setting")]
    public GameObject chestPanel;
    public  bool isChoose = false;
    

    void Start()
    {
        _chest = GetComponentInChildren<TriggerChest>();
        animator = GetComponent<Animator>();
        chestPanel.SetActive(false);
        
        // Khởi tạo trạng thái ban đầu
        if (_chest != null)
        {
            _chest.isOpen = false;
            _lastState = _chest.isOpen;
            animator.SetBool("isOpen", _lastState);
        }
    }

    void Update()
    { 
        if (_chest == null) return;
        
        // Chỉ cập nhật Animator khi trạng thái isOpen thay đổi 
        if (_chest.isOpen != _lastState) 
        {
            animator.SetBool("isOpen", _chest.isOpen);
            _lastState =_chest.isOpen ;
            if (!isChoose && _chest.isOpen )
            {
               StartCoroutine(OpenChessPanel());    
            }
           

        }
    }
    IEnumerator OpenChessPanel()
    {
        yield return new WaitForSeconds(0.5f);
        chestPanel.SetActive(true);
        Time.timeScale = 0f;

    }


}
