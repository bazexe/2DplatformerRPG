using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private float xPos;
    private GameObject cam;
    [SerializeField] private float parallaxEffect;
    // Start is called before the first frame update
    void Start()
    {

        cam = GameObject.Find("Main Camera");
        xPos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToMove = cam.transform.position.x * parallaxEffect;
        transform.position = new Vector3(xPos + distanceToMove, transform.position.y);
    }
}
