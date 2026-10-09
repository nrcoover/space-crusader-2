using Godot;

public partial class WaveManager : Node
{
  const float DEFAULT_WAVE_INTERVAL = 4.0f;
  const float DEFAULT_SHIP_INTERVAL = 1.0f;

  [Export] private Godot.Collections.Array<Wave> _waves = new();
  [Export] private Timer _waveTimer;
  [Export] private Timer _shipTimer;
  [Export] private Node _pathsContainer;

  private Wave _currentWave;
  private Path2D _currentPath;
  private Godot.Collections.Array<Path2D> _pathsList = new();
  private int _currentWaveIndex = -1; // Increments to 0 on initialization
  private int _enemiesSpawned = 0;

  public override void _Ready()
  {
    SetupPaths();

    if (_pathsList.Count == 0 || _waves.Count == 0)
    {
      GD.PrintErr("No Waves or Paths!");
      return;
    }

    SubscribeToSignals();
    StartNextWave();
  }

  private void SubscribeToSignals()
  {
    _waveTimer.Timeout += OnWaveTimerTimeout;
    _shipTimer.Timeout += OnShipTimerTimeout;
  }

  private void OnWaveTimerTimeout()
  {
    StartNextWave();
  }

  private void OnShipTimerTimeout()
  {
    CreateEnemy();

    if (_enemiesSpawned == _currentWave.EnemyCount)
    {
      StopShipTimer();
      StartWaveTimer(_currentWave.WaveInterval);
    }
    else
    {
      StartShipTimer();
    }
  } 

  private void SetupPaths()
  {
    foreach (var node in _pathsContainer.GetChildren())
    {
      if (node is Path2D path) 
      {
        _pathsList.Add(path);
      }
    }
  }

  private void StartNextWave()
  {
    _currentWaveIndex = (_currentWaveIndex + 1) % _waves.Count;
    _currentWave = _waves[_currentWaveIndex];
    _currentPath = _pathsList.PickRandom();
    _enemiesSpawned = 0;
    _shipTimer.Start(_currentWave.SpawnInterval);
  }

  private void StartShipTimer(float interval = DEFAULT_SHIP_INTERVAL)
  {
    _shipTimer.Start(interval);
  }

  private void StopShipTimer()
  {
    _shipTimer.Stop();
  }

  private void StartWaveTimer(float interval = DEFAULT_WAVE_INTERVAL)
  {
    _waveTimer.Start(interval);
  }

  private void StopWaveTimer()
  {
    _waveTimer.Stop();
  }

  private void CreateEnemy()
  {
    var enemy = _currentWave.EnemyScene.Instantiate<EnemyBase>();
    _currentPath.AddChild(enemy);
    _enemiesSpawned++;
  }
}
