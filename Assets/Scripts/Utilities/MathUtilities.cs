using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtilities 
{

    public static Vector3 TestIntersecLineWithPlane()
    {
        Vector3 lineP0 = new Vector3(0, 0, 0);
        Vector3 lineP1 = new Vector3(1, 1, 1);
        Vector3 planeP0 = new Vector3(2, 1, 6);
        Vector3 planeN = new Vector3(1, 0, 0);
        return (IntersecLineWithPlane(lineP0, lineP1, planeP0, planeN));
    }

    public static Vector3 IntersecLineWithPlane(Vector3 lineP0, Vector3 lineP1, Vector3 planeP0, Vector3 planeN)
    {

        Vector3 lineDir = lineP1 - lineP0; //direction?
        var dot =DotVector3(planeN, lineDir);
        if (Mathf.Abs(dot) > 0)
        {// si fuera 0 seria parelo
            Vector3 w = (lineP0 - planeP0);
            var fac = -DotVector3(planeN, w)/dot;
            var distnace = lineDir * fac;
            return lineP0 + distnace;
        }
            
        return Vector3.zero;
    }

    public static Vector3 MultiplyVector3(Vector3 planeN, Vector3 lineDir)
    {
        return new Vector3(planeN.x * lineDir.x, planeN.y * lineDir.y, planeN.z * lineDir.z);
    }

    public static float DotVector3(Vector3 planeN, Vector3 lineDir)
    {
        return (planeN.x * lineDir.x)+ (planeN.y * lineDir.y)+ (planeN.z * lineDir.z);
    }
}
