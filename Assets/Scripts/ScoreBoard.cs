using UnityEngine;
using UnityEngine.Networking;

public class ScoreBoard : NetworkBehaviour
{
	public static ScoreBoard PlayerAScore;
	public static ScoreBoard PlayerBScore;

	public PlayerSide PlayerSide;

	private TextMesh textMesh;

	[SyncVar]
	public int CurrentScore = 0;

	private void Awake ()
	{
		if (PlayerSide == PlayerSide.A)
			PlayerAScore = this;
		else PlayerBScore = this;

		textMesh = GetComponent<TextMesh>();
	}

	private void Update ()
	{
		textMesh.text = CurrentScore.ToString();
	}

	public static void Goal (PlayerSide playerReceivedGoal)
	{
		if (playerReceivedGoal == PlayerSide.A)
			PlayerAScore.IncrementScore();
		else PlayerBScore.IncrementScore();
	}

	private void IncrementScore ()
	{
		CurrentScore++;
	}
}
