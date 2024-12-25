using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Vuforia; 


public class ParticleManager : MonoBehaviour
{
    private float cycleInterval = 0.01f;
    private List<ChargedParticle> chargedParticles;
    private List<MovingChargedParticle> movingChargedParticles;

    public GameObject[] particlePrefabs; // Array of particle prefabs
    private int selectedPrefabIndex = 0; // Index of the currently selected prefab
    private Camera arCamera; // AR Camera for raycasting

    private bool isForcesPaused = true; // Track if forces are paused

    void Start()
    {
        arCamera = Camera.main;
        
        chargedParticles = new List<ChargedParticle>(GetComponentsInChildren<ChargedParticle>());
        movingChargedParticles = new List<MovingChargedParticle>(GetComponentsInChildren<MovingChargedParticle>());

        foreach (MovingChargedParticle mcp in movingChargedParticles)
        {
            StartCoroutine(Cycle(mcp));

        }

    }

    

    public IEnumerator Cycle(MovingChargedParticle mcp)
    {
        while (true)
        {
            if (!isForcesPaused)
            {
                ApplyMovingForce(mcp);
            }
            yield return new WaitForSeconds(cycleInterval);
        }
    }

    private void ApplyMovingForce(MovingChargedParticle mcp)
    {
        Vector3 newForce = Vector3.zero;

        foreach (ChargedParticle cp in chargedParticles)
        {
            if (mcp == cp)
            {
                continue;
            }

            float distance = Vector3.Distance(mcp.transform.position, cp.transform.gameObject.transform.position);
            if (distance == 0)
            {
                continue;
            }
            float force = 100 * mcp.charge * cp.charge / Mathf.Pow(distance, 2);

            Vector3 direction = mcp.transform.position - cp.transform.position;
            direction.Normalize();

            newForce += force * direction * cycleInterval;

            mcp.rb.AddForce(newForce);

        }
    }
    void Update()
    {
        #if UNITY_EDITOR || UNITY_STANDALON
                HandleMouseInput();
        #elif UNITY_ANDROID || UNITY_IOS
                HandleTouchInput();
        #endif
    }
    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {

            
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Mouse is over a UI element. Ignoring input.");
                return; // Do not place a prefab
            }
            // Get the position of the mouse click in screen space
            Vector3 mousePosition = Input.mousePosition;

            // Reference object to determine distance (e.g., an existing GameObject in the scene)
            GameObject referenceObject = GameObject.Find("ImageTarget/GameObject/Cube");
            if (referenceObject == null)
            {
                Debug.LogError("Reference GameObject not found under ImageTarget/particles.");
                return;
            }

            // Calculate the distance between the AR camera and the reference object
            float targetDistance = Vector3.Distance(arCamera.transform.position, referenceObject.transform.position);
            // Convert mouse position to world space at a fixed distance from the camera
            
            Vector3 targetPosition = arCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, targetDistance));

            // Find the "GameObject" under "particles" within "ImageTarget"
            GameObject parentObject = GameObject.Find("ImageTarget/GameObject");
            if (parentObject == null)
            {
                Debug.LogError("Parent GameObject not found under ImageTarget/particles.");
                return;
            }

            // Instantiate the selected prefab at the target position
            GameObject newParticle = Instantiate(particlePrefabs[selectedPrefabIndex], targetPosition, Quaternion.identity);

            // Make the new particle a child of the parentObject
            newParticle.transform.SetParent(parentObject.transform);

            // Get the MovingChargedParticle component
            MovingChargedParticle mcp = newParticle.GetComponent<MovingChargedParticle>();
            mcp.UpdateColor();
            
            if (mcp != null)
            {
                // Add the new particle to the lists and start the coroutine
                chargedParticles.Add(mcp);
                movingChargedParticles.Add(mcp);
                StartCoroutine(Cycle(mcp));
                Debug.Log($"Added {newParticle.name} as a child of {parentObject.name}");
            }
            else
            {
                Debug.LogWarning("The instantiated prefab does not have a MovingChargedParticle component.");
            }
            
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // Check if the touch is over a UI element
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                Debug.Log("Touch is over a UI element. Ignoring input.");
                return; // Do not place a prefab
            }
            // Get the position of the touch in screen space
            Vector3 touchPosition = Input.GetTouch(0).position;

            // Reference object to determine distance (e.g., an existing GameObject in the scene)
            GameObject referenceObject = GameObject.Find("ImageTarget/GameObject/Cube");
            if (referenceObject == null)
            {
                Debug.LogError("Reference GameObject not found under ImageTarget/particles.");
                return;
            }

            // Calculate the distance between the AR camera and the reference object
            float targetDistance = Vector3.Distance(arCamera.transform.position, referenceObject.transform.position);

            // Convert touch position to world space at a fixed distance from the camera
            Vector3 targetPosition = arCamera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, targetDistance));

            // Find the "GameObject" under "particles" within "ImageTarget"
            GameObject parentObject = GameObject.Find("ImageTarget/GameObject");
            if (parentObject == null)
            {
                Debug.LogError("Parent GameObject not found under ImageTarget/particles.");
                return;
            }

            // Instantiate the selected prefab at the target position
            GameObject newParticle = Instantiate(particlePrefabs[selectedPrefabIndex], targetPosition, Quaternion.identity);

            // Make the new particle a child of the parentObject
            newParticle.transform.SetParent(parentObject.transform);

            // Get the MovingChargedParticle component
            MovingChargedParticle mcp = newParticle.GetComponent<MovingChargedParticle>();
            mcp.UpdateColor();

            if (mcp != null)
            {
                // Add the new particle to the lists and start the coroutine
                chargedParticles.Add(mcp);
                movingChargedParticles.Add(mcp);
                StartCoroutine(Cycle(mcp));
                Debug.Log($"Added {newParticle.name} as a child of {parentObject.name}");
            }
            else
            {
                Debug.LogWarning("The instantiated prefab does not have a MovingChargedParticle component.");
            }
        }
    }


    // Function to be called by buttons to select the prefab
    public void SelectPrefab(int index)
    {
        if (index >= 0 && index < particlePrefabs.Length)
        {
            selectedPrefabIndex = index;
        }
        else
        {
            Debug.LogWarning("Invalid prefab index selected.");
        }
    }

    public void PauseForces()
    {   
        isForcesPaused = true;
    }
    public void ResumeForces()
    {       
        isForcesPaused = false;
    }
}
