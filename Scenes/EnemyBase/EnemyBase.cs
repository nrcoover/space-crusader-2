using Godot;

// TODO: Create new giant robot enemy to deliver bombs; use regular size robot enemy to fire two lasers; convert _shootPoint to an array of Marker2Ds.
public partial class EnemyBase : PathFollow2D
{
	[Export] private float _speed = 100;
	[Export] private float _missileChance = 0.8f;
	[Export] private PackedScene _projectile;
	[Export] private PackedScene _explosion;
	[Export] private PackedScene _missileScene;
	[Export] private Area2D _hitArea;
	[Export] private HealthBar _healthBar;
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
		_healthBar.HealthBarDepleted += OnHealthBarDepleted;
		_hitArea.AreaEntered += OnHitAreaEntered;
	}

	private void OnProjectileTimerTimeout()
	{
		FireWeapon();
		StartTimer();
	}

	private void OnHealthBarDepleted()
	{
		SignalManager.EmitSpawnPoolObject(GlobalPosition, _explosion);

		HandleMissileCreation();

		CustomUtils.ActivateArea2D(_hitArea, false);
		PlayObjectDestroyedTween();
	}

	private void OnHitAreaEntered(Area2D area)
	{
		if (area is Projectile projectile && _healthBar.Value > 0)
		{
			_healthBar.TakeDamage(projectile.Damage);
		}
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

	private void HandleMissileCreation()
	{
		if (GD.Randf() < _missileChance)
		{
			SignalManager.EmitSpawnPoolObject(GlobalPosition, _missileScene);
		}
	}

	private void PlayObjectDestroyedTween()
	{
		Tween tween = CreateTween();

		var tweenDuration = 0.25f;
		tween.TweenProperty(
			this,
			CanvasItem.PropertyName.Modulate.ToString(),
			new Color("#ff0000"),
			tweenDuration
		);

		tween.TweenCallback(Callable.From(QueueFree));
	}
}
