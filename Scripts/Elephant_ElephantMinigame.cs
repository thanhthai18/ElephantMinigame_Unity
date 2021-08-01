using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Elephant_ElephantMinigame : MonoBehaviour
{
    public Button btnLake;
    [SerializeField] private float speed;
    private Vector2 direction;
    public SkeletonAnimation anim;
    [SpineAnimation] public string anim_NghichBong, anim_DiChuyen, anim_HutNuoc, anim_PhunNuoc, anim_ChienThang;
    public GameController_ElephantMinigame controller;
    private bool isMove = false;
    

    public float Speed { get => speed; set => speed = value; }


    private void Start()
    {
        anim.state.Complete += AnimComplete;
        Speed = 1.5f;
        PlayAnim(anim, anim_NghichBong, true);
        btnLake.onClick.AddListener(MoveAnimActive);
    }

    private void AnimComplete(Spine.TrackEntry trackEntry)
    {
        //if (trackEntry.Animation.Name == anim_DiChuyen)
        //{
        //    PlayAnim(anim, anim_HutNuoc, true);
        //}
    }

    public void PlayAnim(SkeletonAnimation anim, string nameAnim, bool loop)
    {
        anim.state.SetAnimation(0, nameAnim, loop);
    }

    public void MoveAnimActive()
    {
        PlayAnim(anim, anim_DiChuyen, true);
    }

    public void Move()
    {
        transform.Translate(new Vector3(2, 0, 0) * Speed * Time.deltaTime);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            Speed = 0f;
            isMove = false;
        }
    }


    private void Update()
    {
        if (controller.IsLake)
        {
            if (Speed != 0)
            {
                isMove = true;
            }
            else
            {
                PlayAnim(anim, anim_HutNuoc, false);
            }
        }
        if (isMove)
        {
            Move();
        }
    }


}
