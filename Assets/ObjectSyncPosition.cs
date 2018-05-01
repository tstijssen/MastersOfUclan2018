using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ObjectSyncPosition : NetworkBehaviour {

    private Transform myTransform;
    [SerializeField] float lerpRate = 5;
    [SyncVar] private Vector3 syncPos;
    [SyncVar] private Quaternion syncRot;
    //    private NetworkIdentity theNetID;

    private Vector3 lastPos;
    private Quaternion lastRot;
    public float threshold = 0.5f;
    public float rotationThreshold = 0.5f;

    void Start()
    {
        myTransform = GetComponent<Transform>();
        syncPos = GetComponent<Transform>().position;
        syncRot = GetComponent<Transform>().rotation;
    }


    void FixedUpdate()
    {
        TransmitPosition();
        LerpPosition();
    }

    void LerpPosition()
    {
        if (!hasAuthority)
        {
            myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime * lerpRate);
            myTransform.rotation = Quaternion.Lerp(myTransform.rotation, syncRot, Time.deltaTime * lerpRate);
        }
    }

    [Command]
    void Cmd_ProvidePositionToServer(Vector3 pos)
    {
        syncPos = pos;      
    }

    [Command]
    void Cmd_ProvideRotationToServer(Quaternion rot)
    {
        syncRot = rot;
    }

    [ClientCallback]
    void TransmitPosition()
    {
        if (hasAuthority && (Vector3.Distance(myTransform.position, lastPos) > threshold || Quaternion.Angle(myTransform.rotation, lastRot) > rotationThreshold))
        {
            Cmd_ProvidePositionToServer(myTransform.position);
            Cmd_ProvideRotationToServer(myTransform.rotation);
            lastPos = myTransform.position;
            lastRot = myTransform.rotation;
        }
    }
}
