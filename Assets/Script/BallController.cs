using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{
    public Transform ball;          
    public Transform plane;         
    public Text timeText;           // UI Text to display the time
    public Text heightText;         // UI Text to display the height
    public Slider heightSlider;     // UI Slider to adjust height
    private Rigidbody rb;
    private bool isFalling = false;
    private float startTime;

    void Start()
    {
        rb = ball.GetComponent<Rigidbody>();
        timeText.text = "Time: 0.00s";
        heightText.text = $"Height: {heightSlider.value:F1}m"; 
        //Adding a listener to adjust the height according to the slider valueZTFFDSDSDSDSDSDSDSDSDSDSDSDSDSDSDSDSDSDSDSDSDSDSDSDSDSDSDSDSDSDSDSDSDSDSDSDSDSDS
        heightSlider.onValueChanged.AddListener(AdjustHeight); 
    }

    // Method to adjust height relative to the plane
    public void AdjustHeight(float height)
    {
        if (!isFalling)
        {
            ball.position = new Vector3(
                plane.position.x,                  // Align X with the plane
                plane.position.y + height,         // Adjust height relative to the plane
                plane.position.z                   // Align Z with the plane
            ); 
            heightText.text = $"Height: {height:F1}m"; // Update height text
        }
    }

    // Method to drop the ball
    public void DropBall()
    {
        if (rb != null && !isFalling)
        {
            rb.useGravity = true; // Enable gravity
            isFalling = true;     // Ball is falling
            startTime = Time.time;

            // Calculate the time using the equation t = sqrt(2h / g)
            float height = ball.position.y - plane.position.y; // Relative height to the plane
            float gravity = Physics.gravity.magnitude;
            float theoreticalTime = Mathf.Sqrt(2 * height / gravity);

            Debug.Log($"Theoretical Time: {theoreticalTime:F2}s");
        }
    }

    void Update()
    {
        // This updates the timer while the ball is falling
        if (isFalling && rb.velocity.magnitude > 0)
        {
            float elapsedTime = Time.time - startTime;
            timeText.text = $"Time: {elapsedTime:F2}s";
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Stops the timer when the ball hits the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isFalling = false; 
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero; 
        }
    }
}