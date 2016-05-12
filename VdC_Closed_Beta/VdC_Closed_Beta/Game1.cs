using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;

namespace VdC_Closed_Beta
{
    
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static List<Sprite> scene;
        // lista de background e afins;

        Player player;

        public static float unitSize = 128;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            
            loadScene("teste1.json");
            base.Initialize();
        }

        void loadScene(string filename)
        {
            filename = @"Content\" + filename;
            string sceneContents = File.ReadAllText(filename);
            JObject json = JObject.Parse(sceneContents);
            JArray images = (JArray)json["composite"]["sImages"];
            scene = new List<Sprite>();
            foreach(JObject image in images)
            {
                string imageName = (string)image["imageName"];
                string layerName = (string)image["layerName"];
                float x, y;
                JToken t;
                if (image.TryGetValue("x", out t)) x = (float)t;
                else x = 0f;
                if (image.TryGetValue("x", out t)) y = (float)t;
                else y = 0f;

                if (imageName.Equals("Bush"))
                    player = new Player(Content, imageName, new Vector2(x, y));
                else scene.Add(new Sprite(Content, imageName, new Vector2(x, y)));
                //layers para listas diferentes aqui
            }
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            player.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Vector2 posPlayer = player.GetPosition() + new Vector2(player.bbox.Width, player.bbox.Height) / 2;
            spriteBatch.Begin();

            foreach (Sprite sprite in scene)
                sprite.Draw(spriteBatch);

            player.Draw(spriteBatch);
            base.Draw(gameTime);
        }
    }


}
