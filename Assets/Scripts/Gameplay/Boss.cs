using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public Image fillImage;
    public TextMeshProUGUI healthText;
    public float health = 2f;
    public GameObject bullet;
    public Transform firePoint;

    public Rigidbody2D rb;
    public CircleCollider2D col;


    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();

        InvokeRepeating("Shoot", 1, 2);
    }

    public void Shoot()
    {
        GameObject gameObject = Instantiate(bullet, firePoint.position, firePoint.rotation);
        gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 2000);
    }

    public void UpdateHealth(float damage)
    {
        fillImage.fillAmount = ((fillImage.fillAmount * 2) - damage) / 2;
        health = fillImage.fillAmount * 2;
        healthText.text = (health*100).ToString("F0") + '%';
    }

    public void Kill()
    {
        Destroy(this.gameObject);
    }
}
