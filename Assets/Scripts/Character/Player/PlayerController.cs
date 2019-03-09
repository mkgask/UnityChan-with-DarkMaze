using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UniRx;
using UniRx.Triggers;
using sgffu.EventMessage;
using sgffu.Characters;
using sgffu.Characters.Input;
using sgffu.Characters.Player;
using EquipService = Item.Equip.Service;
using EffectService = Effect.Service;
using CharacterEntity = sgffu.Characters.CharacterEntity;
using SeService = sgffu.Se.Service;



public class PlayerController : MonoBehaviour, IControlCharacterAction
{
    PlayerInputControlAction input_controller;

    //EquipService item_equip_service;

    private bool in_action = false;

    private bool window_open = false;

    private bool dash_key = false;
    private bool up_key = false;
    private bool down_key = false;
    private bool left_key = false;
    private bool right_key = false;

    private const float animation_trigger_do_dash = 0.01f;

    private Vector3 na_destination_offset = new Vector3(0f, 0.5f, 0f);

    private Vector3 hit_cast_offset = new Vector3(0f, 0.5f, 0f);

    private const int hp_dead_line = 0;

    private Animator animator;

    public NavMeshAgent nmAgent;

    private bool do_attack = false;

    private State state = State.Idle;

    private bool character_control = true;

    private GameObject enemy_target;

    [SerializeField]
    private float enemy_target_distance = 1f;

    [SerializeField] private int attack_speed = 500;
    private int max_attack_speed = 1;

    private Vector3 default_position = new Vector3(1.5f, 0.5f, -3.5f);

    private bool initialized = false;

    private Queue<GameObject> enemy_history = new Queue<GameObject>();


    void OnDisable()
    {
        if (input_controller != null) input_controller.stop();
    }


    void Start()
    {
        //Debug.Log("PlayerController.Start()");

        //item_equip_service = new EquipService();
        input_controller = new PlayerInputControlAction(this);
        nmAgent = gameObject.GetComponent<NavMeshAgent>();
        animator = gameObject.GetComponent<Animator>();

        var default_character = new CharacterEntity();
        animator.SetInteger("hp", default_character.hp);

        animator.radioBool("Idle");
        state = State.Active;
        max_attack_speed = Mathf.RoundToInt(attack_speed * 0.8f);

/*
        Observable.Zip(
            MessageBroker.Default.Receive<CreatedGameStageRank>(),
            MessageBroker.Default.Receive<CreatedEquipBaseResouces>(),
            MessageBroker.Default.Receive<InputInitialized>(),
            MessageBroker.Default.Receive<EffectInitialized>(),
            MessageBroker.Default.Receive<CreatedPlayerService>(),
            Tuple.Create
        ).Subscribe(x => {
            initialized = true;
        });
*/

        initialized = true;

/*
        MessageBroker.Default.Receive<CreatedPlayerService>().Subscribe(x => {
            initialized = true;
        });
*/
        /*
        // Update
        this.UpdateAsObservable().Subscribe(x => {
            if (!character_control) { return; }

            animator.SetBool("DoDash", nmAgent.remainingDistance > animation_trigger_do_dash);
            //if (this.in_action) { animator.SetBool("DoIdle", false); }

            if (!nmAgent.pathPending && !nmAgent.hasPath) {
                nmAgent.ResetPath();
            }

            if (enemy_target != null) {
                transform.LookAt(enemy_target.transform.position);

                if (Vector3.Distance(transform.position, enemy_target.transform.position) < enemy_target_distance) {
                    nmAgent.ResetPath();
                    OnAttack();
                }
            }
        });
        */
    }

    void Update()
    {
        if (!initialized) return;

        animator.SetInteger("hp", PlayerService.status("HP"));
        if (!this.character_control) return;
        if (this.state == State.Dead) return;

        //animator.SetBool("DoDash", nmAgent.remainingDistance > animation_trigger_do_dash);
        //if (this.in_action) { animator.SetBool("DoIdle", false); }

        //Debug.Log("nmAgent.remainingDistance : " + nmAgent.remainingDistance.ToString());

        if (nmAgent.remainingDistance > animation_trigger_do_dash) {
            //animator.radioBool("Dash");
            AnimationChange("Dash");
        } else {
            //animator.radioBool("Idle");
            AnimationChange("Idle");
        }

        if (!nmAgent.pathPending && !nmAgent.hasPath) {
            nmAgent.ResetPath();
        }

        if (enemy_target != null) {
            transform.LookAt(enemy_target.transform.position);

            if (Vector3.Distance(transform.position, enemy_target.transform.position) < enemy_target_distance) {
                nmAgent.ResetPath();
                OnAttack();
            }
        }
    }

    public void positionReset()
    {
        //Debug.Log("PlayerController.positionReset()");

        nmAgent.ResetPath();
    
        Observable.NextFrame().Subscribe(x => {
            nmAgent.Warp(default_position);
        });

        //nmAgent.Warp(default_position);
        //transform.position = default_position;
    }

    public void respawn()
    {
        transform.tag = "Player";
        this.state = State.Active;
        this.character_control = true;
        this.in_action = false;
        this.animator.SetInteger("hp", PlayerService.status("HP"));
        this.animator.radioBool("Idle");
        //OnIdle();
    }

    public void AnimationChange(string animationName)
    {
        if (transform == null || transform.tag != "Player") return;
        animator.radioBool(animationName);
    }

    public void PlaySE(string se_name)
    {
        if (transform.tag != "Player") return;
        MessageBroker.Default.Publish<PlaySe>(new PlaySe{ name = se_name });
    }



    //private List<DebugCapsuleEntity> debug_capsule = new List<DebugCapsuleEntity>();
    private List<DebugSphereEntity> debug_sphere = new List<DebugSphereEntity>();

    private float debug_remaining_time_default = 1f;

    private Color debug_object_color = Color.white;



    public void OnDrawGizmos() {
/*
        if (debug_capsule.ToArray().Length < 1) return;

        foreach (var dc in debug_capsule) {
            Debug.Log("OnDrawGizmos.DebugCapsule : " + dc.position + " : to : " + dc.end);

            // Position moving
            dc.position = Vector3.SmoothDamp(
                dc.position,
                dc.end,
                ref dc.velocity,
                debug_remaining_time_default
            );

            // Color moving
            dc.draw_color_alpha = dc.remaining_time / debug_remaining_time_default;
            var c = debug_object_color;
            c.a = dc.draw_color_alpha;
            Gizmos.color = c;

            Gizmos.DrawWireSphere(dc.position, dc.radius);

            // Post process
            dc.remaining_time -= Time.deltaTime;
        }

        foreach (var dc in debug_capsule.Where(x => x.remaining_time <= 0)) {
            debug_capsule.Remove(dc);
        }
*/
        if (debug_sphere.ToArray().Length < 1) return;

        foreach (var ds in debug_sphere.Where(x => x.enable)) {
            //Debug.Log("OnDrawGizmos.DebugSphere : " + ds.position + " : to : " + ds.target_position);

            // Pre process
            //if (!ds.enable) continue;

            // Position moving
            ds.position = Vector3.SmoothDamp(
                ds.position,
                ds.target_position,
                ref ds.velocity,
                debug_remaining_time_default
            );

            // Color moving
            ds.draw_color_alpha = ds.remaining_time / debug_remaining_time_default;
            var c = debug_object_color;
            c.a = ds.draw_color_alpha;
            Gizmos.color = c;

            // Draw
            Gizmos.DrawWireSphere(ds.position, ds.radius);

            // Post process
            ds.remaining_time -= Time.deltaTime;
        }

        foreach (var ds in debug_sphere.Where(x => x.remaining_time <= 0)) {
            ds.enable = false;
            debug_sphere.Remove(ds);
        }
    }

/*
    public void SphereView(Ray ray, float radius, float maxDistance = Mathf.Infinity) {
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(ray.origin, radius);
        Gizmos.DrawSphere(ray.origin, radius / 10);
    }
*/


    public void DebugSphere(Ray ray, float radius, float max_distance)
    {
        //Debug.Log("DebugSphere");
        
        debug_sphere.Add(new DebugSphereEntity {
            enable = true,
            position = ray.origin,
            radius = radius,
            max_distance = max_distance,
            target_position = ray.origin + (ray.direction * max_distance),
            draw_color_alpha = 1f,
            remaining_time = debug_remaining_time_default,
        });
    }


/*
    public void DebugCapsule(Vector3 start, Vector3 end, float radius) {
        debug_capsule.Add(new DebugCapsuleEntity {
            enable = true,
            position = start,
            end = end,
            radius = radius,
            draw_color_alpha = 1f,
            remaining_time = debug_remaining_time_default,
        });
    }
*/
    public void Hit()
    {
        //Debug.Log("PlayerController.Hit()");

        var check_start_offset = 0.3f;

        var check_max_distance = 1.5f;

        var check_radius = 0.1f;

        RaycastHit rayHit = default(RaycastHit);

        Ray ray = new Ray(
            transform.position + 
                new Vector3(0f, 0.5f, 0f) +
                transform.forward * -check_start_offset,
            transform.forward
        );

        int layer_mask = (1 << LayerMask.NameToLayer("Enemy"));
        //int layer_mask = LayerMask.NameToLayer("Enemy");
        bool onhit = false;

        if (enemy_target != null) {
            var ec = enemy_target.GetComponent<EnemyController>();
            check_max_distance = ec.attack_distance + check_start_offset;

            //DebugCapsule(check_start, check_end, check_radius);
            //DebugSphere(ray, check_radius, check_max_distance);

            if (Physics.SphereCast(ray, check_radius, out rayHit, check_max_distance, layer_mask)) {
                Debug.Log("Physics.CheckCapsule hit");
                onhit = true;
            } else {
                Debug.Log("Physics.SphereCast not hit");
            }
        }

        if (onhit) {
            var se_name = SeService.getPlayerAttack(EquipService.rank(), EquipService.type());
            //MessageBroker.Default.Publish<PlaySe>(new PlaySe{ name = se_name });
            PlaySE(se_name);

            rayHit.transform.SendMessage("OnDamage", PlayerService.atk_total());
        } else {
            // miss 
            if (enemy_target != null) {
                enemy_target.transform.SendMessage("OnMiss");
            }
        }
    }

    public void OnDamage(int enemy_atk)
    {
        var damage = PlayerService.getDamage(enemy_atk);
        //Debug.Log("EnemyController: OnDamage: damage: " + damage);

        EffectService.damage(damage, transform.position);

        // ダメージがゼロならエフェクト発生のみで終了
        if (damage == 0) return;

        // ダメージあればHP減少処理
        var remain = PlayerService.OnDamage(damage);
        //Debug.Log("EnemyController: OnDamage: hp_dead_line: " + hp_dead_line);
        //Debug.Log("EnemyController: OnDamage: remain: " + remain);

        if (remain <= hp_dead_line) {
            OnDead();
            //Debug.Log("PlayerController::OnDamage : state : " + state.ToString());
            return;
        }

        var voice_name_damage = SeService.getPlayerDamageVoice();
        //MessageBroker.Default.Publish<PlaySe>(new PlaySe{ name = voice_name_damage });
        PlaySE(voice_name_damage);

        //animator.radioBool("Damage");
        AnimationChange("Damage");
    }

    public void OnDead()
    {
        state = State.Dead;
        character_control = false;
        nmAgent.ResetPath();

        var voice_name_dead = SeService.getPlayerDeadVoice();
        MessageBroker.Default.Publish<PlaySe>(new PlaySe{ name = voice_name_dead });

        transform.tag = "PlayerDead";
        //animator.radioBool("Die");
        MessageBroker.Default.Publish<PlayerDead>(new PlayerDead{});
        //Debug.Log("Publish: PlayerDead");

        PlayerService.game_over();
        EffectService.nextRoomOff();
    }

    public void OnDamageActionEnd()
    {
        //Debug.Log("OnDamageActionEnd");
        //animator.radioBool("Idle");
        AnimationChange("Idle");
    }

    public void OnMiss()
    {
        EffectService.miss(gameObject.transform.position);
        //Debug.Log("EnemyController: OnMiss()");
    }

    public void OnLvUP()
    {
        MessageBroker.Default.Publish<SaveDataSave>(new SaveDataSave());
        EffectService.lvup(gameObject.transform.position);
        //Debug.Log("EnemyController: OnMiss()");
    }

    public void OnEnemyDead()
    {
        MessageBroker.Default.Publish<SaveDataSave>(new SaveDataSave());
        //Debug.Log("PlayerController.OnEnemyDead()");
        //animator.radioBool("Idle");
        AnimationChange("Idle");
        enemy_target = null;
    }

    public void OnCallChangeFace()
    {
        
    }

    void OnAnimatorMove()
    {
        // Update position to agent position
        transform.position = nmAgent.nextPosition;
    }


    private bool on_attack = false;

    void OnAttack()
    {
/*
        if (!animator.GetBool("DoAttack")) {
            animator.SetBool("DoAttack", true);
        }
*/

        if (on_attack) return;
        on_attack = true;

        character_control = false;

        //Debug.Log("PlayerController.OnIdle()");
        //animator.radioBool("Attack");
        AnimationChange("Attack");
        //animator.SetBool("DoIdle", true);
        animator.Update(0);
        AnimatorClipInfo[] anim_clip_info = animator.GetCurrentAnimatorClipInfo(0);
        //Debug.Log("PlayerController : anim_clip_info[0].clip.length : " + (anim_clip_info[0].clip.length).ToString());

        int spd = PlayerService.spd_total();
        //Debug.Log("PlayerController : spd before : " + spd.ToString());

        spd = (max_attack_speed < spd) ? max_attack_speed : spd;
        spd = (spd < 1) ? 1 : spd;
        //Debug.Log("PlayerController : spd after : " + spd.ToString());

        float attack_spd = (((attack_speed * 1f) - (spd * 1f)) / (attack_speed * 1f)) * EquipService.attack_speed_adjust();
        //Debug.Log("PlayerController : attack_speed : " + attack_speed.ToString());
        //Debug.Log("PlayerController : spd : " + spd.ToString());
        //Debug.Log("PlayerController : (attack_speed - spd) / attack_speed : " + ((attack_speed - spd) / attack_speed).ToString());
        //Debug.Log("PlayerController : attack_spd : " + attack_spd.ToString());

        animator.SetFloat("AttackSpeed", attack_spd);

        //animator.Play("Idle2", 0, 0f);
        //Debug.Log("PlayerController::OnAttack() anim_clip_info[0].clip.length: " + anim_clip_info[0].clip.length.ToString());

        Observable.Timer(TimeSpan.FromSeconds(anim_clip_info[0].clip.length))
            .Subscribe(t => {
                //animator.radioBool("Idle");
                AnimationChange("Idle");
                character_control = true;
                on_attack = false;
            });
            //.Subscribe(t => animator.SetBool("DoIdle", false));

/*
        animator.Play("Attack",
            nextFrameAction: () => {
                //Debug.Log("再生開始の次フレーム");
            },
            endAction: () => {
                //Debug.Log("アニメーション終了");
                animator.radioBool("Idle");
                character_control = true;
            });
*/
        var voice_name_attack = SeService.getPlayerAttackVoice();
        //MessageBroker.Default.Publish<PlaySe>(new PlaySe{ name = voice_name_attack });
        PlaySE(voice_name_attack);
    }


    public void OnMainActionAttack(RaycastHit rayHit)
    {
        enemy_target = rayHit.transform.gameObject;
        Debug.Log("new target : " + enemy_target.name);
        enemy_target.SendMessage("OnTarget");
    }

    public void OnMainActionEquip(RaycastHit rayHit)
    {
        rayHit.transform.gameObject.SendMessage("OnTarget");
        EquipService.OnTarget(rayHit.collider.gameObject);

        if (enemy_target != null) {
            enemy_target.SendMessage("TargetDisable");
            enemy_target = null;
        }
    }

    public void OnMainAciton() 
    {
        if(this.in_action) return;
        this.in_action = true;

        //if (!this.character_control) return;
        if (this.state == State.Dead) return;
        //if (!character_control) return;

        //RaycastHit rayHit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.Log("PlayerController.OnMainAciton() : Input.mousePosition: " + Input.mousePosition);

        int layer_mask = (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("ItemEquip")) | (1 << LayerMask.NameToLayer("Enemy"));

        RaycastHit[] hits = Physics.RaycastAll(ray, 100, layer_mask);
        RaycastHit enemy_hit = new RaycastHit();
        RaycastHit item_hit = new RaycastHit();
        RaycastHit floor_hit = new RaycastHit();

        foreach (var ray_hit in hits) {
            //Debug.Log("PlayerController.OnMainAciton() : ray_hit.point: " + ray_hit.point);
            if (ray_hit.collider == null) continue;

            if (ray_hit.transform.tag == "Enemy" && enemy_hit.collider == null) {
                enemy_hit = ray_hit;
            } else
            if (ray_hit.transform.tag == "ItemEquip" && item_hit.collider == null) {
                item_hit = ray_hit;
            }

            if (floor_hit.collider == null) {
                floor_hit = ray_hit;
            }
        }
/*
        if (5 < enemy_history.Count) {
            var max = enemy_history.Count;
            for (var count = 5; count < max; count += 1) {
                var de = enemy_history.Dequeue();
                if (de != null) de.SendMessage("TargetDisable");
            }
        }
*/
        if (enemy_hit.collider != null) {
            if (enemy_target == null || enemy_target != enemy_hit.transform.gameObject) {
                //if (enemy_target != null) { enemy_target.SendMessage("TargetDisable"); }
                //enemy_history.Enqueue(enemy_hit.transform.gameObject);
                enemy_target = enemy_hit.transform.gameObject;

                foreach (var obj in GameObject.FindGameObjectsWithTag("Enemy")) {
                    if (obj == enemy_target) {
                        obj.SendMessage("OnTarget");
                    } else {
                        obj.SendMessage("TargetDisable");
                    }
                }

                var ec = enemy_target.GetComponent<EnemyController>();
                enemy_target_distance = ec.attack_distance;
                Debug.Log("enemy_target_distance : " + enemy_target_distance);

                enemy_target.SendMessage("OnTarget");

                if (enemy_target_distance < Math.Abs(Vector3.Distance(enemy_target.transform.position, transform.position))) {
                    nmAgent.ResetPath();

                    Observable.NextFrame().Subscribe(x => {
                        if (nmAgent == null || !nmAgent.enabled) return;
                        nmAgent.SetDestination(enemy_target.transform.position);
                    });

                    AnimationChange("Idle");
                }

            }
            //Debug.Log("new target : " + enemy_target.name);
        } else
        if (item_hit.collider != null) {
            item_hit.transform.gameObject.SendMessage("OnTarget");
            EquipService.OnTarget(item_hit.collider.gameObject);

            if (enemy_target != null) {
                enemy_target.SendMessage("TargetDisable");
                enemy_target = null;
            }
        } else {
            if (enemy_target != null) {
                enemy_target.SendMessage("TargetDisable");
                enemy_target = null;
            }
        }

        if (enemy_hit.collider == null && floor_hit.collider != null) {
            nmAgent.ResetPath();

            Observable.NextFrame().Subscribe(x => {
                nmAgent.SetDestination(floor_hit.point + na_destination_offset);
            });

            //animator.radioBool("Idle");
            //AnimationChange("Idle");
        }

        AnimationChange("Dash");
        this.in_action = false;

/*
        if (Physics.Raycast(ray, out rayHit, 100, layer_mask)) {
            Debug.Log("rayHit.transform.gameObject.layer : " + rayHit.transform.gameObject.layer);
            Debug.Log("rayHit.transform.tag : " + rayHit.transform.tag);

            if (rayHit.transform.tag == "Enemy") {
                enemy_target = rayHit.transform.gameObject;
                Debug.Log("new target : " + enemy_target.name);
                enemy_target.SendMessage("OnTarget");
            } else
            if (rayHit.transform.tag == "ItemEquip") {
                rayHit.transform.gameObject.SendMessage("OnTarget");
                EquipService.OnTarget(rayHit.collider.gameObject);

                if (enemy_target != null) {
                    enemy_target.SendMessage("TargetDisable");
                    enemy_target = null;
                }
            } else {
                if (enemy_target != null) {
                    enemy_target.SendMessage("TargetDisable");
                    enemy_target = null;
                }
            }

            if (rayHit.collider != null) {
                //Debug.Log("rayHit.point: " + rayHit.point);
                //nmAgent.destination = rayHit.point;
                nmAgent.ResetPath();
                nmAgent.SetDestination(rayHit.point + na_destination_offset);
                animator.SetBool("DoIdle", false);

                //Debug.Log("PlayerController::OnMainAciton() : rayHit.collider.gameObject.name : " + rayHit.collider.gameObject.name);
                //Debug.Log("PlayerController::OnMainAciton() : rayHit.collider.gameObject.tag : " + rayHit.collider.gameObject.tag);
            }
        }
*/
    }

    public void OnSubAction()
    {
        if(this.in_action) { return; }
        this.in_action = true;
        this.in_action = false;
    }

    public void OnFocusAction()
    {
        if(this.in_action) { return; }
        this.in_action = true;
        this.in_action = false;
    }

    public void OnJump()
    {
        if(this.in_action) { return; }
        this.in_action = true;
        this.in_action = false;
    }

    public void OnDash()
    {
        this.dash_key = true;
    }

    public void OnRoll()
    {
        if(this.in_action) { return; }
        this.in_action = true;
        this.in_action = false;
    }

    public void OnReload()
    {
        if(this.in_action) { return; }
        this.in_action = true;
        this.in_action = false;
    }

    public void OnCrouch()
    {
        if(this.in_action) { return; }
        this.in_action = true;
        this.in_action = false;
    }

    public void OnLayDown()
    {
        if(this.in_action) { return; }
        this.in_action = true;
        this.in_action = false;
    }

    public void OnUp()
    {
        this.up_key = true;
    }

    public void OnDown()
    {
        this.down_key = true;
    }

    public void OnLeft()
    {
        this.left_key = true;
    }

    public void OnRight()
    {
        this.right_key = true;
    }

    public void OnMenuOpen()
    {
        if(this.window_open) { return; }
        this.window_open = true;
        this.window_open = false;
    }

    public void OnInventoryOpen()
    {
        if(this.window_open) { return; }
        this.window_open = true;
        this.window_open = false;
    }

    public void OnStatusOpen()
    {
        if(this.window_open) { return; }
        this.window_open = true;
        this.window_open = false;
    }

    public void OnSkillOpen()
    {
        if(this.window_open) { return; }
        this.window_open = true;
        this.window_open = false;
    }

    public void OnMapOpen()
    {
        if(this.window_open) { return; }
        this.window_open = true;
        this.window_open = false;
    }

    public void OnIdle()
    {
        //Debug.Log("PlayerController.OnIdle()");
        //animator.radioBool("Wait");
        AnimationChange("Wait");
        //animator.SetBool("DoIdle", true);
        animator.Update(0);
        AnimatorStateInfo anim_info = animator.GetCurrentAnimatorStateInfo(0);

        //animator.Play("Idle2", 0, 0f);
        Observable.Timer(TimeSpan.FromMilliseconds(anim_info.length * 2000d))
            .Subscribe(t => AnimationChange("Idle"));
            //.Subscribe(t => animator.radioBool("Idle"));
            //.Subscribe(t => animator.SetBool("DoIdle", false));
    }



    public void OffDash()
    {
        this.dash_key = false;
    }

    public void OffUp()
    {
        this.up_key = false;
    }

    public void OffDown()
    {
        this.down_key = false;
    }

    public void OffLeft()
    {
        this.left_key = false;
    }

    public void OffRight()
    {
        this.right_key = false;
    }
    
}
