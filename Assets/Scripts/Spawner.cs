using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Spawner : MonoBehaviour
{
    [SerializeField] private AssetReference objRed;

    private AsyncOperationHandle handle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SpawnGameObject();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Addressables.Release(handle);
        }
    }

    private void SpawnGameObject()
    {
        handle = Addressables.LoadAssetAsync<GameObject>(objRed);
        handle.Completed += (task) =>
        {
            Instantiate((GameObject)task.Result);
        };
    }

    //private void SpawnGameObject(AssetReference assetReference)
    //{
    //    var handle = Addressables.LoadAssetAsync<GameObject>(assetReference);
    //    handle.Completed += (AsyncOperationHandle<GameObject> task) =>
    //    {
    //        Instantiate(task.Result);
    //    };
    //}
}
