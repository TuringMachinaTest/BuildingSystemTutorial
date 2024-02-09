using Godot;
using System;

public partial class Default : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;
	private AnimationTree _animationTree;

	public override void _Ready()
	{
		base._Ready();

		_animationTree = GetNode<AnimationTree>("AnimationTree");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;
		

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("left", "right", "forward", "backward");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}
		
		_animationTree.Set("parameters/conditions/idle", (IsOnFloor() && inputDir == Vector2.Zero));
		_animationTree.Set("parameters/conditions/walking", inputDir != Vector2.Zero);


		Velocity = velocity;
		MoveAndSlide();
	}
}
