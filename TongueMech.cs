using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongueMech : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public AnimationCurve curve;
    public float speed = 5f;
    public float maxDistance = 5f;
    private Animator lizardAnimator;

    private Vector3 targetPosition;
    private bool extending = false;
    private bool hasCaughtTarget = false;
    private bool retracting = false;
    private float distanceTraveled = 0f;
    public GameObject clickedCandyObject;//To assign a GameObject parameter given By clicked candy

    [SerializeField] GameObject lizardObject;
    public float returnThreshold = 0.5f; // set this value to the distance at which you want to trigger the Open_Mouth animation

    //These will be used to know if the tonge is almost in its initial position to play the Open mouth animation
    private float journeyLength;
    private float halfDistance;

    private void Start()
    {
        gameObject.SetActive(false);

        Animator lizardAnimator = lizardObject.GetComponent<Animator>();
    }

    public void LaunchTongue(Vector3 targetPosition, GameObject candyObject)
    {
        Animator lizardAnimator = lizardObject.GetComponent<Animator>();
        lizardAnimator.SetTrigger("shootingFace");
        clickedCandyObject = candyObject;// given Gameobject(clickedbutton) parameter

        if (!extending && !hasCaughtTarget && !retracting)
        {
            gameObject.SetActive(true);
            this.targetPosition = targetPosition;
           
            lineRenderer.enabled = true;
            //distanceTraveled = 0f;

           // initialPosition = transform.position;

            //calculates the distance between the candy and initial position and tales the halfof it that will be later used to triger the OpenMouth animation
            journeyLength = Vector3.Distance( targetPosition, transform.parent.position);
             halfDistance = journeyLength / 2f;
            // startTime = Time.time;
            //  distanceThreshold = journeyLength * 0.5f; // calculate distance threshold as 20% of journey length
            extending = true;
        }
    }

    public void PlayOpenMouthAnimation()
    {

        Animator lizardAnimator = lizardObject.GetComponent<Animator>();
        // lizardAnimator.SetTrigger("openingMouth");
        lizardAnimator.SetBool("isMouthOpen", true);
    }

    private void Update()
    {
        if (extending && !hasCaughtTarget && !retracting)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
          //  distanceTraveled += speed * Time.deltaTime;

            //used for playing OpenMouth
           // distCovered = (Time.time - startTime) * speed;
          //  float fracJourney = distCovered / journeyLength;

            if (transform.position == targetPosition /*|| distanceTraveled >= maxDistance*/)
            {
                hasCaughtTarget = true;
                extending = false;
                retracting = true;
                // startTime = Time.time;
                Debug.Log("Trigered");
            }
            else
            {
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, transform.parent.position);
            }
        }
        else if (!extending && hasCaughtTarget && retracting)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.parent.position, speed * Time.deltaTime);
            clickedCandyObject.transform.position = transform.position; //its moving the clicked candy with the tongue(Dragging effect)
            // distCovered = (Time.time - startTime) * speed;
            //float fracJourney = distCovered / journeyLength;

            // Calculate distance covered by the tongue so far
            // float distanceCovered = Vector3.Distance(transform.position, transform.parent.position);
            float distanceTraveled = Vector3.Distance(targetPosition, transform.position);
            // check if the tongue is close to the starting position and play the animation
          //  Vector3 currentPosition = transform.position;
           // float distanceRemaining = Vector3.Distance(currentPosition, initialPosition);

          

            if (transform.position == transform.parent.position)
            {
                retracting = false;
                hasCaughtTarget = false;
                lineRenderer.enabled = false;
                gameObject.SetActive(false);
                distanceTraveled = 0f; // Reset the distance traveled

                // Deactivate clicked candy
                clickedCandyObject.SetActive(false);

                Animator lizardAnimator = lizardObject.GetComponent<Animator>();
                lizardAnimator.SetBool("isMouthOpen", false);
                //  lizardAnimator.SetTrigger("idlingFrog");
              
            }
            else
            {
                float t = Vector3.Distance(transform.position, targetPosition) / maxDistance;
                float y = curve.Evaluate(t);
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, transform.parent.position + Vector3.up * y);
            }

            if (distanceTraveled > halfDistance/*distanceRemaining <= distanceThreshold*/)
            {
                PlayOpenMouthAnimation();
            }

            // Check if the tongue has covered at least 80% of the total distance back to the start position
           // float percentageComplete = distanceCovered / journeyLength;
          /*  if (distanceRemaining <= distanceThreshold)
             {
                PlayOpenMouthAnimation();
             } */
        }
    }



    /*
    public GameObject mouth;
    public AnimationCurve tongueExtensionCurve;
    public float tongueSpeed = 1.0f;
   // public GameObject target;
    public float tongueMaxLength = 3.0f;
    public float tongueRetractionSpeed = 1.0f;
    public LineRenderer tongueLineRenderer;

    private Vector3 targetPosition;
    private bool extending = false;
    private bool retracting = false;
    private float tongueLength = 0.0f;
    private bool hasCaughtTarget = false;
    private Vector3 originalTonguePosition;

    // Start is called before the first frame update
    void Start()
    {
        originalTonguePosition = transform.position;
        tongueLength = 0.1f;
        extending = false;
        retracting = false;
        hasCaughtTarget = false;
        gameObject.SetActive(false);
        tongueLineRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (extending && !hasCaughtTarget)
        {
            float curveValue = tongueExtensionCurve.Evaluate(tongueLength / tongueMaxLength);
            transform.position = Vector3.Lerp(mouth.transform.position, targetPosition, curveValue);
            tongueLength += Time.deltaTime * tongueSpeed;

            if (tongueLength > tongueMaxLength)
            {
                extending = false;
                hasCaughtTarget = true;
            }
        }
        else if (retracting && hasCaughtTarget)
        {
            float distanceToMouth = Vector3.Distance(transform.position, mouth.transform.position);
            float retractionSpeed = Mathf.Min(tongueRetractionSpeed, distanceToMouth);

            transform.position = Vector3.MoveTowards(transform.position, mouth.transform.position, retractionSpeed * Time.deltaTime);

            if (distanceToMouth < 0.01f)
            {
                retracting = false;
                gameObject.SetActive(false);
                hasCaughtTarget = false;
                tongueLength = 0.1f;
                tongueLineRenderer.enabled = false;
            }
        }

        if (extending || retracting)
        {
            tongueLineRenderer.enabled = true;
            tongueLineRenderer.SetPosition(0, mouth.transform.position);
            tongueLineRenderer.SetPosition(1, transform.position);
        }
        else
        {
            tongueLineRenderer.enabled = false;
        }
    }

    public void LaunchTongue(Vector3 targetPosition)
    {
        if (!extending && !hasCaughtTarget && !retracting)
        {
            this.targetPosition = targetPosition;
            extending = true;
            gameObject.SetActive(true);
            tongueMaxLength = Vector3.Distance(mouth.transform.position, targetPosition);
        }
    }

    public void RetractTongue()
    {
        if (hasCaughtTarget && !retracting)
        {
            retracting = true;
        }
    } */

    /*
    // The speed of the tongue.
    public float speed = 10f;

    // The length of the tongue.
    public float length = 3f;

    // The tongue sprite renderer.
    private SpriteRenderer spriteRenderer;

    // The target position of the tongue.
    private Vector2 targetPosition;

    // A flag indicating whether the tongue is currently extending.
    private bool isExtending;

    // A flag indicating whether the tongue is currently retracting.
    private bool isRetracting;

    // Start is called before the first frame update.
    private void Start()
    {
        // Get the sprite renderer component.
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Disable the tongue sprite renderer initially.
        spriteRenderer.enabled = false;
    }

    // Update is called once per frame.
    private void Update()
    {
        // If the tongue is currently extending, move it towards the target position.
        if (isExtending)
        {
            transform.position = Vector2.Lerp(transform.position, targetPosition, speed * Time.deltaTime);

            // If the tongue has reached the target position, start retracting it.
            if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            {
                isExtending = false;
                isRetracting = true;
            }
        }

        // If the tongue is currently retracting, move it back towards the character's mouth.
        if (isRetracting)
        {
            transform.position = Vector2.Lerp(transform.position, transform.parent.position, speed * Time.deltaTime);

            // If the tongue has reached the character's mouth, disable the sprite renderer and stop retracting.
            if (Vector2.Distance(transform.position, transform.parent.position) < 0.1f)
            {
                spriteRenderer.enabled = false;
                isRetracting = false;
            }
        }
    }

    // Extend the tongue towards a target position.
    public void ExtendTongue(Vector2 targetPosition)
    {
        // Set the target position.
        this.targetPosition = targetPosition;

        // Set the direction of the tongue.
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        transform.right = direction;

        // Set the length of the tongue.
        Vector2 scale = transform.localScale;
        scale.x = length;
        transform.localScale = scale;

        // Enable the tongue sprite renderer and start extending.
        spriteRenderer.enabled = true;
        isExtending = true;
    }

    // This function is called when a button is clicked.
    public void TongueTrigger(RectTransform buttonTransform)
    {
        // Get the position of the button in world space.
        Vector3 buttonPosition = buttonTransform.position;

        // Extend the tongue towards the button position.
        ExtendTongue(buttonPosition);
    }*/
}
