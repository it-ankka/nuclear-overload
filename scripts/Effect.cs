using Godot;

public partial class Effect : Node3D
{
    [Signal]
    public delegate void FinishedEventHandler();
}
