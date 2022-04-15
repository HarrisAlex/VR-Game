using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;

public class VrController : MonoBehaviour
{

    public XRNode inputSource;

    Vector2 inputAxis;
    CharacterController characterController;
    [SerializeField] float speed = 1;

    XROrigin origin;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();

        origin = GetComponent<XROrigin>();
    }

    // Update is called once per frame
    void Update()
    {
        InputDevice inputDevice = InputDevices.GetDeviceAtXRNode(inputSource);
        inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
    }

    void FixedUpdate()
    {
        Quaternion headRotation = Quaternion.Euler(0, origin.Camera.transform.eulerAngles.y, 0);
        Vector3 direction = headRotation * new Vector3(inputAxis.x * speed, 0, inputAxis.y * speed);

        direction.y = Physics.gravity.y;

        characterController.Move(direction * Time.fixedDeltaTime);


        //Match character controller dimensions to camera position
        characterController.height = origin.CameraInOriginSpaceHeight + 0.2f;
        Vector3 capsuleCenter = transform.InverseTransformPoint(origin.Camera.transform.position);
        characterController.center = new Vector3(capsuleCenter.x, characterController.height / 2 + characterController.skinWidth, capsuleCenter.z);
    }
}
