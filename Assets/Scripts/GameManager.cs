using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    int solveCount = 0;
    int timer = 0;
    bool IsRampUnlock = false;
    bool isTargetPushed = false;
    bool isInFrontOfObj = false;
	bool isGaugeOn = false;

	GameObject player;
    GameObject target;
    GameObject rampBoxObj = null;
    public GameObject gaugeUI;
	public GameObject ruleUI;
	public GameObject FeedBackUI;
    public GameObject rampFoot;
	public Text FeedBackText;
    public Text TimeText;
    public Text RuleText;
    public Button retryBtn;


	Vector3 tempPosition;

    MovingObj targetObj = null;
	MovingObjOnRamp rampBox = null;

	public Camera firstPersonCamera;
    public Camera overheadCamera;

	Slider slider;
    GaugeScripts sliderScript;

	void Start () {

		player = GameObject.Find("Player");
		rampBoxObj = GameObject.Find("BoxOnRamp");
		rampBox = rampBoxObj.GetComponent<MovingObjOnRamp>();
        sliderScript = gaugeUI.GetComponent<GaugeScripts>();
        ShowOverheadView();

    }

	public void ShowOverheadView() {
        firstPersonCamera.enabled = false;
        overheadCamera.enabled = true;
    }
    
    public void ShowFirstPersonView() {
        firstPersonCamera.enabled = true;
        overheadCamera.enabled = false;
    }

	public void setTarget(GameObject obj){
		target = obj;
		isInFrontOfObj = true;	
	}

	void BringUpGauge(GameObject objName){
        gaugeUI.SetActive(true);
        sliderScript.enabled = true;
        slider = GameObject.Find("Slider").GetComponent<Slider>();
        player.transform.LookAt(objName.transform.position);
		player.GetComponent<PhysicsController>().enabled = false;
		tempPosition = objName.transform.position;
		ShowFirstPersonView();
        isGaugeOn = true;

        if(target != null){
            target.GetComponent<MovingObj>().checkForceShoudApplied();
        }
    }

    void PushObject()
    {
        targetObj = target.GetComponent<MovingObj>();
        targetObj.isRunning = !targetObj.isRunning;
        target.transform.GetChild(1).gameObject.SetActive(false); //invisible footprints when moving
        targetObj.setPushForce(slider.value);
        CloseGaugeUI();
        isTargetPushed = true;
        isInFrontOfObj = false;
    }

    void CloseGaugeUI(){
        sliderScript.enabled = false;
        player.GetComponent<PhysicsController>().enabled = true;
		gaugeUI.SetActive(false);
		ShowOverheadView();
	}

    void checkObjectStatus()
    {
        CloseGaugeUI();
        FeedBackUI.SetActive(true);
        targetObj = target.GetComponent<MovingObj>();
        if (targetObj.hasExitedDestination)
        {
            FeedBackText.text = "You pushed too hard. Try again";
            retryBtn.onClick.AddListener(Putback);
        }
        else if(targetObj.hasEnteredDestination)
        {
            FeedBackText.text = "Great, You made it! Now move to the next.";
            retryBtn.onClick.AddListener(SolveOne);
        }
        else if(!targetObj.hasExitedDestination && !targetObj.hasEnteredDestination)
        {
            FeedBackText.text = "You need to push harder. Keep pushing";
            retryBtn.onClick.AddListener(KeepPush);
        }
    }

	void Putback(){
        target.transform.position = tempPosition;
        target.transform.GetChild(1).gameObject.SetActive(true); //visible footprints
		FeedBackUI.SetActive(false);
        isGaugeOn = false;
        isTargetPushed = false;
        target.GetComponent<MovingObj>().ResetTrigger();
        retryBtn.onClick.RemoveListener(Putback);
    }

    void SolveOne(){
        solveCount++;
        FeedBackUI.SetActive(false);
        target.transform.GetChild(1).gameObject.SetActive(false);
        target.GetComponent<MovingObj>().ResetTrigger();
        retryBtn.onClick.RemoveListener(SolveOne);
    }

    void KeepPush(){
        FeedBackUI.SetActive(false);
        target.transform.GetChild(1).gameObject.SetActive(true); //visible footprints
        isGaugeOn = false;
        isTargetPushed = false;
        target.GetComponent<MovingObj>().ResetTrigger();
        retryBtn.onClick.RemoveListener(KeepPush);
    }

    void UnlockRamp(){

        IsRampUnlock = true;
        rampFoot.SetActive(true);

        RuleText.text = "Congrats! You've done the 3 tasks. Now move to the ramp and press [Enter] to push the yellow box.";

    }


    void Update()
    {
        //timer += Time.deltaTime;
        timer += 1;
        TimeText.text = "" + timer;

        if (IsRampUnlock && Input.GetKeyDown(KeyCode.Return))// && rampFoot.GetComponent<Collider>().gameObject.name == "Player")
        {
            rampBox.setPushPower(1.6f);
            rampBox.m_isRunning = !rampBox.m_isRunning;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isInFrontOfObj && !isGaugeOn)
        {
            BringUpGauge(target);
        }
        else if (isGaugeOn && Input.GetKeyDown(KeyCode.Space))
        {
            PushObject();
        }
        else if (isGaugeOn && target != null)
        {
            if(target.GetComponent<Rigidbody>().velocity.sqrMagnitude == 0 && isTargetPushed)
            {
                checkObjectStatus();
            }
        }

        if (solveCount == 3)
        {
            UnlockRamp();
        }

    }

   

}
