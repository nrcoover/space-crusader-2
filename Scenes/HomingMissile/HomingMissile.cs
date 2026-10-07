using Godot;

public partial class HomingMissile : Node2D, IPoolItem
{
  [Export] private Area2D _hitArea;
  [Export] private Timer _invincibleTimer;
  [Export] private float _speed = 100.0f;
  [Export] private float _rotationSpeed = 2.0f;

  private Player _playerRef;
  private bool _invincible = false;

  public override void _Ready()
  {
    SubscribeToSignals();
    IdentifyPlayer();
    DeActivate();
  }

  public override void _PhysicsProcess(double delta)
  {
    Move(delta);
  }

  public void Activate()
  {
    ResetRotation();
    Show();
    SetPhysicsProcess(true);
    CustomUtils.ActivateArea2D(_hitArea, true);
    StartTimer();
    SetInvincibility(true);
  }

  public void DeActivate()
  {
    Hide();
    SetPhysicsProcess(false);
    CustomUtils.ActivateArea2D(_hitArea, false);
    StopTimer();
    SetInvincibility(false);
  }

  private void SubscribeToSignals()
  {
    _hitArea.AreaEntered += OnHitAreaEntered;
    _invincibleTimer.Timeout += OnTImerTimeout;
  }

  private void OnHitAreaEntered(Area2D area)
  {
    if (_invincible)
    {
      return;
    }

    DeActivate();
  }

  private void OnTImerTimeout()
  {
    SetInvincibility(false);
  }

  private void IdentifyPlayer() {
		_playerRef = GetTree().GetFirstNodeInGroup(Constants.GroupName.PLAYER) as Player;
		
		if (_playerRef == null) {
			GD.Print($"{Name}: Homing Missile: No player found in scene!");
		}
	}

  private float GetAngleToPlayer()
  {
    return Transform.X.AngleTo(
      GlobalPosition.DirectionTo(_playerRef.GlobalPosition)
    );
  }

  private void Move(double delta)
  {
    float angleToPlayer = GetAngleToPlayer();

    float rotationStep = _rotationSpeed * (float)delta;
    float rotationDirection = Godot.Mathf.Sign(angleToPlayer);
    
    float step = Godot.Mathf.Min(
      Godot.Mathf.Abs(angleToPlayer),
      rotationStep);

    Rotate(step * rotationDirection);

    Position += Transform.X * _speed * (float)delta;
  }

  private void ResetRotation()
  {
    Rotation = 0.0f;
  }

  private void StartTimer()
  {
    _invincibleTimer.Start();
  }

  private void StopTimer()
  {
    _invincibleTimer.Stop();
  }

  private void SetInvincibility(bool value)
  {
    _invincible = value;
  }
}
