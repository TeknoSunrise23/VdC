﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace VingançaDoChocolate_OPENBETA
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Player Jack;
        public Player Bryan;

        public static Vector2 jackpos;
        public static Vector2 bryanpos;

        //Bullet testerino;
        List<Bullet> activeBullets = new List<Bullet>();
        
        float scale = 0.7119f;
        

        // int level;
        public static float unitSize = 128;

        bool t1 = true;
        bool t2 = true;
        float btimer1 = 0f;
        float btimer2 = 0f;


        public static List<Sprite> scene = new List<Sprite>();
        public static Texture2D[,] character = new Texture2D[2, 4];
        Texture2D[] bullets = new Texture2D[2];

        
        

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
                {
                    Jack = new Player(Content, "Blue0R", new Vector2(x, y), 0);
                    
                   /* playersprites[0, 0] = new Sprite(Content, "Blue0R", Jack.GetPosition());
                    playersprites[0, 1] = new Sprite(Content, "Blue1R", Jack.GetPosition());
                    playersprites[0, 2] = new Sprite(Content, "Blue0L", Jack.GetPosition());
                    playersprites[0, 3] = new Sprite(Content, "Blue1L", Jack.GetPosition());*/
                    character[0, 0] = (Content.Load<Texture2D>("Blue0R"));  
                    character[0, 1] = (Content.Load<Texture2D>("Blue1R"));
                    character[0, 2] = (Content.Load<Texture2D>("Blue0L"));  // desenhar
                    character[0, 3] = (Content.Load<Texture2D>("Blue1L"));
                 
                    Sprite.cacheSprite("Blue1R", character[0, 1]);
                    Sprite.cacheSprite("Blue0L", character[0, 2]); // colisoes
                    Sprite.cacheSprite("Blue1L", character[0, 3]);
                    
                }
                else if (imageName.Equals("Red0L"))
                {
                    Bryan = new Player(Content, "Red0L", new Vector2(x, y), 1);
                   
                   /* playersprites[1, 0] = new Sprite(Content, "Red0R", Bryan.GetPosition());
                    playersprites[1, 1] = new Sprite(Content, "Red1R", Bryan.GetPosition());
                    playersprites[1, 2] = new Sprite(Content, "Red0L", Bryan.GetPosition());
                    playersprites[1, 3] = new Sprite(Content, "Red1L", Bryan.GetPosition());*/
                    character[1, 0] = (Content.Load<Texture2D>("Red0R"));
                    character[1, 1] = (Content.Load<Texture2D>("Red1R"));// desenhar
                    character[1, 2] = (Content.Load<Texture2D>("Red0L"));
                    character[1, 3] = (Content.Load<Texture2D>("Red1L"));

                    bullets[0] = (Content.Load<Texture2D>("BulletR"));
                    bullets[1] = (Content.Load<Texture2D>("BulletL"));

                    Sprite.cacheSprite("Red0R", character[1, 0]);
                    Sprite.cacheSprite("Red1R", character[1, 1]);// colisoes
                    Sprite.cacheSprite("Red1L", character[1, 3]);

                    Sprite.cacheSprite("BulletR", bullets[0]);
                    Sprite.cacheSprite("BulletL", bullets[1]);
                }
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
            KeyboardState keys = Keyboard.GetState();
            
            Bryan.name = Bryan.spritename();
            Jack.name = Jack.spritename();
            Bryan.Update(gameTime, spriteBatch);
            Jack.Update(gameTime, spriteBatch);
            bryanpos = Bryan.GetPosition();
            jackpos = Jack.GetPosition();
            if(t1 == false)
                btimer1 += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (t2 == false)
                btimer2 += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            // Console.WriteLine(jackpos);
            if (btimer1 > 500)
            {
                btimer1 = 0;
                t1 = true;
            }

            if (btimer2 > 500)
            {
                btimer2 = 0;
                t2 = true;
            }

            if (keys.IsKeyDown(Keys.D1) && (Jack.getside() == 0 || Jack.getside() == 1))
            {
                
                if (t1 == true)
                {
                    activeBullets.Add(new Bullet(Content, "BulletR", new Vector2(jackpos.X, jackpos.Y), 0, 0));
                    t1 = false;
                }
                
            }

            if (keys.IsKeyDown(Keys.D1) && (Jack.getside() == 2 || Jack.getside() == 3))
            {
                if (t1 == true)
                {
                    activeBullets.Add(new Bullet(Content, "BulletL", new Vector2(jackpos.X, jackpos.Y), 0, 2));
                    t1 = false;
                }
                
            }

            if (keys.IsKeyDown(Keys.L) && (Bryan.getside() == 0 || Bryan.getside() == 1))
            {
                if (t2 == true)
                {
                    activeBullets.Add(new Bullet(Content, "BulletR", new Vector2(bryanpos.X, bryanpos.Y), 0, 0));
                    t2 = false;
                }
            }

            if (keys.IsKeyDown(Keys.L) && (Bryan.getside() == 2 || Bryan.getside() == 3))
            {
                if (t2 == true)
                {
                    activeBullets.Add(new Bullet(Content, "BulletL", new Vector2(bryanpos.X, bryanpos.Y), 0, 2));
                    t2 = false;
                }
            }
            if(activeBullets != null)
            foreach (Bullet b in activeBullets)
                b.Update(gameTime);
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

            
            Bryan.DrawMoves(spriteBatch);
            Jack.DrawMoves(spriteBatch);

            if (activeBullets != null)
                foreach(Bullet b in activeBullets)
                    b.DrawBullet(spriteBatch);   
            spriteBatch.End();

            base.Draw(gameTime);
        }

        
        
    }
}
