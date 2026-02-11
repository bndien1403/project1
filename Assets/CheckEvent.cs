using UnityEngine;
using UnityEngine.EventSystems;
public class CheckEvent : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   

  
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Over UI: " + EventSystem.current.IsPointerOverGameObject());
            }
        }
    

}
