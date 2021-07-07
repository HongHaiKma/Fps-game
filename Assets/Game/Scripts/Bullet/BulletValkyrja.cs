using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BulletValkyrja : Bullet
{
    public int m_TargetIndex;

    public Vector3 v3_Target;
    public bool m_MoveDown;

    public override void SetupBullet(BulletConfigData _bulletInfor, Vector3 _offset, float _speed)
    {
        base.SetupBullet(_bulletInfor, _offset, _speed);
        LaunchRocket(_offset);
    }

    public override void OnEnable()
    {
        m_MoveDown = true;
        base.OnEnable();
    }

    public override void FixedUpdate()
    {
        if (m_MoveDown)
        {
            tf_Onwer.position += tf_Onwer.forward * m_MoveSpd * Time.fixedDeltaTime;

            m_FlyingTime += Time.fixedDeltaTime;

            if (m_FlyingTime >= m_FlyingTimeMax)
            {
                PrefabManager.Instance.DespawnPool(gameObject);
            }
        }
    }

    // public bool IsHasTarget()
    // {
    //     return tf_Target;
    // }

    public void LaunchRocket(Vector3 _offset)
    {
        // DOTween.To(
        //     () =>
        //     _offset,
        //     x => _offset = x, Vector3.zero, 0.5f).SetEase(Ease.InCirc)
        //     .OnUpdate(
        //         () =>
        //         {
        //             Vector3 v33 = v3_Rotation + _offset;
        //             tf_Onwer.LookAt(v33);
        //         }
        //     ).
        //     OnComplete(() =>
        //     {
        //         m_MoveDown = false;

        //         TEAM team = GetTeam();
        //         if (team == TEAM.Team1)
        //         {
        //             List<Character> listChar = ObjectsManager.Instance.m_Team2;
        //             v3_Target = listChar[Random.Range(0, listChar.Count)].m_BodyPart.transform.position;
        //         }
        //         else
        //         {
        //             List<Character> listChar = ObjectsManager.Instance.m_Team1;
        //             v3_Target = listChar[Random.Range(0, listChar.Count)].m_BodyPart.transform.position;
        //         }

        //         tf_Onwer.LookAt(v3_Target + new Vector3(0f, 10f, 0f));

        //         tf_Onwer.DOMove(v3_Target + new Vector3(0f, 10f, 0f), 4f).OnComplete(() =>
        //         {
        //             tf_Onwer.LookAt(v3_Target);
        //             m_MoveDown = true;
        //         });
        //     });

        m_MoveDown = false;

        TEAM team = GetTeam();
        if (team == TEAM.Team1)
        {
            List<Character> listChar = ObjectsManager.Instance.m_Team2;
            v3_Target = listChar[m_TargetIndex].m_BodyPart.transform.position;
        }
        else
        {
            List<Character> listChar = ObjectsManager.Instance.m_Team1;
            v3_Target = listChar[m_TargetIndex].m_BodyPart.transform.position;
        }

        // DOTween.To(
        //     () =>
        //     _offset,
        //     x => _offset = x, Vector3.zero, 0.5f).SetEase(Ease.InCirc)
        //     .OnUpdate(
        //         () =>
        //         {
        //             Vector3 v33 = v3_Rotation + _offset;
        //             tf_Onwer.LookAt(v33);
        //         }
        //     ).
        //     OnComplete(() =>
        //     {
        //         m_MoveDown = false;

        //         TEAM team = GetTeam();
        //         if (team == TEAM.Team1)
        //         {
        //             List<Character> listChar = ObjectsManager.Instance.m_Team2;
        //             v3_Target = listChar[Random.Range(0, listChar.Count)].m_BodyPart.transform.position;
        //         }
        //         else
        //         {
        //             List<Character> listChar = ObjectsManager.Instance.m_Team1;
        //             v3_Target = listChar[Random.Range(0, listChar.Count)].m_BodyPart.transform.position;
        //         }

        //         tf_Onwer.LookAt(v3_Target + new Vector3(0f, 10f, 0f));

        //         tf_Onwer.DOMove(v3_Target + new Vector3(0f, 10f, 0f), 4f).OnComplete(() =>
        //         {
        //             tf_Onwer.LookAt(v3_Target);
        //             m_MoveDown = true;
        //         });
        //     });

        tf_Onwer.LookAt(tf_Onwer.position + new Vector3(0f, 1f, 0f));

        tf_Onwer.DOMove(tf_Onwer.position + new Vector3(0f, 1f, 0f), 0.5f).OnComplete(() =>
        {
            tf_Onwer.LookAt(v3_Target + new Vector3(0f, 10f, 0f));

            tf_Onwer.DOMove(v3_Target + new Vector3(0f, 10f, 0f), 2f).OnComplete(() =>
            {
                tf_Onwer.LookAt(v3_Target);
                m_MoveDown = true;
            });
        });

        // TEAM team = GetTeam();
        // if (team == TEAM.Team1)
        // {
        //     List<Character> listChar = ObjectsManager.Instance.m_Team2;
        //     tf_Target = listChar[Random.Range(0, listChar.Count)].m_BodyPart.transform;
        // }
        // else
        // {
        //     List<Character> listChar = ObjectsManager.Instance.m_Team1;
        //     tf_Target = listChar[Random.Range(0, listChar.Count)].m_BodyPart.transform;
        // }
        // tf_Onwer.DOMove();
    }
}
