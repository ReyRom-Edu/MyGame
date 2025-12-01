using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame
{
    public class ParallaxLayer
    {
        public Texture2D Texture;
        public float Factor;

        public ParallaxLayer(Texture2D texture, float factor)
        {
            Texture = texture;
            Factor = factor;
        }

        public void Draw(SpriteBatch sb, Camera2D camera, int screenW, int screenH)
        {
            Vector2 camPos = camera.Position * Factor;

            int w = Texture.Width;
            int h = Texture.Height;

            float xOffset = camPos.X % w;
            float yOffset = camPos.Y % h;

            if (xOffset < 0) xOffset += w;
            if (yOffset < 0) yOffset += h;

            int tilesX = screenW / w + 1;
            int tilesY = screenH / h + 1;

            for (int x = -1; x <= tilesX; x++)
            {
                for(int y = -1; y <= tilesY; y++)
                {
                    Vector2 pos = new Vector2(
                        -xOffset + x * w, 
                        -yOffset + y * h
                    );

                    sb.Draw(Texture, pos, Color.White);
                }
            }
        }
    }
}
