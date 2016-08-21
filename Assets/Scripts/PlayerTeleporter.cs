using UnityEngine;
using UnityEngine.Networking;

public enum PlayerSide { A, B }

public class PlayerTeleporter : NetworkBehaviour
{
	const float BORDER_X = -4;
	const float GRACE = 1f;

	public PlayerSide PlayerSide;

	private Transform playerASpawnPoint;
	private Transform playerBSpawnPoint;

	public override void OnStartLocalPlayer ()
	{
		PlayerSide = isServer ? PlayerSide.A : PlayerSide.B;
	}

	private void Start ()
	{
		playerASpawnPoint = GameObject.Find("PlayerASpawnPoint").transform;
		playerBSpawnPoint = GameObject.Find("PlayerBSpawnPoint").transform;
	}

	private void Update ()
	{
		if (!isLocalPlayer) return;

		switch (PlayerSide)
		{
			case PlayerSide.A:
				if (transform.position.x < (BORDER_X - GRACE))
					transform.position = playerASpawnPoint.position;
				break;
			case PlayerSide.B:
				if (transform.position.x > (BORDER_X + GRACE))
					transform.position = playerBSpawnPoint.position;
				break;
		}
	}
}
