using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetsSpawner : MonoBehaviour
{ 
    public AssetScriptable data;

    public GameObject[] piso;
    public GameObject[] pared;
    public GameObject[] techo;

    public Transform pivot;

    
    //
    //
    //
    public void SpawnRoof(int intA)
    {
        var temp = Instantiate(techo[intA-1]);
        temp.transform.position = pivot.transform.position + transform.up;
    }
    public void SpawnFloor(int intA)
    {
        var temp = Instantiate(piso[intA - 1]);
        temp.transform.position = pivot.transform.position -transform.up;
        
    }
    public void SpawnWallLeft(int intA)
    {
        var temp = Instantiate(pared[intA - 1]);
        temp.transform.position = pivot.transform.position - transform.right;
        temp.transform.rotation = Quaternion.Euler(0, 90, 0);
    }
    public void SpawnWallRight(int intA)
    {
        var temp = Instantiate(pared[intA-1]);
        temp.transform.position = pivot.transform.position + transform.right;
        temp.transform.rotation = Quaternion.Euler(0, -90, 0);
    }

    //
    //
    //
    public void SpawnWallBack(int intA)
    {
        var temp = Instantiate(pared[intA - 1]);
        temp.transform.position = pivot.transform.position + transform.forward;
        temp.transform.rotation = Quaternion.Euler(0, 180, 0);
    }
    public void SpawnWallLimitUp(int intA)
    {
        var temp = Instantiate(pared[intA - 1]);
        temp.transform.position = pivot.transform.position - transform.forward + (transform.up * 2);
        temp.transform.rotation = Quaternion.Euler(0, 180, 0);
    }
    public void SpawnWallLimitDown(int intA)
    {
        var temp = Instantiate(pared[intA - 1]);
        temp.transform.position = pivot.transform.position - transform.forward - (transform.up * 2);
        temp.transform.rotation = Quaternion.Euler(0, 180, 0);
    }
    public void SpawnWallLimitRight(int intA)
    {
        var temp = Instantiate(pared[intA - 1]);
        temp.transform.position = pivot.transform.position - transform.forward + (transform.right * 2);
        temp.transform.rotation = Quaternion.Euler(0, 180, 0);
    }
    public void SpawnWallLimitLeft(int intA)
    {
        var temp = Instantiate(pared[intA - 1]);
        temp.transform.position = pivot.transform.position - transform.forward - (transform.right * 2);
        temp.transform.rotation = Quaternion.Euler(0, 180, 0);
    }
    public void SpawnWallLimitUpLeft(int intA)
    {
        var temp = Instantiate(pared[intA - 1]);
        temp.transform.position = pivot.transform.position - transform.forward + (transform.up - transform.right) * 2;
        temp.transform.rotation = Quaternion.Euler(0, 180, 0);
    }
    public void SpawnWallLimitUpRight(int intA)
    {
        var temp = Instantiate(pared[intA - 1]);
        temp.transform.position = pivot.transform.position - transform.forward + (transform.up + transform.right) * 2;
        temp.transform.rotation = Quaternion.Euler(0, 180, 0);
    }
    public void SpawnWallLimitDownLeft(int intA)
    {
        var temp = Instantiate(pared[intA - 1]);
        temp.transform.position = pivot.transform.position - transform.forward + (-transform.up - transform.right) * 2;
        temp.transform.rotation = Quaternion.Euler(0, 180, 0);
    }
    public void SpawnWallLimitDownRight(int intA)
    {
        var temp = Instantiate(pared[intA - 1]);
        temp.transform.position = pivot.transform.position - transform.forward + (-transform.up + transform.right) * 2;
        temp.transform.rotation = Quaternion.Euler(0, 180, 0);
    }
    public void CreateLimitWalls(GameObject obj, int intA)
    {
        var pref = GameObject.Instantiate(pared[intA - 1]);
        pref.transform.position = new Vector3(transform.position.x + obj.transform.position.x * 2, transform.position.y + obj.transform.position.y * 2, transform.position.z - 1);
    }

    // 
    //
    //
    public void CreateBackWall(GameObject obj, int Z, int intA)
    {
        var pref = GameObject.Instantiate(pared[intA - 1]);
        pref.transform.position = new Vector3(transform.position.x + obj.transform.position.x * 2, transform.position.y + obj.transform.position.y * 2, transform.position.z + (Z*2)-1);
    }
    public void CreateFloor(GameObject obj, int intA)
    {
        var pref = GameObject.Instantiate(piso[intA - 1]);
        pref.transform.position = new Vector3(transform.position.x + obj.transform.position.x * 2, transform.position.y-1 , transform.position.z + obj.transform.position.z * 2);
    }
    public void CreateLeftWall(GameObject obj, int intA)
    {
        var pref = GameObject.Instantiate(pared[intA - 1]);
        pref.transform.position = new Vector3(transform.position.x - 1, transform.position.y + obj.transform.position.y *2, transform.position.z + obj.transform.position.z *2);
        pref.transform.rotation = Quaternion.Euler(0, 90, 0);
    }
    public void CreateRightWall(GameObject obj, int X,int intA)
    {
        var pref = GameObject.Instantiate(pared[intA - 1]);
        pref.transform.position = new Vector3(transform.position.x +(X*2) - 1, transform.position.y + obj.transform.position.y * 2, transform.position.z + obj.transform.position.z * 2);
        pref.transform.rotation = Quaternion.Euler(0, -90, 0);
    }
    public void CreateRoof(GameObject obj, int y, int intA)
    {
        var pref = GameObject.Instantiate(techo[intA - 1]);
        pref.transform.position = new Vector3(transform.position.x + obj.transform.position.x * 2, transform.position.y + (y*2) - 1, transform.position.z + obj.transform.position.z * 2);
    }
}
