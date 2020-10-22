using Godot;

namespace GhostlyPlatformer.Player
{
    public class PlayerController
    {
        private global::Player Body;
        
        public Vector2 velocity = new Vector2();
        
        public float Speed = 50;
        public float SprintSpeed = 75;
        public float CrouchSpeed = 25;
        public float JumpHeight = 100;
        public float Gravity = 5;

        private CollisionShape2D TopCollision;
        public RayCast2D topRay;

        public bool GravityEnabled = true;

        public PlayerController(global::Player Body)
        {
            this.Body = Body;
            TopCollision = Body.GetNode<CollisionShape2D>("Top");
            topRay = Body.GetNode<RayCast2D>("RayCast2D");
        }

        public Vector2 MovePlayer(float delta)
        {
            if (Input.IsActionPressed("PlayerLeft")) 
            {
                if (Input.IsActionPressed("PlayerSprint") && !(Input.IsActionPressed("PlayerCrouch") || topRay.IsColliding())) 
                {
                    velocity.x = -SprintSpeed;
                    TopCollision.Disabled = false;
                }
                else if ((Input.IsActionPressed("PlayerCrouch") || topRay.IsColliding()) && Body.IsOnFloor()) 
                {
                    velocity.x = -CrouchSpeed;
                    TopCollision.Disabled = true;
                }
                else 
                {
                    velocity.x = -Speed;
                    TopCollision.Disabled = false;
                }
            }
            else if (Input.IsActionPressed("PlayerRight"))
            {
                if (Input.IsActionPressed("PlayerSprint") && !(Input.IsActionPressed("PlayerCrouch") || topRay.IsColliding())) 
                {
                    velocity.x = SprintSpeed;
                    TopCollision.Disabled = false;
                }
                else if ((Input.IsActionPressed("PlayerCrouch") || topRay.IsColliding()) && Body.IsOnFloor()) 
                {
                    velocity.x = CrouchSpeed;
                    TopCollision.Disabled = true;
                }
                else 
                {
                    velocity.x = Speed;
                    TopCollision.Disabled = false;
                }
            }
            else 
            {
                velocity.x = 0;
            }

            if (Input.IsActionJustPressed("PlayerJump") && Body.IsOnFloor()) 
            {
                velocity.y -= JumpHeight;
            }

            if (topRay.IsColliding()) 
            {
                TopCollision.Disabled = true;
            } 
            else if (!Input.IsActionPressed("PlayerCrouch")) 
            {
                TopCollision.Disabled = false;
            }


            if(GravityEnabled)
                velocity.y += Gravity;
            
            //GD.Print(velocity);
            return velocity = Body.MoveAndSlide(velocity, Vector2.Up);
        }

        public void BounceOff()
        {
            velocity.y = -JumpHeight/1.25f;
        }
        
    }
}