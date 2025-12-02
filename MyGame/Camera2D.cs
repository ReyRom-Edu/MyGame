using Microsoft.Xna.Framework;

namespace MyGame
{
    public class Camera2D
    {
        public Vector2 Position;
        public Vector2 LastPosition;

        public float Zoom = 1f;
        public float Rotation = 0f;

        public void Update(Vector2 newPosition)
        {
            LastPosition = Position;
            Position = newPosition;
        }

        public Vector2 Delta => Position - LastPosition;

        public Matrix GetMatrix()
        {
            return Matrix.CreateTranslation(new Vector3(-Position, 0f)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(Zoom, Zoom, 1f);
        }

        public Rectangle GetBounds(int screenW, int screenH)
        {
            Matrix inv = Matrix.Invert(GetMatrix());

            Vector2 topLeft = Vector2.Transform(Vector2.Zero, inv);
            Vector2 bottomRight = Vector2.Transform(new Vector2(screenW, screenH), inv);

            int left = (int)topLeft.X;
            int right = (int)bottomRight.X;
            int top = (int)topLeft.Y;
            int bottom = (int)bottomRight.Y;

            return new Rectangle(left, top, right - left, bottom - top);
        }
    }
}
