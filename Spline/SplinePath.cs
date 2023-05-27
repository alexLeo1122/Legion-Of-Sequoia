using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

namespace RPG.Character
{
    public class SplinePath : MonoBehaviour
    {
        Spline spline;
        [SerializeField] SplineAnimate splineAnimate;
        private void Awake()
        {
            spline = GetComponent<SplineContainer>().Spline;
        }
        private void Start()
        {
 
        }
        private void Update()
        {
            GetSplineStartPosition();

            //test
            if(Input.GetKeyDown(KeyCode.Tab))
            {
                PauseCharacter();
            }
            if (Input.GetKeyDown(KeyCode.CapsLock))
            {
                splineAnimate.Play();
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                splineAnimate.Restart(true);
            }
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                splineAnimate.Restart(false);

            }

        }
        public Vector3  GetSplineStartPosition()
        {
            Vector3 startPos = transform.TransformPoint(spline.Knots.ToArray()[0].Position);
            Debug.Log("Start Pos " +startPos);
            return startPos;
        }
        public void PauseCharacter()
        {
            splineAnimate.Pause();
        }





    }
}

