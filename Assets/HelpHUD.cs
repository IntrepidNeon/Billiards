using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class HelpHUD : MonoBehaviour
{
    public BallController PlayerController;
    public Sprite MouseClickLeft;
    public Sprite MouseClickNone;
    public Sprite MouseClickRight;
    public Sprite MouseClickBoth;

    public Image MouseImage;

    public Text InstructionalText;
    void Update()
    {
        if (PlayerController.BallBody.velocity.magnitude > 0.01f)
        {
            InstructionalText.text = "Great!";
            Invoke("FinishTutorial", 1);

            MouseImage.sprite = MouseClickNone;

        }
        else
        {
            if (!PlayerController.Aiming)
            {
                MouseImage.sprite = MouseClickRight;
                InstructionalText.text = "Hold Right Click To Aim";
            }
            else
            {
                if (PlayerController.Reeling)
                {
                    if (PlayerController.GetReelOffset() < -0.1f)
                    {
                        InstructionalText.text = "Push To Shoot";
                    }
                    else
                    {
                        InstructionalText.text = "Pull Back To Prepare Your Shot";
                    }

                }
                else
                {
                    MouseImage.sprite = MouseClickBoth;
                    InstructionalText.text = "Hold Both Mouse Buttons To Reel";
                }
                if (PlayerController.GetTargetObject() == null)
                {
                    MouseImage.sprite = MouseClickRight;
                    InstructionalText.text = "Move The Cursor To The Ball";
                }

            }
        }

    }
    void FinishTutorial()
    {
        Destroy(this.gameObject);
    }
}
