using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using EffectType = Effect.Type;
using EffectEntity = Effect.Entity;
using File = Filesystem.File;

namespace Effect
{
    class Factory
    {
        private static int effect_id = 1;

        private static Dictionary<EffectType, EffectEntity> base_resources;

        private static List<GameObject> effect_prefabs;

        public static void init(List<GameObject> effectPrefabs)
        {
            effect_prefabs = effectPrefabs;

            var damage_label = new EffectEntity {
                name = "ダメージラベル",
                type = EffectType.DamageLabel,
                //prefab_path = "Effect/DamageLabel",
                prefab_id = 0,
            };

            base_resources = new Dictionary<EffectType, EffectEntity> {
                { EffectType.DamageLabel, damage_label },
            };
        }

        public static EffectEntity create(EffectType type)
        {
            EffectEntity entity = createEntity(type);
            entity.game_object = createGameObject(entity);
            entity.game_object.SetActive(false);
            return entity;
        }

        public static EffectEntity createEntity(EffectType type)
        {
            if (!base_resources.ContainsKey(type)) {
                throw new KeyNotFoundException();
            }

            EffectEntity entity = base_resources[type].clone();
            entity.id = effect_id;
            effect_id += 1;
            return entity;
        }

        public static GameObject createGameObject(EffectEntity entity)
        {
            var go = effect_prefabs[entity.prefab_id];
            return Object.Instantiate(go);

/*
            if (!((new File()).resourceExist(entity.prefab_path + ".prefab")) &&
                    !((new File()).resourceExist(entity.prefab_path + ".fbx"))) {
                throw new System.Exception("EnemyFactory::createGameObject(): EnemyEntity.prefab_path file not found.");
            }

            GameObject go = Object.Instantiate(Resources.Load(entity.prefab_path, typeof(GameObject))) as GameObject;
            return go;
*/
        }
    }
}