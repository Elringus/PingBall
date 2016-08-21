using UnityEngine;
using UnityEngine.Networking;

public class GateTrigger : NetworkBehaviour
{
	public PlayerSide PlayerSide;
	public GameObject ExplosionVfx;

	private void OnTriggerEnter (Collider otherCollider)
	{
		if (isServer && otherCollider.CompareTag("BallTrigger"))
		{
			ScoreBoard.Goal(PlayerSide);
			RpcPlayExplosion(otherCollider.transform.position);
		}
	}

	[ClientRpc]
	void RpcPlayExplosion (Vector3 position)
	{
		Instantiate(ExplosionVfx, position, Quaternion.identity);
	}
}
