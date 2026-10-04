using Godot;

public partial class Spawner : Node
{
	[Export] private Path2D _asteroidSpawnPath;
	[Export] private Path2D _asteroidTargetPath;
	[Export] private PackedScene _asteroidScene;
	[Export] private Timer _asteroidSpawnTimer;

	public override void _Ready()
	{
		SubscribeToSignals();

		// Trigger imediate Asteroid spawning
		OnAsteroidSpawnTimeout();
	}

	private void SubscribeToSignals()
	{
		_asteroidSpawnTimer.Timeout += OnAsteroidSpawnTimeout;
	}

	private void OnAsteroidSpawnTimeout()
	{
		Asteroid asteroid = _asteroidScene.Instantiate<Asteroid>();
		SetRandomSpawnAndTarget(asteroid);
		AddChild(asteroid);
	}

	private Vector2 GetRandomPointOnCurve(Curve2D curve)
	{
		float distance = curve.GetBakedLength() * GD.Randf();
		Vector2 randomCurvePoint = curve.SampleBaked(distance);
		
		return randomCurvePoint;
	}

	private void SetRandomSpawnAndTarget(Asteroid asteroid)
	{
		var spawnPosition = GetRandomPointOnCurve(_asteroidSpawnPath.Curve);
		var targetPosition = GetRandomPointOnCurve(_asteroidTargetPath.Curve);

		GD.Print($"Spawn Position: {spawnPosition}\nTarget Position: {targetPosition}");

		asteroid.SetSpawnAndTarget(spawnPosition, targetPosition);
	}
}
