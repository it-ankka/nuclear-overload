using Godot;

[GlobalClass]
public partial class KillAreaComponent : Area3D
{
    private bool reloadScene = false;
    public override void _Ready()
    {
        BodyEntered += HandleBodyEntered;
    }

    public override void _Process(double delta)
    {
        if (reloadScene)
            GetTree().ReloadCurrentScene();
    }

    public void HandleBodyEntered(Node3D body)
    {
        if (body is PlayerFpsController)
            reloadScene = true;
    }
}
