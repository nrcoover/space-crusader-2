using Godot;

public static class CustomUtils {
  public static void ActivateArea2D(Area2D area, bool active)
  {
    area.SetDeferred(Area2D.PropertyName.Monitoring, active);
    area.SetDeferred(Area2D.PropertyName.Monitorable, active);
  }
}