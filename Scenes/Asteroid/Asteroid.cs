using Godot;

public partial class Asteroid : Node2D
{
	[Export] private int _hits = 5;
	[Export] private float _speed = 50.0f;
	[Export] private float _rotationSpeedDegrees = 10.0f;
	[Export] private float _lifeTime = 10.0f;
	[Export] private Sprite2D _sprite;
	[Export] private Area2D _hitArea;
	[Export] private PackedScene _explosionScene;
	[Export] private Godot.Collections.Array<Texture2D> _textures;

	private Vector2 _velocity = Vector2.Right;
	private float _timeAlive = 0.0f;

#region Overrides

	public override void _Ready()
	{
		SubscribeToSignals();
		SetRandomTexture();
	}

	public override void _PhysicsProcess(double delta)
	{
		Move(delta);
		Rotate(delta);
		IncrementTimeAlive(delta);
		HandleObjectRemoval();
	}

#endregion

	public void SetSpawnAndTarget(Vector2 spawnPosition, Vector2 targetPosition)
	{
		GlobalPosition = spawnPosition;
		var minVariance = 0.5f;
		var maxVariance = 1.5f;
		var randomVariance = (float)GD.RandRange(minVariance, maxVariance);

		_velocity = spawnPosition.DirectionTo(targetPosition) * _speed * randomVariance;
		_rotationSpeedDegrees *= randomVariance;

		// TODO: move to utility function
		var rangeDivider = 0.5f;
		var maintainRotation = 1;
		var reverseRotation = -1;
		_rotationSpeedDegrees *= GD.Randf() > rangeDivider ? maintainRotation : reverseRotation;
	}

#region Signals

	private void SubscribeToSignals()
	{
		_hitArea.AreaEntered += OnAreaEntered;
	}

	private void OnAreaEntered(Area2D area)
	{
		DecrementHits();
		HandleObjectRemoval();
	}

#endregion

	private void SetRandomTexture()
	{
		_sprite.Texture = _textures.PickRandom();
	}

	private void SetVelocity()
	{
		_velocity *= _speed;
	}

	private void Move(double delta)
	{
		Position += _velocity * (float)delta;
	}

	private void Rotate(double delta)
	{
		RotationDegrees += _rotationSpeedDegrees * (float)delta;
	}

	private void IncrementTimeAlive(double delta)
	{
		_timeAlive += (float)delta;
	}

	private void DecrementHits()
	{
		_hits--;
	}

	private void HandleObjectRemoval()
	{
		if (_timeAlive > _lifeTime || _hits <= 0)
		{
			BlowUp();
		}
	}

	private void BlowUp()
	{
		SignalManager.EmitSpawnPoolObject(GlobalPosition, _explosionScene);
		CallDeferred(MethodName.QueueFree);
	}
}
