using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This component is for the gravity object as a child of the planet body */
public class GravityController : MonoBehaviour
{
    ScoreDisplay counter;
    public float factor = 700;
    public float tolerance = 700; 
    public float waitingTime;
    private string targetTag;
    Rigidbody2D target;
    GameObject self;    // the planet body (its parent)
    float mass;
    public int state;   // 0: initial; 1: triggerEnter; 2: stable state
    float rotationTime;

    void Start() {
        mass = gameObject.GetComponent<Rigidbody2D>().mass;
        counter = GameObject.Find("count text").GetComponent<ScoreDisplay>();
        Init();
    }
    void OnTriggerStay2D(Collider2D collider){
        if(collider.gameObject.tag == targetTag && collider.gameObject != transform.parent.gameObject){ // Only attracts "Planet", excluding itself (parent)
            target = collider.gameObject.GetComponent<Rigidbody2D>();
            self = gameObject.transform.parent.gameObject;
            Vector3 distance = transform.position - collider.gameObject.transform.position;
            float mass = target.mass;
            rotationTime += Time.deltaTime;

            if (state != 2 && Vector2.Angle(distance, target.velocity) > 89 ) {
                /* Detect correct angle and speed by comparing k and factor */
                /* Oscillation Equilibrium: Gravitational force == Centrifugal force */
                /* GMm/(R^2) == mv^2/R => k = (v^2)*r == factor == GM */
                float k = target.velocity.magnitude * target.velocity.magnitude * distance.magnitude;
                if((k < factor + tolerance && k > factor - tolerance/2) || state == 1) { // correct angle and speed
                    /* Ready for judge */
                    state = 1; 
                    /* Adjust the velocity to reach equilibrium */
                    target.velocity = target.velocity.normalized * Mathf.Sqrt(factor/distance.magnitude);
                }
                /* if rotationTime > waitingTime, judge as success -> state = 2 */
                /* Change from physical oscillation to fixed motion, so that the moon can stick with earth when dragging */
                /* The below code is all for handling level 4, WTF!! */
                if(rotationTime > waitingTime) {  
                    if(state == 1) {  
                        /* Set angularVelocity of the target to match current velocity -> to be used in RotateAround() */
                        target.gameObject.transform.SetParent(self.transform);
                        target.bodyType = RigidbodyType2D.Kinematic;
                        Vector3 orthogonal = new Vector3(distance.y, -distance.x);
                        Vector3 velocity = Vector3.Project(target.velocity, orthogonal); 
                        // angular speed, θ/360 = velocity/(2*PI*r)
                        // direction of rotation (cw/ccw)
                        if(Vector3.Angle(velocity, orthogonal) < 90) { 
                            target.gameObject.GetComponent<PlanetController>().angularVelocity = velocity.magnitude * 180 / Mathf.PI / distance.magnitude;
                        }
                        else target.gameObject.GetComponent<PlanetController>().angularVelocity = -velocity.magnitude * 180 / Mathf.PI / distance.magnitude;
                        target.gameObject.GetComponent<PlanetController>().RotateAround(self);

                        /* No need to implement rotation by physics, so v = 0 */
                        target.velocity = Vector3.zero; 

                        /* Enable second launch */
                        if(self.tag == "Planet") {
                            targetTag = "Moon";
                            target.gameObject.tag = "Moon";
                            self.GetComponent<PlanetController>().Ready();
                        }
                        state = 2;
                    }
                    counter.Judge(target.gameObject, self);
                }
            }

            /* If success, make the target rotate without gravitational force (fixed motion) */
            if (state == 2) target.gameObject.GetComponent<PlanetController>().RotateAround(self);

            /* Haven't success, apply gravitational force */
            /* factor = GM; F = GMm/(r^2) = factor*mass/(r^2); direction = distance.normalized */
            else target.AddForce(distance.normalized * mass * factor / (distance.magnitude * distance.magnitude));
        }
    }

    /* Reset when the target enter the range again (another new try) */
    void OnTriggerEnter2D(Collider2D collider) {
        if(collider.gameObject.name != "sun" && collider.gameObject.name != "gravity") { // its gravity should not affect by the sun!
            targetTag = "Planet";
            if(collider.gameObject.tag == targetTag){
                Init(); // state = 0
            }
        }
    }

    void Init() {
        state = 0;
        rotationTime = 0f;
        targetTag = "Planet";
        Transform ray = gameObject.transform.parent.Find("ray");
        if(ray) ray.gameObject.SetActive(false); // make the earth stop glowing
    }
}
