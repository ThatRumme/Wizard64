using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBeam : MonoBehaviour
{


    public LineRenderer lr;

    public GameObject targetObj;
    public Vector3 endPos;
    public float unitsPerPoint = 10;
    public float radius = 0.5f;

    float updateTimer = 0;
    public float timePerUpdate = 0.1f;

    public float lineWidth = 0.25f;

    Transform startTransform;

    public GameObject hitmarkerPrefab;
    GameObject hitmarker;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Setup(Transform startTransform, GameObject targetObj, Vector3 endPos, bool hitSomething, Vector3 faceNormal)
    {
        this.startTransform = startTransform;
        this.transform.position = startTransform.position;
        this.targetObj = targetObj;
        this.endPos = endPos;

        transform.LookAt(endPos);

        UpdatePoints();

        void TweenOnComplete()
        {
            Destroy(this.gameObject);
        }

        lr.material.DOColor(new Color(1, 1, 1, 0), 1).SetEase(Ease.InCubic).OnComplete(TweenOnComplete);

        if(hitSomething)
        {
            
            Quaternion rot = faceNormal != Vector3.zero ? Quaternion.Euler(faceNormal) : transform.rotation;
            hitmarker = Instantiate(hitmarkerPrefab, endPos, rot);

            if(faceNormal != Vector3.zero)
            {
                hitmarker.transform.LookAt(faceNormal + endPos);
            }

        }

    }

    private void Update()
    {
        if (startTransform == null) return;

        updateTimer += Time.deltaTime;
        if(updateTimer > timePerUpdate)
        {
            updateTimer -= timePerUpdate;
            UpdatePoints();
        }

        transform.position = startTransform.position;
        lr.SetPosition(0, startTransform.position);

        if(targetObj != null)
        {
            endPos = targetObj.transform.position;
            
        }
        lr.SetPosition(lr.positionCount - 1, endPos);
        transform.LookAt(endPos);

        if(hitmarker != null)
        {
            hitmarker.transform.position = endPos;
        }
    }

    private void UpdatePoints()
    {

        Vector3[] points = CalculatePoints(startTransform.position, endPos);

        lr.positionCount = points.Length;
        lr.SetPositions(points);
    }

    Vector3[] CalculatePoints(Vector3 startPos, Vector3 endPos)
    {
        Vector3 line = endPos - startPos;
        int pointCount = (int)Mathf.Floor(line.magnitude/unitsPerPoint) + 2;
        if (line.magnitude % unitsPerPoint < 4) pointCount--;

        pointCount = Mathf.Max(pointCount, 4);

        Vector3[] points = new Vector3[pointCount];

        points[0] = startPos;
        points[points.Length-1] = endPos;

        for(int i = 1; i < points.Length-1; i++)
        {

            float lengthBetweenPoints = pointCount == 4 ? i*(unitsPerPoint/3) : i * unitsPerPoint;

            Vector3 linePoint = startPos + line.normalized * lengthBetweenPoints;
            int angle = UnityEngine.Random.Range(0, 360);

            Vector3 dirToPointLine = Quaternion.AngleAxis(angle, transform.forward) * transform.right;
            Vector3 point = dirToPointLine.normalized * radius;
            points[i] = linePoint + point;
        }


        return points;
    }
}
