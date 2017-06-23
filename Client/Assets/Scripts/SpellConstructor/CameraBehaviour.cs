using UnityEngine;

[AddComponentMenu("Camera-Control/Mouse drag Orbit with zoom")]
public class CameraBehaviour : MonoBehaviour {
    public Transform target;
    public float distance = 5.0f;
    public float xSpeed = 3f;
    public float ySpeed = 3f;
    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;
    public float distanceMin = .5f;
    public float distanceMax = 15f;
    public float smoothTime = 2f;
    public float zoomSensitivity = 5f;
    float rotationYAxis = 0.0f;
    float rotationXAxis = 0.0f;
    float velocityX = 0.0f;
    float velocityY = 0.0f;

    // Use this for initialization
    void Start() {
        Vector3 angles = transform.eulerAngles;
        rotationYAxis = angles.y;
        rotationXAxis = angles.x;
        // Make the rigid body not change rotation
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }
    }

    void FixedUpdate() {
        if (target)
        {
            if (Input.GetMouseButton(1))
            {
                velocityX += xSpeed * Input.GetAxis("Mouse X") * distance * 0.02f;
                velocityY += ySpeed * Input.GetAxis("Mouse Y") * distance * 0.02f;
            }
            rotationYAxis += velocityX;
            rotationXAxis -= velocityY;
            //rotationXAxis = ClampAngle(rotationXAxis, yMinLimit, yMaxLimit); //TODO: decide if this needs to be active
            Quaternion rotation = Quaternion.Euler(rotationXAxis, rotationYAxis, 0);
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            distance = Mathf.Clamp(distance - scroll * zoomSensitivity, distanceMin, distanceMax);

            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + target.position;

            transform.rotation = rotation;
            transform.position = position;

            velocityX = Mathf.Lerp(velocityX, 0, Time.deltaTime * 1f / smoothTime * (float)System.Math.Sqrt(distance)); //slowly decrease velocity from current to 0
            velocityY = Mathf.Lerp(velocityY, 0, Time.deltaTime * 1f / smoothTime * (float)System.Math.Sqrt(distance));
        }
    }
    //Limits the value of an angle 
    public static float ClampAngle(float angle, float min, float max) {
        if (angle < -360F) 
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}