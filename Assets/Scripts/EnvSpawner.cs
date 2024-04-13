using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script attatched to the environment controller object
// Responsible for endlessly spawning in envrionemts for the player to play
// Utilizing object pooling and cleaning up passed environments
public class EnvSpawner : MonoBehaviour
{
    // Reference the player and pre-placed starting platform
    public GameObject Player;
    public GameObject StartingPlatform;

    // Store a list for prefabs that can be traversed without flipping gravity at all
    private GameObject[] menuEnvironmentPrefabs;

    // Store a pool for pre-instantiated prefabs that can be shifted around
    private List<GameObject> prefabPool = new List<GameObject>();
    // Store the prefabs that are currently in use and set to active
    // Using a FIFO queue to easily set the oldest prefab in use to inactive
    private Queue<GameObject> activePrefabs = new Queue<GameObject>();

    // Track the spawnpoint for environments
    // Track the point the player needs to pass for another environment to spawn
    private Vector2 spawnPoint;
    private Vector2 spawnThreshold;

    private bool isInMenu = false;
    // Allow for multiple of the same environment to be pooled
    private int duplicatePrefabsLimit = 3;

    // Start is called before the first frame update
    void Start()
    {
        // Load all the menu-safe environments from the resources folder
        menuEnvironmentPrefabs = Resources.LoadAll<GameObject>("Prefabs/MenuEnvironments");
        Debug.Log("Loaded " + menuEnvironmentPrefabs.Length + " prefabs.");

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
        // There are 2 starting platforms so the spawn point for the first environment is double the length
        spawnPoint = lengthOfInitialPlatform * 2;
        // The threshold to spawn a new environment is the end of the first platform, not both
        spawnThreshold.x = lengthOfInitialPlatform.x;
    }

    // Update is called once per frame
    void Update()
    {
        // If the player isnt in the main menu
        if(!isInMenu)
        {
            // If the player has passed the spawn threshold
            if(Player.transform.position.x > spawnThreshold.x)
            {
                // Spawn a new environment from the pool
                SpawnPrefabFromPool(spawnPoint);
            }
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

        GameObject prefab;
        int randomIndex;
        // Do while allows prefab to be instantiated prior to the condition check
        do
        {
            // Pick a random environment from the object pool
            randomIndex = Random.Range(0, prefabPool.Count);
            prefab = prefabPool[randomIndex];
        }
        // Loop until we find an environment not in use (active)
        while(prefab.activeInHierarchy);

        // Update the position of the new environment to the given location
        // Set it to active and add it to the active prefabs queue
        prefab.transform.position = location;
        prefab.SetActive(true);
        activePrefabs.Enqueue(prefab);

        // Calculate the length of the prefab jsut spawned
        // Use this to update the spawn threshold and spawnpoint for new environments
        float length = GetPrefabHorizontalLen(prefab);
        spawnThreshold.x += length;
        spawnPoint.x += length;
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
