using Godot;

public partial class enemy : Node3D
{
    [Export] public AnimationTree animationTree { get; private set; }
    public AnimationNodeStateMachinePlayback stateMachine { get; private set; }
    public PlayerFpsController player { get; private set; }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Ready()
    {
        stateMachine = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");
        player = GetTree().CurrentScene.GetNodeOrNull<PlayerFpsController>("%Player");
    }
    public override void _Process(double delta)
    {
        player = player == null ? GetTree().Root.GetNodeOrNull<PlayerFpsController>("%Player") : player;
        LookAt(player.GlobalPosition with { Y = GlobalPosition.Y }, useModelFront: true);
    }

    public void Shoot()
    {
        stateMachine.Travel("Shoot");
    }
}
