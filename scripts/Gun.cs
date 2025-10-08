using Godot;

public partial class Gun : Node3D
{
    [Export] float RecoilScale = 0.1f;
    [Export] Vector3 RecoilAmplitude = Vector3.One;
    [Export] float TweenTime = 0.05f;
    [Export] float ResetTime = 0.1f;
    [Export] float LaserTravelTime = 0.05f;
    [Export] Node3D Muzzle;
    [Export] Node3D LaserNode;
    [Export] PackedScene MuzzleFlashEffectScene;

    Tween recoilTween, laserTween;

    public void Fire(Vector3 hitPos = new Vector3())
    {
        LaserNode.Visible = true;
        var muzzleFlash = MuzzleFlashEffectScene.Instantiate<CpuParticles3D>();
        muzzleFlash.Ready += () =>
        {
            muzzleFlash.Emitting = true;
            if (laserTween != null) laserTween.Kill();
            laserTween = CreateTween();
            LaserNode.Visible = true;
            laserTween.TweenProperty(LaserNode, "global_position", hitPos.IsZeroApprox() ? ToGlobal(Vector3.Forward * 10) : hitPos, 0.1);
            laserTween.Finished += () =>
            {
                LaserNode.Visible = false;
                LaserNode.Position = Vector3.Zero;
            };
        };
        muzzleFlash.Finished += () => muzzleFlash.QueueFree();
        Muzzle.AddChild(muzzleFlash);
        ApplyRecoil();
    }

    private void ApplyRecoil()
    {
        RecoilAmplitude.Y *= GD.Randf() > 0.5 ? -1 : 1;
        Vector3 recoil = new Vector3(GD.Randf() * RecoilAmplitude.X, GD.Randf() * RecoilAmplitude.Y, RecoilAmplitude.Z) * RecoilScale;
        if (recoilTween != null)
            recoilTween.Kill();

        recoilTween = CreateTween();
        recoilTween.TweenProperty(this, "position:z", recoil.Z, TweenTime);
        recoilTween.Parallel().TweenProperty(this, "rotation", new Vector3(recoil.X, Rotation.Y, recoil.Y), TweenTime);
        recoilTween.TweenProperty(this, "position", Vector3.Zero, ResetTime);
        recoilTween.Parallel().TweenProperty(this, "rotation", Vector3.Zero, ResetTime);

    }
}
