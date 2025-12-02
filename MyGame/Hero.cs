using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;


namespace MyGame
{
    public class Hero
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public float MaxSpeed = 10000f;
        public float Acceleration = 50000f;
        private float damping = 0.99f;
        public float RotationSpeed = 5f;
        public float Rotation;

        private Texture2D texture;

        private Vector2 fireDirection;
        private float laserTimer;
        private float laserLength = 3000f;
        public Laser Laser = new Laser();

        private MouseState previousMouseState;

        public Hero(Texture2D texture, Vector2 startPos)
        {
            this.texture = texture;
            Position = startPos;
        }

        public void Update(GameTime time, Camera2D camera)
        {
            float dt = (float)time.ElapsedGameTime.TotalSeconds;


            Vector2 center = Position + new Vector2(texture.Width / 2, texture.Height / 2);

            MouseState mouse = Mouse.GetState();
            Vector2 mouseScreen = new Vector2(mouse.X, mouse.Y);

            Matrix inv = Matrix.Invert(camera.GetMatrix());
            Vector2 mouseWorld = Vector2.Transform(mouseScreen, inv);

            Vector2 direction = mouseWorld - Position;

            float targetRotation = (float)Math.Atan2(direction.Y, direction.X);

            float delta = MathHelper.WrapAngle(targetRotation - Rotation);

            Rotation += delta * RotationSpeed * dt;

            Rotation = MathHelper.WrapAngle(Rotation);


            if (mouse.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton != ButtonState.Pressed && laserTimer <= 0)
            {
                laserTimer = Laser.LifeTime;

                fireDirection = new Vector2(MathF.Cos(Rotation), MathF.Sin(Rotation));

                Laser.Start = Position;
                Laser.End = Position + fireDirection * laserLength;

                Laser.IsActive = true;
            }


            if (laserTimer > 0)
            {
                laserTimer -= dt;
            }
            else
            {
                Laser.IsActive = false;
            }



            Velocity = Vector2.Zero;

            KeyboardState kb = Keyboard.GetState();

            if(kb.IsKeyDown(Keys.W))
            {
                Vector2 dir = new Vector2(MathF.Cos(Rotation), MathF.Sin(Rotation));

                Velocity += dir * Acceleration * dt;
            }


            if (Velocity.Length() > MaxSpeed)
                Velocity = Vector2.Normalize(Velocity) * MaxSpeed;

            Velocity *= damping;

            Position += Velocity * dt;

            previousMouseState = mouse;
        }

        public void Draw(SpriteBatch sb)
        {
            if (Laser.IsActive)
            {
                DrawLine(sb, Laser.Start, Laser.End, Color.Red, Laser.Width);
            }

            sb.Draw(
               texture,
               Position,
               null,
               Color.White,
               Rotation,
               new Vector2(texture.Width/2, texture.Height/2),
               0.2f,
               SpriteEffects.None,
               0);
        }

        private void DrawLine(SpriteBatch sb, Vector2 start, Vector2 end, Color color, float width)
        {
            Vector2 dir = end - start;
            float length = dir.Length();
            float rot = (float)Math.Atan2(dir.Y, dir.X);

            sb.Draw(
                GetPixel(sb.GraphicsDevice),
                start,
                null,
                color,
                rot,
                Vector2.Zero,
                new Vector2(length, width),
                SpriteEffects.None,
                0);
        }

        static Texture2D _pixel;
        private Texture2D GetPixel(GraphicsDevice device)
        {
            if( _pixel == null )
            {
                _pixel = new Texture2D(device, 1, 1);
                _pixel.SetData(new[] { Color.White });
            }
            return _pixel;
        }
    }
}
