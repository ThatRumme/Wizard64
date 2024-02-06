using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    public enum RotationAxes
    {
        MouseXAndY = 0,
        MouseX = 1,
        MouseY = 2
    }

    public RotationAxes axes = RotationAxes.MouseXAndY;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -60F;
    public float maximumY = 60F;

    public float rotationX;
    public float rotationY;

    public Quaternion originalRotation;
    public Quaternion originalRotationWorld;
    public static Vector2 deltaRot;

    public static float sensitivity = 5;
    private Vector2 controllerVector;

    GameManager gm;
    PlayerInput inputs;

    public GameObject player;

    private void Start()
    {
        gm = GameManager.Instance;
        var rb = GetComponent<Rigidbody>();
        if (rb)
        {
            rb.freezeRotation = true;
        }

        originalRotation = transform.localRotation;
        originalRotationWorld = transform.rotation;

        inputs = GameManager.Instance.inputs;
        
    }

    private void LateUpdate()
    {
        transform.position = player.transform.position;

        if (!gm.gameActive)
            return;

        deltaRot.x = inputs.Main.MouseX.ReadValue<float>() * sensitivity * 0.01f;
        deltaRot.y = inputs.Main.MouseY.ReadValue<float>() * sensitivity * 0.01f;

        switch (axes)
        {
            case RotationAxes.MouseXAndY:
                {
                    rotationX = ClampAngle(rotationX + deltaRot.x, minimumX, maximumX);
                    rotationY = ClampAngle(rotationY + deltaRot.y, minimumY, maximumY);

                    Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
                    Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);

                    transform.localRotation = xQuaternion * (originalRotation * yQuaternion);
                    break;
                }

            case RotationAxes.MouseX:
                {
                    rotationX = ClampAngle(rotationX + deltaRot.x, minimumX, maximumX);

                    Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
                    transform.localRotation = originalRotation * xQuaternion;
                    break;
                }

            default:
                {
                    rotationY = ClampAngle(rotationY + deltaRot.y, minimumY, maximumY);

                    Quaternion yQuaternion = Quaternion.AngleAxis(-rotationY, Vector3.right);
                    transform.localRotation = originalRotation * yQuaternion;
                    break;
                }
        }
    }

    public void SetRotation(float x, float y)
    {
        rotationX = ClampAngle(x, minimumX, maximumX);
        rotationY = ClampAngle(y, minimumY, maximumY);

        Quaternion xQuaternion = Quaternion.AngleAxis(x, Vector3.up);
        Quaternion yQuaternion = Quaternion.AngleAxis(y, -Vector3.right);

        transform.localRotation = originalRotation * xQuaternion * yQuaternion;
    }

   

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
        {
            angle += 360F;
        }

        if (angle > 360F)
        {
            angle -= 360F;
        }

        return Mathf.Clamp(angle, min, max);
    }
}

