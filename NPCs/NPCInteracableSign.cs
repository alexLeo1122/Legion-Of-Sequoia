using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Character
{
    public class NPCInteracableSign : MonoBehaviour
    {
        private void Update()
        {
            FacingTowardsCamera();
        }


        private void FacingTowardsCamera()
        {
            //write the code so the canvas facing toward camera
            transform.LookAt(Camera.main.transform.position);    
        }
    }
}


