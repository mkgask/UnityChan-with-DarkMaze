using sgffu.FieldObject;
using FieldObjectID = sgffu.FieldObject.ID;

namespace sgffu.ObjectPool {

    class PoolObjectFactory
    {
        public IPoolObject create(Type type, FieldObjectID id) {
            switch(type) {
                /*
                case Type.Item:
                    return ItemFactory.create(id);
                case Type.Monster:
                    return MonsterFactory.create(id);
                */
                case Type.Props:
                    IPoolObject obj = FieldObjectEntityFactory.createBlank(id);
                    obj.init();
                    return obj;
                    //return PropsFactory.create(id);
                default:
                    break;
            }

            throw new System.Exception("Not created PoolObject: Invalid PoolObject Type");
        }

    }

}
