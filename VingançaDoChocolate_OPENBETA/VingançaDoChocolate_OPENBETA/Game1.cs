using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;

namespace VingançaDoChocolate_OPENBETA
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Player Jack;
        Player Bryan;
        float scale = 0.713f;

       // int level;
        public static float unitSize = 128;

        public static List<Sprite> scene = new List<Sprite>();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //level = 1;
            loadScene("level1.json");
            base.Initialize();
        }
        void loadScene(string filename)
        {
            filename = @"Content\" + filename;
            string sceneContents = File.ReadAllText(filename);
            JObject json = JObject.Parse(sceneContents);
            JArray images = (JArray)json["composite"]["sImages"];
            scene = new List<Sprite>();
            foreach (JObject image in images)
            {
                string imageName = (string)image["imageName"];
                string layerName = (string)image["layerName"];
                float x, y;
                JToken t;
                if (image.TryGetValue("x", out t)) x = (float)t;
                else x = 0f;
                if (image.TryGetValue("y", out t)) y = (float)t;
                else y = 0f;

                if (imageName.Equals("Blue0R"))
                    Jack = new Player(Content, imageName, new Vector2(x, y));
                else if (imageName.Equals("Red0L"))
                    Bryan = new Player(Content, imageName, new Vector2(x, y));
                else scene.Add(new Sprite(Content, imageName, new Vector2(x, y)));
                //layers para listas diferentes aqui
            }
        }
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            // TODO: Add your drawing code here
            spriteBatch.Begin(transformMatrix:
               Matrix.CreateTranslation(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height/2 -64 ,0)  *
                Matrix.CreateScale(scale) *
                Matrix.CreateTranslation(scale* -GraphicsDevice.Viewport.Width / 2,scale*  GraphicsDevice.Viewport.Height, 0)
                );
            foreach (Sprite sprite in scene)
                sprite.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
