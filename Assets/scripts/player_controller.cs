﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


public class player_controller : MonoBehaviour {
    [Header("general")]
    [Tooltip("in m/s")] [SerializeField] float xspeed = 4f;
    [Tooltip("in m/s")][SerializeField] float yspeed = 4f;
    [Header("border")]
    [Tooltip("in m")] [SerializeField] float xmin = -3f;
    [Tooltip("in m")] [SerializeField] float ymin = -2f;
    [Tooltip("in m")] [SerializeField] float xmax = 6f;
    [Tooltip("in m")] [SerializeField] float ymax = 1.5f;

    [Header("position based")]
    [SerializeField] float pitchfactor = -5f;
    [SerializeField] float yawfactor = 5f;
    [Header("throw based")]
    [SerializeField] float throwpitchfactor = -10f;
    [SerializeField] float throwrollfactor = -20;

    [Header("gun")]
    [SerializeField] GameObject[] guns;
 
    float xThrow;
    float yThrow;

    bool cancontrollship = true;

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (cancontrollship)
        { 
            Calculatemovement();
            Calculaterotation();
            Calculatefiring();
        }
    }

    private void Calculatemovement()
    {
        xThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        yThrow = CrossPlatformInputManager.GetAxis("Vertical");

        float xoffset = xThrow * xspeed * Time.deltaTime;
        float yoffset = yThrow * yspeed * Time.deltaTime;

        float rawxpos = transform.localPosition.x + xoffset;
        float rawypos = transform.localPosition.y + yoffset;

        float xpos = Mathf.Clamp(rawxpos, xmin, xmax);
        float ypos = Mathf.Clamp(rawypos, ymin, ymax);

        transform.localPosition = new Vector3(xpos, ypos, transform.localPosition.z);
    }
    private void Calculaterotation()
    {
    
        float ythrowtopitch = yThrow * throwpitchfactor;
        float ypostopitch = transform.localPosition.y * pitchfactor;
        float pitch = ypostopitch + ythrowtopitch;

        float yaw = transform.localPosition.x * yawfactor;
        float roll = xThrow * throwrollfactor;
        transform.localRotation = Quaternion.Euler(pitch,yaw,roll);
    }


    private void Calculatefiring()
    {
        if (CrossPlatformInputManager.GetButton("Fire"))
        {
            Activate_guns();
        }
        else
        {
            Deactivate_guns();
        }
    }

    private void Activate_guns()
    {
        foreach (GameObject gun in guns)
        {
            gun.SetActive(true);
        }
    }

    private void Deactivate_guns()
    {
        foreach (GameObject gun in guns)
        {
            gun.SetActive(false);
        }
    }

    void startdeathsequence()
    {
        cancontrollship = false;
    }
}
