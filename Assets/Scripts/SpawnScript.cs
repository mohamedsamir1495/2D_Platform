using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour {

    public GameObject[] obj;
    public float spawnMin = 1f;
    public float spawnMax = 2f;

	void Start () {
        Spawn();

    }
	
	void Spawn() {
        //Instantiate(obj[Random.Range(0, obj.Length)], transform.position, Quaternion.identity); 
        Get_From_Pool();
       Invoke("Spawn", Random.Range(spawnMin, spawnMax));
       

	}
   
    void Get_From_Pool()
    {
        foreach (GameObject i in obj)
            if (i.active == false)
            {
                i.transform.parent = null;
                i.transform.position = transform.position;
                i.active = true;
                return;
            }
    }
}
