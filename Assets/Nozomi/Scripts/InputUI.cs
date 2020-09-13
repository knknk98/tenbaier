using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputUI : MonoBehaviour
{
    [SerializeField] private GameObject maximizeImage;
    [SerializeField] private GameObject minimizeImage;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Swap(bool isMaximize)
    {
        if (isMaximize)
        {
            minimizeImage.transform.SetAsFirstSibling();
            animator.CrossFadeInFixedTime("Maximize", 0f);
        }
        else
        {
            maximizeImage.transform.SetAsFirstSibling();
            animator.CrossFadeInFixedTime("Minimize", 0f);
        }
    }
}
