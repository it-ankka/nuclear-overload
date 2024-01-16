using Godot;

[GlobalClass]
public partial class KillAreaComponent : Area3D
{
    public override void _Ready()
    {
        BodyEntered += HandleBodyEntered;
    }

    public void HandleBodyEntered(Node3D body)
    {
        if (body is PlayerFpsController)
            GetTree().ReloadCurrentScene();
    }
}
