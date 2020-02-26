using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region easing notes from week1
//Robert Penner talks about easing functions
//sol.gfxile.net/interpolation talks about easing functions
//easings.net shows easing animations
//matthewlein.com/tools/easer talks about easing functions
//github darno query -easing/master/jquery should have code for easing functions
// y = 1-(1-x)^2
//of y = x*x
//y = x*x in a shader gets you a lot of contrast
//Gekko tutorial is something I should look into
//frame rate independant dampen using lerp is an article I should look at
//rory driscoll.com frame rate indepented exponential slide talks about math and stuff
#endregion

public class AnimMath 
{
    public static float Lerp(float min, float max, float p)
    {

        return (max - min) * p + min;
    }
    //const lerp = (a,b,p) =>(b-a)*p+a;//just a little bit of javascript
    

    public static Vector3 Lerp(Vector3 min, Vector3 max, float p)
    {
        float x = Lerp(min.x, max.x, p);
        float y = Lerp(min.y, max.y, p);
        float z = Lerp(min.z, max.z, p);
        return new Vector3(x, y, z);
        
        //This code is the exact same
        //return (max - min) * p + min;
    }

    public static float Map(float v, float min1, float max1, float min2, float max2)
    {
        float p = (v - min1) / (max1 - min1); //This is our percent
        return Lerp(min2, max2, p);
    }

    public static Quaternion Lerp(Quaternion min, Quaternion max, float p)
    {
        return Quaternion.Lerp(min, max, p);
    }

    //Quadratic Interpolation
    public static Vector3 QuadraticBezier(Vector3 a, Vector3 b, Vector3 c, float p)
    {
        Vector3 p1 = Lerp(a, b, p);
        Vector3 p2 = Lerp(b, c, p);

        return Lerp(p1, p2, p);
    }

    //Quadratic easing function
    public static float Smooth(float min, float max, float p)
    {
        //only difference between this and the lerp function is we are bending the percent value
        p = p * p * (3 - 2 * p);
        return (max - min) * p + min;
    }

    //Dampen bois are used for that there exponential slide 
    public static float Dampen(float current, float target, float percentAfter1Second)
    {
        return Lerp(current, target, 1 - Mathf.Pow(percentAfter1Second, Time.deltaTime)); //this is 1- percentage left raised to power of delta time
    }

    public static Vector3 Dampen(Vector3 current, Vector3 target, float percentAfter1Second)
    {
        float p = 1 - Mathf.Pow(percentAfter1Second, Time.deltaTime);
        return Lerp(current, target, p);
    }

    public static Quaternion Dampen(Quaternion current, Quaternion target, float percentAfter1Second)
    {
        float p = 1 - Mathf.Pow(percentAfter1Second, Time.deltaTime);
        return Lerp(current, target, p);
    }

}
