using Microsoft.Xna.Framework;


namespace MyGame
{
    public class Laser
    {
        public Vector2 Start;
        public Vector2 End;

        public float Width = 4f;
        public float LifeTime = 0.05f;
        public bool IsActive = false;
    }
}
