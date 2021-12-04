using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 15;

    private bool isTakingDamage = false;

    private SpriteRenderer spriteRendererRef;
    private Light lightRef;

    private float damageAmount = 0.001f;
    private float totalDamageReceive = 0.0f;

    private bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lightRef = GetComponent<Light>();
        spriteRendererRef = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!canMove)
            return;

        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        Vector2 position = transform.position;
        position += new Vector2(xInput, yInput) / speed;

        rb.MovePosition(position);

        if (isTakingDamage)
        {
            if (totalDamageReceive < 0.5f)
            {
                totalDamageReceive += damageAmount;
                lightRef.color = ColorUtils.ShiftHueColor(lightRef.color, damageAmount);
                spriteRendererRef.color = lightRef.color;
            }
            else
            {
                // GAME OVER
                canMove = false;
                GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
                gm.GameOver();
            }
        }
    }

    public void NextLevel()
    {
        totalDamageReceive = 0.0f;
    }

    public void TakeDamage(bool damage)
    {
        isTakingDamage = damage;
    }
}
