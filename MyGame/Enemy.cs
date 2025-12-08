

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

            Vector2 target = pos - this.Center;

            target = Vector2.Lerp(target, this.Center, dt);

            target.Normalize();

            Position += target * Speed * dt;
            UpdateHitbox();
        }

        private void UpdateHitbox()
        {
            Center = this.Position + new Vector2(texture.Width / 2, texture.Height / 2) * 0.1f;
            Radius = texture.Width / 2 * 0.1f;
        }

        public Vector2 Center;
        public float Radius;

        public bool LaserHitsEnemy(Laser laser)
        {
            Vector2 laserStartToCenter = Center - laser.Start;
            Vector2 dir = laser.End - laser.Start;

            float length = dir.LengthSquared();

            float t = Vector2.Dot(laserStartToCenter, dir) / length;
            t = MathHelper.Clamp(t, 0f, 1f);

            Vector2 closest = laser.Start + dir * t;

            float dist = Vector2.Distance(closest, Center);

            return dist <= Radius;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(
               texture,
               Position,
               null,
               Color.White,
               0,
               Vector2.Zero,
               0.1f,
               SpriteEffects.None,
               0);
        }

        public void DrawHitbox(SpriteBatch sb)
        {
            Vector2 hitboxPos = Center - new Vector2(Radius, Radius);
            Vector2 hitboxSize = new Vector2(Radius*2, Radius*2);

            sb.Draw(
                GetPixel(sb.GraphicsDevice),
                hitboxPos,
                null,
                Color.Aquamarine * 0.5f,
                0,
                Vector2.Zero,
                hitboxSize,
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
