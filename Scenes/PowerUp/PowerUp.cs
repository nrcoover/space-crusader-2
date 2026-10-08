using Godot;

public partial class PowerUp : Area2D
{
	[Export] private float _speed = 70.0f;
	[Export] private int _healthBoost = 30;
	[Export] private Timer _timer;
	[Export] private AudioStreamPlayer2D _healthBoostAudio;

	private Vector2 _velocity;

	public override void _Ready()
	{
		SubscribeToSignals();
		SetVelocity();
		SetPosition();
	}

	public override void _PhysicsProcess(double delta)
	{
		Move(delta);
	}

	private void SubscribeToSignals()
	{
		_timer.Timeout += OnTimerTimeout;
		AreaEntered += OnAreaEntered;
		_healthBoostAudio.Finished += OnHealthBoostAudioFinished;
	}

	private void OnTimerTimeout()
	{
		QueueFreeDeferred();
	}

	private void OnAreaEntered(Area2D area)
	{
		Hide();
		CustomUtils.ActivateArea2D(this, false);
		SignalManager.EmitPlayerHealthBoost(_healthBoost);
		PlayHealthBoostAudio();
	}

	private void OnHealthBoostAudioFinished()
	{
		QueueFreeDeferred();
	}

	private void QueueFreeDeferred()
	{
		CallDeferred(MethodName.QueueFree);
	}

	private void SetVelocity()
	{
		_velocity = _speed * Vector2.Down;
	}

	private void SetPosition()
	{
		Rect2 viewportRect = GetViewportRect();

		var minXPosition = 0.25;
		var maxXPosition = 0.75;
		float xPosition = (float)GD.RandRange(
			viewportRect.Size.X * minXPosition, viewportRect.Size.X * maxXPosition
		);

		var minYPosition = 0.15;
		var maxYPosition = 0.35;
		float yPosition = (float)GD.RandRange(
			viewportRect.Size.Y * minYPosition, viewportRect.Size.Y * maxYPosition
		);

		GlobalPosition = new Vector2(xPosition, yPosition);
	}

	private void Move(double delta)
	{
		Position += _velocity * (float)delta;
	}

	private void PlayHealthBoostAudio()
	{
		_healthBoostAudio.Play();
	}
}
