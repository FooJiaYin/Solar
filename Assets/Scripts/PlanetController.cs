using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PlanetController : MonoBehaviour, IPointerDownHandler//, IPointerUpHandler
{    
    public AudioController SEPlayer;
    ScoreDisplay counter;
    public int onClicked;
    // public bool hasChild;
    private Rigidbody2D rb;
    private SpringJoint2D spring;
    private Vector2 screenBounds;
    private Vector3 originalPos;
    private Vector3 mouseOffset;
    private Vector3 maxVelocity;
    public GameObject planetPrefab;
    public float angularVelocity;
    private Rigidbody2D pivot;
    private bool collided;

    // Start is called before the first frame update
    void Start()
    {
        counter = GameObject.Find("count text").GetComponent<ScoreDisplay>();
        onClicked = 0;
        collided = false;
        // hasChild = false;
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        rb = gameObject.GetComponent<Rigidbody2D>();
        spring = gameObject.GetComponent<SpringJoint2D>();
        pivot = spring.connectedBody = gameObject.transform.parent.GetComponent<Rigidbody2D>();
        originalPos = gameObject.transform.position;
        maxVelocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space")) {
            ResetPlanet();
        }

        /* Reset Object on Out of Screen Bounds */
        if (onClicked==2 && transform.position.x > screenBounds.x+5 || transform.position.x < -screenBounds.x-5 ||
        transform.position.y > screenBounds.y+5 || transform.position.y < -screenBounds.y-5) {
            ResetPlanet();
            SEPlayer.playSE(6);
        }

        /* Dragging Object */
        if(onClicked == 1) {
            if(Input.GetMouseButton(0)) {
                gameObject.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + mouseOffset;
            }
            if(Input.GetMouseButtonUp(0)) {
                onClicked = 2;
                collided = false;
                counter.Add(1);
                spring.enabled = true;
                SEPlayer.playSE(1);
            }
        }
        if(onClicked == 3){
            RotateAround(GameObject.FindGameObjectWithTag("Planet"));
        }
    }

    void FixedUpdate() {
        if(onClicked==2 && spring != null) {
            if(rb.velocity.sqrMagnitude < maxVelocity.sqrMagnitude) {
                Destroy(spring);
                rb.velocity = maxVelocity; 
                maxVelocity = new Vector3(0, 0, 0);
            }
            else {
                maxVelocity = rb.velocity;
            }
        }  
    }
    /* Detect click on Object */
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        SEPlayer.playSE(5);
        originalPos = gameObject.transform.position;
        mouseOffset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Transform gravity = gameObject.transform.Find("gravity");
        if(!gravity || gravity.gameObject.GetComponent<GravityController>().state == 2) onClicked = 1;
    }

    public void RotateAround(GameObject centerObject) {
        gameObject.transform.RotateAround(gameObject.transform.parent.transform.position, Vector3.forward, angularVelocity * Time.deltaTime);
        gameObject.transform.RotateAround(transform.position, Vector3.forward, -angularVelocity * Time.deltaTime);
    }

    public void ResetPlanet() {
        Transform gravity = gameObject.transform.Find("gravity");
        if(!gravity || gravity.gameObject.GetComponent<GravityController>().state != 2) {
            if(gravity) Debug.Log(gravity.gameObject.GetComponent<GravityController>().state);
            else Debug.Log("no gravity");
            Debug.Log(name + " out of bonunds" + transform.position);
            GameObject newPlanet = GameObject.Instantiate(planetPrefab);
            Destroy(this.gameObject);
            newPlanet.transform.position = originalPos;
            newPlanet.transform.SetParent(pivot.gameObject.transform);
            newPlanet.transform.localScale = planetPrefab.transform.localScale;
            newPlanet.GetComponent<PlanetController>().planetPrefab = planetPrefab;
            newPlanet.GetComponent<PlanetController>().SEPlayer = SEPlayer;
            newPlanet.GetComponent<SpringJoint2D>().connectedBody = pivot;
            newPlanet.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            if(SceneManager.GetActiveScene().name == "Level 2" || SceneManager.GetActiveScene().name == "Level 3") {
                newPlanet.GetComponent<PlanetController>().Ready();  
                Destroy(newPlanet.transform.Find("gravity").gameObject);
            }
            else if(gravity = gameObject.transform.parent.transform.Find("gravity")) {
                gravity.GetComponent<GravityController>().state = 0;
                PlanetController p = gameObject.transform.parent.GetComponent<PlanetController>();
                if(p) p.ResetPlanet();
            }
        } 
    }

    /* Enable launch */
    public void Ready() {
        Transform ray = gameObject.transform.Find("ray");
        if(ray) ray.gameObject.SetActive(true);
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        gameObject.GetComponent<CircleCollider2D>().isTrigger = false;
    }

    void OnCollisionEnter2D() {
        if(!collided) {
            SEPlayer.playSE(4);
            collided = true;
        }
    }
}
