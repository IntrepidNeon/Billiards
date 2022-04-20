using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public Rigidbody BallBody;

    public GameObject StickObject;
    private GameObject Stick = null;

    public GameObject TargetSphereObject;
    private GameObject TargetSphere = null;

    public Camera GameCamera;
    public Camera OrthoCamera;

    public Transform CameraPivot;

    public float Sensitivity;

    private float CameraPitch;
    private float CameraYaw;

    public bool Aiming;
    public bool Reeling;

    private float ReelOffset;

    private Vector3 stickPos;

    // Start is called before the first frame update
    void Start()
    {
        CameraPitch = transform.eulerAngles.x;
        CameraYaw = transform.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (BallBody.position.y > BallBody.transform.localScale.y)
        {
            BallBody.velocity = new Vector3(BallBody.velocity.x, 0f, BallBody.velocity.z);
            BallBody.position = new Vector3(BallBody.position.x, BallBody.transform.localScale.x / 2, BallBody.position.z)
;
        }
        if (BallBody.position.y < -BallBody.transform.localScale.y)
        {
            ScoreBoard.Score--;
            BallBody.position = new Vector3(0f, BallBody.transform.localScale.y / 2, 0f);
            BallBody.velocity = new Vector3(0f, 0f, 0f);
            BallBody.angularVelocity = new Vector3(0f, 0f, 0f);
        }
        Aiming = Input.GetAxisRaw("Fire2") > 0 ? true : false;
        Reeling = Input.GetAxisRaw("Fire1") > 0 && Aiming ? true : false;

        if (BallBody.angularVelocity.magnitude < 0.01 && BallBody.velocity.magnitude < 0.01)
        {
            GameCamera.gameObject.SetActive(true);
            OrthoCamera.gameObject.SetActive(false);

            if (!Aiming)
            {
                Cursor.lockState = CursorLockMode.Locked;
                CameraOperator();
                Cleanup();
            }
            else
            {
                Cursor.lockState = CursorLockMode.Confined;
                if (!Reeling)
                {
                    Aim();
                    Destroy(Stick);
                }
                else if (Reeling && TargetSphere != null)
                {
                    Reel();
                }

            }
        }
        else
        {
            GameCamera.gameObject.SetActive(false);
            OrthoCamera.gameObject.SetActive(true);
        }

    }

    void CameraOperator()
    {
        CameraPivot.transform.position = BallBody.position;

        CameraPitch -= Input.GetAxis("Mouse Y") * Sensitivity % 360;
        CameraYaw += Input.GetAxis("Mouse X") * Sensitivity % 360;

        CameraPitch = Mathf.Clamp(CameraPitch, 0, 60);

        CameraPivot.eulerAngles = new Vector3(CameraPitch, CameraYaw, 0f);
    }

    void Aim()
    {
        Ray CamRay = GameCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit Hit;
        Physics.Raycast(CamRay.origin, CamRay.direction, out Hit, 10f);
        if (Hit.transform != null && Hit.transform.gameObject.CompareTag("Player"))
        {
            if (TargetSphere == null)
            {
                TargetSphere = Instantiate(TargetSphereObject);
            }
            TargetSphere.transform.position = Hit.point;
        }
        else
        {
            Destroy(TargetSphere);
        }
    }
    void Reel()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (Stick == null)
        {
            ReelOffset = -0.01f;
            Stick = Instantiate(StickObject);
            Stick.transform.localPosition = new Vector3(
            TargetSphere.transform.position.x - (Mathf.Sin(CameraYaw * Mathf.Deg2Rad) * Stick.transform.localScale.y),
            TargetSphere.transform.position.y,
            TargetSphere.transform.position.z - (Mathf.Cos(CameraYaw * Mathf.Deg2Rad) * Stick.transform.localScale.y));
            stickPos = Stick.transform.localPosition;
        }
        ReelOffset += Input.GetAxis("Mouse Y") / Sensitivity;

        ReelOffset = Mathf.Clamp(ReelOffset, -1f, 0.1f);

        Stick.transform.eulerAngles = new Vector3(90f, CameraYaw, 0f);
        Stick.transform.localPosition = new Vector3(
            TargetSphere.transform.position.x - (Mathf.Sin(CameraYaw * Mathf.Deg2Rad) * (Stick.transform.localScale.y - ReelOffset)),
            TargetSphere.transform.position.y,
            TargetSphere.transform.position.z - (Mathf.Cos(CameraYaw * Mathf.Deg2Rad) * (Stick.transform.localScale.y - ReelOffset)));
        if (stickPos != Stick.transform.localPosition)
        {
            if (ReelOffset >= 0f && BallBody.velocity.magnitude == 0)
            {
                BallBody.AddForce((
                    new Vector3(BallBody.transform.position.x, 0f, BallBody.transform.position.z) - new Vector3(TargetSphere.transform.position.x, 0f, TargetSphere.transform.position.z)).normalized
                    * Mathf.Clamp((Stick.transform.localPosition - stickPos).magnitude * 60, 0f, 100f), ForceMode.VelocityChange);
                Invoke("Cleanup", 1);
            }
            stickPos = Stick.transform.localPosition;
        }
    }
    void Cleanup()
    {
        if (TargetSphere != null)
        {
            Destroy(TargetSphere);
        }
        if (Stick != null)
        {
            Destroy(Stick);
        }
    }
    public GameObject GetTargetObject()
    {
        return TargetSphere;
    }
    public float GetReelOffset()
    {
        return ReelOffset;
    }
}
