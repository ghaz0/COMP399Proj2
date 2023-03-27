using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Cinemachine;
using TMPro;

public class PlayerCtrl : MonoBehaviour
{
    public int Health = 5;
    public float punchDistance = 3f;
    public float punchTime = .5f;
    public int punchPower = 1;
    public int MaxScore = 5;
    private int currentScore = 0;
    private RaycastHit hit;

    public GameObject[] titleCards;
    public GameObject pauseText;
    public GameObject GameOver;


    public Camera mainCam;
    public CinemachineFreeLook followCam;
    public CharacterController controller;
    public Animator anim;
    public Transform headLook;
    public float xRot = 0;
    public float mouseSensitivity = 10;
    public Vector3 playerVelocity;
    public bool grounded;
    private float playerSpeed = 2.0f;
    //private float jumpHeight = 1.0f;
    private float gravityValue = Physics.gravity.y;
    private Vector3 playerMove;

    private float timeSum = 0;

    private void Start()
    {
        currentScore = 0;
        Time.timeScale = 0;
        pauseText.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (currentScore == MaxScore && Time.timeScale != .5f)
        {
            Time.timeScale = .5f;
            GameOver.SetActive(true);
        }

        if (GameOver.activeInHierarchy && Input.GetKeyDown(KeyCode.Space)){
            EditorApplication.isPlaying = false;   
        }

        if (Input.GetKeyDown(KeyCode.Space) && TitleIsVisible() && Time.timeScale == 0)
        {
            foreach (GameObject obj in titleCards)
            {
                Time.timeScale = 1;
                obj.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
                pauseText.SetActive(false);
            }
            else
            {
                Time.timeScale = 0;
                pauseText.SetActive(true);
            }
        }
        Movement();
        Animation();
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("Punch");
            //Punch();
        }
    }

    public void UpdateScore()
    {
        if (currentScore < MaxScore)
        {
            currentScore++;
        }
    }

    private bool TitleIsVisible()
    {
        bool outVal = true;
        foreach (GameObject obj in titleCards)
        {
            if (!obj.activeInHierarchy)
            {
                outVal = false;
                break;
            }
        }
        return outVal;
    }

    private void Animation()
    {

        anim.SetFloat("Walk", Input.GetAxis("Vertical"));
        anim.SetFloat("Strafe", Input.GetAxis("Horizontal"));
    }

    private void Movement()
    {
        grounded = controller.isGrounded;
        if (grounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -0.5f;
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * 2f * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);

        playerMove = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        controller.Move(playerMove * Time.deltaTime * playerSpeed);
        xRot = mainCam.transform.rotation.eulerAngles.y;

        if (Input.GetButtonDown("Jump") && grounded)
        {
            //commenting this out as it isn't needed at the moment
            //playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void SetHealth(int val){
        if (Health <= 0){
            return;
        }
        anim.SetTrigger("TakeHit");
        Health += val;
        if (Health <= 0){

        }
    }

    private void Punch(){
        if (timeSum != 0) return;
        StartCoroutine("PunchAct");
    }

    IEnumerator PunchAct(){
        timeSum = 0;
        while (timeSum < punchTime){
            timeSum += Time.deltaTime;
            yield return null;
        }
        anim.SetTrigger("Punch");
        bool found = Physics.Raycast(transform.position, transform.forward, out hit, punchDistance);
        if (found && hit.collider.gameObject.GetComponent<NPC>() != null){
            hit.collider.gameObject.GetComponent<NPC>().SetHealth(-punchPower);
        }
        yield return null;
    }
}
