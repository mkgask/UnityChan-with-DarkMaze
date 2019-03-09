using UnityEngine;

namespace sgffu.EffectiveFloor
{
    public class EffectiveFloorCollectionFactory
    {
        public static EffectiveFloorCollection create()
        {
            //return new EffectiveFloorCollection();
            EffectiveFloorCollection collection = new EffectiveFloorCollection();

            collection.entities.Add(EffectiveFloorEntityFactory.create(
                new Vector3(-1.5f, 0.5f, -4f),
                new Vector3(0f, 0f, 0f),
                4
            ));
            collection.entities.Add(EffectiveFloorEntityFactory.create(
                new Vector3(-1.5f, 0.5f, -3f),
                new Vector3(0f, 0f, 0f),
                3
            ));
            collection.entities.Add(EffectiveFloorEntityFactory.create(
                new Vector3(-1.5f, 0.5f, -2f),
                new Vector3(0f, 0f, 0f),
                2
            ));
            collection.entities.Add(EffectiveFloorEntityFactory.create(
                new Vector3(-1.5f, 0.5f, -1f),
                new Vector3(0f, 0f, 0f),
                1
            ));
            collection.entities.Add(EffectiveFloorEntityFactory.create(
                new Vector3(-1.5f, 0.5f, 1f),
                new Vector3(0f, 0f, 0f),
                4
            ));
            collection.entities.Add(EffectiveFloorEntityFactory.create(
                new Vector3(-1.5f, 0.5f, 2f),
                new Vector3(0f, 0f, 0f),
                3
            ));
            collection.entities.Add(EffectiveFloorEntityFactory.create(
                new Vector3(-1.5f, 0.5f, 3f),
                new Vector3(0f, 90f, 0f),
                2
            ));
            collection.entities.Add(EffectiveFloorEntityFactory.create(
                new Vector3(-0.5f, 0.5f, 3f),
                new Vector3(0f, 90f, 0f),
                1
            ));
            collection.entities.Add(EffectiveFloorEntityFactory.create(
                new Vector3(1.5f, 0.5f, 3f),
                new Vector3(0f, 90f, 0f),
                2
            ));
            collection.entities.Add(EffectiveFloorEntityFactory.create(
                new Vector3(2.5f, 0.5f, 3f),
                new Vector3(0f, 90f, 0f),
                1
            ));

            return collection;
        }
    }
}