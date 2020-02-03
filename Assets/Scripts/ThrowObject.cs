using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ThrowObject : MonoBehaviour
{
    LineRenderer lr;

    public float throwStrengh = 5f;
    public float throwAngle = 45f;

    [Header("Arc Calculation")]
    public int amountOfPoints = 5;
    float grav = 9.8f;
    float radianAngle;

    // Start is called before the first frame update
    void Awake()
    {
        grav = Mathf.Abs(Physics.gravity.y);
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        RenderArc();
    }

    void RenderArc()
    {
        lr.positionCount = amountOfPoints + 1;
        lr.SetPositions(CalculateArcArray());
    }

    Vector3[] CalculateArcArray()
    {
        Vector3[] arcPoints = new Vector3[amountOfPoints + 1];
        radianAngle = Mathf.Deg2Rad * throwAngle;
        float maxDistance = (throwStrengh * throwStrengh * Mathf.Sin(2 * radianAngle)) / grav;

        for(int i = 0; i <= amountOfPoints; i++)
        {
            float t = (float)i / (float)amountOfPoints;
            arcPoints[i] = CalculateArcPoint(t, maxDistance);
        }


        return arcPoints;
    }

    Vector3 CalculateArcPoint(float progress, float maxDistance)
    {
        float x = progress * maxDistance;
        float y = x * Mathf.Tan(radianAngle) - ((grav * x * x) / (2 * throwStrengh * throwStrengh * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle)));

        Vector3 arcPoint = new Vector3(0, y, x);
        arcPoint = transform.rotation * arcPoint;

        return transform.position + arcPoint;
    }
}
