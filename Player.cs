using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MyNewMine{
    public class Player{
        private Vector3 position;
        public Vector3 Position{
            get { return position; }
            set { position = value; }
        }
        public Quaternion Rotation { get; private set; }
        private float yaw, pitch;
        private Vector3 velocity = Vector3.Zero;
        private float Speed = 10f;
        private float acceleration = 25f;
        private float friction = 10f;
        private float jumpForce = 10f;
        private bool isGrounded = false;
        private Physics physics;
        private float mouseSensitivity = 0.002f;
        private int ScreenWidth, ScreenHeight;
        
        public Player(Vector3 initialPosition,
                    int screenWidth, int screenHeight){
            position = initialPosition;
            Rotation = Quaternion.Identity;
            this.ScreenWidth = screenWidth;
            this.ScreenHeight = screenHeight;
            yaw = 0;
            pitch = 0;
            physics = new Physics();
        }
        public Matrix GetViewMatrix(){
            Vector3 forward = Vector3.Transform(Vector3.Forward, Rotation);//hacia adelante
            Vector3 target = Position + forward;
            return Matrix.CreateLookAt(Position, target, Vector3.Up);
        }
        public void Update(GameTime gameTime){            
            //ToDo: Movimiento del Player.
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            CameraControl();
            Move(deltaTime);
            ApplyPhysics(deltaTime);
        }        
        private void CameraControl() {
            //lee el raton
            MouseState mouseState = Mouse.GetState();
            int deltaX = mouseState.X - (ScreenWidth/2);
            int deltaY = mouseState.Y - (ScreenHeight/2);
            
            //calcula los angulos de rotacion
            yaw -= deltaX * mouseSensitivity; //rotacion en X
            pitch -= deltaY * mouseSensitivity; //rotacion en Y

            // Limitar el pitch para evitar que la cámara gire bruscamente
            pitch = MathHelper.Clamp(
                pitch, 
                -MathHelper.PiOver2 + 0.1f,
                MathHelper.PiOver2 - 0.1f);

            Rotation = Quaternion.CreateFromYawPitchRoll(yaw,pitch,0f);
            
            //vuelve a centrar el raton
            Mouse.SetPosition(ScreenWidth / 2, ScreenHeight / 2);
        }
        private void Move(float deltaTime){
            KeyboardState keyState = Keyboard.GetState();
            Vector3 direction = Vector3.Zero;

            if (keyState.IsKeyDown(Keys.W)){
                direction += Vector3.Transform(
                    Vector3.Forward, Rotation);
            }
            if (keyState.IsKeyDown(Keys.S)){
                direction += Vector3.Transform(
                    Vector3.Backward, Rotation);
            }
            if (keyState.IsKeyDown(Keys.A)){
                direction += Vector3.Transform(
                    Vector3.Left, Rotation);
            }
            if (keyState.IsKeyDown(Keys.D)){
                direction += Vector3.Transform(
                    Vector3.Right, Rotation);
            }
            if (direction != Vector3.Zero){
                direction.Normalize();
                velocity += direction * acceleration * deltaTime;
                if (velocity.Length() > Speed){
                    velocity = Vector3.Normalize(velocity) * Speed;
                }
            } else {
                velocity *= (1 - friction * deltaTime);
                if(velocity.LengthSquared() < 0.01f){
                    velocity = Vector3.Zero;
                }
            }
            if (keyState.IsKeyDown(Keys.Space) && isGrounded){
                velocity.Y = jumpForce;
                isGrounded = false;
            }
        }
        private void ApplyPhysics(float deltaTime){
            physics.ApplyGravity(ref position,ref velocity, ref isGrounded, deltaTime);
            
        }
    }
}