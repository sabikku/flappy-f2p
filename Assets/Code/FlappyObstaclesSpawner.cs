using UnityEngine;
using System.Collections.Generic;

public class FlappyObstaclesSpawner : MonoBehaviour {

	public GameObject obstaclePrefab;

	List<GameObject> spawnedObstacles = new List<GameObject>();

	void Spawn( float x, float y, float gapHeight ) {
		GameObject spawned = GameObject.Instantiate( obstaclePrefab );
        spawned.transform.parent = transform;
		spawned.transform.position = new Vector3( x, y, 0 );
		spawnedObstacles.Add( spawned );

        Transform bottomTransform = spawned.transform.FindChild( "Bottom" );
        Transform topTransform = spawned.transform.FindChild( "Top" );
        float bottomY = -gapHeight/2;
        float topY = +gapHeight/2;
        bottomTransform.localPosition = Vector3.up * bottomY;
        topTransform.localPosition = Vector3.up * topY;
    }

}
