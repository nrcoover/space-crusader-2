using Godot;
using System;

public partial class HealthBar : TextureProgressBar
{
	[Signal] public delegate void HealthBarDepletedEventHandler();

	private static readonly Color COLOUR_DANGER = new Color("9A33FF");
	private static readonly Color COLOUR_MIDDLE = new Color("FF00CC");	
	private static readonly Color COLOUR_GOOD = new Color("00FFC8");

	[Export] private int _levelLow = 30;
	[Export] private int _levelMedium = 60;
	[Export] private int _maxHealth = 100;
	[Export] private int _startHealth = 100;

	private bool _isDead = false;

	public override void _Ready()
	{
		InitializeHealthValues();
		SetColor();
	}

	public void IncrementValue(int value)
	{
		Value += value;

		if (Value <= 0 && !_isDead)
		{
			_isDead = true;
			EmitSignal(SignalName.HealthBarDepleted);
		}

		SetColor();
	}

	public void TakeDamage(int value)
	{
		IncrementValue(-value);
		SetColor();
	}

	private void InitializeHealthValues()
	{
		MaxValue = _maxHealth;
		Value = _startHealth;
	}

	private void SetColor()
	{
		if (Value < _levelLow)
		{
			TintProgress = COLOUR_DANGER;
		}
		else if (Value < _levelMedium)
		{
			TintProgress = COLOUR_MIDDLE;
		}
		else
		{
			TintProgress = COLOUR_GOOD;
		}
	}
}
