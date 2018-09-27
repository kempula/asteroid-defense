using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    public GameObject asteroid;
    private float asteroidSpawnRate = 5f;

    private Vector2 startPos, endPos, direction;

    public enum Position
    {
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

            //TODO: CHANGE THIS TO BE MORE SANE...
            Vector3 stageDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
            Vector3 velocity = Vector3.zero;

            switch(position) {
                case Position.LEFT:
                    velocity = Vector3.right;
                    Spawn(ConvertValueToNegative(stageDimensions.x), Random.Range(ConvertValueToNegative(stageDimensions.y), stageDimensions.y), velocity);
                    break;
                case Position.RIGHT:
                    velocity = Vector3.left;
                    Spawn(stageDimensions.x, Random.Range(ConvertValueToNegative(stageDimensions.y), stageDimensions.y), velocity);
                    break;
                case Position.TOP:
                    velocity = Vector3.down;
                    Spawn(Random.Range(ConvertValueToNegative(stageDimensions.x), stageDimensions.x), stageDimensions.y, velocity);
                    break;
                case Position.BOTTOM:
                    velocity = Vector3.up;
                    Spawn(Random.Range(ConvertValueToNegative(stageDimensions.x), stageDimensions.x), ConvertValueToNegative(stageDimensions.y), velocity);
                    break;
            }

            yield return new WaitForSeconds(asteroidSpawnRate);
        }
    }

    public void Spawn(float x, float y, Vector3 velocity) {
        GameObject go = (GameObject)Instantiate(asteroid, new Vector3(x, y, -1), Quaternion.identity);
        Asteroid a = go.GetComponent<Asteroid>();
        a.SetVelocity(velocity);
    }

    private float ConvertValueToNegative(float value) {
        return value * -1;
    }

    private void Update()
    {
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
