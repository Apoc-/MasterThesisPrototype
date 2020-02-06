using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class SkyBehaviour : MonoBehaviour
{
    public List<Sprite> cloudSprites;
    
    public int cloudMaxHeight = 250;
    public int cloudMinHeight = 150;

    public int cloudSpawnX = -500;
    public int cloudDespawnX = 500;

    [Range(1f,1000f)] public float currentCloudSpeed = 100f;
    public float spawnBaseInterval = 1;
    public float cloudBaseSpeed = 250f;

    private List<GameObject> _spawnedClouds = new List<GameObject>();
    private List<GameObject> _cloudPool = new List<GameObject>();
    // Start is called before the first frame update
    
    private float _spawnTimer = 0;
    void Start()
    {
        InitCloudPool();
        _spawnTimer = spawnBaseInterval * (cloudBaseSpeed / currentCloudSpeed);
    }
    
    void Update()
    {
        _spawnTimer += Time.deltaTime;

        if (_spawnTimer >= spawnBaseInterval * (cloudBaseSpeed / currentCloudSpeed))
        {
            SpawnCloudFromPool();
            _spawnTimer = 0;
        }
        
        MoveClouds();    
    }

    private void InitCloudPool()
    {
        var prefab = Resources.Load("prefabs/Cloud");
        var container = new GameObject("Clouds");
        container.transform.parent = transform;
        
        cloudSprites.ForEach(sprite =>
        {
            var go = Instantiate(prefab, container.transform, true) as GameObject;

            if (go == null) return;
            
            go.GetComponent<SpriteRenderer>().sprite = sprite;
            go.SetActive(false);

            _cloudPool.Add(go);
        });
    }

    private void SpawnCloudFromPool()
    {        
        if (_cloudPool.Count == 0) return;
        
        var cloud = _cloudPool[Random.Range(0, _cloudPool.Count)];
        _cloudPool.Remove(cloud);
        cloud.SetActive(true);
        var pos = cloud.transform.position;
        pos.x = -500;
        pos.y = Random.Range(cloudMinHeight, cloudMaxHeight);

        cloud.transform.position = pos;
        _spawnedClouds.Add(cloud);
    }

    private void MoveClouds()
    {
        var despawnClouds = new List<GameObject>();
        _spawnedClouds.ForEach(cloud =>
        {
            var pos = cloud.transform.position;
            pos.x += currentCloudSpeed * Time.deltaTime;
            cloud.transform.position = pos;

            if (cloud.transform.position.x >= cloudDespawnX) despawnClouds.Add(cloud);
        });

        despawnClouds.ForEach(DespawnCloud);
    }

    private void DespawnCloud(GameObject cloud)
    {
        cloud.SetActive(false);
        _spawnedClouds.Remove(cloud);
        _cloudPool.Add(cloud);
    }
}
