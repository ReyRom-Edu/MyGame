using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MyGame
{
    public class SpaceGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D _heroTexture;
        private Texture2D _spaceFarTexture;
        private Texture2D _spaceMidTexture;
        private Texture2D _spaceNearTexture;

        private Hero _hero;
        private Camera2D _camera;

        ParallaxLayer _layerFar;
        ParallaxLayer _layerMid;
        ParallaxLayer _layerNear;

        int screenW, screenH;

        public SpaceGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            screenW = GraphicsDevice.Viewport.Width;
            screenH = GraphicsDevice.Viewport.Height;

            _heroTexture = Content.Load<Texture2D>("hero");
            _spaceFarTexture = Content.Load<Texture2D>("space_far");
            _spaceMidTexture = Content.Load<Texture2D>("space_mid");
            _spaceNearTexture = Content.Load<Texture2D>("space_near");


            _hero = new Hero(_heroTexture, new Vector2(100,100));
            _camera = new Camera2D();

            _layerFar = new ParallaxLayer(_spaceFarTexture, 0.1f);
            _layerMid = new ParallaxLayer(_spaceMidTexture, 0.3f);
            _layerNear = new ParallaxLayer(_spaceNearTexture, 0.6f);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _hero.Update(gameTime, _camera);

            Vector2 targetCameraPos = _hero.Position - new Vector2(screenW/2, screenH/2);

            _camera.Update(
                Vector2.Lerp(_camera.Position, targetCameraPos, 0.1f)  
            );

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _layerFar.Draw(_spriteBatch, _camera, screenW, screenH);
            _layerMid.Draw(_spriteBatch, _camera, screenW, screenH);
            _layerNear.Draw(_spriteBatch, _camera, screenW, screenH);

            _spriteBatch.End();


            _spriteBatch.Begin();

            _hero.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
