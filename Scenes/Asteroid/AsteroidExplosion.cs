using System.Collections.Generic;
using Godot;

public partial class AsteroidExplosion : Node2D, IPoolItem
{
	[Export] private Timer _timer;
	[Export] private Node2D _piecesContainer;
	[Export] private AudioStreamPlayer2D _audio;

	private List<AsteroidPiece> _pieces = new();

	public override void _Ready()
	{
		SubscribeToSignals();
		GetPieces();
	}

#region IPoolItem Interface

	public void Activate()
	{
		Show();
		ActivatePieces();
		StartTimer();
		PlayAudio();
	}

	public void DeActivate()
	{
		Hide();
		ResetPieces();
	}

#endregion

#region Signals

	private void SubscribeToSignals()
	{
		_timer.Timeout += DeActivate;
	}

#endregion

	private void GetPieces()
	{
		foreach (var node in _piecesContainer.GetChildren())
		{
			if (node is AsteroidPiece piece)
			{
				_pieces.Add(piece);
			}
		}
	}

	private void ActivatePieces()
	{
		foreach (var piece in _pieces)
		{
			piece.SetProcess(true);
		}
	}

	private void ResetPieces()
	{
		foreach (var piece in _pieces)
		{
			piece.Reset();
		}
	}

	private void StartTimer()
	{
		_timer.Start();
	}

	private void PlayAudio()
	{
		_audio.Play();
	}
}
