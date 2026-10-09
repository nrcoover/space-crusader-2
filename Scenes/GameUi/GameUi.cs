using Godot;

public partial class GameUi : Control
{
	[Export] private HealthBar _healthBar;
	[Export] private Label _scoreLabel;
	[Export] private ColorRect _gameOverColorRect;
	[Export] private AudioStreamPlayer _musicPlayer;
	[Export] private AudioStreamPlayer _gameOverSoundPlayer;

	private int _score = 0;

	public override void _Ready()
	{
		GetTree().Paused = false;

		SubscribeToSignals();
		HideGameOverScreen();
	}

	public override void _ExitTree()
	{
		UnsubscribeFromSignals();
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_cancel"))
		{
			GetTree().ReloadCurrentScene();
		}
	}

	private void SubscribeToSignals()
	{
		SignalManager.Instance.PlayerTakeDamage += OnPlayerTakeDamage;
		SignalManager.Instance.PlayerHealthBoost += OnPlayerHealthBoost;
		SignalManager.Instance.PlayerScored += OnPlayerScored;
		_healthBar.HealthBarDepleted += OnHealthBarDepleted;
	}

	private void UnsubscribeFromSignals()
	{
		SignalManager.Instance.PlayerTakeDamage -= OnPlayerTakeDamage;
		SignalManager.Instance.PlayerHealthBoost -= OnPlayerHealthBoost;
		SignalManager.Instance.PlayerScored -= OnPlayerScored;
	}

	private void OnPlayerTakeDamage(int damage)
	{
		_healthBar.TakeDamage(damage);
	}

	private void OnPlayerHealthBoost(int health)
	{
		_healthBar.IncrementValue(health);
	}

	private void OnHealthBarDepleted()
	{
		ShowGameOverScreen();
		PlayGameOverAudio();

		GetTree().Paused = true;
	}

	private void OnPlayerScored(int points)
	{
		_score += points;
		UpdateScoreLabel();
	}

	private void ShowGameOverScreen()
	{
		_gameOverColorRect.Show();
	}

	private void HideGameOverScreen()
	{
		_gameOverColorRect.Hide();
	}

	private void PlayGameOverAudio()
	{
		_gameOverSoundPlayer.Play();
	}

	private void UpdateScoreLabel()
	{
		_scoreLabel.Text = _score.ToString("D5");
	}
}
