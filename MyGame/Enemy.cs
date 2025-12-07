

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
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

            UpdateHitbox();
        }

        private void UpdateHitbox()
        {
            // Вычисляем центр спрайта
            center = Position + new Vector2(texture.Width / 2, texture.Height / 2) * 0.1f;

            // Используем половину меньшей стороны для радиуса (или половину ширины для круглого врага)
            radius = Math.Min(texture.Width, texture.Height) / 2f * 0.1f;
        }

        Vector2 center;
        float radius;

        public bool LaserHitsEnemy(Laser laser)
        {
            // Алгоритм пересечения отрезка (лазера) и круга (хитбокса врага)
            Vector2 laserStartToCenter = center - laser.Start;
            Vector2 laserDirection = laser.End - laser.Start;

            // Длина лазера
            float laserLengthSquared = laserDirection.LengthSquared();

            // Если лазер нулевой длины, проверяем расстояние от начала до центра
            if (laserLengthSquared == 0)
            {
                return Vector2.Distance(laser.Start, center) <= radius;
            }

            // Проекция вектора от начала лазера к центру на направление лазера
            float t = Vector2.Dot(laserStartToCenter, laserDirection) / laserLengthSquared;

            // Ограничиваем t в пределах отрезка лазера
            t = MathHelper.Clamp(t, 0f, 1f);

            // Находим ближайшую точку на отрезке лазера к центру врага
            Vector2 closestPoint = laser.Start + laserDirection * t;

            // Расстояние от ближайшей точки до центра врага
            float distance = Vector2.Distance(closestPoint, center);

            // Проверяем, пересекается ли с радиусом
            return distance <= radius;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Рисуем врага
            spriteBatch.Draw(
                texture,
                Position,
                null,
                Color.White,
                0f,
                Vector2.Zero, // Используем левый верхний угол как точку отсчета
                0.1f, // Масштаб 1:1
                SpriteEffects.None,
                0f);
        }

        public void DrawHitbox(SpriteBatch spriteBatch)
        {
            Texture2D pixel = GetPixel(spriteBatch.GraphicsDevice);

            // Рисуем круг хитбокса (квадрат для простоты отладки)
            // Для точного круга нужно было бы использовать примитивы или специальную текстуру
            Vector2 hitboxPosition = center - new Vector2(radius, radius);
            Vector2 hitboxSize = new Vector2(radius * 2, radius * 2);

            // Рисуем прозрачный красный квадрат для хитбокса
            spriteBatch.Draw(
                pixel,
                hitboxPosition,
                null,
                Color.Red * 0.5f, // Полупрозрачный красный
                0f,
                Vector2.Zero,
                hitboxSize,
                SpriteEffects.None,
                0f);
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
