using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using Cinemachine;
using UnityEditor;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPun
{
    public float moveSpeed = 5;
    public Camera playerCamera;
    public CinemachineVirtualCamera vcam;
    private Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public Transform firePoint;
    public PhotonView photonview;
    public int score;

    Vector2 movement;
    Vector2 mousePos;

    private bool allowMoving = true;
    public bool disableInputs = false;

    public TextMeshProUGUI playerName;
    public TextMeshProUGUI scoreText;

    public GameObject bulletCircle;
    public GameObject bulletTriangle;
    public GameObject bulletStar;

    public float bulletForce = 20f;

    // Use this for initialization
    void Awake()
    {
        if (photonView.IsMine)
        {
            GameManager.instance.LocalPlayer = this.gameObject;
            playerCamera = FindObjectOfType<Camera>();
            vcam = FindObjectOfType<CinemachineVirtualCamera>();
            vcam.Follow = this.gameObject.transform;
            playerName.text = PhotonNetwork.NickName;

        }
        else
        {
            
            playerName.text = photonview.Owner.NickName;
            playerName.color = Color.red;
            
        }

        InvokeRepeating("AddScoreMinute", 1, 60);

    }




    public void AddScoreMinute()
    {
        score++;
    }

    public void AddScore(int score)
    {
        this.score += score;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (UIManager.multiplayer == true)
        {
            Debug.Log(UIManager.teamName);
            PhotonNetwork.NickName += "|" + UIManager.teamName;
            playerName.text = PhotonNetwork.NickName;
        }
    }
    // Update is called once per frame
    void Update()
    {
        scoreText.text = score.ToString();
        if (photonView.IsMine && !disableInputs)
        {
            Inputs();
        }
    }

    private void Inputs()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePos = playerCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }

    public void Shoot()
    {
        GameObject bullet;
        switch (OptionSettings.bulletType)
        {
            case OptionSettings.BulletType.triangle:
                bullet = bulletTriangle;
                break;
            case OptionSettings.BulletType.circle:
                bullet = bulletCircle;
                break;
            case OptionSettings.BulletType.star:
                bullet = bulletStar;
                break;
            default:
                bullet = bulletCircle;
                break;
        }
        Debug.Log(OptionSettings.color);
        Debug.Log(OptionSettings.bulletType);
        switch (OptionSettings.color)
        {
            case OptionSettings.Colors.red:
                bullet.GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case OptionSettings.Colors.orange:
                bullet.GetComponent<SpriteRenderer>().color = new Color(255, 140, 0);
                break;
            case OptionSettings.Colors.yellow:
                bullet.GetComponent<SpriteRenderer>().color = Color.yellow;
                break;
            case OptionSettings.Colors.green:
                bullet.GetComponent<SpriteRenderer>().color = Color.green;
                break;
            case OptionSettings.Colors.cyan:
                bullet.GetComponent<SpriteRenderer>().color = Color.cyan;
                break;
            case OptionSettings.Colors.blue:
                bullet.GetComponent<SpriteRenderer>().color = Color.blue;
                break;
            case OptionSettings.Colors.violet:
                bullet.GetComponent<SpriteRenderer>().color = Color.magenta;
                break;
            default:
                break;
        }


        GameObject gameObject = PhotonNetwork.Instantiate(bullet.name, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        gameObject.GetComponent<Bullet>().localPlayerObj = this.gameObject;
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Pickup")
        {
            score++;
            Destroy(collision.gameObject);
        }
    }

}
