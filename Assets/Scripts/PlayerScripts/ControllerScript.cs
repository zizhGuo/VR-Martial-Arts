using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using UnityEngine.SceneManagement;

public class ControllerScript : MonoBehaviour
{
    public GameObject shield;
    public GameObject punch;
    public GameObject hand;
    public float shieldGenerateTime;
    public float punchGenerateTime;
    public float shieldSize;
    public float punchSize;
    //public GameProcess gameManager;
    public VRTK_ControllerActions controllerActions;

    public Vector3 originalPunchSize;
    public Vector3 originalShieldSize;
    public bool isAction;

	// Use this for initialization
	void Start ()
    {
        originalPunchSize = punch.transform.localScale;
        originalShieldSize = shield.transform.localScale;
        isAction = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(VRTK_SDK_Bridge.IsButtonOnePressedDownOnIndex(VRTK_DeviceFinder.GetControllerIndex(gameObject)))
        {
            //print("One");
            SceneManager.LoadScene("Test");
        }

        if (VRTK_SDK_Bridge.IsButtonOnePressedDownOnIndex(VRTK_DeviceFinder.GetControllerIndex(gameObject)))
        {
            //print("Two");
        }

        /*
        if (!gameManager.isGameStart)
        {
            if (VRTK_SDK_Bridge.IsTriggerPressedUpOnIndex(VRTK_DeviceFinder.GetControllerIndex(gameObject)) && gameManager.currentPage == 1)
            {
                gameManager.currentPage++;
                gameManager.tutorialPage1.SetActive(false);
                gameManager.obsidianExample.SetActive(false);
                gameManager.tutorialPage2.SetActive(true);
            }

            else if (VRTK_SDK_Bridge.IsTriggerPressedUpOnIndex(VRTK_DeviceFinder.GetControllerIndex(gameObject)) && gameManager.currentPage == 2)
            {
                gameManager.currentPage++;
                gameManager.tutorialPage2.SetActive(false);
                gameManager.tutorialPage3.SetActive(true);
            }

            else if (VRTK_SDK_Bridge.IsTriggerPressedUpOnIndex(VRTK_DeviceFinder.GetControllerIndex(gameObject)) && gameManager.currentPage == 3)
            {
                gameManager.currentPage++;
                gameManager.tutorialPage3.SetActive(false);
                gameManager.isGameStart = true;
                gameManager.obsidianCore.SetActive(true);
                gameManager.gameStartTime = Time.time;
            }
        }

        if (gameManager.isGameStart && Time.time - gameManager.gameStartTime >= 5 && !gameManager.playerLose)
        {
            if (VRTK_SDK_Bridge.IsTriggerPressedDownOnIndex(VRTK_DeviceFinder.GetControllerIndex(gameObject)) && !isAction)
            {
                //print("Controller " + VRTK_DeviceFinder.GetControllerIndex(gameObject) + " trigger down");
                isAction = true;
                hand.SetActive(false);
                shield.SetActive(true);
                StartCoroutine(generateShield(shieldGenerateTime));
            }

            if (VRTK_SDK_Bridge.IsTriggerPressedUpOnIndex(VRTK_DeviceFinder.GetControllerIndex(gameObject)))
            {
                //print("Controller " + VRTK_DeviceFinder.GetControllerIndex(gameObject) + " trigger up");
                shield.transform.localScale = originalShieldSize;
                shield.SetActive(false);
                hand.SetActive(true);
                isAction = false;
            }

            if (VRTK_SDK_Bridge.IsGripPressedDownOnIndex(VRTK_DeviceFinder.GetControllerIndex(gameObject)) && !isAction)
            {
                //print("Controller " + VRTK_DeviceFinder.GetControllerIndex(gameObject) + " grip down");
                isAction = true;
                hand.SetActive(false);
                punch.SetActive(true);
                StartCoroutine(generatePunch(punchGenerateTime));
            }

            if (VRTK_SDK_Bridge.IsGripPressedUpOnIndex(VRTK_DeviceFinder.GetControllerIndex(gameObject)))
            {
                //print("Controller " + VRTK_DeviceFinder.GetControllerIndex(gameObject) + " grip up");
                punch.transform.localScale = originalPunchSize;
                punch.SetActive(false);
                hand.SetActive(true);
                isAction = false;
            }
        }
        */
    }

    public IEnumerator generateShield(float generateTime)
    {
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / (generateTime / 2f))
        {
            shield.transform.localScale = new Vector3(Mathf.Lerp(shield.transform.localScale.x, shieldSize, t), shield.transform.localScale.y, shield.transform.localScale.z);
            
            yield return null;
        }

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / (generateTime / 2f))
        {
            shield.transform.localScale = new Vector3(shield.transform.localScale.x, shield.transform.localScale.y, Mathf.Lerp(shield.transform.localScale.z, shieldSize, t));

            yield return null;
        }
    }

    public IEnumerator generatePunch(float generateTime)
    {
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / generateTime)
        {
            punch.transform.localScale = new Vector3(Mathf.Lerp(punch.transform.localScale.x, punchSize, t), Mathf.Lerp(punch.transform.localScale.x, punchSize, t), Mathf.Lerp(punch.transform.localScale.x, punchSize, t));

            yield return null;
        }
    }
}
