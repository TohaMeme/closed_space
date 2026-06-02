using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private bool playedOnce;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && playedOnce == false)
        {
            animator.SetBool("StartAnim", true);
            audioSource.Play();
            playedOnce = true;
        }
        else if (other.CompareTag("Player") && playedOnce == true)
        {
            animator.SetBool("StartAnim", false);
        }
    }
}
