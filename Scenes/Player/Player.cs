using Godot;
using System;

public partial class Player : Area2D
{
	private const float MARGIN = 64.0f;

	[Export] private float _speed = 500;

	private Vector2 _upperLeft;
	private Vector2 _lowerRight;

	public override void _EnterTree() 
	{
		AddToGroup(Constants.GroupName.PLAYER);
	}

	public override void _Ready()
	{
		SetLimits();
	}

	private void SetLimits() {
		Rect2 viewport = GetViewportRect();

		_upperLeft = new Vector2(MARGIN, MARGIN);
		_lowerRight = new Vector2(viewport.Size.X - MARGIN, viewport.Size.Y - MARGIN);
	}

	public override void _PhysicsProcess(double delta)
	{
		MovePlayer(delta);
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
}
