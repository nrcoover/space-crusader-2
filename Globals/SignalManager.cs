using Godot;

public partial class SignalManager : Node
{
	public static SignalManager Instance { get; private set; }

	[Signal] public delegate void SpawnPoolObjectEventHandler(Vector2 position, PackedScene scene);
	[Signal] public delegate void PlayerTakeDamageEventHandler(int damage);
	[Signal] public delegate void PlayerHealthBoostEventHandler(int health);
	[Signal] public delegate void PlayerScoredEventHandler(int score);

	public override void _Ready()
	{
		Instance = this;
	}

	public static void EmitSpawnPoolObject(Vector2 position, PackedScene scene)
	{
		Instance.EmitSignal(SignalName.SpawnPoolObject, position, scene);
	}

	public static void EmitPlayerTakeDamage(int damage)
	{
		Instance.EmitSignal(SignalName.PlayerTakeDamage, damage);
	}

	public static void EmitPlayerHealthBoost(int health)
	{
		Instance.EmitSignal(SignalName.PlayerHealthBoost, health);
	}

	public static void EmitPlayerScored(int score)
	{
		Instance.EmitSignal(SignalName.PlayerScored, score);
	}
}
