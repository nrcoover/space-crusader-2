using Godot;

public partial class SignalManager : Node
{
	public static SignalManager Instance { get; private set; }

	[Signal] public delegate void SpawnPoolObjectEventHandler(Vector2 position, PackedScene scene);

	public override void _Ready()
	{
		Instance = this;
	}

	public static void EmitSpawnPoolObject(Vector2 position, PackedScene scene)
	{
		Instance.EmitSignal(SignalName.SpawnPoolObject, position, scene);
	}
}
