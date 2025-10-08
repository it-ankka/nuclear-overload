using Godot;

public partial class PlayerShootComponent : Node
{
    [Export] RayCast3D ShootRay;
    [Export] Gun Gun;
    [Export] PackedScene HitEffectScene;
    [Export] float SplashRadius = 5.0f;
    [Export] float ImpactMaxPushback = 20f;
    [Export] Curve ImpactFalloff;
    PlayerFpsController player;
    float secondaryFireCharge = 0.0f;

    public override void _Ready()
    {
        player = GetParent<PlayerFpsController>();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionPressed("secondary_fire") && Input.IsActionJustPressed("fire"))
        {

            if (!ShootRay.IsColliding())
            {
                Gun.Fire();
                return;
            }

            var point = ShootRay.GetCollisionPoint();
            var normal = ShootRay.GetCollisionNormal();
            var hitEffect = HitEffectScene.Instantiate<Effect>();
            Gun.Fire(point);
            hitEffect.Position = point;
            hitEffect.Rotation = normal;
            hitEffect.Finished += () => hitEffect.QueueFree();
            GetTree().Root.AddChild(hitEffect);
            var playerCenter = !player.StandingCollisionShape.Disabled ? player.StandingCollisionShape.GlobalPosition : player.CrouchingCollisionShape.GlobalPosition;
            var distanceFromPlayer = (point - playerCenter).Length();
            var impactPushback = ImpactFalloff.Sample(distanceFromPlayer / SplashRadius) * ImpactMaxPushback;
            if (distanceFromPlayer < SplashRadius)
            {
                player.Velocity = player.Velocity with { Y = Mathf.Max(player.Velocity.Y, -9.8f) } + point.DirectionTo(playerCenter) * (ImpactFalloff.Sample(distanceFromPlayer / SplashRadius) * ImpactMaxPushback);
                player.MoveAndSlide();
            }
        }
    }
}

