using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.Networking;
using System.Linq;

public class Player : RigidbodyFirstPersonController
{
	public static Player PlayerB;

	public GameObject BallPrototype;
	public Transform GrabPointTransform;
	public float ShootForce = 10;
	public bool IsBallInsideTrigger = false;

	public override void OnStartLocalPlayer ()
	{
		base.OnStartLocalPlayer();

		if (isServer)
		{
			var ball = Instantiate(BallPrototype, transform.position, Quaternion.identity) as GameObject;
			NetworkServer.Spawn(ball);
			gameObject.name = "PlayerA";
		}
	}

	private void Start ()
	{
		if (name == "Player(Clone)") PlayerB = this;
	}

	protected override void Update ()
	{
		base.Update();

		if (!IsInitialized)
			return;

		if (Input.GetButtonDown("Grab"))
		{
			CmdOnGrabInput(isServer ? PlayerSide.A : PlayerSide.B, IsBallInsideTrigger);
		}

		if (Input.GetButtonDown("Shoot") || Input.GetAxis("Shoot") > 0)
			if (IsBallInsideTrigger)
				CmdOnShootInput(cam.transform.forward * ShootForce);
	}

	[Command]
	public void CmdOnGrabInput (PlayerSide playerSide, bool isBallInsideTrigger)
	{
		Player owningPlayer;

		if (playerSide == PlayerSide.A) owningPlayer = this;
		else owningPlayer = PlayerB;

		if (Ball.Instance.OwningPlayer == owningPlayer)
			Ball.Instance.Release();
		else if (isBallInsideTrigger)
			Ball.Instance.Grab(owningPlayer);
	}

	[Command]
	public void CmdOnShootInput (Vector3 shootVector)
	{
		Ball.Instance.Shoot(shootVector);
	}
}
