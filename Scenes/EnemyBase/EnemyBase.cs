using Godot;

public partial class EnemyBase : PathFollow2D
{
	[Export] private float _speed = 100;
	[Export] private PackedScene _projectile;
	[Export] private Marker2D _shootPoint;
	[Export] private Timer _projectileTimer;
	[Export] private float _projectileWaitTime = 3.0f;
	[Export] private float _projectileTimeVariance = 0.75f;

	public override void _Ready()
	{
		SubscribeToSignals();
		StartTimer();
	}

	public override void _PhysicsProcess(double delta)
	{
		MoveAlongPath(delta);
	}

	private void SubscribeToSignals()
	{
		_projectileTimer.Timeout += OnProjectileTimerTimeout;
	}

	private void OnProjectileTimerTimeout()
	{
		FireWeapon();
		StartTimer();
	}

	private void MoveAlongPath(double delta)
	{
		Progress += _speed * (float)delta;

		HandleObjectRemoval();
	}

	private void HandleObjectRemoval()
	{
		var maxProgress = 0.99f;
		if (ProgressRatio > maxProgress)
		{
			SetPhysicsProcess(false);
			CallDeferred(MethodName.QueueFree);
		}
	}

	private void FireWeapon()
	{
		if (_projectile == null)
		{
			return;
		}

		SignalManager.EmitSpawnPoolObject(_shootPoint.GlobalPosition, _projectile);
	}

	private void StartTimer()
	{
		CustomUtils.SetAndStartTimer(_projectileTimer, _projectileWaitTime, _projectileTimeVariance);
	}
}
