using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCurveEdited : MonoBehaviour
{
    public List<Transform> points = new List<Transform>();

    public BezierCurve curve;

    [Range(0, 1)] public float percent = 0;

    public bool shouldAnimate = true;

    public AnimationCurve speed; //Animation curve to ease through camera
    public float animationLength = 5;//How long the animation should last

    float timeCurrent = 0;


   
    // Update is called once per frame
    void Update()
    {
        if(transform.position.z >= points[0].transform.position.z)
        {
            shouldAnimate = false;
            //need reference to an animation manager script
        }
        if (shouldAnimate)
        {
            timeCurrent += Time.deltaTime;
            percent = timeCurrent / animationLength;
            percent = Mathf.Clamp(percent, 0, 1);
        }

        SetPositionToCurve();
    }

    private void SetPositionToCurve()
    {
        if (curve)
        {
            float p = speed.Evaluate(percent);//taking our percent value passing it in to get new percent values
            transform.position = curve.FindPositionAt(p);
        }
    }

    void OnValidate()
    {
        SetPositionToCurve();
    }
   
}
