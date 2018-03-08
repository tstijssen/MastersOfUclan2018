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
	// Use this for initialization
	void Start () {
		//switch(Type)
  //      {
  //          case PickupType.Electric:
  //              Value = 50;     // shield health value
  //              Duration = 0;   // instant, shield stays until destroyed
  //              break;
  //          case PickupType.Health:
  //              Value = 20;   // health pack value
  //              Duration = 0;   // instant
  //              break;
  //          case PickupType.PowerUp:
  //              Value = 0;     // dunno
  //              Duration = 0;   // dunno
  //              break;
  //          case PickupType.ReloadSpeed:
  //              Value = 2;     // multiplier of reload speed
  //              Duration = 5.0f; // 5 seconds
  //              break;
  //      }
	}

    private void OnTriggerEnter(Collider other)
    {
        ColliderTag = other.tag;
    }

    // Update is called once per frame
    void Update () {
        transform.Rotate(SpinSpeed * Time.deltaTime,SpinSpeed * Time.deltaTime,0);
	}

}
