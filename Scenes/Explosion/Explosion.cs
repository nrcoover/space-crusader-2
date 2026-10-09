using Godot;

public partial class Explosion : Node2D, IPoolItem
{
	[Export] private CpuParticles2D _particles;
	[Export] private AudioStreamPlayer2D _sound;

	public override void _Ready()
	{
		SubscribeToSignals();
	}

	public void Activate()
	{
		_particles.Restart();
		_sound.Play();
	}

	public void DeActivate()
	{
		Hide();
	}

	private void SubscribeToSignals()
	{
		_sound.Finished += DeActivate;
	}
}
