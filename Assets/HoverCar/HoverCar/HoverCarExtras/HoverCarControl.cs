using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class HoverCarControl : MonoBehaviour
{
  Rigidbody m_body;
  float m_deadZone = 0.1f;

  public float m_hoverForce = 9.0f;
	//Force of hover
  public float m_StabilizedHoverHeight = 2.0f;
	//Height of hover
  public GameObject[] HoverPointsGameObjects;
	//Points where hover will push down
  public float m_forwardAcl = 100.0f;
	//Foward Acceleation of car
  public float m_backwardAcl = 25.0f;

  public float m_sideAcl = 50.0f;
	//Backwords/reverse Acceleration of car
  float m_currThrust = 0.0f;

    // left right accelleration of car
  float m_sideThrust = 0.0f;

	//Do not modify! Current speed
  public float m_turnStrength = 10f;
	//Strength of the turn
  float CurrentTurnAngle = 0.0f;
	//Current Turn Rotation
  public GameObject LeftBreak;
	//Break Left GameObject
  public GameObject RightBreak;
    //Break Right GameObject

    CarFireControl m_FireControl;

  int m_layerMask;

    string HorizontalMove = "AuxButtons";
    string VerticalMove = "Vertical";
    string TurnMove = "Horizontal";
    string FireButton = "Fire1";

  void Start()
  {
    m_body = GetComponent<Rigidbody>();

    m_layerMask = 1 << LayerMask.NameToLayer("Characters");
    m_layerMask = ~m_layerMask;

       m_FireControl = GetComponent<CarFireControl>();
    }

  void OnDrawGizmos()
  {

    //  Hover Force
    RaycastHit hit;
    for (int i = 0; i < HoverPointsGameObjects.Length; i++)
    {
      var hoverPoint = HoverPointsGameObjects [i];
      if (Physics.Raycast(hoverPoint.transform.position, 
                          -Vector3.up, out hit,
                          m_StabilizedHoverHeight, 
                          m_layerMask))
      {
        Gizmos.color = Color.green;
				//Color if correctly alligned
        Gizmos.DrawLine(hoverPoint.transform.position, hit.point);
        Gizmos.DrawSphere(hit.point, 0.5f);
      } else
      {
        Gizmos.color = Color.red;
				//Color if incorrectly alligned
        Gizmos.DrawLine(hoverPoint.transform.position, 
                       hoverPoint.transform.position - Vector3.up * m_StabilizedHoverHeight);
      }
    }
  }
	
  void Update()
  {

        if(!m_FireControl.m_Alive)
        {
            return;
        }

        // shooting
        bool fire = Input.GetButton(FireButton);
        bool fireRelease = Input.GetButtonUp(FireButton);

        if (fire)
            m_FireControl.Shoot();
        if (fireRelease)
            m_FireControl.ShootRelease();

        // Main Thrust
        m_currThrust = 0.0f;
        float aclAxis = Input.GetAxis(VerticalMove);
        if (aclAxis > m_deadZone)
          m_currThrust = aclAxis * m_forwardAcl;
        else if (aclAxis < -m_deadZone)
          m_currThrust = aclAxis * m_backwardAcl;

        m_sideThrust = 0.0f;
        float aclSAxis = Input.GetAxis(HorizontalMove);
        if (Mathf.Abs(aclSAxis) > m_deadZone)
            m_sideThrust = aclSAxis * m_sideAcl;

        // Turning
        CurrentTurnAngle = 0.0f;
        float turnAxis = Input.GetAxis(TurnMove);
        if (Mathf.Abs(turnAxis) > m_deadZone)
            CurrentTurnAngle = turnAxis;
  }

  void FixedUpdate()
  {

    //  Hover Force
    RaycastHit hit;
    for (int i = 0; i < HoverPointsGameObjects.Length; i++)
    {
      var hoverPoint = HoverPointsGameObjects [i];
      if (Physics.Raycast(hoverPoint.transform.position, 
                          -Vector3.up, out hit,
                          m_StabilizedHoverHeight,
                          m_layerMask))
        m_body.AddForceAtPosition(Vector3.up 
          * m_hoverForce
          * (1.0f - (hit.distance / m_StabilizedHoverHeight)), 
                                  hoverPoint.transform.position);
      else
      {
        if (transform.position.y > hoverPoint.transform.position.y)
          m_body.AddForceAtPosition(
            hoverPoint.transform.up * m_hoverForce,
            hoverPoint.transform.position);
        else
					//adding force to car
          m_body.AddForceAtPosition(
            hoverPoint.transform.up * -m_hoverForce,
            hoverPoint.transform.position);
      }
    }

    // Forward
    if (Mathf.Abs(m_currThrust) > 0)
      m_body.AddForce(transform.forward * m_currThrust);

    if (Mathf.Abs(m_sideThrust) > 0)
      m_body.AddForce(transform.right * m_sideThrust);

        // Turn
        if (CurrentTurnAngle > 0)
    {
      m_body.AddRelativeTorque(Vector3.up * CurrentTurnAngle * m_turnStrength);
    } else if (CurrentTurnAngle < 0)
    {
      m_body.AddRelativeTorque(Vector3.up * CurrentTurnAngle * m_turnStrength);
    }
  }
}
