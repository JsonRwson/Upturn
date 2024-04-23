using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script attatched to the environment controller object
// Responsible for endlessly spawning in envrionemts for the player to play
// Utilizing object pooling and cleaning up passed environments
public class EnvSpawner : MonoBehaviour
{
    // Reference the player and pre-placed starting platform
    public GameObject player;
    private float playerOffset;
    public GameObject StartingPlatform;

    // Store a list for prefabs that can be traversed without flipping gravity at all
    private GameObject[] menuEnvironmentPrefabs;
    // Store a list for prefabs that are used in the endless mode
    private GameObject[] endlessEnvironmentPrefabs;

    // Store a pool for pre-instantiated prefabs that can be shifted around
    private List<GameObject> prefabPool = new List<GameObject>();
    // Store the prefabs that are currently in use and set to active
    // Using a FIFO queue to easily set the oldest prefab in use to inactive
    private Queue<GameObject> activePrefabs = new Queue<GameObject>();

    // Track the spawnpoint for environments
    // Track the point the player needs to pass for another environment to spawn
    private Vector2 spawnPoint;
    private Vector2 spawnThreshold;

    // Allow for multiple of the same environment to be pooled
    private int duplicatePrefabsLimit = 3;

    // Start is called before the first frame update
    void Start()
    {
        // Load all the menu-safe environments from the resources folder
        menuEnvironmentPrefabs = Resources.LoadAll<GameObject>("Prefabs/MenuEnvironments");
        Debug.Log("Loaded " + menuEnvironmentPrefabs.Length + " prefabs.");

        playerOffset = player.transform.position.x;

        // Iterate through the menu-safe environments
        for(int i = 0; i < menuEnvironmentPrefabs.Length; i++)
        {
            // Instantiate each environment and add them to object pool
            // Do this multiple times, in accordance to the duplicate prefabs limit
            for(int x = 0; x < duplicatePrefabsLimit; x++)
            {
                GameObject obj = Instantiate(menuEnvironmentPrefabs[i]);
                obj.SetActive(false);
                prefabPool.Add(obj);
            }
        }

        // Calculate the length of the initial platform
        Vector2 lengthOfInitialPlatform = new Vector2(GetPrefabHorizontalLen(StartingPlatform), 0);
        // The threshold to spawn a new environment
        spawnThreshold.x = lengthOfInitialPlatform.x/2;
        // Set the spawn point for the sequential platforms
        spawnPoint.x = lengthOfInitialPlatform.x -playerOffset;
    }

    // Update is called once per frame
    void Update()
    {
        // If the player has passed the spawn threshold
        if(player.transform.position.x > spawnThreshold.x)
        {
            // Spawn a new environment from the pool
            SpawnPrefabFromPool(spawnPoint);
        }
    }

    // "Spawning" is setting an instantiated environment to visible and updating its poistion to the spawnpoint
    void SpawnPrefabFromPool(Vector2 location)
    {
        // If there are already 3 environments in the scene, then we need to delete an old one
        if(activePrefabs.Count == 3)
        {
            // Set the oldest environment to inactive to save resources
            activePrefabs.Dequeue().SetActive(false);
        }

        // Select a prefab to spawn from the pool
        GameObject prefabToSpawn = GetRandomInactivePrefab();

        // Update the position of the new environment to the given location
        // Set it to active and add it to the active prefabs queue
        prefabToSpawn.transform.position = location;
        prefabToSpawn.SetActive(true);
        activePrefabs.Enqueue(prefabToSpawn);

        // Calculate the length of the prefab just spawned
        // Use this to update the spawn threshold and spawnpoint for new environments
        float length = GetPrefabHorizontalLen(prefabToSpawn);
        spawnThreshold.x += length - playerOffset;
        spawnPoint.x += length - playerOffset;
    }

    public void StartEndless()
    {
        // Load all the endless environments from the resources folder
        endlessEnvironmentPrefabs = Resources.LoadAll<GameObject>("Prefabs/EndlessEnvironments");
        Debug.Log("Loaded " + menuEnvironmentPrefabs.Length + " prefabs.");

        // Clear out the menu-safe environments
        List<GameObject> itemsToRemove = new List<GameObject>();

        foreach(GameObject prefab in prefabPool)
        {
            if(prefab.name.Contains("MenuEnv"))
            {
                itemsToRemove.Add(prefab);
            }
        }

        foreach(GameObject item in itemsToRemove)
        {
            prefabPool.Remove(item);
        }

        // Iterate through the endless environments
        for(int i = 0; i < endlessEnvironmentPrefabs.Length; i++)
        {
            // Instantiate each environment and add them to object pool
            // Do this multiple times, in accordance to the duplicate prefabs limit
            for(int x = 0; x < duplicatePrefabsLimit; x++)
            {
                GameObject obj = Instantiate(endlessEnvironmentPrefabs[i]);
                obj.SetActive(false);
                prefabPool.Add(obj);
            }
        }
    }

    // Function to get a random inactive prefab from the pool
    GameObject GetRandomInactivePrefab()
    {
        GameObject prefab;
        int randomIndex;
        do
        {
            // Pick a random environment from the object pool
            randomIndex = Random.Range(0, prefabPool.Count);
            prefab = prefabPool[randomIndex];
        }
        // Loop until we find an environment not in use (active)
        while(prefab.activeInHierarchy);

        return prefab;
    }

    // Function to fetch the horizontal length of a given prefab
    float GetPrefabHorizontalLen(GameObject prefab)
    {
        // Store all renderers in the given prefab
        Renderer[] renderers = prefab.GetComponentsInChildren<Renderer>();
        // Create a bounds to store the final bounds of the prefab
        Bounds bounds = new Bounds(prefab.transform.position, Vector3.zero);

        // Iterate throught each renderer in the prefab
        foreach(Renderer renderer in renderers)
        {
            // Grow the bounds to encapsulate the bounds of each renderer
            bounds.Encapsulate(renderer.bounds);
        }

        // Fetch the x value from the bounds and return it
        float fullWidth = bounds.size.x;
        return fullWidth;
    }
}
