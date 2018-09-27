using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    public GameObject asteroidGO;
    private float asteroidSpawnRate = 5f;

    private Vector2 startPos, endPos, direction;

    public enum Position {
        TOP,
        BOTTOM,
        LEFT,
        RIGHT,
    }

	// Use this for initialization
	void Start () {
        StartCoroutine(AsteroidSpawnRoutine());
	}

    IEnumerator AsteroidSpawnRoutine() {
        while(true) {

            Position position = (Position)Random.Range(0, 4);

            var lowerLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
            var upperRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
            var upperLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));
            var lowerRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0));

            Vector3 velocity = Vector3.zero;

            switch(position) {
                case Position.LEFT:
                    velocity = Vector3.right;
                    Spawn(lowerLeft.x, Random.Range(lowerLeft.y, upperLeft.y), velocity);
                    break;
                case Position.RIGHT:
                    velocity = Vector3.left;
                    Spawn(lowerRight.x, Random.Range(lowerRight.y, upperRight.y), velocity);
                    break;
                case Position.TOP:
                    velocity = Vector3.down;
                    Spawn(Random.Range(upperLeft.x, upperRight.x), upperLeft.y, velocity);
                    break;
                case Position.BOTTOM:
                    velocity = Vector3.up;
                    Spawn(Random.Range(lowerLeft.x, lowerRight.x), lowerLeft.y, velocity);
                    break;
            }

            yield return new WaitForSeconds(asteroidSpawnRate);
        }
    }

    public void Spawn(float x, float y, Vector3 velocity) {
        GameObject go = (GameObject)Instantiate(asteroidGO, new Vector3(x, y, -1), Quaternion.identity);
        Asteroid asteroid = go.GetComponent<Asteroid>();
        asteroid.SetVelocity(velocity);
        asteroid.OnAsteroidSpawned();
    }

    private float ConvertValueToNegative(float value) {
        return value * -1;
    }

    private void Update() {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            if(touch.phase == TouchPhase.Began) {
                startPos = touchPosition;
            }

            if(touch.phase == TouchPhase.Ended) {
                endPos = touchPosition;
                direction = startPos - endPos;
                direction.Normalize();
                Debug.Log(direction);
                Spawn(startPos.x, startPos.y, direction);
            }
        }
    }
}
