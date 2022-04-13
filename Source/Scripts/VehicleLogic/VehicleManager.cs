using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VehicleManager : MonoBehaviour {
    
    public int numVehicles = 10;
    private RoadManager RoadManager;
    public GameObject vehiclePrefab;

    void Start() 
    {
        RoadManager = GameObject.Find("RoadManager").GetComponent<RoadManager>();
    }
    
    public void spawnVehicle() 
    {
        GameObject vehicle = Instantiate(vehiclePrefab);
        vehicle.GetComponent<Vehicle>().initCar();
    }
}