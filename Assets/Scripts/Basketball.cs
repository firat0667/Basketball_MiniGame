using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Basketball : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform spawnPoint;
    public float minThrowForce = 5f;
    public float maxThrowForce = 20f;
    public Slider powerSlider;

    private float throwForce;
    private bool isCharging;
    private float chargeTime = 0f;
    private float maxChargeTime = 2f;

    public Text scoreText;
    private int score = 0;

    private void OnEnable()
    {
        BasketDedector.OnBasketScored += OnBasketScored;  
    }

    private void OnDisable()
    {
        BasketDedector.OnBasketScored -= OnBasketScored;  
    }

    void Update()
    {
        HandleInput();
        AdjustPowerSlider();
    }

    void HandleInput()
    {
        float mouseY = Input.mousePosition.y / Screen.height;
        float angle = Mathf.Lerp(30, 80, mouseY); 
        spawnPoint.rotation = Quaternion.Euler(new Vector3(0, 0, -angle));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isCharging = true;
            chargeTime = 0f;
            powerSlider.gameObject.SetActive(true);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            chargeTime += Time.deltaTime;
            float chargePercent = Mathf.Clamp01(chargeTime / maxChargeTime);
            throwForce = Mathf.Lerp(minThrowForce, maxThrowForce, chargePercent);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isCharging = false;
            SpawnBall();
            powerSlider.gameObject.SetActive(false);
        }
    }

    void AdjustPowerSlider()
    {
        if (isCharging)
        {
            float chargePercent = Mathf.Clamp01(chargeTime / maxChargeTime);
            powerSlider.value = chargePercent;
        }
    }

    void SpawnBall()
    {
        GameObject ball = Instantiate(ballPrefab, spawnPoint.position, spawnPoint.rotation);
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.AddForce(-spawnPoint.right * throwForce, ForceMode.Impulse);
    }

    public void OnBasketScored()
    {
        score += 1;
        scoreText.text = score.ToString();
    }
}
