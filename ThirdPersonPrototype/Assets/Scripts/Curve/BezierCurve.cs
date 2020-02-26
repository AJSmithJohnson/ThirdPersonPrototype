using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//REWATCH CLASS VIDEO YOU MISSED THE VERY LAST PART OF THAT
[ExecuteInEditMode]
public class BezierCurve : MonoBehaviour
{

    //YOU CAN USE ALT+ARROW KEYS TO MOVE A LINE OF CODE UP AND DOWN

    /// <summary>
    /// The resolution to calculate each individual curve not the total spline
    /// A spline with n points will have a total resolution of     RESOLUTION * (n - 2)
    /// </summary>
    public const int RESOLUTION = 10; 
   
    
    //CTRL + M
    // CTRL + O
    //Allows us to collapse this stuff into one object


     public Vector3[] points = new Vector3[0];
    ////////////Cached Values: 
     float[] curveLengths = new float[0];
     float splineLength = 0;
    public Vector3[] worldPoints { get; private set; }
    /// /////////////////
    
    
    void Update()
    {
        CashSplineData();

    }
    void OnValidate()
    {
        CashSplineData();
    }

    public void CashSplineData()
    {
        CalcWorldPositions();
        LengthOfSpline();
    }

    public void AddPoint()
    {
        Vector3[] pts = new Vector3[points.Length + 1];
        for(int i = 0; i < points.Length; i ++)
        {
            pts[i] = points[i]; //can't increase array size but can make an array one larger than the previous array
        }

        if (pts.Length == 1) pts[0] = Vector3.zero;//if new array of points has only one point first point should be at 0
        else if (pts.Length == 2) pts[1] = pts[0] + Vector3.right;//if new array of points has 2 points second one should be further foward than first
        else
        {
            Vector3 posLast = pts[pts.Length - 2];
            Vector3 pos2ndToLast = pts[pts.Length - 3];
            Vector3 dir = posLast - pos2ndToLast;
            pts[pts.Length - 1] = posLast + dir;
        }
        points = pts;
    }

    public Vector3 FindPositionAt(float p)
    {
        if (worldPoints == null) return Vector3.zero;
        if(worldPoints.Length == 0)
        {
            return Vector3.zero;
        }
        if(worldPoints.Length == 1)
        {
            return points[0];
        }
        if(worldPoints.Length ==2)
        {
          return  AnimMath.Lerp(worldPoints[0], worldPoints[1], p);
        }

        Vector3 result = Vector3.zero;
        float leftValue = 0; //this is how far we have walked down the line
        for(int i = 0; i < curveLengths.Length; i ++)
        {
            float rightValue = leftValue + curveLengths[i];
            float rightPercent = rightValue / splineLength;
            if(rightPercent >= p)
            {
                float leftPercent = leftValue / splineLength;
                float curvePercent = (p - leftPercent) / (rightPercent - leftPercent);
                Vector3 a = worldPoints[i];
                Vector3 b = worldPoints[i + 1];
                Vector3 c = worldPoints[i + 2];

                if (i > 0) a = AnimMath.Lerp(a, b, .5f);
                if (i < curveLengths.Length - 1) c = AnimMath.Lerp(b, c, .5f);
                 result = AnimMath.QuadraticBezier(a, b, c, curvePercent);
                break;
            }
            leftValue = rightValue;
            
        }


        return result;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.grey;
        for(int i = 1; i < worldPoints.Length; i++)
        {
            Vector3 p1 = worldPoints[i - 1];
            Vector3 p2 = worldPoints[i];
            Gizmos.DrawLine(p1, p2);
        }
        Gizmos.color = Color.white;
        DrawSpline();
    }

    void DrawSpline()
    {
        int numOfCurves = worldPoints.Length - 2;

        for (int i = 1; i <= numOfCurves; i++)
        {
            Vector3 a = worldPoints[i - 1];
            Vector3 b = worldPoints[i];
            Vector3 c = worldPoints[i + 1];

            if (i > 1) a = AnimMath.Lerp(a, b, .5f);
            if (i < numOfCurves) c = AnimMath.Lerp(b, c, .5f);

            DrawCurve(a, b, c);
        }
    }

    void DrawCurve(Vector3 a, Vector3 b, Vector3 c)
    {
        
        Vector3 pos1 = new Vector3();
        for(int i = 0; i <= RESOLUTION; i++)
        {
            float p = i / (float)RESOLUTION;
            Vector3 pos2 = AnimMath.QuadraticBezier(a, b, c, p);

           if(i > 0) Gizmos.DrawLine(pos1, pos2);
            pos1 = pos2;
        }
    }


    void CalcWorldPositions()
    {
            worldPoints = new Vector3[points.Length];
        //Take points in local space multipy by matrix and get them in world space
        for(int i = 0; i < points.Length; i ++)
        {

            worldPoints[i] = transform.TransformPoint(points[i]);

        }
    }

    void LengthOfSpline()
    {
        if(worldPoints.Length <= 1)
        {
            curveLengths = new float[0];
            splineLength = 0;
            return;
        }else if(worldPoints.Length == 2)
        {
            curveLengths = new float[0];
            splineLength = (worldPoints[0] - worldPoints[1]).magnitude;
        }

        int numOfCurves = worldPoints.Length - 2;
        curveLengths = new float[numOfCurves];
        splineLength = 0;

        for (int i = 1; i <= numOfCurves; i++)
        {
            Vector3 a = worldPoints[i - 1];
            Vector3 b = worldPoints[i];
            Vector3 c = worldPoints[i + 1];

            if (i > 1) a = AnimMath.Lerp(a, b, .5f);
            if (i < numOfCurves) c = AnimMath.Lerp(b, c, .5f);

           float length =  LengthOfCurve(a, b, c);
            curveLengths[i - 1] = length;//
            splineLength += length;
        }
    }

    /// <summary>
    /// Calculate the length of bezier curve from A to c
    /// 
    /// A is an anchor point
    /// B is a curve handle
    /// C is an anchor point
    /// </summary>
    /// <returns> returns the length of the curve</returns>
    float LengthOfCurve(Vector3 a, Vector3 b, Vector3 c)
    {
        float result = 0;
       
        Vector3 pos1 = new Vector3();
        for (int i = 0; i <= RESOLUTION; i++)
        {
            float p = i / (float)RESOLUTION;
            Vector3 pos2 = AnimMath.QuadraticBezier(a, b, c, p);

            if (i > 0)result += (pos2 - pos1).magnitude;
            
            pos1 = pos2;
        }
        return result;
    }
}

