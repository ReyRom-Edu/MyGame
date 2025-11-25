using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace MyGame
{
    public class Hero
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public float Speed = 300f;

        private Texture2D texture;

        public Hero(Texture2D texture, Vector2 startPos)
        {
            this.texture = texture;
            Position = startPos;
        }

        public void Update(GameTime time, Camera2D camera)
        {
            float dt = (float)time.ElapsedGameTime.TotalSeconds;

            Velocity = Vector2.Zero;

            KeyboardState kb = Keyboard.GetState();

            if (kb.IsKeyDown(Keys.W)) Velocity.Y -= 1;
            if (kb.IsKeyDown(Keys.S)) Velocity.Y += 1;
            if (kb.IsKeyDown(Keys.A)) Velocity.X -= 1;
            if (kb.IsKeyDown(Keys.D)) Velocity.X += 1;

            if (Velocity != Vector2.Zero)
                Velocity.Normalize();

            Position += Velocity * Speed * dt;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(
               texture,
               Position,
               null,
               Color.White,
               0,
               new Vector2(texture.Width/2, texture.Height/2),
               0.2f,
               SpriteEffects.None,
               0);
        }
    }
}
