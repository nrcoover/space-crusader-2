using Godot;
using System;

public partial class EnemyBomb : Projectile
{
	[Export] private Sprite2D _sprite;

	private float _rotationSpeed = 10.0f;

	public override void _Ready() {
		base._Ready();

		//TODO: set this rotation speed with helper function to generate random direction and speed
		_rotationSpeed = 1.0f;
	}
	
	public override void _Process(double delta)
	{
		Rotate(delta);
	}

	private void Rotate(double delta) {
		RotationDegrees += 90f * (float)delta * _rotationSpeed;
	}
}
