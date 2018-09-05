using UnityEngine;

public class Player : MonoBehaviour
{
	public GameObject ghostObject;
	public GameObject ghostSpawn;
	GameObject ghost;
	float ghostTimer = 0;
	int punch = 1;

	public float maxRotAngleUp = 40;
	public float maxRotAngleDown = 30;
	float rotAngle;

	public float dashForce = 1;
	bool canDash = true;

	public GameObject camLook;
	public float rotSpeed = 3;
	public float movSpeed = 10;
	private CharacterController cc;

	void Start()
	{
		cc = GetComponent<CharacterController>();
	}

	void Update()
	{
		MovementAndRotation();
		
		SummonGhost();

		CheckAttack();

		if(Input.GetMouseButtonDown(1))
		{
			UnsummonGhost();
		}

		if(Input.GetKeyDown(KeyCode.Space))
		{
			Dash();
		}
	}

	private void Dash()
	{
		if(canDash)
		{
			Vector3 dir = new Vector3(Mathf.Clamp(Input.GetAxis("Horizontal") * 1000, -1, 1) * dashForce, 0.5f, Mathf.Clamp(Input.GetAxis("Vertical") * 1000, -1, 1) * dashForce);
			dir = transform.TransformDirection(dir);
			cc.Move(dir);

			canDash = false;
			Invoke("DashWait", 1);
		}
	}

	void DashWait()
	{
		canDash = true;
	}

	void MovementAndRotation()
	{
		//lock mouse
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		//set up move vector
		Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), Physics.gravity.y * Time.deltaTime / 1, Input.GetAxis("Vertical"));
		dir = transform.TransformDirection(dir);

		rotAngle += -Input.GetAxis("Mouse Y") * rotSpeed;

		//rotate
		camLook.transform.Rotate(Vector3.left * Input.GetAxis("Mouse Y") * rotSpeed);

		//up
		if(rotAngle <= -maxRotAngleUp)
		{
			rotAngle = -maxRotAngleUp;
			camLook.transform.eulerAngles = new Vector3(-maxRotAngleUp, camLook.transform.eulerAngles.y, camLook.transform.eulerAngles.z);
		}

		//down
		if(rotAngle >= maxRotAngleDown)
		{
			rotAngle = maxRotAngleDown;
			camLook.transform.eulerAngles = new Vector3(maxRotAngleDown, camLook.transform.eulerAngles.y, camLook.transform.eulerAngles.z);
		}
		
		transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * rotSpeed);

		//move
		cc.Move(dir * Time.deltaTime * movSpeed);
	}

	void SummonGhost()
	{
		if(Input.GetMouseButtonDown(0))
		{
			if(ghost == null)
			{
				ghost = Instantiate(ghostObject, ghostSpawn.transform.position, ghostSpawn.transform.rotation);
			}

			if (punch <= 2)
			{
				ghost.GetComponent<Animator>().Play("Punch_" + punch);
			}
			else if(punch == 3)
			{
				punch = 1;
				ghost.GetComponent<Animator>().Play("Punch_" + punch);
			}

			ghostTimer = 1;
			punch++;
		}

		ghostTimer -= Time.deltaTime;

		if(ghostTimer <= 0)
		{
			UnsummonGhost();
		}

		if(ghost != null)
		{
			foreach(Material m in ghost.GetComponentInChildren<Renderer>().materials)
			{
				if(ghostTimer >= 0.25f)
				{
					m.color = new Color(m.color.r, m.color.g, m.color.b, 1);
				}
				else
				{
					m.color = new Color(m.color.r, m.color.g, m.color.b, ghostTimer + 0.5f);
				}
			}
		}
	}

	void UnsummonGhost()
	{
		Object.Destroy(ghost);
		ghostTimer = 0;
		punch = 1;
	}

	void CheckAttack()
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(ray, out hit, 1))
		{
			print(hit.transform.name);
		}
	}
}