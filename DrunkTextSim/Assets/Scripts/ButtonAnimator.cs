using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimator : MonoBehaviour
{
    private Animator animator;
    private float secondsToWait;

    public bool shouldPlaySmallMovement = true;
    public bool shouldPlayLargeMovement = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        secondsToWait = Random.Range(0.1f, 0.5f);
        StartCoroutine(WaitForRandomSeconds());
        Debug.Log(this.name + " is awake.");
    }

    void Update()
    {
        
    }

    IEnumerator WaitForRandomSeconds()
    {
        Debug.Log(this.name + " is waiting for " + secondsToWait + " seconds.");
        //float time = Random.Range(0.1f, 0.5f);
        yield return new WaitForSeconds(secondsToWait);

        Debug.Log(this.name + "animation is about to play");

        if (shouldPlaySmallMovement)
        {
            animator.SetBool("smallMovement", true);
            Debug.Log("small animation should play");
        }
        else if (shouldPlayLargeMovement)
        {
            animator.SetBool("largeMovement", true);
        }
    }
}
