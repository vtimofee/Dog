 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using System.Linq;

public class AlphabetaScript : MonoBehaviour
{
	GameObject underlineCamera;
	Vector3 underlineCameraOnPosition;
	Vector3 underlineCameraOffPosition;
	Vector3 underlineCameraTargetPosition;
	float underlineCameraSpeed = 2;
	public Transform rotation0;
	public Transform rotation1;
	public Transform rotation2;
	public Transform rotation3;
	public Transform rotation0_fl;
	public Transform rotation1_fl;
	public Transform rotation2_fl;
	public Transform rotation3_fl;
	public GameObject tempPrefab1ForSwap;
	public GameObject tempPrefab2ForSwap;
	public GameObject tempPrefab3ForSwap;
	public GameObject tempPrefab4ForSwap;
	public GameObject sentenceParentPrefab;
	public GameObject underlinePrefab;
	public GameObject[] prefabAlphabet = new GameObject[40];
	private Quaternion[] rotations = new Quaternion[8];
	private int[] letterTopCount = new int[26];
	private int[] letterBottomCount = new int[26];
	private Button[] buttons_q1 = new Button[26];
	private Button[] buttons_q2 = new Button[26];
	private Button[] buttons_q3 = new Button[26];
	private Button[] buttons_q4 = new Button[26];
	private Button[] abstractButtons_q1 = new Button[26];
	private Button[] abstractButtons_q2 = new Button[26];
	private Button[] abstractButtons_q3 = new Button[26];
	private Button[] abstractButtons_q4 = new Button[26];
	private Vector3[] q1StartPositions = new Vector3[26];
	private Vector3[] q2StartPositions = new Vector3[26];
	private Vector3[] q3StartPositions = new Vector3[26];
	private Vector3[] q4StartPositions = new Vector3[26];
	private Sprite[] q1OriginalSprites = new Sprite[26];
	private Sprite[] q2OriginalSprites = new Sprite[26];
	private Sprite[] q3OriginalSprites = new Sprite[26];
	private Sprite[] q4OriginalSprites = new Sprite[26];
	private Vector3 originalMasterSelfPosition_q1;
	private Vector3 originalMasterSelfPosition_q2;
	private Vector3 originalMasterSelfPosition_q3;
	private Vector3 originalMasterSelfPosition_q4;
	private Vector3 originalSlaveSelfPosition_q1;
	private Vector3 originalSlaveSelfPosition_q2;
	private Vector3 originalSlaveSelfPosition_q3;
	private Vector3 originalSlaveSelfPosition_q4;
	private bool letterChange;
	private Button currentq1;
	private Button currentq2;
	private Button currentq3;
	private Button currentq4;
	private bool start;
	private int selector = -1;
	private string currentq1s;
	private string currentq2s;
	private string currentq3s;
	private string currentq4s;
	private bool isCoroutineRunning;
	private bool swapInProgress;
	private bool letterActive;
	private bool isSwapBottomCompleted;
	private bool swap;
	private int swapCutter = 1;
	private int swapThreshold;
	private Color targetBlackColor = new Color(0, 0, 0, 1);
	private Color swapperBlue = new Color(.27f, .545f, .749f, 1);
	//private Color dummyRed = new Color(1, .48f, .48f, 1);
	//private Color dummyBlue = new Color(.694f, .87f, 1f, 1);
	private Color dummyRed = Color.red;
	private Color dummyBlue = Color.blue;
	private int coCounter = 0;
	private int swapFrequency = 1;
	private int initialTopCount = 1;
	private int initialBottomCount = 1;
	private int letterToSwap1 = 1;
	private int letterToSwap2 = 15;
	private bool useGeneratedText = true;
	List<string> sentencesList = new List<string>();
	List<string> sentencesMasterList = new List<string>();
	List<GameObject> underlinesList = new List<GameObject>();
	private float letterWidth;
	private float letterHeight;
	private Vector3 startingSlaveTextPosition;
	private Vector3 startingUnderlinePosition;
	private Vector3 textPosition;
	private Vector3 underlinePosition;
	const float horizontalCushionBetweenTextLetters = -20;
	const float verticalCushionBetweenTextLetters = 50;
	private int horizontalCharacterLimit = 22;
	private int scrambleCounter;
	private bool isScrambleOn;
	private Vector3[] originalLocalPositions = new Vector3[4];
	private Color baseTextColor = Color.black;
	private List<GameObject> grabbedSentence = new List<GameObject>();
	List<GameObject> sentenceList = new List<GameObject>();
	List<GameObject> underlineList = new List<GameObject>();
	List<GameObject> sentenceObjects = new List<GameObject>();
	private List<Vector3> sentenceStartPositions = new List<Vector3>();
	private Vector3[] sentence1LetterStartPositions;
	private int[] sentenceLineNumberAmounts = new int[4];
	private List<GameObject> sentenceParentPrefabs = new List<GameObject>();
	public Canvas GeneratedText;
	private int sentenceDisplayCounter;
	public CameraScript cameraScript;
	private GameObject abcCanvas;
	private GameObject generatedTextCanvas;
	private string keyCodeString;
	private bool isEnterPressed;
	List<GameObject> slaveLettersList;
	GameObject[] slaveLettersIndex = new GameObject[26];
	List<Button> slaveButtons_q1;
	List<Button> slaveButtons_q2;
	List<Button> slaveButtons_q3;
	List<Button> slaveButtons_q4;
	Sprite[] q1OriginalSlaveSprites = new Sprite[26];
	Sprite[] q2OriginalSlaveSprites = new Sprite[26];
	Sprite[] q3OriginalSlaveSprites = new Sprite[26];
	Sprite[] q4OriginalSlaveSprites = new Sprite[26];
	private bool isTextGenerated;
	List<string> charactersInWrittenSentence = new List<string>();
	int writeCharacterCounter = 0;
	int numberOfSentences = 5;
	private System.Random _random = new System.Random();
	const string glyphs = "abcdefghijklmnopqrstuvwxyz ";
	string randomString;
	int charAmount;
	private bool dueToSpeedUp;
	public Transform clockHand;
	public Transform questionHand;
	public Transform clockHandScrambler1;
	public Transform clockHandScrambler2;
	public Transform clockHandAbstract;
	private Transform destinationRotationAbstract;
	private float bigHandTimer;
	public Transform questionDestinationRotation;
	private Transform destinationRotation;
	private Transform destinationScrambleRotation1;
	private Transform destinationScrambleRotation2;
	private float clockScrambleSpeed = 4f;
	private Transform[] clockHandRotations = new Transform[26];
	public Transform[] questionHandRotations = new Transform[10];
	public Image[] lines = new Image[26];
	Vector3 clockHandScaleNormal = new Vector3(1f, 1.1f, 1f);
	Vector3 clockHandScaleSmall1 = new Vector3(1f, 1f, 1f);
	Vector3 clockHandScaleSmall2 = new Vector3(1f, .8f, 1f);
	Vector3 clockHandScaleSmall3 = new Vector3(1f, .5f, 1f);
	Vector3 clockHandScale;
	private float clockHandSizeSpeed = 2;
	public bool movingToNextQuestion;
	string currentSentence;
	private int sentenceUpperLimit = 4;
	private int futureSentenceCounter;
	private int currentSentenceCounter;
	int sentenceLoopCounter = 0;
	int letterReplacementCount;
	private int pictureScrambleCount;
	private Vector3[] pictureOriginalLocations = new Vector3[9];
	private CameraFilterPack_Glow_Glow glow;
	public Camera clockCameraFocus;
	public Camera clockCameraBlur;
	private float targetIntensityClock = 0;
	private int[] abstractArray = new int[26];
	private float targetIntensityMax = .9f;
	private float targetIntensityMin = .1f;
	public Material clockMaterial;
	public Material clockMaterialLines;
	private Color clockTargetColor = new Color(1, 1, 1, .9f);
	private Color clockTransparentColor = new Color(1, 1, 1, .1f);
	private Color underlineTargetColor = new Color(1, 1, 1, 1f);
	private Color underlineTransparentColor = new Color(1, 1, 1, 0f);
	private int swapHandCounter;
	private float gameTimeScale = 1;
	private float timescaleTarget = 1;
	private float timescaleTargetSpeed = 4f;
	private float timescaleTimerCount = 1;
	public int questionCounter;
	private int questionCycleCounter = 1;

	private float finalTimescaleTimerCount = 2;

	private bool isTimescalePaused;
	private Color cameraBackgroundColor;
	private Color cameraBackgroundColorOn = new Color(.83f, .83f, .83f, 0);
	private Color cameraBackgroundColorOff = new Color(.08f, .08f, .08f, 0);
	public PostProcessVolume volumeClockBlur;
	DepthOfField depthOfFieldClockBlur;
	private float clockBlurTarget;
	private float clockBlurOn = 2.78f;
	private float clockBlurOff = 6.91f;
	private float targetBlackSpeed;
	private float targetBlackSpeedPause = 3f;
	private float targetBlackSpeedNormal = .3f;
	public float underlineAlpha;
	public GameObject underlineWord;
	public CanvasGroup underlineWordCanvas;
	public CanvasGroup underlineCanvas;
	private int underlineGeneratorMax = 13;
	public CanvasGroup radialCanvas1;
	public CanvasGroup radialCanvas2;
	public CanvasGroup radialCanvas3;
	List<Vector3> underlineWordPositions;
	public GameObject textCamera;
	public GameObject textCameraBlank;
	List<Vector3> textCameraPositions;
	public Image[] inputRadials = new Image[4];
	public Image[] inputRadials2 = new Image[4];
	public Image[] inputRadials3 = new Image[4];
	int radialSelector = 1;
	float radialDelay = 2;
	bool isRadialFilled;
	bool doRadialFill;
	List<Vector2> underlineWidths;
	List<Vector3> radialPositions;
	public GameObject radialObject1;
	public CanvasGroup pictureCanvasGroup;
	float pictureAlpha;
	public GameObject clockFace;
	Vector3 clockFaceTargetScale;
	Vector3 clockFaceScaleMin;
	Vector3 clockDivide = new Vector3(.5f, .5f, .5f);
	private bool isPictureScrambling = true;
	Color radialColorOn = new Color(1, 1, 1, 1);
	Color radialColorOff = new Color(1, 1, 1, 0f);
	Color radialTargetColor;
	bool isRadialBlinkerCalled;
	int radialCounter;
	public GameObject cursor;
	bool isUnderlineBlink;
	int underlineBlinkCounter;
	public bool isPaused;
	float durationOfInput = 8;
	bool isFinalTimeScalePaused;

	// important variables to play with
	private bool isDebugModeOn = false;
	private float resetLerpDuration = 2f;
	private float clockTime = .25f; // .4 is a good one..also 1/3/5
	private float lerpDuration = .75f; // 1 is a good one
	private float slowLerpDuration = 1.25f; // 3 is a good one
	private float scrambleLerpDuration = 3f;
	private float scrambleLerpDuration2 = 4f;
	private float punctuationDelay = 2;
	float pictureLerpSpeed = 2f;
	private int letterLimit = 25;  // 25 is max
	float endOfQuestionDelay = 2;
	private bool isSwapOn = true;

	float startClockTime = .4f;
	float startLerpDuration = 1f;
	float startSlowLerpDuration = 3f;
	float startScrambleLerpDuration = 3f;
	float startScrambleLerpDuration2 = 3f;
	bool isResetting;
	
	void Start()
	{
        for (int i = 1; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
        }

        if (isDebugModeOn)
		{
			endOfQuestionDelay = 1;
			durationOfInput = 1;
			radialDelay = 1f;
			timescaleTimerCount = .5f;
			punctuationDelay = .1f;
		}
		radialTargetColor = new Color(1, 1, 1, 1);
		clockFaceTargetScale = clockFace.transform.localScale;
		clockFaceScaleMin.x = clockFaceTargetScale.x * .95f;
		clockFaceScaleMin.y = clockFaceTargetScale.x * .95f;
		clockFaceScaleMin.z = clockFaceTargetScale.z * .95f;
		InvokeRepeating("UnderlineBlinker", 0, .5f);
		InvokeRepeating("CursorBlinker", 0, .5f);
		//InvokeRepeating("UnderlineInputBlinker", 0, .5f);
		volumeClockBlur.profile.TryGetSettings(out depthOfFieldClockBlur);
		underlineAlpha = 0;
		charAmount = Random.Range(0, 20);
		targetBlackSpeed = targetBlackSpeedNormal;
		for (int i = 0; i<charAmount; i++)
        {
			randomString += glyphs[Random.Range(0, glyphs.Length)];
        }
		randomString += " $";

		clockHandScale = clockHandScaleNormal;
		cameraBackgroundColor = cameraBackgroundColorOn;
		//ResetSlaveTextCount();
		InitializeValues();
		//ResetSlaveTextCount();
		StartPacers();
		//GenerateText();
		ConvertStrings();
		//GenerateUnderlines(24, 0,0,0);

		UpdateSlaveTextAmountAndColor();
		ScramblePicture();
		GenerateNextSentence();

		//ResetSlaveTextCount();
	}

	// Update is called once per frame
	void Update()
	{
		Debug.Log("questionDestinationRotations : " + questionDestinationRotation.transform.rotation);
			TurnColorsBlack();
		//Debug.Log("radialtargetcolor: " + radialTargetColor);
		if (isFinalTimeScalePaused)
		{
			Debug.Log("finalTimescaleTimerCount : " + finalTimescaleTimerCount);
			finalTimescaleTimerCount -= Time.unscaledDeltaTime;
			if (finalTimescaleTimerCount <= 1)
			{
				isFinalTimeScalePaused = false;
				finalTimescaleTimerCount = 10;
				ResetValues();
				Time.timeScale = 1;
				timescaleTarget = 1;
				Debug.Log("in 5 seconds");
				Invoke("Restart", 5);
			}

		}

		//clockHandCycleCounter.eulerAngles = new Vector3(0, 0, 0 * .5f);
		//clockHandScrambler1.transform.rotation = Quaternion.Lerp(clockHandScrambler1.transform.rotation, destinationRotation.rotation, 5f * Time.deltaTime);
		//clockHandScrambler2.transform.rotation = Quaternion.Lerp(clockHandScrambler2.transform.rotation, destinationRotation.rotation, 5f * Time.deltaTime);
		//clockHandAbstract.transform.rotation = Quaternion.Lerp(clockHandAbstract.transform.rotation, destinationRotationAbstract.rotation, 5f * Time.deltaTime);
		glow.Intensity = Mathf.Lerp(glow.Intensity, targetIntensityClock, 2 * Time.deltaTime);
		//clockMaterial.color = Color.Lerp(clockMaterial.color, clockTransparentColor, .4f * Time.deltaTime);
		//clockMaterialLines.color = Color.Lerp(clockMaterialLines.color, clockTransparentColor, .4f * Time.deltaTime);
		clockFace.transform.localScale = Vector3.Lerp(clockFace.transform.localScale, clockFaceScaleMin, .2f * Time.deltaTime);
		if (isResetting)
        {
			clockHand.transform.rotation = Quaternion.Lerp(clockHand.transform.rotation, destinationRotation.rotation, 2f * Time.unscaledDeltaTime);
			clockHandScrambler1.transform.rotation = Quaternion.Lerp(clockHandScrambler1.transform.rotation, destinationRotation.rotation, 2f * Time.unscaledDeltaTime);
			clockHandScrambler2.transform.rotation = Quaternion.Lerp(clockHandScrambler2.transform.rotation, destinationRotation.rotation, 2f * Time.unscaledDeltaTime);
			questionHand.transform.rotation = Quaternion.Lerp(questionHand.transform.rotation, destinationRotation.rotation, 2f * Time.unscaledDeltaTime);

		}

		if (isPaused)
        {
			Time.timeScale = Mathf.Lerp(Time.timeScale, timescaleTarget, 6 * Time.unscaledDeltaTime);
		}

		if (start)
		{
			if (isTimescalePaused)
            {
				AbstractLetterEffect();
				timescaleTimerCount -= Time.unscaledDeltaTime;
			//	Debug.Log("timescaleCounter : " + timescaleTimerCount + " unscaled : "+ Time.unscaledTime);
				if(timescaleTimerCount <= 0)
                {
					timescaleTarget = 1f;
					cameraBackgroundColor = cameraBackgroundColorOn;
					isTimescalePaused = false;
					timescaleTimerCount = 1;
					clockBlurTarget = clockBlurOn;
					targetBlackSpeed = targetBlackSpeedNormal;
					if (letterReplacementCount <= letterLimit) letterReplacementCount++;

				}
			}

			
			if (letterActive)
			{
				LetterEvolve(selector);
				letterActive = false;
				Swapper(letterToSwap2, letterToSwap1);
			}
			if (!isTimescalePaused)TurnSelectorRed(selector);

            if (doRadialFill && !isRadialFilled) 
			{
				if (radialSelector == 1) FillRadial(inputRadials);
				else if (radialSelector == 2) FillRadial(inputRadials2);
				else if (radialSelector == 3) FillRadial(inputRadials3);
			}
			Time.timeScale = Mathf.Lerp(Time.timeScale, timescaleTarget, timescaleTargetSpeed * Time.deltaTime);
			bigHandTimer += Time.deltaTime;
			clockHand.transform.rotation = Quaternion.Lerp(clockHand.transform.rotation, destinationRotation.rotation, 5f * Time.deltaTime);
			questionHand.transform.rotation = Quaternion.Lerp(questionHand.transform.rotation, questionDestinationRotation.rotation, 1f * Time.deltaTime);

			//clockHandAbstract.transform.rotation = Quaternion.Lerp(clockHandAbstract.transform.rotation, destinationRotationAbstract.rotation, 5f * Time.deltaTime);
			glow.Intensity = Mathf.Lerp(glow.Intensity, targetIntensityClock, 2 * Time.deltaTime);
			clockMaterial.color = Color.Lerp(clockMaterial.color, clockTransparentColor, .4f * Time.deltaTime);
			//clockMaterialLines.color = Color.Lerp(clockMaterialLines.color, clockTransparentColor, .4f * Time.deltaTime);
			//clockFace.transform.localScale = Vector3.Lerp(clockFace.transform.localScale, clockFaceScaleMin, .2f * Time.deltaTime);
			//clockCameraBlur.backgroundColor = Color.Lerp(clockCameraBlur.backgroundColor, cameraBackgroundColor, 3 * Time.unscaledDeltaTime);
			depthOfFieldClockBlur.focalLength.value = Mathf.Lerp(depthOfFieldClockBlur.focalLength.value, clockBlurTarget, 3 * Time.deltaTime);
			pictureCanvasGroup.alpha = Mathf.Lerp(pictureCanvasGroup.alpha, pictureAlpha, 3 * Time.deltaTime);
		
	
			if (isSwapOn && destinationScrambleRotation2 && destinationScrambleRotation1)
			{
				clockHandScrambler1.transform.rotation = Quaternion.Lerp(clockHandScrambler1.transform.rotation, destinationScrambleRotation1.rotation, clockScrambleSpeed * Time.deltaTime);
				clockHandScrambler2.transform.rotation = Quaternion.Lerp(clockHandScrambler2.transform.rotation, destinationScrambleRotation2.rotation, clockScrambleSpeed * Time.deltaTime);
			}
			clockHand.transform.localScale = Vector3.Lerp(clockHand.transform.localScale, clockHandScale, clockHandSizeSpeed * Time.deltaTime);
			//questionHand.eulerAngles = new Vector3(0, 0, -bigHandTimer * .5f);
			//underlineCamera.transform.position = Vector3.Lerp(underlineCamera.transform.position, underlineCameraTargetPosition, underlineCameraSpeed * Time.deltaTime);
		}

		if (Input.GetKeyDown("3"))
		{
			End();
		}

		if (Input.GetKeyDown("4"))
		{
			//StartLetters();
			
		}
		if (Input.GetKey("5"))
		{
			dueToSpeedUp = true;
		}
		if (Input.GetKeyDown("6"))
        {
			AbstractLetterTrigger();
		}
		if (Input.GetKey("7"))
		{
			timescaleTarget = 0f;
		}
		if (Input.GetKey("8"))
		{
			timescaleTarget = 1f;
		}
		if (Input.GetKey("9"))
		{
			Debug.Log("position : " + underlineWord.transform.position);
		}
		if (Input.GetKey("0"))
		{
			Debug.Log("position cursor: " + cursor.transform.position);
		}
		if (Input.GetKey("p"))
		{
			isPictureScrambling = false;
		}
		if (Input.GetKey("escape"))
		{
			Application.Quit();
		}


	}

	Vector3[] ShuffleArray(Vector3[] array)
    {
		for (int t = 0; t < array.Length; t++)
		{
			Vector3 tmp = array[t];
			int r = Random.Range(t, array.Length);
			array[t] = array[r];
			array[r] = tmp;
		}
		return array;
    }

	int[] ShuffleArrayInt(int[] array)
	{
		for (int t = 0; t < array.Length; t++)
		{
			int tmp = array[t];
			int r = Random.Range(t, array.Length);
			array[t] = array[r];
			array[r] = tmp;
		}
		return array;
	}

	IEnumerator LerpTop1(Button object1, Button object2, Quaternion target1Rotation, Quaternion target2Rotation)
	{
		float timeElapsed = 0;
		Quaternion initial1Rotation = object1.transform.rotation;
		Quaternion initial2Rotation = object2.transform.rotation;

		//if (initial1Rotation == target1Rotation) Debug.Log("SAME ALREADY");
		while (timeElapsed < lerpDuration)
		{
			float t = timeElapsed / lerpDuration;
			t = t * t * t * (t * (6f * t - 15f) + 10f);
			object1.transform.rotation = Quaternion.Lerp(initial1Rotation, target1Rotation, t);
			object2.transform.rotation = Quaternion.Lerp(initial2Rotation, target2Rotation, t);
			timeElapsed += Time.deltaTime;
			yield return null;
		}
		object1.transform.rotation = target1Rotation;
		object2.transform.rotation = target2Rotation;
	}

	IEnumerator LerpTop2(Button object1, Button object2, Quaternion target1Rotation, Quaternion target2Rotation)
	{
		float timeElapsed = 0;
		Quaternion initial1Rotation = object1.transform.rotation;
		Quaternion initial2Rotation = object2.transform.rotation;

		if (initial1Rotation == target1Rotation) Debug.Log("SAME ALREADY");
		while (timeElapsed < lerpDuration)
		{
			float t = timeElapsed / lerpDuration;
			t = t * t * t * (t * (6f * t - 15f) + 10f);
			object1.transform.rotation = Quaternion.Lerp(initial1Rotation, target1Rotation, t);
			object2.transform.rotation = Quaternion.Lerp(initial2Rotation, target2Rotation, t);
			timeElapsed += Time.deltaTime;
			yield return null;
		}
		object1.transform.rotation = target1Rotation;
		object2.transform.rotation = target2Rotation;
	}

	IEnumerator LerpTop3(Button object1, Button object2, Quaternion target1Rotation, Quaternion target2Rotation)
	{

		float timeElapsed = 0;
		Quaternion initial1Rotation = object1.transform.rotation;
		Quaternion initial2Rotation = object2.transform.rotation;

		//if (initial1Rotation == target1Rotation) Debug.Log("SAME ALREADY");
		while (timeElapsed < lerpDuration)
		{
			float t = timeElapsed / lerpDuration;
			t = t * t * t * (t * (6f * t - 15f) + 10f);
			object1.transform.rotation = Quaternion.Lerp(initial1Rotation, target1Rotation, t);
			object2.transform.rotation = Quaternion.Lerp(initial2Rotation, target2Rotation, t);
			timeElapsed += Time.deltaTime;
			yield return null;
		}
		object1.transform.rotation = target1Rotation;
		object2.transform.rotation = target2Rotation;
	}

	IEnumerator LerpBottom(Button object1, Button object2, Quaternion target1Rotation, Quaternion target2Rotation)
	{
		float timeElapsed = 0;
		Quaternion initial1Rotation = object1.transform.rotation;
		Quaternion initial2Rotation = object2.transform.rotation;
		while (timeElapsed < lerpDuration)
		{
			float t = timeElapsed / lerpDuration;
			t = t * t * t * (t * (6f * t - 15f) + 10f);
			object1.transform.rotation = Quaternion.Lerp(initial1Rotation, target1Rotation, t);
			object2.transform.rotation = Quaternion.Lerp(initial2Rotation, target2Rotation, t);
			timeElapsed += Time.deltaTime;
			yield return null;
		}
		object1.transform.rotation = target1Rotation;
		object2.transform.rotation = target2Rotation;
		isCoroutineRunning = false;
	}

	IEnumerator LerpTopSwap(Button object1, Button object2)
	{
		float timeElapsed = 0;
		Vector3 initial1Position = object1.transform.localPosition;
		Vector3 initial2Position = object2.transform.localPosition;
		Vector3 target1Position = object2.transform.localPosition;
		Vector3 target2Position = object1.transform.localPosition;
		while (timeElapsed < lerpDuration)
		{
			float t = timeElapsed / lerpDuration;
			t = t * t * t * (t * (6f * t - 15f) + 10f);
			object1.transform.localPosition = Vector3.Lerp(initial1Position, target1Position, t);
			object2.transform.localPosition = Vector3.Lerp(initial2Position, target2Position, t);
			timeElapsed += Time.deltaTime;
			yield return null;
		}
		object1.transform.localPosition = target1Position;
		object2.transform.localPosition = target2Position;
	}

	IEnumerator LerpBottomSwap(Button object1, Button object2)
	{
		float timeElapsed = 0;
		Vector3 initial1Position = object1.transform.localPosition;
		Vector3 initial2Position = object2.transform.localPosition;
		Vector3 target1Position = object2.transform.localPosition;
		Vector3 target2Position = object1.transform.localPosition;

		while (timeElapsed < lerpDuration)
		{
			float t = timeElapsed / lerpDuration;
			t = t * t * t * (t * (6f * t - 15f) + 10f);
			object1.transform.localPosition = Vector3.Lerp(initial1Position, target1Position, t);
			object2.transform.localPosition = Vector3.Lerp(initial2Position, target2Position, t);
			timeElapsed += Time.deltaTime;
			yield return null;
		}
		object1.transform.localPosition = target1Position;
		object2.transform.localPosition = target2Position;
	}

	IEnumerator LerpVerticalSwap(Button object1, Button object2, Button target1, Button target2, int swapCutter)
	{
		float timeElapsed = 0;
		Vector3 object1InitialPosition = object1.transform.position;
		Vector3 object1TargetPosition = target1.transform.position;
		Vector3 object2InitialPosition = object2.transform.position;
		Vector3 object2TargetPosition = target2.transform.position;
		Vector3 target1InitialPosition = target1.transform.position;
		Vector3 target1TargetPosition = object1.transform.position;
		Vector3 target2InitialPosition = target2.transform.position;
		Vector3 target2TargetPosition = object2.transform.position;
		Quaternion target1Rotation = target1.transform.rotation;
		Quaternion target2Rotation = target2.transform.rotation;
		Quaternion object1Rotation = object1.transform.rotation;
		Quaternion object2Rotation = object2.transform.rotation;
		Sprite object1Copy = object1.image.sprite;
		Sprite object2Copy = object2.image.sprite;
		Sprite target1Copy = target1.image.sprite;
		Sprite target2Copy = target2.image.sprite;
		object1.transform.position = object1TargetPosition;
		object2.transform.position = object2TargetPosition;
		target1.transform.position = target1TargetPosition;
		target2.transform.position = target2TargetPosition;
		object1.transform.rotation = target1Rotation;
		object2.transform.rotation = target2Rotation;
		target1.transform.rotation = object1Rotation;
		target2.transform.rotation = object2Rotation;
		object1.image.sprite = target1Copy;
		object2.image.sprite = target2Copy;
		target1.image.sprite = object1Copy;
		target2.image.sprite = object2Copy;

		while (timeElapsed < slowLerpDuration)
		{
			if (!isTimescalePaused)
            {
				if (swapCutter == 1)
				{
					object1.image.color = swapperBlue;
					object2.image.color = swapperBlue;
					target1.image.color = swapperBlue;
					target2.image.color = swapperBlue;
				}
				else
				{
					object1.image.color = Color.green;
					object2.image.color = Color.green;
					target1.image.color = Color.green;
					target2.image.color = Color.green;
				}
			}
	

			float t = timeElapsed / slowLerpDuration;
			t = t * t * t * (t * (6f * t - 15f) + 10f);
			object1.transform.position = Vector3.Lerp(object1TargetPosition, object1InitialPosition, t);
			object2.transform.position = Vector3.Lerp(object2TargetPosition, object2InitialPosition, t);
			target1.transform.position = Vector3.Lerp(target1TargetPosition, target1InitialPosition, t);
			target2.transform.position = Vector3.Lerp(target2TargetPosition, target2InitialPosition, t);
			timeElapsed += Time.deltaTime;
			yield return null;
		}
		object1.transform.position = object1InitialPosition;
		object2.transform.position = object2InitialPosition;
		target1.transform.position = target1InitialPosition;
		target2.transform.position = target2InitialPosition;
		isCoroutineRunning = false;
		swapInProgress = false;
	}

	IEnumerator ResetPositionsAndRotations()
	{
		float timeElapsed = 0;

		//yield return new WaitForSeconds(4);
		ResetSlaveText();

		Vector3[] q1CurrentPositions = new Vector3[26];
		Vector3[] q2CurrentPositions = new Vector3[26];
		Vector3[] q3CurrentPositions = new Vector3[26];
		Vector3[] q4CurrentPositions = new Vector3[26];


		for (int i = 0; i < buttons_q1.Length; i++)
		{
			for (int j = 0; j < buttons_q1.Length; j++)
			{
				if (buttons_q1[i].image.sprite == q1OriginalSprites[j])
				{
					q1CurrentPositions[i] = buttons_q1[j].transform.position;
				}
				if (buttons_q2[i].image.sprite == q2OriginalSprites[j])
				{
					q2CurrentPositions[i] = buttons_q2[j].transform.position;
				}
				if (buttons_q3[i].image.sprite == q3OriginalSprites[j])
				{
					q3CurrentPositions[i] = buttons_q3[j].transform.position;
				}
				if (buttons_q4[i].image.sprite == q4OriginalSprites[j])
				{
					q4CurrentPositions[i] = buttons_q4[j].transform.position;
				}
			}
		}
		Vector3[] q1InitialPositions = new Vector3[26];
		Vector3[] q2InitialPositions = new Vector3[26];
		Vector3[] q3InitialPositions = new Vector3[26];
		Vector3[] q4InitialPositions = new Vector3[26];
		for (int i = 0; i < buttons_q1.Length; i++)
		{
			q1InitialPositions[i] = buttons_q1[i].transform.position;
			q2InitialPositions[i] = buttons_q2[i].transform.position;
			q3InitialPositions[i] = buttons_q3[i].transform.position;
			q4InitialPositions[i] = buttons_q4[i].transform.position;
		}
		Debug.Log("Aq1 original : " + q1InitialPositions[0] + " Aq1 current " + q1CurrentPositions[0]);
		Quaternion[] q1CurrentRotations = new Quaternion[26];
		Quaternion[] q2CurrentRotations = new Quaternion[26];
		Quaternion[] q3CurrentRotations = new Quaternion[26];
		Quaternion[] q4CurrentRotations = new Quaternion[26];

		for (int i = 0; i < buttons_q1.Length; i++)
		{
			buttons_q1[i].transform.position = q1InitialPositions[i];
			buttons_q2[i].transform.position = q2InitialPositions[i];
			buttons_q3[i].transform.position = q3InitialPositions[i];
			buttons_q4[i].transform.position = q4InitialPositions[i];
			q1CurrentRotations[i] = buttons_q1[i].transform.rotation;
			q2CurrentRotations[i] = buttons_q2[i].transform.rotation;
			q3CurrentRotations[i] = buttons_q3[i].transform.rotation;
			q4CurrentRotations[i] = buttons_q4[i].transform.rotation;
			/*q1SpriteCopy[i] = buttons_q1[i].image.sprite;
			q2SpriteCopy[i] = buttons_q2[i].image.sprite;
			q3SpriteCopy[i] = buttons_q3[i].image.sprite;
			q4SpriteCopy[i] = buttons_q4[i].image.sprite;*/
			buttons_q1[i].image.sprite = q1OriginalSprites[i];
			buttons_q2[i].image.sprite = q2OriginalSprites[i];
			buttons_q3[i].image.sprite = q3OriginalSprites[i];
			buttons_q4[i].image.sprite = q4OriginalSprites[i];
		}

		while (timeElapsed < resetLerpDuration)
		{
			float t = timeElapsed / resetLerpDuration;
			t = t * t * t * (t * (6f * t - 15f) + 10f);
			for (int i = 0; i < buttons_q1.Length; i++)
			{
				buttons_q1[i].transform.position = Vector3.Lerp(q1CurrentPositions[i], q1StartPositions[i], t);
				buttons_q2[i].transform.position = Vector3.Lerp(q2CurrentPositions[i], q2StartPositions[i], t);
				buttons_q3[i].transform.position = Vector3.Lerp(q3CurrentPositions[i], q3StartPositions[i], t);
				buttons_q4[i].transform.position = Vector3.Lerp(q4CurrentPositions[i], q4StartPositions[i], t);
				buttons_q1[i].transform.rotation = Quaternion.Lerp(q1CurrentRotations[i], rotations[0], t);
				buttons_q2[i].transform.rotation = Quaternion.Lerp(q2CurrentRotations[i], rotations[0], t);
				buttons_q3[i].transform.rotation = Quaternion.Lerp(q3CurrentRotations[i], rotations[0], t);
				buttons_q4[i].transform.rotation = Quaternion.Lerp(q4CurrentRotations[i], rotations[0], t);
			}
			timeElapsed += Time.deltaTime;
			yield return null;
		}
	}

	public void ForcedEvolution(int topcount, int bottomcount)
	{
		if ((bottomcount == 5 && topcount == 2) || (bottomcount == 9 && topcount == 2)) //preventing the bottomcount case 5,9 from repeatedly swapping on the bottom
		{
			isSwapBottomCompleted = true;
		}
		else if ((bottomcount == 6) || (bottomcount == 1))
		{
			isSwapBottomCompleted = false; //resetting and preparing bottomcount case 5,9 to fire
		}

		switch (bottomcount)
		{
			case 1:
				isSwapBottomCompleted = false;
				StartCoroutine(LerpBottom(currentq3, currentq4, rotations[0], rotations[0]));
				SetSlaveTextQuadrants(currentq3s, currentq4s, rotations[0], rotations[0]);
				break;

			case 2:
				StartCoroutine(LerpBottom(currentq3, currentq4, rotations[1], rotations[2]));
				SetSlaveTextQuadrants(currentq3s, currentq4s, rotations[1], rotations[2]);
				break;

			case 3:
				StartCoroutine(LerpBottom(currentq3, currentq4, rotations[3], rotations[3]));
				SetSlaveTextQuadrants(currentq3s, currentq4s, rotations[3], rotations[3]);
				break;

			case 4:
				StartCoroutine(LerpBottom(currentq3, currentq4, rotations[2], rotations[1]));
				SetSlaveTextQuadrants(currentq3s, currentq4s, rotations[2], rotations[1]);
				break;

			case 5:
				if (!isSwapBottomCompleted)
				{
					StartCoroutine(LerpBottomSwap(currentq3, currentq4));
					SwapSlaveTextQuadrants(currentq3s, currentq4s);
				}
				break;

			case 6:
				StartCoroutine(LerpBottom(currentq3, currentq4, rotations[3], rotations[3]));
				SetSlaveTextQuadrants(currentq3s, currentq4s, rotations[3], rotations[3]);
				break;

			case 7:
				StartCoroutine(LerpBottom(currentq3, currentq4, rotations[1], rotations[2]));
				SetSlaveTextQuadrants(currentq3s, currentq4s, rotations[1], rotations[2]);
				break;

			case 8:
				StartCoroutine(LerpBottom(currentq3, currentq4, rotations[0], rotations[0]));
				SetSlaveTextQuadrants(currentq3s, currentq4s, rotations[0], rotations[0]);
				break;

			case 9:
				if (!isSwapBottomCompleted)
				{
					StartCoroutine(LerpBottomSwap(currentq4, currentq3));
					SwapSlaveTextQuadrants(currentq4s, currentq3s);
				}
				break;
		}

		switch (topcount)
		{
			case 1:
				LaunchCoroutineCounterTop(topcount, currentq1, currentq2, rotations[0], rotations[0]);
				SetSlaveTextQuadrants(currentq1s, currentq2s, rotations[0], rotations[0]);
				break;

			case 2:
				LaunchCoroutineCounterTop(topcount, currentq1, currentq2, rotations[1], rotations[2]);
				SetSlaveTextQuadrants(currentq1s, currentq2s, rotations[1], rotations[2]);
				break;

			case 3:
				LaunchCoroutineCounterTop(topcount, currentq1, currentq2, rotations[3], rotations[3]);
				SetSlaveTextQuadrants(currentq1s, currentq2s, rotations[3], rotations[3]);
				break;

			case 4:
				LaunchCoroutineCounterTop(topcount, currentq1, currentq2, rotations[2], rotations[1]);
				SetSlaveTextQuadrants(currentq1s, currentq2s, rotations[2], rotations[1]);
				break;

			case 5:
				StartCoroutine(LerpTopSwap(currentq1, currentq2));
				SwapSlaveTextQuadrants(currentq1s, currentq2s);
				break;

			case 6:
				LaunchCoroutineCounterTop(topcount, currentq1, currentq2, rotations[3], rotations[3]);
				SetSlaveTextQuadrants(currentq1s, currentq2s, rotations[3], rotations[3]);
				break;

			case 7:
				LaunchCoroutineCounterTop(topcount, currentq1, currentq2, rotations[1], rotations[2]);
				SetSlaveTextQuadrants(currentq1s, currentq2s, rotations[1], rotations[2]);
				break;

			case 8:
				LaunchCoroutineCounterTop(topcount, currentq1, currentq2, rotations[0], rotations[0]);
				SetSlaveTextQuadrants(currentq1s, currentq2s, rotations[0], rotations[0]);
				break;

			case 9:
				StartCoroutine(LerpTopSwap(currentq2, currentq1));
				SwapSlaveTextQuadrants(currentq2s, currentq1s);
				break;
		}
	}

	void LaunchCoroutineCounterTop(int bottomcount, Button firstQuadrant, Button secondQuadrant, Quaternion targetRotation1, Quaternion targetRotation2)
	{
		coCounter++;
		if (coCounter > 3) coCounter = 1;

		switch (coCounter)
		{
			case 1:
				StartCoroutine(LerpTop1(firstQuadrant, secondQuadrant, targetRotation1, targetRotation2));
				break;
			case 2:
				StartCoroutine(LerpTop2(firstQuadrant, secondQuadrant, targetRotation1, targetRotation2));
				break;
			case 3:
				StartCoroutine(LerpTop3(firstQuadrant, secondQuadrant, targetRotation1, targetRotation2));
				break;
		}
	}

	void Swapper(int letterToSwap2, int letterToSwap1)
	{
		if (!swap)
		{
			swap = true;
			SlaveTextSwapper(letterToSwap2, letterToSwap1, swapCutter);
			switch (swapCutter)
			{
				case 1:
					StartCoroutine(LerpVerticalSwap(buttons_q1[letterToSwap2], buttons_q2[letterToSwap2], buttons_q1[letterToSwap1], buttons_q2[letterToSwap1], swapCutter));
					break;
				case 2:
					StartCoroutine(LerpVerticalSwap(buttons_q3[letterToSwap2], buttons_q4[letterToSwap2], buttons_q3[letterToSwap1], buttons_q4[letterToSwap1], swapCutter));
					break;
				case 3:
					StartCoroutine(LerpVerticalSwap(buttons_q1[letterToSwap2], buttons_q3[letterToSwap2], buttons_q1[letterToSwap1], buttons_q3[letterToSwap1], swapCutter));
					break;
				case 4:
					StartCoroutine(LerpVerticalSwap(buttons_q2[letterToSwap2], buttons_q4[letterToSwap2], buttons_q2[letterToSwap1], buttons_q4[letterToSwap1], swapCutter));
					break;
			}
		}
	}

	void ConvertStrings()
	{
		int tempNum = Random.Range(1, 4);
		string Greeting;
		string Goodbye;
		if (tempNum == 1)
        {
			Greeting = "hello";
			Goodbye = "OK";
        }
		else if (tempNum == 2)
        {
			Greeting = "hey";
			Goodbye = "good";
        }
        else
        {
			Greeting = "Hi";
			Goodbye = "thanks";
        }
		Greeting = Greeting.Insert(0, "1[");
		Greeting += ". $";

		Goodbye = Goodbye.Insert(0, "9[");
		Goodbye += "...... Next. $";
		sentencesMasterList.Clear();
		sentencesMasterList.Add(Greeting);//0
        sentencesMasterList.Add("KKKKKKKKKKKKK $");//1
        sentencesMasterList.Add("1[KKKKKKKKKKKKK $"); //2

        sentencesMasterList.Add("2[Good. Now Repeat the following words: DOG IS GOD. $"); //3
        sentencesMasterList.Add("KKK KK KKK $");//4
        sentencesMasterList.Add("2[KKK KK KKK. $");//5

        sentencesMasterList.Add("3[I am down bc?...      1[ I Dont know        2[ They know          3[ I know             4[ Not saying  $");//6
        sentencesMasterList.Add("K $");//7
        sentencesMasterList.Add("3[ K $");//8

        sentencesMasterList.Add("4[Please repeat the following:           I WILL NOT PLAY GOD! $");//9
        sentencesMasterList.Add("K KKKK KKK KK         KK KKK $");//10
        sentencesMasterList.Add("4[ K KKK KKK KKKK        KKK! $");//11

        sentencesMasterList.Add("5[OK Now write whatever you feel:  $");//12
        sentencesMasterList.Add("KKKKKKKKKKKKK    KKKKKKKKKKKKK    KKKKKKKKKKKKK    KKKKKKKKKKKKK $");//13
        sentencesMasterList.Add("5[KKKKKKKKKKK    KKKKKKKKKKKKK    KKKKKKKKKKKKK    KKKKKKKKKKKKK $");//14

        sentencesMasterList.Add("6[Yes. True (T) False (F): I remember the last time I felt happy. I remember the last time I felt sad. I am looking forward to tomorrow. $");//15
        sentencesMasterList.Add("K K K $");//16
        sentencesMasterList.Add("6[K and K and K $");//17

        sentencesMasterList.Add("7[Good. Now Repeat the following words: DOG IS GOD. $"); //18
        sentencesMasterList.Add("KKK KK KKK $");//19
        sentencesMasterList.Add("7[KKK KK KKK. $");//20

        sentencesMasterList.Add("8[Now look at the picture, write what you see:  $");//21
        sentencesMasterList.Add("KKKKKKKKKKKKK $");//22
        sentencesMasterList.Add("8[KKKKKKKKKKKKK $");//23

        sentencesMasterList.Add(Goodbye);//24
    }

	void DeleteSentence()
	{
		if (isTextGenerated)
		{
			Debug.Log("deleting : current sentence counter : " + currentSentenceCounter);
			GameObject currentParentSentence;

			if (currentSentenceCounter == 0)
			{
				currentParentSentence = GameObject.Find("sentence" + currentSentenceCounter);
			}
			else
			{
				currentParentSentence = GameObject.Find("sentence" + (currentSentenceCounter - 1));
			}

			int children = currentParentSentence.transform.childCount;
			for (int i = 0; i < children; ++i)
			{
				GameObject tempChild = currentParentSentence.transform.GetChild(i).gameObject;
				Destroy(tempChild.gameObject);
				Destroy(currentParentSentence);
			}
		}
	}


	void GenerateNextSentence()
	{
		Debug.Log("here! " + "futuresencne counter " +  futureSentenceCounter + "sencenMasterlistcount : " + sentencesMasterList.Count + "isTextGenrated ? : " + isTextGenerated + " sentenceUpperLimit : " + sentenceUpperLimit);

		sentencesList.Clear();
		UpdateSlaveTextAmountAndColor(); //this is important here
		if (futureSentenceCounter >= sentencesMasterList.Count) //if generator hits the limit, reset
        {
			futureSentenceCounter = 0;
			sentenceLoopCounter++;
			sentenceParentPrefabs.Clear();
		}

		if (!isTextGenerated) //if first pass generate 3 sentences
		{
			Debug.Log("made it here");
			for (int i = 0; i < sentenceUpperLimit; i++)
			{
				currentSentence = sentencesMasterList[i];
				sentencesList.Add(currentSentence);
				ProcessSentenceAndSetToParent(sentencesList[i], i);
			}
			isTextGenerated = true;
			futureSentenceCounter = sentenceUpperLimit;
		}
		else //prevent trigger on first pass
		{
			currentSentence = sentencesMasterList[futureSentenceCounter];
            sentencesList.Add(currentSentence);
            if (!isPaused)ProcessSentenceAndSetToParent(sentencesList[0], futureSentenceCounter);
			futureSentenceCounter++;
		}
		UpdateSlaveTextAmountAndColor(); //also important here
	}

	void ReinitializeValues()
    {
		ClearRadials();
		pictureCanvasGroup.alpha = 0;
		bigHandTimer = 0;
		clockHandScrambler1.eulerAngles = new Vector3(0, 0, 0);
		clockHandScrambler2.eulerAngles = new Vector3(0, 0, 0);
		destinationRotationAbstract = clockHandRotations[0];
		for (int i = 0; i < letterTopCount.Length; i++)
		{
			letterTopCount[i] = initialTopCount;
			letterBottomCount[i] = initialBottomCount;
		}

		clockTime = startClockTime;
		lerpDuration = startLerpDuration;
		slowLerpDuration = startSlowLerpDuration;
		scrambleLerpDuration = startScrambleLerpDuration;
		scrambleLerpDuration2 = startScrambleLerpDuration2;
	}

	void InitializeValues()
	{
		ClearRadials();
		pictureCanvasGroup.alpha = 0;

		underlineWordPositions = new List<Vector3>();
		underlineWordPositions.Add(new Vector3(0, 0f, 0)); //off
		underlineWordPositions.Add(new Vector3(4025, -516.5f, -182)); // sentence 4
		underlineWordPositions.Add(new Vector3(4570, -585.5f, -143)); // sentence 9
		underlineWordPositions.Add(new Vector3(4765, -120f, -182)); // 4 lines

		textCameraPositions = new List<Vector3>();
        /*	textCameraPositions.Add(new Vector3(7669.6f, 54f, 1574)); // 1 line
            textCameraPositions.Add(new Vector3(7669.6f, -195f, 1574)); // 2 line
            textCameraPositions.Add(new Vector3(7669.6f, -495f, 1574)); // 3 line
            textCameraPositions.Add(new Vector3(7669.6f, -694f, 1574)); // 4 lines needs readjustment
            textCameraPositions.Add(new Vector3(7669.6f, -987f, 1574)); // 5 lines
            textCameraPositions.Add(new Vector3(7669.6f, -1670f, 1574)); // 7 lines*/

        textCameraPositions.Add(new Vector3(7197, 54f, 1574)); // 0 line
		textCameraPositions.Add(new Vector3(7197, 54f, 1574)); // 1 line
		textCameraPositions.Add(new Vector3(7505, 54f, 1574)); // 2 line
        textCameraPositions.Add(new Vector3(7736, -543f, 1574)); // 3 line
		textCameraPositions.Add(new Vector3(7736, -543f, 1574)); // 4 line
		textCameraPositions.Add(new Vector3(7344, 54f, 1574)); // 5 lines
        textCameraPositions.Add(new Vector3(7736, -1030f, 1574)); // 6 lines
		textCameraPositions.Add(new Vector3(7736, -1030f, 1574)); // 6 lines
		textCameraPositions.Add(new Vector3(6727, 54, 1574)); // 8 line
        textCameraPositions.Add(new Vector3(7736, -572, 1574)); // 9 line
		textCameraPositions.Add(new Vector3(7736, -572, 1574)); // 9 line
		textCameraPositions.Add(new Vector3(7669.6f, -195f, 1574)); // 11 lines
        textCameraPositions.Add(new Vector3(7669.6f, -196.9f, 1574)); // 12 lines
		textCameraPositions.Add(new Vector3(7669.6f, -231f, 1574)); // 12 lines
		textCameraPositions.Add(new Vector3(7394f, -751f, 1574)); // 14 line
        textCameraPositions.Add(new Vector3(7851f, -1584.5f, 1574)); // 15 line
		textCameraPositions.Add(new Vector3(7553f, 16f, 1574)); // 16 line
		textCameraPositions.Add(new Vector3(7553f, 16f, 1574)); // 17 line
		textCameraPositions.Add(new Vector3(7735f, -500f, 1574)); // 18 lines
		textCameraPositions.Add(new Vector3(7735f, -500f, 1574)); // 19 lines
		textCameraPositions.Add(new Vector3(7405.6f, 54f, 1574)); // 20 line
        textCameraPositions.Add(new Vector3(7669.6f, -470f, 1574)); // 21 lines needs readjustment
		textCameraPositions.Add(new Vector3(7669.6f, -470f, 1574)); // 22 lines needs readjustment
		textCameraPositions.Add(new Vector3(7669.6f, 54f, 1574)); // 23 lines
        textCameraPositions.Add(new Vector3(7489.6f, 54f, 1574)); // 24 lines
		textCameraPositions.Add(new Vector3(7197, 54f, 1574)); // 0 line



		underlineWidths = new List<Vector2>();
		underlineWidths.Add(new Vector2(1616.34f, 338)); // normal
		underlineWidths.Add(new Vector2(2795, 338)); // large

		radialPositions = new List<Vector3>();
		radialPositions.Add(new Vector3(12491, -2062.5f, 0));
		radialPositions.Add(new Vector3(12491, -2589.5f, 0));

		destinationScrambleRotation2 = clockHandRotations[letterToSwap2];
		for (int i = 0; i < abstractArray.Length; i++)
		{
			abstractArray[i] = i;
		}
		abstractArray = ShuffleArrayInt(abstractArray);
		for (int i = 0; i < abstractArray.Length; i++)
		{
		//	Debug.Log("shuffled : " + abstractArray[i]);
		}

		glow = clockCameraFocus.GetComponent<CameraFilterPack_Glow_Glow>();
		underlineCamera = GameObject.Find("UnderlineCamera");
		underlineCameraOnPosition = underlineCamera.transform.position;
		Vector3 tempPos = underlineCameraOnPosition;
		tempPos.x += 5000;
		underlineCameraOffPosition = tempPos;
		underlineCameraTargetPosition = underlineCameraOffPosition;
		//underlineCamera.transform.position = underlineCameraOffPosition;

		//Initialize Clock hands, Assign Clock Rotations
		bigHandTimer = 0;
		clockHandScrambler1.eulerAngles = new Vector3(0, 0, 0);
		clockHandScrambler2.eulerAngles = new Vector3(0, 0, 0);
		string clockHandRotationString = "clock";
		for (int i = 0; i< clockHandRotations.Length; i++)
        {
			clockHandRotations[i] = GameObject.Find(clockHandRotationString + i).transform;
		}

		string questionHandRotationString = "question";

		for (int i = 0; i < questionHandRotations.Length; i++)
		{
			questionHandRotations[i] = GameObject.Find(questionHandRotationString + i).transform;
		}

		destinationRotationAbstract = clockHandRotations[0];
		questionDestinationRotation = questionHandRotations[0];

		GameObject[] rotationObjects = GameObject.FindGameObjectsWithTag("rotations");
		rotationObjects = rotationObjects.OrderBy(i => i.name).ToArray();

		for (int i = 0; i < rotationObjects.Length; i++)
		{
			rotations[i] = rotationObjects[i].transform.rotation;
		}



		for (int i = 0; i < letterTopCount.Length; i++)
		{
			letterTopCount[i] = initialTopCount;
			letterBottomCount[i] = initialBottomCount;
		}
		GameObject[] masterLetters = GameObject.FindGameObjectsWithTag("masterLetter");
		masterLetters = masterLetters.OrderBy(i => i.name).ToArray();


		for (int i = 0; i < masterLetters.Length; i++)
		{
			buttons_q1[i] = masterLetters[i].transform.GetChild(0).GetComponent<Button>();
			buttons_q2[i] = masterLetters[i].transform.GetChild(1).GetComponent<Button>();
			buttons_q3[i] = masterLetters[i].transform.GetChild(2).GetComponent<Button>();
			buttons_q4[i] = masterLetters[i].transform.GetChild(3).GetComponent<Button>();
		}
		originalLocalPositions[0] = buttons_q1[0].transform.localPosition;
		originalLocalPositions[1] = buttons_q2[0].transform.localPosition;
		originalLocalPositions[2] = buttons_q3[0].transform.localPosition;
		originalLocalPositions[3] = buttons_q4[0].transform.localPosition;
		for (int i = 0; i < buttons_q1.Length; i++)
		{
			q1StartPositions[i] = buttons_q1[i].transform.position;
			q2StartPositions[i] = buttons_q2[i].transform.position;
			q3StartPositions[i] = buttons_q3[i].transform.position;
			q4StartPositions[i] = buttons_q4[i].transform.position;
			q1OriginalSprites[i] = buttons_q1[i].image.sprite;
			q2OriginalSprites[i] = buttons_q2[i].image.sprite;
			q3OriginalSprites[i] = buttons_q3[i].image.sprite;
			q4OriginalSprites[i] = buttons_q4[i].image.sprite;
			//Debug.Log("original position : " + i + "q1StartPositions[i]" + q1StartPositions[i]);
		}


		Vector3[] v = new Vector3[4];
		buttons_q1[0].GetComponent<RectTransform>().GetWorldCorners(v);
		letterWidth = (v[2].x - v[0].x);
		letterHeight = (v[2].y - v[0].y);
		startingSlaveTextPosition = GameObject.Find("Starting Point Object").transform.position;
		startingUnderlinePosition = GameObject.Find("Starting Point Underline").transform.position;

		originalMasterSelfPosition_q1 = buttons_q1[0].transform.localPosition;
		originalMasterSelfPosition_q2 = buttons_q2[0].transform.localPosition;
		originalMasterSelfPosition_q3 = buttons_q3[0].transform.localPosition;
		originalMasterSelfPosition_q4 = buttons_q4[0].transform.localPosition;

		textPosition = startingSlaveTextPosition;
		underlinePosition = startingUnderlinePosition;
		letterHeight *= 2;
		letterHeight += verticalCushionBetweenTextLetters;
		letterWidth *= 2.2f;
		letterWidth += horizontalCushionBetweenTextLetters;
		GameObject testText = GameObject.Find("ExampleText");
		//textPosition.x = startingSlaveTextPosition.x;
		if (testText)testText.SetActive(false);

		slaveLettersIndex = GameObject.FindGameObjectsWithTag("slaveLetter");
		slaveLettersIndex.OrderBy(i => i.name);
		Debug.Log("slaveleterindex 1  : " + slaveLettersIndex[0]);

		slaveLettersList = new List<GameObject>(GameObject.FindGameObjectsWithTag("slaveLetter"));
		Debug.Log("slaveletterlist : count" + slaveLettersList.Count);
		slaveButtons_q1 = new List<Button>();
		slaveButtons_q2 = new List<Button>();
		slaveButtons_q3 = new List<Button>();
		slaveButtons_q4 = new List<Button>();

		for (int i = 0; i < slaveLettersList.Count; i++)
		{
			if (slaveLettersList[i].transform.childCount > 1) //Exclude punctuation
			{
				slaveButtons_q1.Add(slaveLettersList[i].transform.GetChild(0).GetComponent<Button>());
				slaveButtons_q2.Add(slaveLettersList[i].transform.GetChild(1).GetComponent<Button>());
				slaveButtons_q3.Add(slaveLettersList[i].transform.GetChild(2).GetComponent<Button>());
				slaveButtons_q4.Add(slaveLettersList[i].transform.GetChild(3).GetComponent<Button>());
			}
		}
		for (int i = 0; i < slaveButtons_q1.Count; i++)
        {
			q1OriginalSlaveSprites[i] = slaveButtons_q1[i].image.sprite;
			q2OriginalSlaveSprites[i] = slaveButtons_q2[i].image.sprite;
			q3OriginalSlaveSprites[i] = slaveButtons_q3[i].image.sprite;
			q4OriginalSlaveSprites[i] = slaveButtons_q4[i].image.sprite;
		}
	}

	public void DisplayText(bool isInput)
    {
        switch (currentSentenceCounter)
		{ 
            case 0:
				underlineWord.transform.position = underlineWordPositions[0];
				textCameraBlank.transform.localPosition = textCameraPositions[0];
				textCamera.transform.localPosition = textCameraPositions[0];
				break;
			case 1:
				var tempPos = cursor.transform.position;
				tempPos.y = -2039;
				cursor.transform.position = tempPos;
				GenerateUnderlines(13, 0, 0, 0, 0);
				break;

			case 2:
				break;


			case 3:
				underlineWord.transform.position = underlineWordPositions[1];
				textCameraBlank.transform.localPosition = textCameraPositions[3];
				textCamera.transform.localPosition = textCameraPositions[3]; // move camera to 3 line position
                // dueToSpeedUp = true;
              AbstractLetterTrigger();
                break;

			case 4:
				GenerateUnderlines(11, 3, 6, 10, 11);
				underlineWord.transform.position = underlineWordPositions[0];
				break;

			case 5:
                textCamera.transform.localPosition = textCameraPositions[0];
				textCameraBlank.transform.localPosition = textCameraPositions[0];
				underlineWord.transform.position = underlineWordPositions[0];
               // dueToSpeedUp = true;
                radialSelector = 1;
                break;
			case 6:
	
				textCamera.transform.localPosition = textCameraPositions[4];
				textCameraBlank.transform.localPosition = textCameraPositions[4];

				//dueToSpeedUp = true;
				//AbstractLetterTrigger();
				break;

			case 7:
				GenerateUnderlines(1, 0, 0, 0, 0);
				//dueToSpeedUp = true;
				break;

			case 8:
				textCamera.transform.localPosition = textCameraPositions[0];
				textCameraBlank.transform.localPosition = textCameraPositions[0];
				AbstractLetterTrigger();

				//dueToSpeedUp = true;
				break;

			case 9:
				textCamera.transform.localPosition = textCameraPositions[3];
				textCameraBlank.transform.localPosition = textCameraPositions[3];
				underlineWord.transform.position = underlineWordPositions[2];
				underlineWord.GetComponent<RectTransform>().sizeDelta = underlineWidths[1]; //sentence 9
				//dueToSpeedUp = true;
				AbstractLetterTrigger();
				break;

			case 10:
				radialSelector = 2;
				GenerateUnderlines(19, 1, 6, 10, 15);
				underlineWord.transform.position = underlineWordPositions[0];
				//dueToSpeedUp = true;
				break;

			case 11:
				radialSelector = 1;
				textCamera.transform.localPosition = textCameraPositions[1];
				textCameraBlank.transform.localPosition = textCameraPositions[1];
				underlineWord.transform.position = underlineWordPositions[0];
				startingUnderlinePosition.y -= 527;
				//dueToSpeedUp = true;
				break;

			case 12:
				textCameraBlank.transform.localPosition = textCameraPositions[2];
				AbstractLetterTrigger();
				textCamera.transform.localPosition = textCameraPositions[2]; //move camera to 1 line position
				var tempPos1 = cursor.transform.position;
				tempPos1.y = -1585;
				cursor.transform.position = tempPos1;
				//dueToSpeedUp = true;
				break;

			case 13:
				startingUnderlinePosition.y += 527;
				startingUnderlinePosition.y += 427;
				GenerateUnderlines(52, 0, 0, 0, 0);
				radialSelector = 3;
				radialObject1.transform.position = radialPositions[1];
				//dueToSpeedUp = true;
				break;

			case 14:
				tempPos1 = cursor.transform.position;
				tempPos1.y = -2039;
				cursor.transform.position = tempPos1;
				textCamera.transform.localPosition = textCameraPositions[3];
				textCameraBlank.transform.localPosition = textCameraPositions[3];
				break;


			case 15:
				underlineWord.transform.position = underlineWordPositions[0];
				textCameraBlank.transform.localPosition = textCameraPositions[5];
				textCamera.transform.localPosition = textCameraPositions[5]; // move camera to 3 line position
				//AbstractLetterTrigger();
				pictureAlpha = 0f;
				break;

			case 16:
				//startingUnderlinePosition.y -= 527;
				startingUnderlinePosition.y -= 427;
				GenerateUnderlines(5, 1, 3, 5, 6);
				radialSelector = 1;
				radialObject1.transform.position = radialPositions[0];
				/*radialSelector = 1;
				pictureAlpha = .7f;
				textCamera.transform.localPosition = textCameraPositions[0];
				//dueToSpeedUp = true;*/
				break;

			case 17:
				textCameraBlank.transform.localPosition = textCameraPositions[1];
				textCamera.transform.localPosition = textCameraPositions[1]; // move camera to 3 line position
				/*				pictureAlpha = 0f;
				*/
				break;

			case 18:
				ClearRadials();
				textCameraBlank.transform.localPosition = textCameraPositions[3];
				textCamera.transform.localPosition = textCameraPositions[3]; // move camera to 3 line position
				underlineWord.transform.position = underlineWordPositions[1];
				GenerateUnderlines(11, 3, 6, 10, 11);
				underlineWord.GetComponent<RectTransform>().sizeDelta = underlineWidths[0];
				// dueToSpeedUp = true;
				AbstractLetterTrigger();
				break;

			case 19:
				underlineWord.transform.position = underlineWordPositions[0];
				textCameraBlank.transform.localPosition = textCameraPositions[0];
				textCamera.transform.localPosition = textCameraPositions[0];
				//dueToSpeedUp = true;
				break;
			case 20:
				break;

			case 21:
				textCameraBlank.transform.localPosition = textCameraPositions[2];
				textCamera.transform.localPosition = textCameraPositions[2];
				radialObject1.transform.position = radialPositions[1];
				//dueToSpeedUp = true;
				AbstractLetterTrigger();
				pictureAlpha = 0f;
				break;

			case 22:
				var tempPos2 = cursor.transform.position;
				tempPos2.y = -2605;
				cursor.transform.position = tempPos2;

				startingUnderlinePosition.y -= 527;
				GenerateUnderlines(13, 0, 0, 0, 0);
				radialSelector = 1;
				pictureAlpha = .7f;
				textCameraBlank.transform.localPosition = textCameraPositions[0];
				textCamera.transform.localPosition = textCameraPositions[0];
				//dueToSpeedUp = true;
				break;

			case 23:
				pictureAlpha = 0f;
				radialObject1.transform.position = radialPositions[0];
				ClearRadials();
				GenerateUnderlines(0, 0, 0, 0, 0);
				startingUnderlinePosition.y += 527;
				break;
		}
		textCamera.transform.localPosition = textCameraPositions[currentSentenceCounter];
		textCameraBlank.transform.localPosition = textCameraPositions[currentSentenceCounter];

		if (currentSentenceCounter >= sentencesMasterList.Count) //end of the sentences
		{
			//END OF CYCLE
			End();

		}
		if (cameraScript.isDialogueStarted)
        {
			//dueToSpeedUp = true;
			StartCoroutine(WriteLetters(currentSentenceCounter, isInput));
			currentSentenceCounter++;
		}
	}

	IEnumerator WriteLetters(int sentenceCounter, bool isInput)
	{
		if (isPaused)yield break;
		

		Debug.Log("WriteLetters , sentenceCounter : " + sentenceCounter + " currentSentenceCounter : " + currentSentenceCounter + " questionCounter : " + questionCounter + " questionHanRotations : " + questionHandRotations[questionCounter].name);

		if (!isDebugModeOn) yield return new WaitForSeconds(1.5f);

		string sentenceString = "sentence" + sentenceCounter;
		Debug.Log("Write Letters sentenceString: " + sentenceString);
		GameObject sentenceParent = GameObject.Find(sentenceString);
		Transform firstChild = sentenceParent.transform.GetChild(0);
		float delay;
		Vector3 spawnPointDialogueBottom = GameObject.Find("Onscreen Point Object-bottom").transform.position;
	
		Vector3 spawnPointInput = startingUnderlinePosition;
		spawnPointInput.y += 20; //buffer above the underline
	
		//Vector3 spawnPointDialogueTop = GameObject.Find("Onscreen Point Object-top").transform.position;
		float distanceY;
		Vector3 distanceXYZ;
		Vector3 tempPos;
		distanceY = spawnPointDialogueBottom.y - firstChild.position.y;
		distanceXYZ = spawnPointInput - firstChild.position;
		distanceXYZ.y += 20;

		//float finalDelay = 0;
		if (!isInput) //Normal Dialogue (Question, Answer)
        {
			int i = 0;
			foreach (Transform child in sentenceParent.transform)
			{
				i++;
				if (isDebugModeOn) delay = 0;
				else delay = .1f;
				if (sentenceCounter == 2 || sentenceCounter == 5 || sentenceCounter == 8 || sentenceCounter == 11 | sentenceCounter == 14 || sentenceCounter == 17 || sentenceCounter == 21)
                {
					if (i > 2)
                    {
						child.gameObject.layer = 15;
                    }
                }
				tempPos = child.position;
				tempPos.y += distanceY;
				child.transform.position = tempPos;
				if (child.name == "[(Clone)" || child.name == "!(Clone)" || child.name == "?(Clone)") //if period, explamantion of question mark
				{
					delay = punctuationDelay;
				}
				Debug.Log("insinde 1 Write Letters" + "isInput" + isInput);

				yield return new WaitForSeconds(delay);
			}
			yield return new WaitForSeconds(endOfQuestionDelay);
			StartCoroutine(ReturnLetters(sentenceCounter, false));
		}
		else //Input
        {
			radialTargetColor = new Color(1, 1, 1, 1);
			ResetRadials(inputRadials);
			ResetRadials(inputRadials2);
			ResetRadials(inputRadials3);
			doRadialFill = true;
			isRadialFilled = false;
			yield return new WaitForSeconds(1);
			cursor.layer = 18;
			foreach (Transform child in sentenceParent.transform)
			{
				delay = .01f;
				tempPos = child.position;
				tempPos += distanceXYZ;
				child.transform.position = tempPos;
				yield return new WaitForSeconds(delay);
				Debug.Log("insinde 2 Write Letters" + "isInput" + isInput);
			}
			if (sentenceCounter == 22) isPictureScrambling = false;
			yield return new WaitForSeconds(durationOfInput);

			StartCoroutine(ReturnLetters(sentenceCounter, true));
		}
		cursor.layer = 12;
		DeleteSentence();
		if (!isPaused)GenerateNextSentence();
	}


	IEnumerator ReturnLetters(int sentenceCounter, bool isInput)
    {
		float delay;
		float distanceY;
		Vector3 distanceXYZ;
		Debug.Log("Return 1: sentenceCounter");

        string sentenceString = "sentence" + sentenceCounter;
        GameObject sentenceParent = GameObject.Find(sentenceString);
       // Debug.Log("Return Letters sentenceString: " + sentenceString);
        Transform firstChild = sentenceParent.transform.GetChild(0);
        Vector3 spawnPointDialogue = GameObject.Find("Starting Point Object").transform.position;
        Vector3 tempPos;
        distanceXYZ = firstChild.transform.position - spawnPointDialogue;
        distanceXYZ.y += (letterHeight * (sentenceCounter));
		Debug.Log("Return 2: sentenceCounter");

		foreach (Transform child in sentenceParent.transform)
        {
            int tempNum = Random.Range(0, 10);
            delay = 0;
            tempPos = child.transform.position;
            tempPos -= distanceXYZ;
            child.transform.position = tempPos;
        }
		yield return new WaitForSeconds(.1f);

		if (!isInput)
        {
			//bring in the underlines, etc
			if (movingToNextQuestion)
			{
				cameraScript.Question();
				//underlineCameraTargetPosition = underlineCameraOffPosition;
				DisplayText(false);
			}
			else
			{
				//cameraScript.Answer();
				cameraScript.PrepareForInput();
				//underlineCameraTargetPosition = underlineCameraOnPosition;
				DisplayText(true);
			}
		}
		else
        {
			//display next sentence
			cameraScript.Answer();
			//underlineCameraTargetPosition = underlineCameraOffPosition;
			DisplayText(false);
		}
	}

	void GenerateUnderlines(int underlineAmount, int break1, int break2, int break3, int break4)
	{
		ClearRadials();
		Vector3 incrementValueDown = new Vector3(0, letterHeight, 0);
		if (underlinesList.Count > 0) //delete the last instantiations
        {
			GameObject tempParentSentence;
			tempParentSentence = GameObject.Find("Underlines");
			int children = tempParentSentence.transform.childCount;
			for (int i = 0; i < children; ++i)
			{
				GameObject tempChild = tempParentSentence.transform.GetChild(i).gameObject;
				if (tempChild.tag != "Untagged")Destroy(tempChild.gameObject); //dont destroy the input box
			}
		}
		underlinesList.Clear(); // clear list
		underlinePosition = startingUnderlinePosition;
		Vector3 incrementValueRight = new Vector3(letterWidth, 0, 0);
		bool droppedNewLine = false;
		int j = 0;

		Debug.Log("underlineAmount / underlineGeneratorMax : " + Mathf.Floor(underlineAmount / underlineGeneratorMax));
		if (currentSentenceCounter == 13)
		{
			for (int i = 0; i < underlineAmount; i++)
			{
				if (i > (underlineGeneratorMax - 1))
				{
					underlineAmount -= underlineGeneratorMax;
					underlinePosition -= incrementValueDown;
					underlinePosition.x = startingUnderlinePosition.x;
					i = 0;
				}
				underlinesList.Add(Instantiate(underlinePrefab, underlinePosition, Quaternion.identity, GameObject.Find("Underlines").transform));
				underlinesList[j].name = "Underline" + j;
				underlinePosition += incrementValueRight;
				
				j++;
			}
		}
		else
		{
			for (int i = 0; i < underlineAmount; i++)
			{
				if ((i != break1 & i != break2 & i != break3 & i != break4) || (break1 == 0 && break2 == 0 && break3 == 0 && break4 == 0))
                {
					underlinesList.Add(Instantiate(underlinePrefab, underlinePosition, Quaternion.identity, GameObject.Find("Underlines").transform));
					//Debug.Log("generator" + i);
				}
				underlinePosition += incrementValueRight;
				if (i > (underlineGeneratorMax - 2) && !droppedNewLine)
				{
					droppedNewLine = true;
					underlinePosition -= incrementValueDown;
					underlinePosition.x = startingUnderlinePosition.x;
				}
			}
		}
	}

	void ProcessSentenceAndSetToParent(string sentence, int sentenceCounter)
	{
		Debug.Log("process - sentenceCounter: " + sentenceCounter + "sentence : " + sentence);

		List<GameObject> emptySentenceList = new List<GameObject>();
		List<string> charactersInSentence = new List<string>();
		List<GameObject> prefabAlphabetLetters = new List<GameObject>();
		int printCharacterCounter = 0;
		string convertedSentence = ConvertSentenctToSpecialScript(sentence);
		Vector3 incrementValueRight = new Vector3(letterWidth, 0, 0);
		Vector3 incrementValueDown = new Vector3(0, letterHeight, 0);
		string parentSentenceName = "sentence" + sentenceCounter;
		Vector3 largeScale1 = new Vector3(1f, 2f, 1f);
		//Debug.Log("process2 - sentenceCounter: " + sentenceCounter);
		textPosition.x = startingSlaveTextPosition.x;
		sentenceParentPrefabs.Add(Instantiate(sentenceParentPrefab, GameObject.Find("GeneratedText").transform));
		sentenceParentPrefabs[sentenceCounter].name = "sentence" + sentenceCounter; // this line
		Debug.Log("process3 - sentenceCounter: " + sentenceCounter);

		sentenceParentPrefabs[sentenceCounter].transform.SetParent(GameObject.Find("GeneratedText").transform);

		for (int i = 0; i < convertedSentence.Length; i++)
		{
			charactersInSentence.Add(convertedSentence[i].ToString());
		}
		if (useGeneratedText)
		{
			for (int i = 0; i < charactersInSentence.Count; i++)
			{
				foreach (GameObject j in prefabAlphabet)
				{
					if (charactersInSentence[i] == j.name) //If letters in array match the directory of Prefabs - print letter.
					{
                        if (charactersInSentence[i] == "%")
                        {
                            //Drop to a new line
                            textPosition -= incrementValueDown;
                            textPosition.x = startingSlaveTextPosition.x;
                            printCharacterCounter = 0;
                            Debug.Log("process3.2 - sentenceCounter: " + sentenceCounter + "  i: " + i + " j: " + j.name);
                        }
                        else
                        {
                            printCharacterCounter++;
                            prefabAlphabetLetters.Add(Instantiate(j, textPosition, Quaternion.identity, GameObject.Find(parentSentenceName).transform));
							for (int k = 0; k < slaveLettersIndex.Length; k++)
                            {
								if (j.name == slaveLettersIndex[k].name)
                                {
									//Debug.Log("match letter : " + prefabAlphabetLetters[i]);
									MatchSlaveLetter(prefabAlphabetLetters[i], slaveLettersIndex[k]);
                                }
                            }

							//prefabAlphabetLetters[i].layer = 15;
							//Debug.Log(" emptysentenceList.Count : " + emptySentenceList.Count);
							emptySentenceList.Add(prefabAlphabetLetters[i]);
                            textPosition += incrementValueRight;
                            Debug.Log("process3.4 - sentenceCounter: " + sentenceCounter);


                            if ((horizontalCharacterLimit - printCharacterCounter) <= 5 && charactersInSentence[i + 1] != "$") // Start checking for words larger than 15 characters long, 15 spaces away from end of line, $ is the end of sentence
                            {
                               // Debug.Log("process3.5 - sentenceCounter: " + sentenceCounter);

                                //measure the distance between this space and the next space
                                int nextSpaceCounter = 0;
                                string nextCharacter = charactersInSentence[i + 1];
                                do
                                {
                                    nextSpaceCounter++;
                                    nextCharacter = charactersInSentence[i + nextSpaceCounter];
                                } while (nextCharacter != "_");
                                if (nextSpaceCounter > (horizontalCharacterLimit - printCharacterCounter))
                                {
                                    //Drop to a new line
                                    textPosition -= incrementValueDown;
                                    textPosition.x = startingSlaveTextPosition.x;
                                    printCharacterCounter = 0;
                                }
                            }
                            if (printCharacterCounter >= horizontalCharacterLimit)
                            {
                                textPosition -= incrementValueDown;
                                textPosition.x = startingSlaveTextPosition.x;
                                printCharacterCounter = 0;
                            }

                        }
						//Debug.Log("process3.6 - sentenceCounter: " + sentenceCounter);


					}
				}
			}

		}
		//Debug.Log("process4 - sentenceCounter: " + sentenceCounter);

		Vector3 tempPosition = sentenceParentPrefabs[sentenceCounter].transform.position;
		tempPosition.x = sentenceParentPrefabs[sentenceCounter].transform.position.x;
		sentenceParentPrefabs[sentenceCounter].transform.position = tempPosition;
		textPosition -= incrementValueDown * 2;
		prefabAlphabetLetters.Clear();
        charactersInSentence.Clear();
		//Debug.Log("process4 - sentenceCounter: " + sentenceCounter);

	}

	void MatchSlaveLetter(GameObject spawnedLetter, GameObject targetLetter)
    {
		//Debug.Log("IN THE MATCHMAKER with : spawnedLetter : " + spawnedLetter.name + " targetLetter : " + targetLetter.name);
		GameObject tempSpawnedQ1 = spawnedLetter.transform.GetChild(0).gameObject;
		GameObject tempSpawnedQ2 = spawnedLetter.transform.GetChild(1).gameObject;
		GameObject tempSpawnedQ3 = spawnedLetter.transform.GetChild(2).gameObject;
		GameObject tempSpawnedQ4 = spawnedLetter.transform.GetChild(3).gameObject;
		GameObject tempTargetQ1 = targetLetter.transform.GetChild(0).gameObject;
		GameObject tempTargetQ2 = targetLetter.transform.GetChild(1).gameObject;
		GameObject tempTargetQ3 = targetLetter.transform.GetChild(2).gameObject;
		GameObject tempTargetQ4 = targetLetter.transform.GetChild(3).gameObject;
		tempSpawnedQ1.transform.rotation = tempTargetQ1.transform.rotation;
		tempSpawnedQ2.transform.rotation = tempTargetQ2.transform.rotation;
		tempSpawnedQ3.transform.rotation = tempTargetQ3.transform.rotation;
		tempSpawnedQ4.transform.rotation = tempTargetQ4.transform.rotation;
		tempSpawnedQ1.transform.localPosition = tempTargetQ1.transform.localPosition;
		tempSpawnedQ2.transform.localPosition = tempTargetQ2.transform.localPosition;
		tempSpawnedQ3.transform.localPosition = tempTargetQ3.transform.localPosition;
		tempSpawnedQ4.transform.localPosition = tempTargetQ4.transform.localPosition;
		tempSpawnedQ1.GetComponent<Button>().image.sprite = tempTargetQ1.GetComponent<Button>().image.sprite;
		tempSpawnedQ2.GetComponent<Button>().image.sprite = tempTargetQ2.GetComponent<Button>().image.sprite;
		tempSpawnedQ3.GetComponent<Button>().image.sprite = tempTargetQ3.GetComponent<Button>().image.sprite;
		tempSpawnedQ4.GetComponent<Button>().image.sprite = tempTargetQ4.GetComponent<Button>().image.sprite;
	}

	public void UpdateSlaveTextAmountAndColor()
	{
		//Turn all the punctuation a specific color
		GameObject[] slavePunctuations = GameObject.FindGameObjectsWithTag("EditorOnly");
		for (int i = 0; i < slavePunctuations.Length; i++)
		{
			slavePunctuations[i].GetComponent<Image>().color = baseTextColor;
		}

		GameObject[] slaveLetterArr = GameObject.FindGameObjectsWithTag("slaveLetter");
		slaveLettersList = slaveLetterArr.ToList();
		slaveLettersList.OrderBy(i => i.name);

		//slaveLettersList = new List<GameObject>(GameObject.FindGameObjectsWithTag("slaveLetter"));
		slaveButtons_q1 = new List<Button>();
		slaveButtons_q2 = new List<Button>();
		slaveButtons_q3 = new List<Button>();
		slaveButtons_q4 = new List<Button>();

		for (int i = 0; i < slaveLettersList.Count; i++)
		{
			if (slaveLettersList[i].transform.childCount > 1) //Exclude punctuation
			{
				slaveButtons_q1.Add(slaveLettersList[i].transform.GetChild(0).GetComponent<Button>());
				slaveButtons_q2.Add(slaveLettersList[i].transform.GetChild(1).GetComponent<Button>());
				slaveButtons_q3.Add(slaveLettersList[i].transform.GetChild(2).GetComponent<Button>());
				slaveButtons_q4.Add(slaveLettersList[i].transform.GetChild(3).GetComponent<Button>());
			}
		}

        originalSlaveSelfPosition_q1 = slaveButtons_q1[0].transform.localPosition;
        originalSlaveSelfPosition_q2 = slaveButtons_q2[0].transform.localPosition;
        originalSlaveSelfPosition_q3 = slaveButtons_q3[0].transform.localPosition;
        originalSlaveSelfPosition_q4 = slaveButtons_q4[0].transform.localPosition;

		/*GameObject[] allSlaveTextQuadrants1 = new GameObject[slaveButtons_q1.Count];
		GameObject[] allSlaveTextQuadrants2 = new GameObject[slaveButtons_q1.Count];
		allSlaveTextQuadrants1 = GameObject.FindGameObjectsWithTag(q1Tag);
		allSlaveTextQuadrants2 = GameObject.FindGameObjectsWithTag(q2Tag);
		foreach (GameObject i in allSlaveTextQuadrants1)
		{
			foreach (GameObject j in allSlaveTextQuadrants2)
			{
				i.transform.rotation = target1Rotation;
				i.GetComponent<Image>().color = dummyRed;
				j.transform.rotation = target2Rotation;
				j.GetComponent<Image>().color = dummyRed;
			}
		}*/
	}



	public void GetAllLettersInSentence(Transform parent, string tag, List<GameObject> list)
    {
		foreach (Transform child in parent)
        {
			if (child.gameObject.tag == tag)
            {
				list.Add(child.gameObject);
            }
			GetAllLettersInSentence(child, tag, list);
		}
    }

	string ConvertSentenctToSpecialScript(string dummySentence)
	{
		dummySentence = dummySentence.Replace(" ", "_");
		dummySentence = dummySentence.Replace("?", "]");
		dummySentence = dummySentence.Replace(".", "[");
		dummySentence = dummySentence.Replace(":", "+");
		dummySentence = dummySentence.Replace("/", "#");
		dummySentence = dummySentence.ToUpper();
		return dummySentence;
	}


	void Pacer()
	{
		//Debug.Log("clockSpeed : " + clockTime + " LerpDuration : " + lerpDuration);
		if (dueToSpeedUp)
        {
			SpeedUpPacer();
			return;
        }
		swapInProgress = false;
		if (!start) start = true;
		letterChange = true;
		letterActive = true;
		isCoroutineRunning = false;

		if (selector < letterLimit)
		{
			selector++;
		}
		else
		{
			selector = 0;
		}
	}

	void SwapPacer()
	{
		if (dueToSpeedUp) return;
		if (!isSwapOn) isSwapOn = true;
		swapThreshold++;
		if (swapThreshold < swapFrequency) return;

		else swapThreshold = 0;
	
		swapCutter = Random.Range(1, 3);
		//swapCutter = 1;
		letterToSwap1 = letterToSwap2;
		if (letterToSwap1 == selector || letterToSwap1 == (selector + 1) || letterToSwap1 == (selector + 2) )
        {
			return;
        }
		swapInProgress = true;
		swap = false;
		destinationScrambleRotation1 = clockHandRotations[letterToSwap1];
		do
		{
			letterToSwap2 = Random.Range(0, letterLimit + 1);

		} while (letterToSwap2 == letterToSwap1 || letterToSwap2 == selector || letterToSwap2 == (selector + 1) || letterToSwap2 == (selector + 2) ||
				letterToSwap2 == (selector + 3) || letterToSwap2 == (selector + 4)
				|| (letterToSwap2 == 0 && selector == 22) || (letterToSwap2 == 0 && selector == 23) || (letterToSwap2 == 0 && selector == 24) || (letterToSwap2 == 0 && selector == 25)
				|| (letterToSwap2 == 1 && selector == 23) || (letterToSwap2 == 1 && selector == 24) || (letterToSwap2 == 1 && selector == 25) || (letterToSwap2 == 2 && selector == 24)
				|| (letterToSwap2 == 2 && selector == 25));


		swapHandCounter++;
		if (swapHandCounter == 1)
        {
			Debug.Log("1");
			destinationScrambleRotation1 = clockHandRotations[letterToSwap1];
			destinationScrambleRotation2 = destinationScrambleRotation1;
			destinationScrambleRotation1 = clockHandRotations[letterToSwap2];
		}
        else
        {
			Debug.Log("2");
			destinationScrambleRotation2 = clockHandRotations[letterToSwap1];
			destinationScrambleRotation1 = destinationScrambleRotation2;
			destinationScrambleRotation2 = clockHandRotations[letterToSwap2];
			swapHandCounter = 0;
        }

		//turn based system for walking
	}

	void StartPacers()
	{
		InvokeRepeating("Pacer", 1, clockTime);
		if (isSwapOn) InvokeRepeating("SwapPacer", 1, slowLerpDuration);
		//InvokeRepeating("ScramblePacer", 1, scrambleLerpDuration - .5f);
	}

	void SpeedUpPacer()
    {

		clockTime /= 1.1f;
		slowLerpDuration /= 1.1f;
		lerpDuration /= 1.1f;
		targetBlackSpeed *= 1.1f;
		CancelInvoke("Pacer");
		InvokeRepeating("Pacer", 0, clockTime);
		if (isSwapOn)
		{
			CancelInvoke("SwapPacer");
			InvokeRepeating("SwapPacer", 1, slowLerpDuration);

		}
			dueToSpeedUp = false;


	}

	void LetterEvolve(int i)
	{
		var tempColor = clockMaterial.color;
		tempColor.a -= .05f;
		clockMaterial.color = tempColor;
		clockMaterialLines.color = tempColor;
		if (i == 8 || i == 14 || i == 20 || i == 1)
        {
			targetIntensityClock = targetIntensityMax;
			clockMaterial.color = clockTargetColor;
			clockMaterialLines.color = clockTargetColor;
			clockFace.transform.localScale = clockFaceTargetScale;
		}
		else
        {
			targetIntensityClock = targetIntensityMin;
		}
		destinationRotation = clockHandRotations[i];
		currentq1 = buttons_q1[i];
		currentq2 = buttons_q2[i];
		currentq3 = buttons_q3[i];
		currentq4 = buttons_q4[i];
		currentq1s = i + "_q1";
		currentq2s = i + "_q2";
		currentq3s = i + "_q3";
		currentq4s = i + "_q4";

		//lines[i].fillAmount = 1;

		if ( selector == 3 || selector == 10 || selector == 16 || selector == 22) // 3, 10, 16, 23
        {
			clockHandScale = clockHandScaleSmall1; //.9
        }
		else if (selector == 4 || selector == 9 || selector == 17 || selector == 21) //4, 9, 17, 22
        {
			clockHandScale = clockHandScaleSmall2; //.75
		}
		else if ((selector >= 5 && selector < 9) || (selector >= 18 && selector < 21)) //5, 6, 7, 8 + 18, 19, 20, 21
		{
			clockHandScale = clockHandScaleSmall3; //.5
		}
		else if ((selector >= 11 && selector < 16) || (selector >= 23 && selector < 3)) //11, 12, 13, 14, 15 + 24, 25, 0, 1, 2
		{
			clockHandScale = clockHandScaleNormal; //1
		}

		if (letterChange)
		{
			letterChange = false;
			letterTopCount[i]++;

			if (letterTopCount[i] == 9) //Last cycle of top, increment bottom and reset the top count at the end
			{
				letterBottomCount[i]++;
			}
			else if (letterTopCount[i] > 9)
			{
				letterTopCount[i] = 2;
			}


			if (letterBottomCount[i] > 9)//Last cycle of bottom, reset the bottom count, reset letter positions
			{
				letterBottomCount[i] = 2;
			}
			ResetPositions(letterTopCount[i], letterBottomCount[i]); //this should lock into place any quadrants left loose by failure of coroutines to finish
		}
		//Debug.Log("sentencedisplaycounter" + sentenceDisplayCounter + "bigHandTimer : " + bigHandTimer + "current source a1" + buttons_q1[0].image.sprite + "currentq : " + currentq1s + " 3 selector: " + selector + currentq1.transform.parent.name + " TopCount : " + letterTopCount[i] + " BottomCount : " + letterBottomCount[i] + " swapperthresh : " + swapThreshold + " letterToSwap : " + letterToSwap1 + " isSwapInProgress? " + swapInProgress);
		//Debug.Log("currentq : " + currentq1s + " 3 selector: " + selector + currentq1.transform.parent.name + " TopCount : " + letterTopCount[i] + " BottomCount : " + letterBottomCount[i] + " letterToSwap : " + letterToSwap1 + " isSwapInProgress? " + swapInProgress);

		if (!isCoroutineRunning) // this prevents the coroutines from being fired more than once per "pace"
		{
			ForcedEvolution(letterTopCount[i], letterBottomCount[i]);
			isCoroutineRunning = true;
		}
	}

	void ScramblePacer()
	{
		scrambleCounter++;
		if (scrambleCounter > 4) scrambleCounter = 1;

		//reassign variables incase there is an issue with positions due to swap
		Image[] tempButtons_q1 = new Image[1];
		Image[] tempButtons_q2 = new Image[1];
		Image[] tempButtons_q3 = new Image[1];
		Image[] tempButtons_q4 = new Image[1];

		GameObject[] pictures = GameObject.FindGameObjectsWithTag("Respawn");
		for (int i = 0; i < pictures.Length; i++)
		{
			tempButtons_q1[i] = pictures[i].transform.GetChild(0).GetComponent<Image>();
			tempButtons_q2[i] = pictures[i].transform.GetChild(1).GetComponent<Image>();
			tempButtons_q3[i] = pictures[i].transform.GetChild(2).GetComponent<Image>();
			tempButtons_q4[i] = pictures[i].transform.GetChild(3).GetComponent<Image>();
		}
		/*switch (scrambleCounter)
		{
			case 1:
				StartCoroutine(ScrambleCoroutine1(tempButtons_q1, tempButtons_q3, tempButtons_q4, tempButtons_q2, originalLocalPositions));
				break;
			case 2:
				StartCoroutine(ScrambleCoroutine2(tempButtons_q2, tempButtons_q1, tempButtons_q3, tempButtons_q4, originalLocalPositions));
				break;
			case 3:
				StartCoroutine(ScrambleCoroutine1(tempButtons_q4, tempButtons_q2, tempButtons_q1, tempButtons_q3, originalLocalPositions));
				break;
			case 4:
				StartCoroutine(ScrambleCoroutine2(tempButtons_q3, tempButtons_q4, tempButtons_q2, tempButtons_q1, originalLocalPositions));
				break;
		}*/
	}

	void ScramblePicture()
	{
		if (!isPictureScrambling) return;
		Debug.Log("Scramble Picture");
		/*scrambleCounter++;
		if (scrambleCounter > 4) scrambleCounter = 1;*/

		
		//reassign variables incase there is an issue with positions due to swap
		

		GameObject[] pictureQuadrants = new GameObject[9];
		Vector3[] pictureLocations = new Vector3[9];
		GameObject picture = GameObject.FindGameObjectWithTag("Finish");
		
		for (int i = 0; i < pictureQuadrants.Length; i++)
		{
			pictureQuadrants[i] = picture.transform.GetChild(i).gameObject;
		}

		if (pictureScrambleCount < 1)
		{
			for (int i = 0; i < pictureQuadrants.Length; i++)
			{
				pictureOriginalLocations[i] = pictureQuadrants[i].transform.position;
			}
		}

		for (int i = 0; i < pictureQuadrants.Length; i++)
		{
			pictureLocations[i] = pictureQuadrants[i].transform.position;

		}
		StartCoroutine(ScrambleCoroutine3(pictureQuadrants, pictureLocations, false));

		pictureScrambleCount++;
	}
	

	IEnumerator ScrambleCoroutine3(GameObject[] pictureQuadrants, Vector3[] pictureLocations, bool reset)
	{
		float timeElapsed = 0;
		Vector3[] randomizedLocations = new Vector3[9];
		Vector3[] currentLocations = new Vector3[9];
		for (int i = 0; i < pictureQuadrants.Length; i++)
		{
			currentLocations[i] = pictureQuadrants[i].transform.position;
		}
		if (!reset) randomizedLocations = ShuffleArray(pictureLocations);
		else randomizedLocations = pictureOriginalLocations;
		/*int[] randomizedTestArray = new int[9];
		randomizedTestArray = ShuffleArray(testArray);*/

		while (timeElapsed < pictureLerpSpeed)
		{
			float t = timeElapsed / pictureLerpSpeed;
			t = t * t * t * (t * (6f * t - 15f) + 10f);
			for (int i = 0; i < pictureQuadrants.Length; i++)
			{
				pictureQuadrants[i].transform.position = Vector3.Lerp(currentLocations[i], randomizedLocations[i], t); //something in picture quadrants that need to be resolved
				//pictureQuadrants[i].transform.position = Vector3.Lerp(currentLocations[i], randomizedLocations[i], 1 * Time.deltaTime); //something in picture quadrants that need to be resolved
			}
			timeElapsed += Time.deltaTime;
			yield return null;
		}
		Debug.Log("isPictureScrabling : " + isPictureScrambling);
		if (!reset)ScramblePicture();
	}


	void SetSlaveTextQuadrants(string q1Tag, string q2Tag, Quaternion target1Rotation, Quaternion target2Rotation) //this sets quadrant rotations of the text
	{
		List<GameObject> allSlaveTextQuadrants1 = new List<GameObject>();
		List<GameObject> allSlaveTextQuadrants2 = new List<GameObject>();
		GameObject[] allSlaveTextQuadrants1Arr = GameObject.FindGameObjectsWithTag(q1Tag);
		GameObject[] allSlaveTextQuadrants2Arr = GameObject.FindGameObjectsWithTag(q2Tag);
		allSlaveTextQuadrants1 = allSlaveTextQuadrants1Arr.ToList();
		allSlaveTextQuadrants2 = allSlaveTextQuadrants2Arr.ToList();

		/*	GameObject[] allSlaveTextQuadrants1 = GameObject.FindGameObjectsWithTag(q1Tag);
			GameObject[] allSlaveTextQuadrants2 = GameObject.FindGameObjectsWithTag(q2Tag);

			GameObject[] allSlaveTextQuadrants1 = new GameObject[slaveButtons_q1.Count];
			GameObject[] allSlaveTextQuadrants2 = new GameObject[slaveButtons_q1.Count];
			allSlaveTextQuadrants1 = GameObject.FindGameObjectsWithTag(q1Tag);
			allSlaveTextQuadrants2 = GameObject.FindGameObjectsWithTag(q2Tag);*/
		foreach (GameObject i in allSlaveTextQuadrants1)
		{
			foreach (GameObject j in allSlaveTextQuadrants2)
			{
				i.transform.rotation = target1Rotation;
				i.GetComponent<Image>().color = dummyRed;
				j.transform.rotation = target2Rotation;
				j.GetComponent<Image>().color = dummyRed;
			}
		}
	}

	void SwapSlaveTextQuadrants(string q1Tag, string q2Tag) //this sets quadrant swaps
	{
	//	Debug.Log("q1Tag : " + q1Tag + "q2Tag : " + q2Tag);
		/* GameObject[] allSlaveTextQuadrants1 = new GameObject[slaveButtons_q1.Count];
		 GameObject[] allSlaveTextQuadrants2 = new GameObject[slaveButtons_q1.Count];
		 Vector3[] tempPositions1 = new Vector3[slaveButtons_q1.Count];
		 Vector3[] tempPositions2 = new Vector3[slaveButtons_q1.Count];
		 allSlaveTextQuadrants1 = GameObject.FindGameObjectsWithTag(q1Tag);
		 allSlaveTextQuadrants2 = GameObject.FindGameObjectsWithTag(q2Tag);*/

		//UpdateSlaveTextAmountAndColor();
		/*        GameObject[] allSlaveTextQuadrants1;
				GameObject[] allSlaveTextQuadrants2;
				allSlaveTextQuadrants1 = GameObject.FindGameObjectsWithTag(q1Tag);
				allSlaveTextQuadrants2 = GameObject.FindGameObjectsWithTag(q2Tag);
				Vector3[] tempPositions1 = new Vector3[allSlaveTextQuadrants1.Length];
				Vector3[] tempPositions2 = new Vector3[allSlaveTextQuadrants1.Length];*/

		List<GameObject> allSlaveTextQuadrants1 = new List<GameObject>();
		List<GameObject> allSlaveTextQuadrants2 = new List<GameObject>();
		GameObject[] allSlaveTextQuadrants1Arr = GameObject.FindGameObjectsWithTag(q1Tag);
		GameObject[] allSlaveTextQuadrants2Arr = GameObject.FindGameObjectsWithTag(q2Tag);
		allSlaveTextQuadrants1 = allSlaveTextQuadrants1Arr.ToList();
		allSlaveTextQuadrants2 = allSlaveTextQuadrants2Arr.ToList();
		List<Vector3> tempPositions1 = new List<Vector3>(allSlaveTextQuadrants1.Count);
		List<Vector3> tempPositions2 = new List<Vector3>();
		Debug.Log("heretop of swap : A;;S;aveTextQuadrants 1 count = " + allSlaveTextQuadrants1.Count + "allslaveTextQuadrants 2 count = " + allSlaveTextQuadrants2.Count);

/*		foreach (GameObject i in allSlaveTextQuadrants1)
		{
			foreach (GameObject k in allSlaveTextQuadrants2)
			{
				for (int j = 0; j < allSlaveTextQuadrants2.Count; j++)
				{
					tempPositions2[j] = i.transform.localPosition;
					tempPositions1[j] = k.transform.localPosition;
				}
			}
		}*/
		foreach (GameObject i in allSlaveTextQuadrants1)
		{
			foreach (GameObject k in allSlaveTextQuadrants2)
			{
				for (int j = 0; j < allSlaveTextQuadrants2.Count; j++)
				{
					tempPositions2.Add(i.transform.localPosition);
					tempPositions1.Add(k.transform.localPosition);
				}
			}
		}

		Debug.Log("here middle of swap");
		foreach (GameObject i in allSlaveTextQuadrants1)
		{
			foreach (GameObject k in allSlaveTextQuadrants2)
			{
				for (int j = 0; j < allSlaveTextQuadrants2.Count; j++)
				{
					i.transform.localPosition = tempPositions1[j];
					k.transform.localPosition = tempPositions2[j];
					i.GetComponent<Image>().color = dummyRed;
					k.GetComponent<Image>().color = dummyRed;
				}
			}
		}

	}

	void SlaveTextSwapper(int letter1, int letter2, int swapCutter) //swaps out pairs of quadrants in swapping tops, bottoms, lefts, rights with no animation. swapCutter decides the orientation of swap"Cut"
	{
/*		List<GameObject> allSlaveTextObject1 = new List<GameObject>();
		List<GameObject> allSlaveTextObject2 = new List<GameObject>();
		List<GameObject> allSlaveTextTarget1 = new List<GameObject>();
		List<GameObject> allSlaveTextTarget2 = new List<GameObject>();
        GameObject[] allSlaveTextObject1Arr = new GameObject[slaveButtons_q1.Count];
        GameObject[] allSlaveTextObject2Arr = new GameObject[slaveButtons_q1.Count];
        GameObject[] allSlaveTextTarget1Arr = new GameObject[slaveButtons_q1.Count];
        GameObject[] allSlaveTextTarget2Arr = new GameObject[slaveButtons_q1.Count];*/

        GameObject[] allSlaveTextObject1 = new GameObject[slaveButtons_q1.Count];
        GameObject[] allSlaveTextObject2 = new GameObject[slaveButtons_q1.Count];
        GameObject[] allSlaveTextTarget1 = new GameObject[slaveButtons_q1.Count];
        GameObject[] allSlaveTextTarget2 = new GameObject[slaveButtons_q1.Count];
        string[] letter1Tags = new string[4];
		string[] letter2Tags = new string[4];

		//assigns temporary strings for purposes of matching to appropriate letters to swap
		for (int i = 0; i < letter1Tags.Length; i++)
		{
			letter1Tags[i] = letter1 + "_q" + (i + 1);
			letter2Tags[i] = letter2 + "_q" + (i + 1);
		}
		switch (swapCutter)
		{
            case 1:
                allSlaveTextObject1 = GameObject.FindGameObjectsWithTag(letter1Tags[0]);
                allSlaveTextObject2 = GameObject.FindGameObjectsWithTag(letter1Tags[1]);
                allSlaveTextTarget1 = GameObject.FindGameObjectsWithTag(letter2Tags[0]);
                allSlaveTextTarget2 = GameObject.FindGameObjectsWithTag(letter2Tags[1]);
                break;
            case 2:
                allSlaveTextObject1 = GameObject.FindGameObjectsWithTag(letter1Tags[2]);
                allSlaveTextObject2 = GameObject.FindGameObjectsWithTag(letter1Tags[3]);
                allSlaveTextTarget1 = GameObject.FindGameObjectsWithTag(letter2Tags[2]);
                allSlaveTextTarget2 = GameObject.FindGameObjectsWithTag(letter2Tags[3]);
                break;
            case 3:
                allSlaveTextObject1 = GameObject.FindGameObjectsWithTag(letter1Tags[0]);
                allSlaveTextObject2 = GameObject.FindGameObjectsWithTag(letter1Tags[2]);
                allSlaveTextTarget1 = GameObject.FindGameObjectsWithTag(letter2Tags[0]);
                allSlaveTextTarget2 = GameObject.FindGameObjectsWithTag(letter2Tags[2]);
                break;
            case 4:
                allSlaveTextObject1 = GameObject.FindGameObjectsWithTag(letter1Tags[1]);
                allSlaveTextObject2 = GameObject.FindGameObjectsWithTag(letter1Tags[3]);
                allSlaveTextTarget1 = GameObject.FindGameObjectsWithTag(letter2Tags[1]);
                allSlaveTextTarget2 = GameObject.FindGameObjectsWithTag(letter2Tags[3]);
                break;

          /*  case 1:
				allSlaveTextObject1Arr = GameObject.FindGameObjectsWithTag(letter1Tags[0]);
				allSlaveTextObject2Arr = GameObject.FindGameObjectsWithTag(letter1Tags[1]);
				allSlaveTextTarget1Arr = GameObject.FindGameObjectsWithTag(letter2Tags[0]);
				allSlaveTextTarget2Arr = GameObject.FindGameObjectsWithTag(letter2Tags[1]);
				break;
			case 2:
				allSlaveTextObject1Arr = GameObject.FindGameObjectsWithTag(letter1Tags[2]);
				allSlaveTextObject2Arr = GameObject.FindGameObjectsWithTag(letter1Tags[3]);
				allSlaveTextTarget1Arr = GameObject.FindGameObjectsWithTag(letter2Tags[2]);
				allSlaveTextTarget2Arr = GameObject.FindGameObjectsWithTag(letter2Tags[3]);
				break;
			case 3:
				allSlaveTextObject1Arr = GameObject.FindGameObjectsWithTag(letter1Tags[0]);
				allSlaveTextObject2Arr = GameObject.FindGameObjectsWithTag(letter1Tags[2]);
				allSlaveTextTarget1Arr = GameObject.FindGameObjectsWithTag(letter2Tags[0]);
				allSlaveTextTarget2Arr = GameObject.FindGameObjectsWithTag(letter2Tags[2]);
				break;
			case 4:
				allSlaveTextObject1Arr = GameObject.FindGameObjectsWithTag(letter1Tags[1]);
				allSlaveTextObject2Arr = GameObject.FindGameObjectsWithTag(letter1Tags[3]);
				allSlaveTextTarget1Arr = GameObject.FindGameObjectsWithTag(letter2Tags[1]);
				allSlaveTextTarget2Arr = GameObject.FindGameObjectsWithTag(letter2Tags[3]);
				break;*/
		}
        //rotationObjects = rotationObjects.OrderBy(i => i.name).ToArray();

        /*allSlaveTextObject1 = allSlaveTextObject1Arr.ToList();
		allSlaveTextObject2 = allSlaveTextObject2Arr.ToList();
		allSlaveTextTarget1 = allSlaveTextTarget1Arr.ToList();
		allSlaveTextTarget2 = allSlaveTextTarget2Arr.ToList();

		List<Sprite> tempSpriteObject1 = new List<Sprite>();
		List<Sprite> tempSpriteObject2 = new List<Sprite>();
		List<Sprite> tempSpriteTarget1 = new List<Sprite>();
		List<Sprite> tempSpriteTarget2 = new List<Sprite>();

		List<Quaternion> tempRotationsObject1 = new List<Quaternion>();
		List<Quaternion> tempRotationsObject2 = new List<Quaternion>();
		List<Quaternion> tempRotationsTarget1 = new List<Quaternion>();
		List<Quaternion> tempRotationsTarget2 = new List<Quaternion>();*/


        Sprite[] tempSpriteObject1 = new Sprite[allSlaveTextObject1.Length];
        Sprite[] tempSpriteObject2 = new Sprite[allSlaveTextObject2.Length];
        Sprite[] tempSpriteTarget1 = new Sprite[allSlaveTextTarget1.Length];
        Sprite[] tempSpriteTarget2 = new Sprite[allSlaveTextTarget2.Length];

        Quaternion[] tempRotationsObject1 = new Quaternion[slaveButtons_q1.Count];
        Quaternion[] tempRotationsObject2 = new Quaternion[slaveButtons_q1.Count];
        Quaternion[] tempRotationsTarget1 = new Quaternion[slaveButtons_q1.Count];
        Quaternion[] tempRotationsTarget2 = new Quaternion[slaveButtons_q1.Count];

        //its a "trick" - its only swapping out the sprite source images, is there a better way to do this rather than have 2 foreach loops?
        //temporarily store, for some reason order of operations matters.. i tried putting all into one nested collection and there were issues.
        //only the first ones matter since they are identical and the first one is the "Index" collection
        /*  for (int m = 0; m < allSlaveTextObject1.Count; m++)
          {
              tempSpriteObject1.Add(allSlaveTextObject1[0].GetComponent<Image>().sprite);
              tempRotationsObject1.Add(allSlaveTextObject1[0].transform.rotation);
              tempSpriteObject2.Add(allSlaveTextObject2[0].GetComponent<Image>().sprite);
              tempRotationsObject2.Add(allSlaveTextObject2[0].transform.rotation);
          }
          for (int m = 0; m < allSlaveTextTarget1.Count; m++)
          {
              tempSpriteTarget1.Add(allSlaveTextTarget1[0].GetComponent<Image>().sprite);
              tempRotationsTarget1.Add(allSlaveTextTarget1[0].transform.rotation);
              tempSpriteTarget2.Add(allSlaveTextTarget2[0].GetComponent<Image>().sprite);
              tempRotationsTarget2.Add(allSlaveTextTarget2[0].transform.rotation);
          }*/


        for (int m = 0; m < allSlaveTextObject1.Length; m++)
        {
            tempSpriteObject1[0] = allSlaveTextObject1[0].GetComponent<Image>().sprite;
            tempRotationsObject1[0] = allSlaveTextObject1[0].transform.rotation;
            tempSpriteObject2[0] = allSlaveTextObject2[0].GetComponent<Image>().sprite;
            tempRotationsObject2[0] = allSlaveTextObject2[0].transform.rotation;
        }
        for (int m = 0; m < allSlaveTextTarget1.Length; m++)
        {
            tempSpriteTarget1[0] = allSlaveTextTarget1[0].GetComponent<Image>().sprite;
            tempRotationsTarget1[0] = allSlaveTextTarget1[0].transform.rotation;
            tempSpriteTarget2[0] = allSlaveTextTarget2[0].GetComponent<Image>().sprite;
            tempRotationsTarget2[0] = allSlaveTextTarget2[0].transform.rotation;
        }

        foreach (GameObject i in allSlaveTextObject1)
		{
			foreach (GameObject j in allSlaveTextObject2)
			{
				foreach (GameObject k in allSlaveTextTarget1)
				{
					foreach (GameObject l in allSlaveTextTarget2)
					{
						i.GetComponent<Image>().sprite = tempSpriteTarget1[0];
						i.transform.rotation = tempRotationsTarget1[0];
						j.GetComponent<Image>().sprite = tempSpriteTarget2[0];
						j.transform.rotation = tempRotationsTarget2[0];
						k.GetComponent<Image>().sprite = tempSpriteObject1[0];
						k.transform.rotation = tempRotationsObject1[0];
						l.GetComponent<Image>().sprite = tempSpriteObject2[0];
						l.transform.rotation = tempRotationsObject2[0];
						if (!isTimescalePaused)
						{
							if (swapCutter == 1)
							{
								i.GetComponent<Image>().color = dummyBlue;
								j.GetComponent<Image>().color = dummyBlue;
								k.GetComponent<Image>().color = dummyBlue;
								l.GetComponent<Image>().color = dummyBlue;
							}
							else
							{
								i.GetComponent<Image>().color = Color.green;
								j.GetComponent<Image>().color = Color.green;
								k.GetComponent<Image>().color = Color.green;
								l.GetComponent<Image>().color = Color.green;
							}
						}
					}
				}
			}
		}
	}

	void ResetSlaveText()
	{
		slaveLettersList = new List<GameObject>(GameObject.FindGameObjectsWithTag("slaveLetter"));
		slaveButtons_q1 = new List<Button>();
		slaveButtons_q2 = new List<Button>();
		slaveButtons_q3 = new List<Button>();
		slaveButtons_q4 = new List<Button>();

		for (int i = 0; i < slaveLettersList.Count; i++)
		{
			if (slaveLettersList[i].transform.childCount > 1) //Exclude punctuation
			{
				slaveButtons_q1.Add(slaveLettersList[i].transform.GetChild(0).GetComponent<Button>());
				slaveButtons_q2.Add(slaveLettersList[i].transform.GetChild(1).GetComponent<Button>());
				slaveButtons_q3.Add(slaveLettersList[i].transform.GetChild(2).GetComponent<Button>());
				slaveButtons_q4.Add(slaveLettersList[i].transform.GetChild(3).GetComponent<Button>());
			}
		}

		Debug.Log("slaveLetterListCount : " + slaveLettersList.Count);
		for (int i = 0; i < slaveLettersList.Count; i++)
		{
			if (slaveLettersList[i].transform.childCount > 1)
			{
				slaveButtons_q1[i].transform.localPosition = originalSlaveSelfPosition_q1;
				slaveButtons_q2[i].transform.localPosition = originalSlaveSelfPosition_q2;
				slaveButtons_q3[i].transform.localPosition = originalSlaveSelfPosition_q3;
				slaveButtons_q4[i].transform.localPosition = originalSlaveSelfPosition_q4;
				slaveButtons_q1[i].transform.rotation = rotations[0];
				slaveButtons_q2[i].transform.rotation = rotations[0];
				slaveButtons_q3[i].transform.rotation = rotations[0];
				slaveButtons_q4[i].transform.rotation = rotations[0];
				//slaveButtons_q1[i].image.sprite = q1OriginalSlaveSprites[i];
				//slaveButtons_q2[i].image.sprite = q2OriginalSlaveSprites[i];
				//slaveButtons_q3[i].image.sprite = q3OriginalSlaveSprites[i];
				//slaveButtons_q4[i].image.sprite = q4OriginalSlaveSprites[i];

			}
		}
	}

	void ResetPositions(int topCount, int bottomCount)
	{
		
		switch (topCount)
		{
			case 2:
			case 3:
			case 4:
				for (int i = 0; i < buttons_q1.Length; i++)
				{
					buttons_q1[i].transform.localPosition = originalMasterSelfPosition_q1;
					buttons_q2[i].transform.localPosition = originalMasterSelfPosition_q2;
				}
				/*for (int i = 0; i < slaveButtons_q1.Count; i++)
				{
					if (!slaveButtons_q1[i]) break;
					slaveButtons_q1[i].transform.localPosition = originalSlaveSelfPosition_q1;
					slaveButtons_q2[i].transform.localPosition = originalSlaveSelfPosition_q2;
				}*/
				break;

			case 6:
			case 7:
			case 8:
                for (int i = 0; i < buttons_q1.Length; i++)
                {
                    buttons_q1[i].transform.localPosition = originalMasterSelfPosition_q2;
                    buttons_q2[i].transform.localPosition = originalMasterSelfPosition_q1;
                }
				break;
               /* for (int i = 0; i < slaveButtons_q1.Count; i++)
                {
                    if (!slaveButtons_q1[i]) break;
                    slaveButtons_q1[i].transform.localPosition = originalSlaveSelfPosition_q2;
                    slaveButtons_q2[i].transform.localPosition = originalSlaveSelfPosition_q1;
                }
                break;*/
        }
		switch (bottomCount)
		{
			// case 2:
			//case 3:
			case 4:
				for (int i = 0; i < buttons_q1.Length; i++)
				{
					buttons_q3[i].transform.localPosition = originalMasterSelfPosition_q3;
					buttons_q4[i].transform.localPosition = originalMasterSelfPosition_q4;
				}
				/*for (int i = 0; i < slaveButtons_q1.Count; i++)
				{
					if (!slaveButtons_q3[i]) break;
					slaveButtons_q3[i].transform.localPosition = originalSlaveSelfPosition_q3;
					slaveButtons_q4[i].transform.localPosition = originalSlaveSelfPosition_q4;
				}*/
				break;

			//case 6:
			case 7:
			//case 8:
				for (int i = 0; i < buttons_q1.Length; i++)
				{
					buttons_q3[i].transform.localPosition = originalMasterSelfPosition_q4;
					buttons_q4[i].transform.localPosition = originalMasterSelfPosition_q3;
				}
				/*for (int i = 0; i < slaveButtons_q1.Count; i++)
				{
					if (!slaveButtons_q3[i]) break;
					slaveButtons_q3[i].transform.localPosition = originalSlaveSelfPosition_q4;
					slaveButtons_q4[i].transform.localPosition = originalSlaveSelfPosition_q3;
				}*/
				break;
		}
	}

	void TurnColorsBlack()
	{
		//underlines
		underlineCanvas.alpha = Mathf.Lerp(underlineCanvas.alpha, underlineAlpha, 3 * Time.deltaTime);
		radialCanvas1.alpha = Mathf.Lerp(radialCanvas1.alpha, underlineAlpha, 3 * Time.deltaTime);
		radialCanvas2.alpha = Mathf.Lerp(radialCanvas2.alpha, underlineAlpha, 3 * Time.deltaTime);
		radialCanvas3.alpha = Mathf.Lerp(radialCanvas3.alpha, underlineAlpha, 3 * Time.deltaTime);
		cursor.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(cursor.GetComponent<CanvasGroup>().alpha, underlineAlpha, 3 * Time.deltaTime);

		for (int i = 0; i < slaveButtons_q1.Count; i++)
		{
			if (slaveButtons_q1[i])
			{
				if (!slaveButtons_q1[i]) break;
				slaveButtons_q1[i].image.color = Color.Lerp(slaveButtons_q1[i].image.color, baseTextColor, targetBlackSpeed * Time.deltaTime);
				slaveButtons_q2[i].image.color = Color.Lerp(slaveButtons_q2[i].image.color, baseTextColor, targetBlackSpeed * Time.deltaTime);
				slaveButtons_q3[i].image.color = Color.Lerp(slaveButtons_q3[i].image.color, baseTextColor, targetBlackSpeed * Time.deltaTime);
				slaveButtons_q4[i].image.color = Color.Lerp(slaveButtons_q4[i].image.color, baseTextColor, targetBlackSpeed * Time.deltaTime);
			}
		}

		for (int i = 0; i < buttons_q1.Length; i++)
		{
			if (buttons_q1[i])
			{
				if (!buttons_q1[i]) break;
				buttons_q1[i].image.color = Color.Lerp(buttons_q1[i].image.color, targetBlackColor, targetBlackSpeed * Time.deltaTime);
				buttons_q2[i].image.color = Color.Lerp(buttons_q2[i].image.color, targetBlackColor, targetBlackSpeed * Time.deltaTime);
				buttons_q3[i].image.color = Color.Lerp(buttons_q3[i].image.color, targetBlackColor, targetBlackSpeed * Time.deltaTime);
				buttons_q4[i].image.color = Color.Lerp(buttons_q4[i].image.color, targetBlackColor, targetBlackSpeed * Time.deltaTime);
			}
		}
	}

	void TurnSelectorRed(int i)
	{
		currentq1.GetComponent<Image>().color = Color.red;
		currentq2.GetComponent<Image>().color = Color.red;
		currentq3.GetComponent<Image>().color = Color.red;
		currentq4.GetComponent<Image>().color = Color.red;
	}

	void ResetLetters()
	{
/*		if (isPaused) return;
		timescaleTarget = 1f;
		isPictureScrambling = true;
		isPaused = true;
		start = false;
		isTextGenerated = false;
		cameraScript.Pause();
		CancelInvoke("Pacer");
		CancelInvoke("SwapPacer");
		DeleteGeneratedText();
		//UpdateSlaveTextAmountAndColor();
		StartCoroutine(ResetPositionsAndRotations());
		ReinitializeValues();
		ConvertStrings();
		//UpdateSlaveTextAmountAndColor();
		//GenerateNextSentence();
		destinationRotation = clockHandRotations[0];*/

	}

	void End()
    {
		underlineAlpha = 0;
		cameraScript.isDialogueStarted = false;
		cameraScript.ReturnCamera();
		isPaused = true;
		start = false;
		isTextGenerated = false;
		CancelInvoke("Pacer");
		CancelInvoke("SwapPacer");
		DeleteGeneratedText();
		//UpdateSlaveTextAmountAndColor();
		StartCoroutine(ResetPositionsAndRotations());
		//destinationRotation = clockHandRotations[0];
		//textPosition.y = startingSlaveTextPosition.y; // resets the sentences to write on top again
		//currentSentenceCounter = 0;
		
		isFinalTimeScalePaused = true;
		timescaleTarget = .1f;
	}

    void ResetValues()
    {
		Debug.Log("hit reset");
		isResetting = true;
		destinationRotation = clockHandRotations[0];
		StartCoroutine(ResetPositionsAndRotations());
		targetBlackSpeed = targetBlackSpeedPause;
	}


	void Restart()
    {
		Time.timeScale = 1;
		Resources.UnloadUnusedAssets();
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single); start = true;
		isPaused = false;
		StartPacers();
	}

	void StartLetters()
    {
		
    }

	void AbstractLetterTrigger()
    {
		return;
		Debug.Log("triggered Abstract ");
		//if (isTimescalePaused) return;
		//isTimescalePaused = true;
		//timescaleTarget = 0f;
		 cameraBackgroundColor = cameraBackgroundColorOff;
		clockBlurTarget = clockBlurOff;
		targetBlackSpeed = targetBlackSpeedPause;
		AbstractLetterEffect();
	}

	void AbstractLetterEffect()
    {
		Debug.Log("letterreplacementCount : " + letterReplacementCount);
		//current letter
		string tempString_q1 = abstractArray[letterReplacementCount] + "_q1";
		string tempString_q2 = abstractArray[letterReplacementCount] + "_q2";
		string tempString_q3 = abstractArray[letterReplacementCount] + "_q3";
		string tempString_q4 = abstractArray[letterReplacementCount] + "_q4";
		Sprite replacementSprite_q1 = Resources.Load<Sprite>("AbstractBorders/" + tempString_q1);
		Sprite replacementSprite_q2 = Resources.Load<Sprite>("AbstractBorders/" + tempString_q2);
		Sprite replacementSprite_q3 = Resources.Load<Sprite>("AbstractBorders/" + tempString_q3);
		Sprite replacementSprite_q4 = Resources.Load<Sprite>("AbstractBorders/" + tempString_q4);
		Sprite replacementSpriteS_q1 = Resources.Load<Sprite>("AbstractBorders/" + tempString_q1);
		Sprite replacementSpriteS_q2 = Resources.Load<Sprite>("AbstractBorders/" + tempString_q2);
		Sprite replacementSpriteS_q3 = Resources.Load<Sprite>("AbstractBorders/" + tempString_q3);
		Sprite replacementSpriteS_q4 = Resources.Load<Sprite>("AbstractBorders/" + tempString_q4);
		buttons_q1[abstractArray[letterReplacementCount]].image.sprite = replacementSprite_q1;
		buttons_q2[abstractArray[letterReplacementCount]].image.sprite = replacementSprite_q2;
		buttons_q3[abstractArray[letterReplacementCount]].image.sprite = replacementSprite_q3;
		buttons_q4[abstractArray[letterReplacementCount]].image.sprite = replacementSprite_q4;

		buttons_q1[abstractArray[letterReplacementCount]].image.color = Color.yellow;
		buttons_q2[abstractArray[letterReplacementCount]].image.color = Color.yellow;
		buttons_q3[abstractArray[letterReplacementCount]].image.color = Color.yellow;
		buttons_q4[abstractArray[letterReplacementCount]].image.color = Color.yellow;

        GameObject[] allSlaveTextQuadrantsWithLetter = GameObject.FindGameObjectsWithTag(tempString_q1);
        GameObject[] allSlaveTextQuadrants1 = new GameObject[allSlaveTextQuadrantsWithLetter.Length];
        GameObject[] allSlaveTextQuadrants2 = new GameObject[allSlaveTextQuadrantsWithLetter.Length];
        GameObject[] allSlaveTextQuadrants3 = new GameObject[allSlaveTextQuadrantsWithLetter.Length];
        GameObject[] allSlaveTextQuadrants4 = new GameObject[allSlaveTextQuadrantsWithLetter.Length];
        allSlaveTextQuadrants1 = GameObject.FindGameObjectsWithTag(tempString_q1);
        allSlaveTextQuadrants2 = GameObject.FindGameObjectsWithTag(tempString_q2);
        allSlaveTextQuadrants3 = GameObject.FindGameObjectsWithTag(tempString_q3);
        allSlaveTextQuadrants4 = GameObject.FindGameObjectsWithTag(tempString_q4);
		for (int i = 0; i < allSlaveTextQuadrantsWithLetter.Length; i++)
        {
            allSlaveTextQuadrants1[i].GetComponent<Image>().sprite = replacementSpriteS_q1;
            allSlaveTextQuadrants2[i].GetComponent<Image>().sprite = replacementSpriteS_q2;
            allSlaveTextQuadrants3[i].GetComponent<Image>().sprite = replacementSpriteS_q3;
            allSlaveTextQuadrants4[i].GetComponent<Image>().sprite = replacementSpriteS_q4;
            allSlaveTextQuadrants1[i].GetComponent<Image>().color = Color.yellow;
            allSlaveTextQuadrants2[i].GetComponent<Image>().color = Color.yellow;
            allSlaveTextQuadrants3[i].GetComponent<Image>().color = Color.yellow;
            allSlaveTextQuadrants4[i].GetComponent<Image>().color = Color.yellow;
        }
		return;
		if (letterReplacementCount < 1) return;
        //highlight all other ones previous
        for (int i = 0; i < letterReplacementCount; i++)
		{
			buttons_q1[abstractArray[i]].image.color = Color.yellow;
			buttons_q2[abstractArray[i]].image.color = Color.yellow;
			buttons_q3[abstractArray[i]].image.color = Color.yellow;
			buttons_q4[abstractArray[i]].image.color = Color.yellow;

			tempString_q1 = abstractArray[i] + "_q1";
			tempString_q2 = abstractArray[i] + "_q2";
			tempString_q3 = abstractArray[i] + "_q3";
			tempString_q4 = abstractArray[i] + "_q4";
			allSlaveTextQuadrantsWithLetter = GameObject.FindGameObjectsWithTag(tempString_q1);
			allSlaveTextQuadrants1 = new GameObject[allSlaveTextQuadrantsWithLetter.Length];
			allSlaveTextQuadrants2 = new GameObject[allSlaveTextQuadrantsWithLetter.Length];
			allSlaveTextQuadrants3 = new GameObject[allSlaveTextQuadrantsWithLetter.Length];
			allSlaveTextQuadrants4 = new GameObject[allSlaveTextQuadrantsWithLetter.Length];
			allSlaveTextQuadrants1 = GameObject.FindGameObjectsWithTag(tempString_q1);
			allSlaveTextQuadrants2 = GameObject.FindGameObjectsWithTag(tempString_q2);
			allSlaveTextQuadrants3 = GameObject.FindGameObjectsWithTag(tempString_q3);
			allSlaveTextQuadrants4 = GameObject.FindGameObjectsWithTag(tempString_q4);

            for (int j = 0; j < allSlaveTextQuadrantsWithLetter.Length; j++)
            {
				allSlaveTextQuadrants1[j].GetComponent<Image>().color = Color.yellow;
				allSlaveTextQuadrants2[j].GetComponent<Image>().color = Color.yellow;
				allSlaveTextQuadrants3[j].GetComponent<Image>().color = Color.yellow;
				allSlaveTextQuadrants4[j].GetComponent<Image>().color = Color.yellow;
			}
        }
	}

	void UnderlineBlinker()
    {
		if (underlineWordCanvas.alpha == 1)
        {
			underlineWordCanvas.alpha = 0;
		}
        else
        {
			underlineWordCanvas.alpha = 1;
		}
	}

	void CursorBlinker()
    {
		if (isRadialFilled)
        {
			Color tempColor = cursor.GetComponent<Image>().color;
			tempColor.a = 0;
			cursor.GetComponent<Image>().color = tempColor;
			return;
		}

		if (cursor.GetComponent<Image>().color.a == .7f)
		{
			Color tempColor = cursor.GetComponent<Image>().color;
			tempColor.a = 0;
			cursor.GetComponent<Image>().color = tempColor;
		}
		else
		{
			Color tempColor = cursor.GetComponent<Image>().color;
			tempColor.a = .7f;
			cursor.GetComponent<Image>().color = tempColor;
		}
	}

	void FillRadial( Image[] inputRadials)
    {
		if (inputRadials[0].fillAmount < 1)
		{
			inputRadials[0].fillAmount += 1f / radialDelay * Time.deltaTime;
		}
		else
		{
			if (inputRadials[1].fillAmount < 1)
			{
				inputRadials[1].fillAmount += 1f / radialDelay * Time.deltaTime;
			}
			else
			{
				if (inputRadials[2].fillAmount < 1)
				{
					if (!isRadialBlinkerCalled)
						InvokeRepeating("RadialBlinker", 0, .5f);
					for (int i = 0; i < inputRadials.Length; i++)
					{

						inputRadials[i].color = radialTargetColor;
					}
					inputRadials[2].fillAmount += 1f / radialDelay * Time.deltaTime;
				}
				else
				{
					if (inputRadials[3].fillAmount < 1)
					{
						for (int i = 0; i < inputRadials.Length; i++)
						{
							inputRadials[i].color = radialTargetColor;
						}
						inputRadials[3].fillAmount += 1f / radialDelay * Time.deltaTime;
					}
					else
					{	if (currentSentenceCounter == 22)
						{
							isPictureScrambling = false;
						}
						
						isRadialFilled = true;
						doRadialFill = false;
						CancelInvoke("RadialBlinker");
						radialTargetColor = new Color(1, 0, 0, 1);
						isRadialBlinkerCalled = false;

					}
				}
			}
		}
	}

	void ResetRadials(Image[] inputRadials)
    {
		for (int i = 0; i < inputRadials.Length; i++)
		{
			inputRadials[i].color = radialTargetColor;
		}
	}

	void RadialBlinker()
    {
		if (!isRadialBlinkerCalled)
        {
			radialTargetColor = new Color(1, 1, 1, 1);
		}
		isRadialBlinkerCalled = true;

		if (radialTargetColor.a == 0)
		{
			Color tempColor1 = radialTargetColor;
			tempColor1.a = 1;
			radialTargetColor = tempColor1;
		}
        else
        {
			Color tempColor1 = radialTargetColor;
			tempColor1.a = 0;
			radialTargetColor = tempColor1;
		}

		Color tempColor;
		tempColor = radialTargetColor;
		tempColor.b -= .2f;
		tempColor.g -= .2f;
		radialTargetColor = tempColor;

	}
	void ClearRadials()
    {
		for (int i = 0; i < inputRadials.Length; i++)
		{
			inputRadials[i].fillAmount = 0;
		}
		for (int i = 0; i < inputRadials2.Length; i++)
		{
			inputRadials2[i].fillAmount = 0;
		}
		for (int i = 0; i < inputRadials3.Length; i++)
		{
			inputRadials3[i].fillAmount = 0;
		}
	}

	void UnderlineInputBlinker()
    {
		underlineBlinkCounter++;
		GameObject[] allUnderlines = GameObject.FindGameObjectsWithTag("Player");
		if (allUnderlines.Length == 0) return;

		Debug.Log("CALLED UNDERLINE underline count: " + allUnderlines.Length);

		for (int i = 0; i<allUnderlines.Length; i++)
        {
			Debug.Log("here i : " + i + "underlineblinker : " + underlineBlinkCounter );
			if (allUnderlines[i].GetComponent<Image>().color == underlineTransparentColor)
            {
				allUnderlines[i].GetComponent<Image>().color = underlineTargetColor;
			}
            else
            {
				allUnderlines[i].GetComponent<Image>().color = underlineTransparentColor;
			}

			/*if (underlineBlinkCounter == 3)
            {
                allUnderlines[i].GetComponent<Image>().color = underlineTransparentColor;
                underlineBlinkCounter = 0;
            }
            else
            {
                allUnderlines[i].GetComponent<Image>().color = underlineTargetColor;
            }*/
		}
	}

	void DeleteGeneratedText()
    {
		int children = GameObject.Find("GeneratedText").transform.childCount;
		for (int i = 0; i < children; ++i)
		{
			GameObject tempChild = GameObject.Find("GeneratedText").transform.GetChild(i).gameObject;
			Destroy(tempChild.gameObject);
		}
	}

}