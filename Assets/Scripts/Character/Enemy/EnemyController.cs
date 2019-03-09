using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using UniRx;
using UniRx.Triggers;
using sgffu.EventMessage;
using PlayerService = sgffu.Characters.Player.PlayerService;
using EnemyEntity = sgffu.Characters.Enemy.Entity;
using EnemyService = sgffu.Characters.Enemy.Service;
using EnemyState = sgffu.Characters.Enemy.State;
using Outline = cakeslice.Outline;
using EffectService = Effect.Service;
using SeService = sgffu.Se.Service;

public class EnemyController : MonoBehaviour
{
    public int id = 0;

    public int player_detect_speed = 1000;

    public float attack_distance = 1f;

    public EnemyEntity entity;

    public NavMeshAgent nmAgent;

    public NavMeshSurface nmSurface;

    private Animator animator;

    private Outline outline;

    private Collider collider;

    private Vector3 player_position = Vector3.zero;

    private EnemyState state = EnemyState.Idle;

    private bool character_control = true;

    private int hp_dead_line = 1;

    void Awake()
    {
        MessageBroker.Default.Receive<PlayerDead>().Subscribe(x => {
            //Debug.Log("Recieve: PlayerDead");
            DoIdleMode();
        });
    }

    void Start()
    {
        player_detect_speed = EnemyService.get_detect_interval(entity.rank);
        nmSurface = GameObject.FindGameObjectWithTag("SystemManager").GetComponent<NavMeshSurface>();
        nmAgent  = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        outline  = GetComponentInChildren<Outline>();
        collider = GetComponent<Collider>();

        Observable.Interval(TimeSpan.FromMilliseconds(player_detect_speed)).Subscribe(x => {
            // プレイヤー位置を取得する
            player_position = PlayerService.position();
        }).AddTo(this);

        character_control = true;
        DoMoveMode();

        //Debug.Log("EnemyController : id : " + id);
        //Debug.Log("EnemyController : entity.name : " + entity.name);

        //this.UpdateAsObservable().ObserveOnMainThread().Subscribe(x => {
        //});

    }

    void Update()
    {
        if (animator) animator.SetInteger("HP", entity.hp);
        if (!character_control) return;

        // プレイヤーと一定距離以内かどうかの判定　true：範囲内　false：範囲外
        //var p_e_distance = Mathf.Abs(Vector3.Distance(transform.position, player_position)) < attack_distance;
        var p_e_distance = Mathf.Abs(Vector3.Distance(transform.position, player_position)) < entity.attack_distance;

        if (state == EnemyState.Move) {
            if (p_e_distance) {
                // 移動中で範囲内なら攻撃モードに移行
                DoAttackMode();
            } else {
                // 移動中で範囲外ならプレイヤーに近づく
                DoMoveMode();
            }
        } else
        if (state == EnemyState.Attack) {
            if (p_e_distance) {
                // 攻撃中で範囲内は攻撃継続
                DoAttackMode();
            } else {
                // 攻撃中に範囲外なら移動モードに移行
                DoMoveMode();
            }
        } else {
            //state = EnemyState.Move;
        }
    }

    private void DoIdleMode()
    {
        state = EnemyState.Idle;
        if (animator) animator.SetTrigger("Move");
        if (nmAgent && nmAgent.enabled) nmAgent.ResetPath();
    }

    private void DoMoveMode()
    {
        // 移動モードに移行
        state = EnemyState.Move;
        if (animator) animator.SetTrigger("Move");

        if (nmAgent) {
            nmAgent.ResetPath();

            if (!nmAgent.SetDestination(player_position)) {
                throw new Exception("NavMeshAgent.SetDestination return false: " + player_position);
            }
        }
    }

    private void DoAttackMode()
    {
        // 攻撃モードに移行
        if (state == EnemyState.Attack) return;
        state = EnemyState.Attack;

        if (animator) {
            animator.SetTrigger("Attack");
            AnimatorClipInfo[] anim_clip_info = animator.GetCurrentAnimatorClipInfo(0);

            Observable.Timer(TimeSpan.FromSeconds(anim_clip_info[0].clip.length))
                .Subscribe(t => {
                    if (state == EnemyState.Dead) return;
                    state = EnemyState.Move;
                }).AddTo(this);

            MessageBroker.Default.Publish<PlaySe>(new PlaySe{ name = entity.attack_voice });
        }

        if (nmAgent) nmAgent.ResetPath();
        transform.LookAt(player_position);

    }

    private void DoDeadMove()
    {
        state = EnemyState.Dead;
        transform.tag = "EnemyDead";
        //if (animator) animator.SetTrigger("Die");
        PlayerService.OnEnemyDead();
        //Debug.Log("PlayerService.OnEnemyDead()");
        outline.enabled = false;
        collider.enabled = false;

        nmAgent.enabled = false;
/*
        Observable.NextFrame().Subscribe(x => {
            nmAgent.radius = 0.08f;
            nmSurface.BuildNavMesh();
        }).AddTo(this);
*/
        MessageBroker.Default.Publish<PlaySe>(new PlaySe{ name = entity.dead_voice });
        PlayerService.add_exp(entity.exp);
    }

    public void OnTarget()
    {
        outline.enabled = true;
        //Debug.Log("EnemyController.OnTarget: tag: " + transform.tag);
    }

    public void TargetDisable()
    {
        outline.enabled = false;
    }



    public void Hit()
    {
        //Debug.Log("PlayerController.Hit()");
        var check_start_offset = 0.3f;

        var check_max_distance = 1.5f;

        var check_radius = 0.1f;

        RaycastHit rayHit;
        
        Ray ray = new Ray(
            transform.position + 
                new Vector3(0f, 0.5f, 0f) +
                transform.forward * -check_start_offset,
            transform.forward
        );


        int layer_mask = (1 << LayerMask.NameToLayer("Player"));
        bool onhit = false;

        if (Physics.SphereCast(ray, check_radius, out rayHit, check_max_distance, layer_mask)) {
        //if (Physics.Raycast(ray, out rayHit, attack_distance, layer_mask)) {
            //Debug.Log("EnemyController.Hit: " + rayHit.transform.tag);
            if (rayHit.transform.tag == "Player") {
                onhit = true;
            } else
            if (rayHit.transform.tag == "PlayerDead") {
                DoIdleMode();
                return;
            }
        }

        if (onhit) {
            var se_name = SeService.getEnemyAttack(entity.rank);
            MessageBroker.Default.Publish<PlaySe>(new PlaySe{ name = se_name });

            rayHit.transform.SendMessage("OnDamage", entity.atk);
            //Debug.Log("OnDamage: " + entity.atk);
        } else {
            // miss 
            PlayerService.OnMiss();
            //Debug.Log("OnMiss");
        }
    }



    public void OnDamage(int player_atk)
    {
        int damage = EnemyService.getDamage(player_atk, entity.def);

        EffectService.damage(damage, transform.position);

        entity.hp -= damage;
        //Debug.Log("EnemyController: OnDamage: damage: " + damage);

        if (entity.hp < hp_dead_line) {
            DoDeadMove();
            return;
        }

        if (animator) {
            animator.SetTrigger("Damage");
            MessageBroker.Default.Publish<PlaySe>(new PlaySe{ name = entity.damage_voice });
        }
    }

    public void OnDamageActionEnd()
    {
        DoMoveMode();
    }

    public void OnMiss()
    {
        EffectService.miss(gameObject.transform.position);
        //Debug.Log("EnemyController: OnMiss()");
    }

}
