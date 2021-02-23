using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


public class CameraScript : MonoBehaviour
{
    private Camera myCamera;
    public Transform[] cameraTargets = new Transform[2];
    public Transform[] spotlightTargets = new Transform[2];
    public PostProcessVolume volume;
    DepthOfField depthOfField;
    public PostProcessVolume volumeBlur;
    DepthOfField depthOfFieldBlur;
    float dofBlurOn = 3.71f;
    float dofBlurOff = 17.3f;
    float blurSpeed = 1f;


    public Transform cameraTarget;
    public Transform spotlightTarget;

    float spotlightIntensitySpeed = .5f;
    float spotlightTargetIntensity = 0;
    float blurTarget;
    float blurTargetMin = 31;
    float blurTargetMax = 70;
    

    private float cameraSpeed = .2f;
    private AlphabetaScript alphabet;
    private float lerpDuration = 2f;
    private float swapCounter;
    private GameObject spotlight;
    public Camera cameraDoll1;
    public Camera cameraDoll2;
    public Camera cameraMid;
    private Animator animDoll1;
    private Animator animDoll2;
    private bool isStandingDoll1;
    private bool isStandingDoll2;
    private bool isTalkingDoll1;
    private bool isTalkingDoll2;
    public Transform camTarget;
    public float distance = .50f;
    public float xSpeed = 120f;
    public float ySpeed = 120f;
    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;
    public float distanceMin = .1f;
    public float distanceMax = 1f;
    public float smoothTime = 2f;
    float rotationYAxis = 0f;
    float rotationXAxis = 0f;
    float velocityX = 0f;
    float velocityY = 0f;
    float doll1TargetSpeed;
    float doll2TargetSpeed;
    public bool isDialogueStarted;
    Vector3 cameraMidStartingPosition;
    float returnSpeed = 1f;
    public bool returnCamera;
    public Transform middleTarget;
    public Transform spotlightMiddleTarget;
    bool isReturnStarted;
    bool testRotation = true;
    // Start is called before the first frame update
    void Start()
    {
      //  middleTarget.position = cameraTarget.transform.position;
        cameraMidStartingPosition = cameraMid.transform.position;
        volumeBlur.profile.TryGetSettings(out depthOfFieldBlur);
        blurTarget = dofBlurOff;
        Vector3 angles = transform.eulerAngles;
        rotationXAxis = angles.y;
        rotationYAxis = angles.x;
        animDoll1 = GameObject.Find("Doll1").GetComponent<Animator>();
        animDoll2 = GameObject.Find("Doll2").GetComponent<Animator>();
        spotlight = GameObject.Find("Spotlight");
        alphabet = GameObject.Find("MainScript").GetComponent<AlphabetaScript>();
        animDoll1.speed = 0f;
        animDoll2.speed = 0f;
       // target.position = targets[0].position;
    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log("DOF " + depthOfFieldBlur.focusDistance.value);
        if (Input.GetKeyDown("1"))
        {
            StartDoll1();
        }
        if (Input.GetKeyDown("2"))
        {
            StartQuestions();

        }
        if (isTalkingDoll1)
        {
            animDoll1.SetBool("IsTalking", true);
        }
        if (Input.GetKeyDown("t"))
        {
            testRotation = false;
            ReturnCamera();
        }
  

        cameraMid.transform.LookAt(cameraTarget.position, Vector3.up);
        spotlight.transform.LookAt(spotlightTarget.position);
        spotlight.GetComponent<Light>().intensity = Mathf.Lerp(spotlight.GetComponent<Light>().intensity, spotlightTargetIntensity, spotlightIntensitySpeed * Time.deltaTime);
        depthOfFieldBlur.focusDistance.value = Mathf.Lerp(depthOfFieldBlur.focusDistance.value, blurTarget, blurSpeed * Time.deltaTime);

       /* if (testRotation)
        {
            cameraMid.transform.RotateAround(cameraTarget.position, Vector3.down, 1 * Time.deltaTime);
            animDoll1.speed = Mathf.Lerp(animDoll1.speed, doll1TargetSpeed, 3f * Time.deltaTime);
            animDoll2.speed = Mathf.Lerp(animDoll2.speed, doll2TargetSpeed, 3f * Time.deltaTime);
            this.transform.RotateAround(cameraTarget.position, Vector3.up, 5 * Time.deltaTime);
            this.transform.LookAt(cameraTarget.position);
        }*/
        if (isDialogueStarted)
        {
            
            this.transform.RotateAround(cameraTarget.position, Vector3.up, 5 * Time.deltaTime);
            this.transform.LookAt(cameraTarget.position);
            cameraMid.transform.RotateAround(cameraTarget.position, Vector3.down, 1 * Time.deltaTime);
            /* cameraMid.transform.LookAt(cameraTarget.position, Vector3.up);
             spotlight.transform.LookAt(spotlightTarget.position);*/
            animDoll1.speed = Mathf.Lerp(animDoll1.speed, doll1TargetSpeed, 3f * Time.deltaTime);
            animDoll2.speed = Mathf.Lerp(animDoll2.speed, doll2TargetSpeed, 3f * Time.deltaTime);
          //  cameraMid.transform.RotateAround(cameraTarget.position, Vector3.down, 1 * Time.deltaTime);
        }

        if (returnCamera)
        {
            cameraMid.transform.position = Vector3.Lerp(cameraMid.transform.position, cameraMidStartingPosition, returnSpeed * Time.deltaTime);
            animDoll1.speed = Mathf.Lerp(animDoll1.speed, doll1TargetSpeed, 2f * Time.deltaTime);
            animDoll2.speed = Mathf.Lerp(animDoll2.speed, doll2TargetSpeed, 2f * Time.deltaTime);
        }

    }


    IEnumerator SwapTargets(Transform object1, Transform target1)
    {
        float timeElapsed = 0;
        Vector3 objectInitialPosition = object1.position;
        Vector3 objectTargetPosition = target1.position;

        while (timeElapsed < lerpDuration)
        {
            float t = timeElapsed / lerpDuration;
            t = t * t * t * (t * (6f * t - 15f) + 10f);
            object1.position = Vector3.Lerp(objectInitialPosition, objectTargetPosition, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        object1.position = objectTargetPosition;
    }

    public void ReturnCamera()
    {
        isDialogueStarted = false;
        Debug.Log("here");
        StartCoroutine(SwapTargets(cameraTarget, middleTarget));
        StartCoroutine(SwapTargets(spotlightTarget, spotlightMiddleTarget));
       // spotlightTargetIntensity = 0;
        //isTalkingDoll1 = false;
        //isTalkingDoll2 = false;
        doll1TargetSpeed = -1;
        doll2TargetSpeed = -1;
        blurTarget = dofBlurOff;
        returnCamera = true;
        spotlightTargetIntensity = 0f;
        blurTarget = dofBlurOff;
    }


    public void StartQuestions()
    {
        spotlightTargetIntensity = 1f;
        isDialogueStarted = true;
        alphabet.questionCounter = 1;
        alphabet.questionDestinationRotation = alphabet.questionHandRotations[alphabet.questionCounter];
        Debug.Log("Toggle");
        alphabet.DisplayText(false);
        if (isTalkingDoll1)
        {
            isTalkingDoll1 = false;
            isTalkingDoll2 = true;
            doll1TargetSpeed = 0;
            doll2TargetSpeed = 1;
            StartCoroutine(SwapTargets(cameraTarget, cameraTargets[0]));
            StartCoroutine(SwapTargets(spotlightTarget, spotlightTargets[0]));
        }
        else
        {
            isTalkingDoll1 = true;
            isTalkingDoll2 = false;
            doll1TargetSpeed = 1;
            doll2TargetSpeed = 0;
            StartCoroutine(SwapTargets(cameraTarget, cameraTargets[1]));
            StartCoroutine(SwapTargets(spotlightTarget, spotlightTargets[1]));
        }
    }

    public void PrepareForInput() //turn off left character
    {
        Debug.Log("Prepare for Input");
        
        alphabet.underlineAlpha = .5f;
        isTalkingDoll1 = false;
        doll1TargetSpeed = 0;
        StartCoroutine(SwapTargets(cameraTarget, cameraTargets[0]));
        spotlightTargetIntensity = 0;
        blurTarget = dofBlurOn;
        StartCoroutine(SwapTargets(spotlightTarget, spotlightTargets[0]));
    }


    public void Answer() // turn on right character
    {
        Debug.Log("Answer");

        alphabet.underlineAlpha = 0;
        blurTarget = dofBlurOff;
        isTalkingDoll2 = true;
        doll2TargetSpeed = 1;
        spotlightTargetIntensity = 1;
        alphabet.movingToNextQuestion = true;
    }

    public void Question() //turn on left character, turn off right character
    {
        Debug.Log("Question");  
        alphabet.questionCounter++;
        alphabet.questionDestinationRotation = alphabet.questionHandRotations[alphabet.questionCounter];
        if (!isDialogueStarted) return;
        alphabet.movingToNextQuestion = false;
        isTalkingDoll1 = true;
        isTalkingDoll2 = false;
        doll1TargetSpeed = 1;
        doll2TargetSpeed = 0;
        StartCoroutine(SwapTargets(cameraTarget, cameraTargets[1]));
        StartCoroutine(SwapTargets(spotlightTarget, spotlightTargets[1]));
    }


    void StartDoll1()
    {
        isTalkingDoll1 = true;
        animDoll1.speed = 1;
    }
}
