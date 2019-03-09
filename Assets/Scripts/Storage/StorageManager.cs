using UnityEngine;
using StorageService = sgffu.Storage.Service;

public class StorageManager : MonoBehaviour
{
    void Awake ()
    {
        StorageService.init();
    }
	
}
