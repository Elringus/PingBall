using UnityEngine;
using UnityEngine.Networking;

public class Ball : NetworkBehaviour
{
	public static Ball Instance;

	public float MagnetizeForce = 10f;
	public float MagnetizeCD = 10f;

	public Player OwningPlayer;
	public bool IsGrabbed { get { return OwningPlayer; } }
	[SyncVar]
	public string OwningPlayerName = "null";

	private Transform magnetizer;
	private Rigidbody rigidBody;
	private SphereCollider sphereCollider;
	private Material trailMaterial;
	private float magnetizeTimer = 0;
	private NetworkTransform networkTransform;

	public override void OnStartLocalPlayer ()
	{
		
	}

	public void Grab (Player owningPlayer)
	{
		OwningPlayer = owningPlayer;
		sphereCollider.enabled = false;
		rigidBody.isKinematic = true;
		rigidBody.Sleep();
	}

	public void Release ()
	{
		OwningPlayer = null;
		sphereCollider.enabled = true;
		rigidBody.isKinematic = false;
		rigidBody.WakeUp();
		magnetizeTimer = MagnetizeCD;
	}

	public void Shoot (Vector3 shootVector)
	{
		if (IsGrabbed) Release();

		rigidBody.AddForce(shootVector, ForceMode.Impulse);
	}

	private void Awake ()
	{
		Instance = this;
		rigidBody = GetComponent<Rigidbody>();
		sphereCollider = GetComponent<SphereCollider>();
		trailMaterial = GetComponent<TrailRenderer>().material;
		magnetizer = GameObject.Find("BallMagnetizer").transform;
		networkTransform = GetComponent<NetworkTransform>();
	}

	private void Update ()
	{
		UpdateTrailColor();

		if (!isServer)
		{
			if (OwningPlayerName == "Player(Clone)")
			{
				networkTransform.transformSyncMode = NetworkTransform.TransformSyncMode.SyncNone;
				transform.position = Player.PlayerB.GrabPointTransform.position;
			}
			else networkTransform.transformSyncMode = NetworkTransform.TransformSyncMode.SyncRigidbody3D;
			return;
		}

		OwningPlayerName = OwningPlayer != null ? OwningPlayer.name : "null";

		magnetizeTimer -= Time.deltaTime;
		magnetizeTimer = Mathf.Clamp(magnetizeTimer, 0, MagnetizeCD);

		if (IsGrabbed)
			transform.position = OwningPlayer.GrabPointTransform.position;
		else if (magnetizeTimer == 0)
		{
			var directionToMagnetizer = (magnetizer.position - transform.position).normalized;
			rigidBody.AddForce(directionToMagnetizer * MagnetizeForce, ForceMode.Acceleration);
		}
	}

	private void UpdateTrailColor ()
	{
		var velocity = rigidBody.velocity.sqrMagnitude / 400;
		var trailAlpha = Mathf.Clamp(velocity, 0, 0.1f);

		trailMaterial.SetColor("_TintColor", new Color(1, 1, 1, trailAlpha));
	}
}
