using Microsoft.Xna.Framework;

namespace MyNewMine{
    public class Physics{
        private Vector3 velocity = Vector3.Zero;
        private const float gravity = -9.81f;
        private float groundHeight = 0f;
        private const float terminalVelocity = 50f;
        public void ApplyGravity(ref Vector3 position,ref Vector3 velocity, ref bool isGrounded, float deltaTime){
            if (position.Y <= groundHeight){
                isGrounded = true;
                velocity.Y = 0f;
                position.Y = groundHeight;
            }   else {
                isGrounded = false;
                velocity.Y += gravity * deltaTime;
            }
            position.Y += velocity.Y * deltaTime;
        }
        public void ResetVerticalVelocity(){
            velocity.Y = 0f;
        }
    }   
}