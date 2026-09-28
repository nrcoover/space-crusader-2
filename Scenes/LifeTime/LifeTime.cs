using Godot;
using System;

public partial class LifeTime : Node
{
	[Signal] public delegate void LifeTimeExpiredEventHandler();

	[Export] private float _lifeTime = 2.0f;
	[Export] private Timer _timer;

	public override void _Ready()
	{
		SubscribeToSignals();
		BeginTimer();
	}

	private void SubscribeToSignals() {
		_timer.Timeout += OnTimeout;
	}

	private void OnTimeout() {
		EmitSignal(SignalName.LifeTimeExpired);
	}

	private void BeginTimer() {
		_timer.Start(_lifeTime);
	}
}
