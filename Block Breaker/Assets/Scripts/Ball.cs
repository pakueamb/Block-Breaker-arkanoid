﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that controls the ball's behavior
/// </summary>

public class Ball : MonoBehaviour
{
    [SerializeField] PlayerController player1;

    [Header("Launch Paramaters")]
    [SerializeField] float xPush = 5f;
    [SerializeField] float yPush = 5f;
    [SerializeField] public float ballSpeed = 5f;

    [Header("Randomization")]
    [Space(10)]
    [SerializeField] float RandomContactFactor = .2f;

    [Header("SFX")]
    [Space(10)]
    [SerializeField] AudioClip[] ballContactSounds;


    Vector2 playerToBallVector;
    bool lockedBall = true;

    AudioSource ballAudioSource;
    GameSession gameSession;

    new Rigidbody2D rigidbody;
    
    // Start is called before the first frame update
    void Start()
    {
        playerToBallVector = transform.position - player1.transform.position;
        ballAudioSource = GetComponent<AudioSource>();
        gameSession = FindObjectOfType<GameSession>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lockedBall) { HoldBall(); LaunchBall(); }
        rigidbody.velocity = ballSpeed * (rigidbody.velocity.normalized);
    }

    private void LaunchBall()
    {
        if (Input.GetMouseButtonDown(0) || gameSession.GetAutoplay())
        {
            lockedBall = false;
            rigidbody.velocity = new Vector2(xPush, yPush);
        }
    }

    private void HoldBall()
    {
        Vector2 player1Pos = player1.transform.position;
        transform.position = player1Pos + playerToBallVector;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!lockedBall)
        {
            if (ballAudioSource)
            {//play a random(53) contact sound (54) whenever the ball touches something(49), if there is an audio source attached to the ball(51), and if the ball has been detached from the player(51)
                AudioClip audioClip = ballContactSounds[Random.Range(0, ballContactSounds.Length)];
                ballAudioSource.PlayOneShot(audioClip);
            }

            Vector2 VelocityTweak = new Vector2(Random.Range(0, RandomContactFactor), Random.Range(0, RandomContactFactor));
            GetComponent<Rigidbody2D>().velocity += VelocityTweak;
        }
        
    }
}
