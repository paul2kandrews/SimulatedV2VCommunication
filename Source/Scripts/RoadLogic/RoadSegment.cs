using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

public class RoadSegment : MonoBehaviour
{
    private int id;
    private Transform position;
    private float spdLimit;
    private float travelTime;
    private float physicalLen;
    private List<float> transitTimes = new List<float>();
    private List<TransitInstance> activeTransits = new List<TransitInstance>();

    public List<GameObject> snapPoints;
    public List<RoadSegment> nexts;
    public List<RoadSegment> prevs;
    public List<Node> nodes;

    public int weight = 20;
    public int delta = 2;

    public int Id {
        get => id;
    }

    public List<RoadSegment> Nexts
    {
        get => nexts;
    }

    public float SpeedLimit
    {
        get =>  spdLimit;
    }

    public float PhysicalLength
    {
        get => physicalLen;
    }

    public void setId(int id)
    {
        this.id = id;
    }

    //set function to set speed limit and physical length
    public void setSpeedLimit(int speed)
    {
        this.spdLimit = speed;
    }

    public void setPhysicalLength(int length)
    {
        this.physicalLen = length;
    }

    private void SetSnapPoints()
    {
        //Transform parent = this.transform.parent;
        //GameObject gameObject = this.gameObject;
        //Transform transform = gameObject.transform;
        // snapPoints.Add(this.transform.Find("start").gameObject);
        // snapPoints.Add(this.transform.Find("end").gameObject);
    }

	private void Awake()
	{
        SetSnapPoints();
	}

	// start function for some reason
	void Start()
    {
        // removes times from the transit time list every 10 seconds, 10 seconds after the program starts
        InvokeRepeating("popTransitTime", 10, 10);
    }
 
    private void OnTriggerEnter(Collider car)
    {
        //GameObject carObj = car.parent.something
        //transit.carId = carObj.GetComponent<CarClassWhatever>;
        TransitInstance? transit = activeTransits.Where(entry => entry.carId == car.GetInstanceID()).FirstOrDefault();
        if (transit == null)
        {
            transit = new TransitInstance();
            transit.carId = car.GetInstanceID();
            transit.enterTime = DateTime.Now;
        }
        else
        {
            transit.exitTime = DateTime.Now;
            computeTransitTime(transit);
            activeTransits.Remove(transit);
        }
    }

    private void computeTransitTime(TransitInstance transit)
    {
        float totalTime = (float)(transit.exitTime - transit.enterTime).TotalSeconds;
        transitTimes.Add(totalTime);
    }

    // create a weighted average of the transit times recorded
    private void averageTransitTime()
    {
        float totalTime = 0;
        int totalWeight = 0;

        // for every transit time recorded total the time and weight
        foreach (float time in transitTimes)
        {
            totalTime += time * weight;
            totalWeight += weight;
            weight -= delta;
        }

        // take the total time with weight divided by the total weight to find the weighted average
        float avgTime = totalTime / totalWeight;
        travelTime = avgTime;
    }

    // remove transit time from the list
    void popTransitTime()
    {
        var instance = transitTimes.FirstOrDefault();
        if (instance != null)
            transitTimes.Remove(instance);
    }
}