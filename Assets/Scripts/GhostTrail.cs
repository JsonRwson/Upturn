using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTrail : MonoBehaviour
{
    public GameObject ghost;
    public float ghostDelay;
    public float ghostExipreTime = 1f;
    private float ghostDelaySeconds;
    
    public bool generateGhosts = false;
    // Start is called before the first frame update
    void Start()
    {
        ghostDelaySeconds = ghostDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if(generateGhosts)
        {
            if(ghostDelaySeconds > 0)
            {
                ghostDelaySeconds -= Time.deltaTime;
            }
            else
            {
                // Generate a ghost for the trail
                // TODO, replace with pooled version
                GameObject currentGhost = Instantiate(ghost, transform.position, transform.rotation);
                Sprite currentSprite = GetComponent<SpriteRenderer>().sprite;
                var currentRenderer = currentGhost.GetComponent<SpriteRenderer>();

                currentRenderer.sprite = currentSprite;
                currentRenderer.sortingOrder = 1;
                currentGhost.transform.localScale = transform.localScale;
                ghostDelaySeconds = ghostDelay;
                Destroy(currentGhost, ghostExipreTime);
            }
        }
    }
}
