using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using EquipFactory = Item.Equip.Factory;
using EquipService = Item.Equip.Service;
using sgffu.EventMessage;
using LogService = sgffu.Log.Service;

public class EquipManager : MonoBehaviour
{

    //[SerializeField] private List<GameObject> prefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> prefabs;

    void Awake()
    {
        EquipFactory.init(prefabs);
        //EquipService.init();
        
        MessageBroker.Default.Receive<EquipEvent>().Subscribe(x => {
            Debug.Log("MessageBroker.Default.Receive<EquipEvent>().Subscribe()");
            EquipService.onlyEquip(x.id, x.go, x.update_service_data);
        });
        
        MessageBroker.Default.Receive<ChangeEquipEvent>().Subscribe(x => {
            Debug.Log("MessageBroker.Default.Receive<ChangeEquipEvent>().Subscribe()");
            EquipService.equipChange(x.id, x.go);
            MessageBroker.Default.Publish<SaveDataSave>(new SaveDataSave());
        });
    }

    void Start()
    {
        //EquipFactory.init(prefabs);
        //EquipService.init();
        //LogService.write("EquipManager::Start()::Publish<CreatedEquipBaseResouces>");
        MessageBroker.Default.Publish<CreatedEquipBaseResouces>(new CreatedEquipBaseResouces());
    }
}
