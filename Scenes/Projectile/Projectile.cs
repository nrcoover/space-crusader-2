using Godot;
using System;

public partial class Projectile : Area2D
{
	[Export] private float _speed = 350.0f;
	[Export] private Vector2 _direction = Vector2.Zero;
	[Export] private bool _isMovingTowardsPlayer = false;
	[Export] private VisibleOnScreenNotifier2D _notifier;
	[Export] private LifeTime _lifeTime;

	private Player _playerRef;	
	private Vector2 _velocity;

	public override void _Ready()
	{
		SubscribeToSignals();
		IdentifyPlayer();
		SetDirection();
		SetVelocity();
	}

	public override void _PhysicsProcess(double delta)
	{
		SetPosition(delta);
	}

	private void SubscribeToSignals() {
		_lifeTime.LifeTimeExpired += OnLifeTimeExpired;
		_notifier.ScreenExited += OnScreenExited;
		AreaEntered += OnAreaEntered;
	}

	private void OnLifeTimeExpired() {
		GD.Print($"{Name} OnLifeTimeExpired!");
		CallDeferred(MethodName.QueueFree);
	}

	private void OnScreenExited() {
		GD.Print($"{Name} OnScreenExited!");
		CallDeferred(MethodName.QueueFree);
	}

	private void OnAreaEntered(Area2D area) {
		GD.Print($"{Name} OnAreaEntered!");
		CallDeferred(MethodName.QueueFree);
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
}
