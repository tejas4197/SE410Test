using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    private CharacterController charCtrl;
    private Interactable selected = null;
    private bool measureMode;
    public bool interactButtonDown = false;
    private Vector3 cameraMeasurePosition = new Vector3(0, 30, -7.5f);
    private Vector3 cameraSelectPosition = new Vector3(0, 4f, -10);
    public float cameraMoveDuration;
    public float speed;
    public float rotSpeed;
    public GameObject charBody;
    public float selectRange;
    public Camera playerCam;

    // Start is called before the first frame update
    void Start()
    {
        charCtrl = gameObject.GetComponent<CharacterController>();
        measureMode = false;
        UIManager.GetInstance().SetMeasureMode(measureMode);
    }

    // Update is called once per frame
    void Update()
    {
        MoveChar();

        if (!measureMode)
        {
            CheckSelection();
        }

        CheckInteraction();
    }

    private void MoveChar()
    {
        float moveHorizontal = Input.GetAxis("Horizontal") * speed;
        float moveVertical = Input.GetAxis("Vertical") * speed;
        charCtrl.Move(new Vector3(moveHorizontal, 0, moveVertical));
        if (moveHorizontal != 0.0f || moveVertical != 0.0f)
        {
            Quaternion targetRot = Quaternion.LookRotation(new Vector3(moveHorizontal, 0, moveVertical));
            charBody.transform.rotation = Quaternion.RotateTowards(charBody.transform.rotation, targetRot, rotSpeed * UnityEngine.Time.deltaTime);
        }
    }

    private void CheckSelection()
    {
        RaycastHit hit;
        // TODO: We want to change this to be selectable by the mouse instead of raycasting from the player
        // Likely will want to interact with different things
        if (Physics.Raycast(transform.position, charBody.transform.TransformDirection(Vector3.forward), out hit, selectRange))
        {
            if (hit.transform.GetComponent<Interactable>())
            {
                if (selected == null)
                {
                    selected = hit.transform.GetComponent<Interactable>();
                    selected.OnSelected();
                }
                else if (selected != hit.transform.GetComponent<Interactable>())
                {
                    selected.OnDeselected();
                    selected = hit.transform.GetComponent<Interactable>();
                    selected.OnSelected();
                }
            }
            else if (selected != null)
            {
                selected.OnDeselected();
                selected = null;
            }
        }
        else if (selected != null)
        {
            selected.OnDeselected();
            selected = null;
        }
    }

    private void CheckInteraction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (measureMode)
            {
                //measureScript.MeasureTrigger(); TODO: refactor player
            }
            else if (selected)
            {
                selected.OnInteract();
            }
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMeasureMode();

        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (measureMode)
            {
                //measureScript.CancelMeasure();
            }
            else if (selected)
            {
                // open options menu
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!measureMode)
            {
                interactButtonDown = true;
            }
        }
        else
        {
            interactButtonDown = false;
        }
    }

    private void ToggleMeasureMode()
    {
        measureMode = !measureMode;
        UIManager.GetInstance().SetMeasureMode(measureMode);
        if (measureMode)
        {
            StartCoroutine(MoveCamera(cameraMeasurePosition, cameraMoveDuration));
        }
        else
        {
            StartCoroutine(MoveCamera(cameraSelectPosition, cameraMoveDuration));
        }
    }

    IEnumerator MoveCamera(Vector3 targetPos, float duration)
    {
        Vector3 startPos = playerCam.transform.localPosition;
        float timer = 0;
        while (timer < duration)
        {
            timer += UnityEngine.Time.deltaTime;
            float progress = timer / duration;
            playerCam.transform.localPosition = Vector3.Lerp(startPos, targetPos, progress);
            playerCam.transform.LookAt(transform.position);
            yield return null;
        }
        playerCam.transform.localPosition = Vector3.Lerp(startPos, targetPos, 1.0f);
        playerCam.transform.LookAt(transform.position);
    }
}
