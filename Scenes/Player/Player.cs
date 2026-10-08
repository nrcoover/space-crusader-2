using Godot;

public partial class Player : Area2D
{
	private const float MARGIN = 64.0f;

	[Export] private float _speed = 500;
	[Export] private PackedScene _playerLaser;
	[Export] private Marker2D _shootPoint;
	[Export] private int _collisionDamage = 30;

	private Vector2 _upperLeft;
	private Vector2 _lowerRight;

	public override void _EnterTree() 
	{
		AddToGroup(Constants.GroupName.PLAYER);
	}

	public override void _Ready()
	{
		SubscribeToSignals();
		SetLimits();
	}

	private void SubscribeToSignals()
	{
		AreaEntered += OnAreaEntered;
	}

	private void OnAreaEntered(Area2D area)
	{
		if (area is Projectile projectile)
		{
			SignalManager.EmitPlayerTakeDamage(projectile.Damage);
		}
		else if (area.IsInGroup(Constants.GroupName.COLLISION))
		{
			GD.Print("Collision Damage, ", area.Name);
			SignalManager.EmitPlayerTakeDamage(_collisionDamage);
		}
	}

	private void SetLimits() {
		Rect2 viewport = GetViewportRect();

		_upperLeft = new Vector2(MARGIN, MARGIN);
		_lowerRight = new Vector2(viewport.Size.X - MARGIN, viewport.Size.Y - MARGIN);
	}

	public override void _PhysicsProcess(double delta)
	{
		MovePlayer(delta);
		ShootLaser();
	}

	private Vector2 GetInput() {
		var vector = new Vector2 (
			Input.GetAxis("left", "right"),
			Input.GetAxis("up", "down")
		);

		return vector.Normalized();
	}

	private void MovePlayer(double delta) {
		var moveDirection = GetInput();
		
		Position += moveDirection * (float)delta * _speed;
		Position = Position.Clamp(_upperLeft, _lowerRight);
	}

	private void ShootLaser()
	{
		if (Input.IsActionJustPressed("shoot"))
		{
			SignalManager.EmitSpawnPoolObject(_shootPoint.GlobalPosition, _playerLaser);
		}
	}
}
