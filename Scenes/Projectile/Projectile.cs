using Godot;
using System;

public partial class Projectile : Area2D, IPoolItem
{
	[Export] private float _speed = 350.0f;
	[Export] private Vector2 _direction = Vector2.Zero;
	[Export] private bool _isMovingTowardsPlayer = false;
	[Export] private VisibleOnScreenNotifier2D _notifier;
	[Export] private AudioStreamPlayer2D _launchSound;
	[Export] private Timer _lifeTimer;
	[Export] private PackedScene _explosionScene;
	[Export] private Marker2D _explosionMarker;
	[Export] private bool _usesMarker;
	[Export] private float _explosionMargin = 30.0f;
	[Export] private int _damage = 10;

	public int Damage => _damage;

	private Player _playerRef;	
	private Vector2 _velocity;

	public override void _Ready()
	{
		SubscribeToSignals();
		IdentifyPlayer();
		DeActivate();
	}

	public override void _PhysicsProcess(double delta)
	{
		SetPosition(delta);
	}

	public void Activate() {
		StartLifetimeTimer();
		CustomUtils.ActivateArea2D(this, true);
		SetDirection();
		SetVelocity();
		SetPhysicsProcess(true);
		PlayAudio();
	}

	public void DeActivate() {
		CustomUtils.ActivateArea2D(this, false);
		SetPhysicsProcess(false);
		Hide();
		StopLifetimeTimer();
	}

	private void SubscribeToSignals() {
		_lifeTimer.Timeout += OnLifeTimerTimeout;
		_notifier.ScreenExited += OnScreenExited;
		AreaEntered += OnAreaEntered;
	}

	private void OnLifeTimerTimeout() {
		GD.Print($"{Name} OnLifeTimerTimeout!");
		DeActivate();
	}

	private void OnScreenExited() {
		GD.Print($"{Name} OnScreenExited!");
		DeActivate();
	}

	private void OnAreaEntered(Area2D area) {
		GD.Print($"{Name} OnAreaEntered!");

		var explosionPosition = _explosionMarker.GlobalPosition;

		if (!_usesMarker)
		{
			Vector2 direction = GlobalPosition.DirectionTo(area.GlobalPosition).Normalized();
			explosionPosition = GlobalPosition + direction * _explosionMargin;
		}

		SignalManager.EmitSpawnPoolObject(
			_explosionMarker.GlobalPosition, _explosionScene
		);

		DeActivate();
	}

	private void IdentifyPlayer() {
		_playerRef = GetTree().GetFirstNodeInGroup(Constants.GroupName.PLAYER) as Player;
		
		if (_playerRef == null) {
			GD.Print($"{Name}: No player found in scene!");
		}
	}

	private void SetDirection() {
		if (_isMovingTowardsPlayer && _playerRef != null) {
			_direction = GlobalPosition.DirectionTo(_playerRef.GlobalPosition);
		}
	}

	private void SetVelocity() {
		_velocity = _speed * _direction;
	}

	private void SetPosition(double delta) {
		Position += _velocity * (float)delta;
	}

	private void StartLifetimeTimer()
	{
		_lifeTimer.Start();
	}

	private void StopLifetimeTimer()
	{
		_lifeTimer.Stop();
	}

	private void PlayAudio()
	{
		_launchSound.Play();
	}
}
