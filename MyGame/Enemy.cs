

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace MyGame
{
    public class Enemy
    {
        public Vector2 Position;
        public float Speed = 50f;

        private Texture2D texture;

        public Enemy(Texture2D texture, Vector2 startPos)
        {
            this.texture = texture;
            this.Position = startPos;
        }

        public void Update(GameTime t, Vector2 pos)
        {
            float dt = (float)t.ElapsedGameTime.TotalSeconds;

            Vector2 target = pos - this.Position;

            target = Vector2.Lerp(target, this.Position, dt);

            target.Normalize();

            Position += target * Speed * dt;

            center = this.Position + new Vector2(texture.Width / 2, texture.Height / 2);
            radius = texture.Width / 2;
        }

        Vector2 center;
        float radius;

        public bool LaserHitsEnemy(Laser laser)
        {
            center = this.Position + new Vector2(texture.Width/2, texture.Height/2);
            radius = texture.Width/2;

            Vector2 dir = laser.End - laser.Start;
            float t = Vector2.Dot(center - laser.End, dir) / Vector2.Dot(dir,dir);
            t = MathHelper.Clamp(t, 0f, 1f);

            Vector2 closest = laser.Start + dir * t;

            float dist = Vector2.Distance(closest, center);

            return dist <= radius;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(
               texture,
               Position,
               null,
               Color.White,
               0,
               new Vector2(texture.Width / 2, texture.Height / 2),
               0.1f,
               SpriteEffects.None,
               0);

            sb.Draw(
                GetPixel(sb.GraphicsDevice),
                Position,
                null,
                Color.Aquamarine,
                0,
                Vector2.Zero,
                new Vector2(radius, radius),
                SpriteEffects.None,
                0);
        }

        static Texture2D _pixel;
        private Texture2D GetPixel(GraphicsDevice device)
        {
            if (_pixel == null)
            {
                _pixel = new Texture2D(device, 1, 1);
                _pixel.SetData(new[] { Color.White });
            }
            return _pixel;
        }
    }
}
