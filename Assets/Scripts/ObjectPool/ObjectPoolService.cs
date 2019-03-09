using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UniRx;

namespace sgffu.ObjectPool {

    class ObjectPoolService {
        private static Dictionary<Type, Dictionary<ObjectID, Queue<IPoolObject>>> pool;

        private static Dictionary<Type, int> pool_keep = new Dictionary<Type, int> {
            { Type.None,     0 },
            { Type.Item,    10 },
            { Type.Monster, 10 },
            { Type.Props,   10 },
        };

        private static PoolObjectFactory object_factory;
        
        private static int pool_gc_interval = 10;

        private static int pool_gc_count = 0;

        private static bool gc_activating = false;

        public static void init(PoolObjectFactory factory)
        {
            object_factory = factory;
            pool = new Dictionary<Type, Dictionary<ObjectID, Queue<IPoolObject>>>();
        }

        private static void gc()
        {
            pool_gc_count += 1;
            if (pool_gc_interval < pool_gc_count) { return; }

            pool_gc_count = 0;
            gc_activating = true;

            Observable.FromCoroutine(() => {
                foreach (Type type in Enum.GetValues(typeof(Type))) {
                    if (type == Type.None) { continue; }

                    foreach (KeyValuePair<ObjectID, Queue<IPoolObject>> pair in pool[type]) {
                        while (pair.Value.Count < pool_keep[type]) {
                            pair.Value.Dequeue();
                        }
                    }
                }

                return null;
            });

            gc_activating = false;
        }

        public static IPoolObject takeOut(Type type, ObjectID id)
        {
            //try {
                if (!pool.ContainsKey(type)) { pool.Add(type, new Dictionary<ObjectID, Queue<IPoolObject>>()); }
                if (!pool[type].ContainsKey(id)) { pool[type].Add(id, new Queue<IPoolObject>()); }
                if (gc_activating || pool[type][id].Count < 1) { return null; }
                //if (gc_activating || pool[type][id].Count < 1) { return object_factory.create(type, id); }
            //} catch(KeyNotFoundException e) {
                //Debug.Log("KeyNotFoundException: " + ObjectPoolTypeDebugLog(type) + " : " + ObjectIDDebugLog(id));
            //}

            return pool[type][id].Dequeue();
        }

        public static void ret(Type type, ObjectID id, IPoolObject obj)
        {
            if (!pool.ContainsKey(type)) { pool.Add(type, new Dictionary<ObjectID, Queue<IPoolObject>>()); }
            if (pool[type].Count < 1) { pool[type].Add(id, new Queue<IPoolObject>()); }
            pool[type][id].Enqueue(obj);
            gc();
        }

/*
        public static string ObjectPoolTypeDebugLog(Type id)
        {
            switch(id) {
                case Type.None: return "ObjectPoolTypeDebugLog: None";
                case Type.Item: return "ObjectPoolTypeDebugLog: Item";
                case Type.Monster: return "ObjectPoolTypeDebugLog: Monster";
                case Type.Props: return "ObjectPoolTypeDebugLog: Props";
                default: return "ObjectPoolTypeDebugLog: default";
            }
        }

        public static string ObjectIDDebugLog(ObjectID id)
        {
            switch(id) {
                case ObjectID.None: return "ObjectIDDebugLog: None";
                case ObjectID.Torch: return "ObjectIDDebugLog: Torch";
                case ObjectID.TreasureBox: return "ObjectIDDebugLog: TreasureBox";
                default: return "ObjectIDDebugLog: default";
            }
        }
*/

    }

}