using Godot;

public partial class AsteroidPiece : Node2D
{
	[Export] private float _speed = 350.0f;
	[Export] private float _rotationSpeedDegreesBase = 400.0f;
	[Export] private Godot.Collections.Array<Texture2D> _textures;
	[Export] private Sprite2D _sprite;

	private Vector2 _velocity = Vector2.Right;
	private float _rotationSpeedDegrees = 0.0f;
	private Transform2D _startTransform;

	public override void _Ready()
	{
		InitializeStartTransform();
		Reset();
	}

	public override void _Process(double delta)
	{
		Move(delta);
		Rotate(delta);
	}
	
	public void Reset()
	{
		ResetTransform();
		SetRotationSpeed();
		SetVelocity();
		SetRandomTexture();
		SetProcess(false);
	}

	private void SetRotationSpeed()
	{
		var minSpeed = 0.6;
		var maxSpeed = 1.2;
		_rotationSpeedDegrees = _rotationSpeedDegreesBase;
		_rotationSpeedDegrees *= (float)GD.RandRange(minSpeed, maxSpeed);

		// TODO: move to utility function
		var rangeDivider = 0.5f;
		var maintainRotation = 1;
		var reverseRotation = -1;
		_rotationSpeedDegrees *= GD.Randf() > rangeDivider ? maintainRotation : reverseRotation;
	}

	private void InitializeStartTransform()
	{
		_startTransform = Transform;
	}

	private void ResetTransform()
	{
		Transform = _startTransform;
	}

	private void SetVelocity()
	{
		var minVelocity = 0.6;
		var maxVelocity = 1.2;
		_velocity = Position.Normalized() * _speed * (float)GD.RandRange(minVelocity, maxVelocity);
	}

	private void SetRandomTexture()
	{
		_sprite.Texture = _textures.PickRandom();
	}

	private void Move(double delta)
	{
		Position += _velocity * (float)delta;
	}

	private void Rotate(double delta)
	{
		RotationDegrees += _rotationSpeedDegrees * (float)delta;
	}
}
