using UnityEngine;
using System.Collections;

[System.Serializable]
public class Cloud
{
	// Move speed of cloud
	public float m_MoveSpeed;
	// Handle of cloud's gameObject
	public GameObject m_Cloud;		
	// Handle of duplicate of m_Cloud
	public GameObject m_CloudFollower;
	// LocalPosition before first Update() is called
	public Vector3 m_OriginalLocalPos;				
}

public class CloudFlow : MonoBehaviour
{
	[Tooltip("Speed of slowest cloud")]
	public float m_MinSpeed = 0.05f;
	[Tooltip("Speed of fastest cloud")]
	public float m_MaxSpeed = 0.3f;
	[Tooltip("Is this a near cloud or not?")]
	public bool NearCloud;

	// Array of cloud
	private Cloud[] m_CloudList = null;	
	// Handle Orthographic Camera in the scene
	private Camera m_Camera = null;
	// Vector3 of middle-left most position at the edge of the camera view
	private Vector3 LeftMostOfScreen;
	// Vector3 of middle-right most position at the edge of the camera view
	private Vector3 RightMostOfScreen;


	// Use this for initialization
	void Start()
	{
		// init m_CloudList
		m_CloudList = new Cloud[transform.childCount];

		// Survey all chilren and put them to m_CloudList
		int index = 0;

		foreach(Transform child in transform)
		{
			// Create new Cloud class
			m_CloudList[index] = new Cloud();

			// Random speed
			m_CloudList[index].m_MoveSpeed = Random.Range(m_MinSpeed,m_MaxSpeed);

			// Set this gameObject to current m_CloudList.m_Cloud
			m_CloudList[index].m_Cloud = child.gameObject;

			// Keep original LocalPosition to use later when we have to pool this cloud when it move off the screen edge
			m_CloudList[index].m_OriginalLocalPos = m_CloudList[index].m_Cloud.transform.localPosition;

			// Increase index
			index++;
		}

		// Make sure we have Orthographic Camera
		FindTheOrthographicCamera();
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Update all children cloud
		int index = 0;

		foreach(Cloud child in m_CloudList)
		{
			if(child.m_Cloud.activeSelf==true)
			{
				// Move current cloud
				m_CloudList[index].m_Cloud.transform.localPosition = new Vector3(m_CloudList[index].m_Cloud.transform.localPosition.x + (m_CloudList[index].m_MoveSpeed * Time.unscaledDeltaTime),
				                                                                 m_CloudList[index].m_Cloud.transform.localPosition.y,
				                                                                 m_CloudList[index].m_Cloud.transform.localPosition.z);

				//Move Left to Right
				if(m_CloudList[index].m_MoveSpeed>0)
				{
					// Move duplicated cloud
					if(m_CloudList[index].m_CloudFollower!=null)
					{
						m_CloudList[index].m_CloudFollower.transform.localPosition = new Vector3(m_CloudList[index].m_Cloud.transform.localPosition.x-(m_CloudList[index].m_Cloud.GetComponent<Renderer>().bounds.size.x),
						                                                                         m_CloudList[index].m_Cloud.transform.localPosition.y,
						                                                                         m_CloudList[index].m_Cloud.transform.localPosition.z);
					}

					// Is this cloud move off right most of the camera edge?
					if(m_CloudList[index].m_Cloud.transform.localPosition.x>RightMostOfScreen.x+m_CloudList[index].m_Cloud.GetComponent<Renderer>().bounds.size.x/2)
					{
							// Random new speed
							m_CloudList[index].m_MoveSpeed = Random.Range(m_MinSpeed,m_MaxSpeed);

							// Pool cloud to other side of screen
						if(NearCloud==true)
						{
							m_CloudList[index].m_Cloud.transform.localPosition = 
								new Vector3(LeftMostOfScreen.x-m_CloudList[index].m_Cloud.GetComponent<Renderer>().bounds.size.x,
							    Random.Range(-m_Camera.orthographicSize/2, m_Camera.orthographicSize),
								Random.Range(-1,-3));
						}
						else
						{
							m_CloudList[index].m_Cloud.transform.localPosition = 
								new Vector3(LeftMostOfScreen.x-m_CloudList[index].m_Cloud.GetComponent<Renderer>().bounds.size.x,
								Random.Range(-m_Camera.orthographicSize/2, m_Camera.orthographicSize),
								Random.Range(1,3));
						}
							
					}
				}
			}
			index++;
		}
	}

	// Find an Orthographic camera in the scene
	void FindTheOrthographicCamera()
	{
		if(m_Camera==null)
		{
			Camera[] CameraList = FindObjectsOfType<Camera>();
			foreach(Camera child in CameraList)
			{
				if(child.orthographic==true)
				{
					// Keep only first Orthographic Camera
					m_Camera = child;
					break;
				}
			}
		}
		
		// Calculate Left/Right most position at the edge of camera view
		if(m_Camera!=null)
		{
			LeftMostOfScreen = m_Camera.ScreenToWorldPoint(new Vector3(0, 0, 0));
			RightMostOfScreen = m_Camera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
		}
	}
}