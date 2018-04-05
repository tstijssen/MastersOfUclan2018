using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum PickupType {Health, PowerUp, Shield, ReloadSpeed};

public class PickupBehaviour : MonoBehaviour {

    public PickupType Type;
    public float Value;
    public float SpinSpeed = 5;
    public float Duration;

    public string ColliderTag;

    private void OnTriggerEnter(Collider other)
    {
        ColliderTag = other.tag;
    }

    // Update is called once per frame
    void Update ()
    {
        transform.Rotate(SpinSpeed * Time.deltaTime,SpinSpeed * Time.deltaTime,0);
	}

}
