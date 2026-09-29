using System.Collections.Generic;
using System.Linq;
using Godot;

public class ScenePool
{
  private const bool DEBUG_POOL = true;

  private readonly List<Node2D> _items = new();
  private readonly PackedScene _packedScene;
  private readonly Node _container;

  int ActiveCount => _items.Count(n => n.Visible);

  public ScenePool(int initialCount, PackedScene scene, Node container)
  {
    _container = container;
    _packedScene = scene;

    Log($"Creating pool with {initialCount} items");

    for (int i = 0; i < initialCount; i++)
    {
      AddNewItem();
    }
  }

  private void AddNewItem()
  {
    Node2D newItem = _packedScene.Instantiate<Node2D>();

    if (newItem is not IPoolItem)
    {
      GD.PrintErr($"{newItem.Name} does not implement IPoolItem");
    }

    newItem.Hide();
    _container.AddChild(newItem);
    _items.Add(newItem);

    Log($"New instance {newItem.Name} total: {_items.Count}");
  }

  private void Log(string message) 
  {
    if (DEBUG_POOL)
    {
      GD.Print($"[ScenePool: {_packedScene.ResourcePath}] {message}");
    }
  }
}
