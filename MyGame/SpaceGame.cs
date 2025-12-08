using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

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
        private Texture2D _enemyTexture;
        private SpriteFont _font;

        private Hero _hero;
        private Camera2D _camera;
        private List<Enemy> _enemies = new List<Enemy>();

        ParallaxLayer _layerFar;
        ParallaxLayer _layerMid;
        ParallaxLayer _layerNear;

        int screenW, screenH;

        int score = 0;

        bool isGameOver = false;

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
            _enemyTexture = Content.Load<Texture2D>("enemy");
            _font = Content.Load<SpriteFont>("DefaultFont");


            _hero = new Hero(_heroTexture, new Vector2(screenW / 2, screenH / 2));
            _camera = new Camera2D();

            _layerFar = new ParallaxLayer(_spaceFarTexture, 0.1f);
            _layerMid = new ParallaxLayer(_spaceMidTexture, 0.3f);
            _layerNear = new ParallaxLayer(_spaceNearTexture, 0.6f);

            for (int i = 0; i < 10; i++)
            {
                Enemy enemy = new Enemy(_enemyTexture, GetRandomPos());

                _enemies.Add(enemy);
            }
        }

        private Vector2 GetRandomPos()
        {
            Random rnd = new Random();

            int side = rnd.Next(4);
            Vector2 spawnPos = Vector2.Zero;
            Rectangle bounds = _camera.GetBounds(screenW, screenH);

            float margin = 200f;

            switch(side)
            {
                case 0: // top
                    spawnPos = new Vector2(bounds.Left + rnd.Next(bounds.Width), bounds.Top - margin);
                    break;
                case 1: //right 
                    spawnPos = new Vector2(bounds.Right + margin, bounds.Top + rnd.Next(bounds.Height));
                    break;
                case 2: // bottom
                    spawnPos = new Vector2(bounds.Left + rnd.Next(bounds.Width), bounds.Bottom + margin);
                    break;
                case 3: // left
                    spawnPos = new Vector2(bounds.Left - margin, bounds.Top + rnd.Next(bounds.Height));
                    break;
            }

            return spawnPos;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (isGameOver)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    isGameOver = false;
                    score = 0;
                    foreach (var e in _enemies)
                    {
                        e.Position = GetRandomPos();
                    }
                }

                return;
            }

            _hero.Update(gameTime, _camera);

            foreach (var e in _enemies)
            {
                e.Update(gameTime, _hero.Position);

                if (_hero.Laser.IsActive && e.LaserHitsEnemy(_hero.Laser))
                {
                    score++;
                    e.Position = GetRandomPos();
                }

                float dist = Vector2.Distance(_hero.Center, e.Center);
                if (dist < _hero.Radius) 
                { 
                    isGameOver = true;
                }
            }

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


            _spriteBatch.Begin(transformMatrix: _camera.GetMatrix());

            foreach(var e in _enemies)
            {
                e.Draw(_spriteBatch);
                e.DrawHitbox(_spriteBatch);
            }

            _hero.Draw(_spriteBatch);
            _hero.DrawHitbox(_spriteBatch);
            _spriteBatch.DrawString(_font,$"Счет: {score}", _camera.Position + new Vector2(30,30), Color.White);

            if (isGameOver)
            {
                _spriteBatch.DrawString(_font, $"ПОТРАЧЕНО", _camera.Position + new Vector2(screenW/2, screenH/2), Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
