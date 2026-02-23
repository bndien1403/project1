using System;
using System.Globalization;
using UnityEngine;
using TMPro ;
using Random = UnityEngine.Random;


public class DamagePopup : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float moveSpeed = 2f;
    public float lifeTime = 1f;
    public TextMeshPro text;

    private Color textColor;

    private void Awake()
    {
        if (text == null) { return; }
        text = GetComponent<TextMeshPro>();
        
        textColor = text.color;
    }
    private Vector3 moveDir;

    void Start()
    {
        moveDir = new Vector3(
            Random.Range(-0.5f, 0.5f),  // lệch trái phải
            1f,
            Random.Range(-0.5f, 0.5f)   // lệch trước sau
        ).normalized;
    }

    public void SetUp(float damage)
    {
         text.text = damage.ToString();
    }
    

    // Update is called once per frame
    void Update()
    {
        transform.position += moveDir * moveSpeed * Time.deltaTime;
            // transform.rotation = Quaternion.Euler(
            // Random.Range(-15f, 15f),
            // Random.Range(0f, 360f),
            // 0f
            // );
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0f)
        {
            textColor.a -= Time.deltaTime * 2;
            text.color = textColor;
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }   
        
    }
    void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }
}
