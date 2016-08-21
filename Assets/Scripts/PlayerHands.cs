using UnityEngine;

public class PlayerHands : MonoBehaviour
{
	public Player player;

	private void OnTriggerEnter (Collider otherCollider)
	{
		if (!player.isLocalPlayer) return;

		if (otherCollider.CompareTag("BallTrigger"))
			player.IsBallInsideTrigger = true;
	}

	private void OnTriggerExit (Collider otherCollider)
	{
		if (!player.isLocalPlayer) return;

		if (otherCollider.CompareTag("BallTrigger"))
			player.IsBallInsideTrigger = false;
	}
}
