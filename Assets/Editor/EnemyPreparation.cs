using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UniRx;
using cakeslice;

using UnityEditor.Animations;

using UnityEngine.AI;

using AnimationClip = UnityEngine.AnimationClip;

public class EnemyPreparation : EditorWindow
{

    private string name;

    private string tag;
    private string layer;

    private GameObject go;
    
    private AnimationClip animation_idle;
    private AnimationClip animation_move;
    private AnimationClip animation_attack;
    private AnimationClip animation_damage;
    private AnimationClip animation_die;

    private string state_name_idle = "Idle";
    private string state_name_move = "Move";
    private string state_name_attack = "Attack";
    private string state_name_damage = "Damage";
    private string state_name_die = "Die";

    private string animation_layer_name_default = "Base Layer";
    private string animation_parameter_name_hp = "HP";
    private string animation_parameter_name_move = "Move";
    private string animation_parameter_name_attack = "Attack";
    private string animation_parameter_name_damage = "Damage";

    int progress = 0;
    int progress_max = 52;



    [MenuItem("Custom/EnemyPreparation")]
    static void ShowWindow()
    {
        EditorWindow.GetWindow<EnemyPreparation>();
    }


    void OnGUI()
    {
        try {
            name = EditorGUILayout.TextField("Name (string)", name);
            tag = EditorGUILayout.TextField("Tag (string)", tag);
            layer = EditorGUILayout.TextField("Layer (string)", layer);

            go = EditorGUILayout.ObjectField("Prefab", go, typeof(GameObject), true) as GameObject;

            animation_idle = EditorGUILayout.ObjectField("Idle Animation", animation_idle, typeof(AnimationClip), true) as AnimationClip;
            animation_move = EditorGUILayout.ObjectField("Move Animation", animation_move, typeof(AnimationClip), true) as AnimationClip;
            animation_attack = EditorGUILayout.ObjectField("Attack Animation", animation_attack, typeof(AnimationClip), true) as AnimationClip;
            animation_damage = EditorGUILayout.ObjectField("Damage Animation", animation_damage, typeof(AnimationClip), true) as AnimationClip;
            animation_die = EditorGUILayout.ObjectField("Die Animation", animation_die, typeof(AnimationClip), true) as AnimationClip;

        } catch (UnityException e) {
            Debug.Log(e);
        }

        if (GUILayout.Button("生成する"))
            OnEnemyObjectCreate();

    }

    void UpdateProgressBar(int append)
    {
        progress += append;
        float p = (float)progress / progress_max;

        EditorUtility.DisplayProgressBar (
            "Progress", 
            progress.ToString() + " / " + progress_max.ToString() + " (" + (p * 100).ToString("F2") + "%)", 
            p
        );
    }

    void ClearProgressBar()
    {
        EditorUtility.ClearProgressBar();
    }



    void OnEnemyObjectCreate()
    {
        progress = 0;
        Debug.Log("OnEnemyObjectCreate");

        // 生成    1

            // gameObjectからPrefabをInstantiate
            var prefab = GameObject.Instantiate(go);

            UpdateProgressBar(1);

        // 基本情報設定    5

            // 保存先のパスを生成
            string create_dir_path = $"{Application.dataPath}/Resources/Character/Enemy/{name}/{name}.prefab";
            Filesystem.Dir.create(create_dir_path);

            string path_dir = $"Assets/Resources/Character/Enemy/{name}";
            string prefab_path = $"{path_dir}/{name}.prefab";
            UpdateProgressBar(1);

            // 名前を設定
            prefab.name = name;
            UpdateProgressBar(1);

            // position, rotationを固定
                // position 0f, 0.5f, 0f
                prefab.transform.position = new Vector3(0f, 0.5f, 0f);
                UpdateProgressBar(1);
                // rotation 0f, 0f, 0f
                prefab.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                UpdateProgressBar(1);

            // Tag Enemy
            prefab.tag = tag;
            UpdateProgressBar(1);

            // Layer Enemy
            prefab.layer = LayerMask.NameToLayer(layer);
            UpdateProgressBar(1);

        // Animation    29

            // Animator持ってなかったら
            var anim = prefab.GetComponent<Animator>();

            if (anim == null) {
                // Animator追加
                anim = prefab.AddComponent<Animator>();
            }

            UpdateProgressBar(1);

            // AnimationController生成
            //string anim_path = $"{Application.dataPath}/Resources/Character/Enemy/{name}/{name}_Animator.controller";
            string anim_path = $"{path_dir}/{name}_Animator.controller";
            //string path = Application.dataPath + "/" + name + "/" + name + "_Animator.controller";
            anim.runtimeAnimatorController = AnimatorController.CreateAnimatorControllerAtPath(anim_path);
            if (anim.runtimeAnimatorController == null) throw new System.Exception("Failed to Animator Controller Creation.");
            UpdateProgressBar(1);

            // AnimatorからAnimationController取得
            var controller = anim.runtimeAnimatorController as AnimatorController;
            if (controller.layers.Length < 1) throw new System.Exception("Nothing to controller.layers.");
            UpdateProgressBar(1);

            // AnimationControllerからAnimationLayer取得
            var anim_layer = controller.layers[0];
            anim_layer.name = animation_layer_name_default;
            UpdateProgressBar(1);

            // AnimationLayerからStateMachine取得
            var state_machine = anim_layer.stateMachine;
            if (state_machine == null) throw new System.Exception("stateMachine is null.");
            //if (state_machine.states.Length < 1) throw new System.Exception("Nothing to state_machine.states.");
            UpdateProgressBar(1);

            // AnimationControllerにState追加    5
                // Idle
                var state_idle = state_machine.AddState(state_name_idle);
                UpdateProgressBar(1);

                // Move
                var state_move = state_machine.AddState(state_name_move);
                UpdateProgressBar(1);

                // Attack
                var state_attack = state_machine.AddState(state_name_attack);
                UpdateProgressBar(1);

                // Damage
                var state_damage = state_machine.AddState(state_name_damage);
                UpdateProgressBar(1);

                // Die
                var state_die = state_machine.AddState(state_name_die);
                UpdateProgressBar(1);

            // AnimationControllerにパラメータ追加    4
                // int HP
                controller.AddParameter(animation_parameter_name_hp, AnimatorControllerParameterType.Int);
                UpdateProgressBar(1);
                // trigger Move
                controller.AddParameter(animation_parameter_name_move, AnimatorControllerParameterType.Trigger);
                UpdateProgressBar(1);
                // trigger Attack
                controller.AddParameter(animation_parameter_name_attack, AnimatorControllerParameterType.Trigger);
                UpdateProgressBar(1);
                // trigger Damge
                controller.AddParameter(animation_parameter_name_damage, AnimatorControllerParameterType.Trigger);
                UpdateProgressBar(1);

            // 各Stateにトランジション追加    12
                // EntryからIdle（無条件）
                var transition_entry_to_idle = state_machine.AddEntryTransition(state_idle);
                UpdateProgressBar(1);
                // IdleからMove（Move On）
                var transition_idle_to_move = state_idle.AddTransition(state_move);
                transition_idle_to_move.AddCondition(AnimatorConditionMode.If, 0, animation_parameter_name_move);
                UpdateProgressBar(1);

                // MoveからAttack（Attack On）
                var transition_move_to_attack = state_move.AddTransition(state_attack);
                transition_move_to_attack.AddCondition(AnimatorConditionMode.If, 0, animation_parameter_name_attack);
                UpdateProgressBar(1);
                // MoveからDamage（Damage On）
                var transition_move_to_damage = state_move.AddTransition(state_damage);
                transition_move_to_damage.AddCondition(AnimatorConditionMode.If, 0, animation_parameter_name_damage);
                UpdateProgressBar(1);
                // MoveからDie（HPが1より下）
                var transition_move_to_die = state_move.AddTransition(state_die);
                transition_move_to_die.AddCondition(AnimatorConditionMode.Less, 1, animation_parameter_name_hp);
                UpdateProgressBar(1);

                // AttackからMove（Move On）
                var transition_attack_to_move = state_attack.AddTransition(state_move);
                transition_attack_to_move.AddCondition(AnimatorConditionMode.If, 0, animation_parameter_name_move);
                UpdateProgressBar(1);
                // AttackからDamage（Damage On）
                var transition_attack_to_damage = state_attack.AddTransition(state_damage);
                transition_attack_to_damage.AddCondition(AnimatorConditionMode.If, 0, animation_parameter_name_damage);
                UpdateProgressBar(1);
                // AttackからDie（HPが1より下）
                var transition_attack_to_die = state_attack.AddTransition(state_die);
                transition_attack_to_die.AddCondition(AnimatorConditionMode.Less, 1, animation_parameter_name_hp);
                UpdateProgressBar(1);

                // DamageからMove（Move On）
                var transition_damage_to_move = state_damage.AddTransition(state_move);
                transition_damage_to_move.AddCondition(AnimatorConditionMode.If, 0, animation_parameter_name_move);
                UpdateProgressBar(1);
                // DamageからAttack（Attack On）
                var transition_damage_to_attack = state_damage.AddTransition(state_attack);
                transition_damage_to_attack.AddCondition(AnimatorConditionMode.If, 0, animation_parameter_name_attack);
                UpdateProgressBar(1);
                // DamageからDie（HPが1より下）
                var transition_damage_to_die = state_damage.AddTransition(state_die);
                transition_damage_to_die.AddCondition(AnimatorConditionMode.Less, 1, animation_parameter_name_hp);
                UpdateProgressBar(1);

                // Dieからはトランジションなし

            // 各StateにMotion設定    5
                // Idle
                if (animation_idle != null)
                    state_idle.motion = animation_idle;
                UpdateProgressBar(1);
                // Move
                if (animation_move != null)
                    state_move.motion = animation_move;
                UpdateProgressBar(1);
                // Attack
                if (animation_attack != null)
                    state_attack.motion = animation_attack;
                UpdateProgressBar(1);
                // Damage
                if (animation_damage != null)
                    state_damage.motion = animation_damage;
                UpdateProgressBar(1);
                // Die
                if (animation_die != null)
                    state_die.motion = animation_die;
                UpdateProgressBar(1);

        // NavMeshAgent    1

            // NavMeshAgent持ってなかったら
            var nma = prefab.GetComponent<NavMeshAgent>();

            if (nma == null) {
                // NavMeshAgent追加
                nma = prefab.AddComponent<NavMeshAgent>();
            }

            nma.speed = 1;
            nma.angularSpeed = 3000;

            UpdateProgressBar(1);

        // Rigidbody    14

            // Rigidbody持ってなかったら
            var rb = prefab.GetComponent<Rigidbody>();

            if (rb == null) {
                // Rigidbody追加
                rb = prefab.AddComponent<Rigidbody>();
            }

            UpdateProgressBar(1);
            
            // Mass 1
            rb.mass = 1;
            UpdateProgressBar(1);
            // Drag 0
            rb.drag = 0;
            UpdateProgressBar(1);
            // Angular Drag 0.5
            rb.angularDrag = 0.5f;
            UpdateProgressBar(1);

            // use Gravity false
            rb.useGravity = false;
            UpdateProgressBar(1);
            // IsKinematic true
            rb.isKinematic = true;
            UpdateProgressBar(1);

            // Interpolate Interpolate
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            UpdateProgressBar(1);
            // Collision Detection Continuos
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            UpdateProgressBar(1);

            // Freeze Position / Freeze Rotation
            rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            UpdateProgressBar(1);


        // Collider    2
/*
            // Mesh Collier持ってたら
            var mc = prefab.GetComponent<MeshCollider>();

            if (mc != null) {
                // 外し
                GameObject.Destroy(prefab.GetComponent<MeshCollider>());
            }

            UpdateProgressBar(1);
*/
            // Collider持ってなかったら
            var collider = prefab.GetComponent<Collider>();

            if (collider == null) {
                // Capsule Collider追加
                prefab.AddComponent<CapsuleCollider>();
            }

            UpdateProgressBar(1);

        // Script    2
/*
            // Script持ってたら
            var scripts = prefab.GetComponents<Script>();

            if (0 < scripts.Length) {
                // 削除
                for (var i  = 0; i < scripts.Length; i += 1) {
                    GameObject.Destroy(scripts[i]);
                }
            }

            UpdateProgressBar(1);
*/
            // Outline追加
            var smr = prefab.GetComponent<SkinnedMeshRenderer>();

            if (smr != null) {
                var outline = smr.gameObject.AddComponent<Outline>();
                outline.enabled = false;
            } else {
                Debug.Log("SkinnedMeshRenderer not found.");
            }

            UpdateProgressBar(1);

            // Enemy Controller追加
            prefab.AddComponent<EnemyController>();
            UpdateProgressBar(1);
            
        // 新しいprefabとして保存（保存先に同じ名前のprefabがあったら上書き）
        UnityEditor.PrefabUtility.CreatePrefab(prefab_path, prefab);
        UnityEditor.AssetDatabase.SaveAssets();
        UpdateProgressBar(1);

        ClearProgressBar();
    }
}
