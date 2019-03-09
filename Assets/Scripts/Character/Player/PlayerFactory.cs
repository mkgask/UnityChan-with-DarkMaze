using System.Collections.Generic;
using UnityEngine;
using EquipEntity = Item.Equip.EquipEntity;
using EquipFactory = Item.Equip.Factory;



namespace sgffu.Characters.Player
{
    public class PlayerFactory
    {
        private static GameObject prefab;

        public static void init(GameObject player_prefab)
        {
            prefab = player_prefab;
        }



        public static PlayerEntity createEntity(EquipEntity equip_entity = null)
        {
            PlayerEntity player_entity = new PlayerEntity();

            if (equip_entity != null) {
                player_entity.equip = equip_entity;
            } /* else {
                EquipEntity equip = EquipFactory.createRandomEntity();
                ItemEquipController iec = equip.game_object.GetComponentInChildren<ItemEquipController>();
                iec.OnTarget();
                player_entity.equip = equip;
            } */

            return player_entity;
        }

        public static GameObject createGameObject()
        {
            return Object.Instantiate(prefab);
        }
    }

}