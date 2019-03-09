using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace sgffu.EffectiveFloor
{

    public class EffectiveFloorService {
        public static EffectiveFloorCollection collection;
        
        public static void init()
        {
            collection = EffectiveFloorCollectionFactory.create();
        }

        public static void reset()
        {
            if (collection == null || collection.entities == null || collection.entities.Count < 1) return;
            collection.entities.ForEach(x => x.enabled = true);
        }

        public static EffectiveFloorEntity next(int size)
        {
            int index = collection.entities.FindIndex(x => x.enabled == true);

            if (index < 0 || collection.entities[index].effective_size < size) {
                return null;
            }

            int max = index + size;

            for (int i = index; i < max; i += 1) {
                try {
                    if (!collection.entities[i].enabled) {
                        return null;
                    }
                } catch(ArgumentOutOfRangeException e) {
                    return null;
                }
            }

            collection.entities[index].enabled = false;
            return collection.entities[index];

        }

        public static EffectiveFloorEntity rand(int size)
        {
            //Debug.Log("EffectiveFloorService.rand: size: " + size);
            if (false == collection.entities.Exists(x => x.enabled == true)) {
                return null;
            }

            int index = Int32.MinValue;

            do {
                index = Random.Range(0, collection.entities.Count);
            } while(collection.entities[index].enabled == false);

            int max = index + size;
            
            for (int i = index; i < max; i += 1) {
                try {
                    if (!collection.entities[i].enabled) {
                        return null;
                    }
                } catch(ArgumentOutOfRangeException e) {
                    return null;
                }
            }

            //Debug.Log("EffectiveFloorService.rand: index: " + index);

            collection.entities[index].enabled = false;
            return collection.entities[index];
        }
    }

}
