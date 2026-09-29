using Godot;
using System;

public partial class Projectile : Area2D, IPoolItem
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
		DeActivate();
	}

	public override void _PhysicsProcess(double delta)
	{
		SetPosition(delta);
	}

	public void Activate() {
		CustomUtils.ActivateArea2D(this, true);
		SetDirection();
		SetVelocity();
		SetPhysicsProcess(true);
		Show();
	}

	public void DeActivate() {
		CustomUtils.ActivateArea2D(this, false);
		SetPhysicsProcess(false);
		Hide();
	}

	private void SubscribeToSignals() {
		_lifeTime.LifeTimeExpired += OnLifeTimeExpired;
		_notifier.ScreenExited += OnScreenExited;
		AreaEntered += OnAreaEntered;
	}

	private void OnLifeTimeExpired() {
		// GD.Print($"{Name} OnLifeTimeExpired!");
		// CallDeferred(MethodName.QueueFree);
	}

	private void OnScreenExited() {
		GD.Print($"{Name} OnScreenExited!");
		DeActivate();
	}

	private void OnAreaEntered(Area2D area) {
		GD.Print($"{Name} OnAreaEntered!");
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
}
