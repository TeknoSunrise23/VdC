using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace VingançaDoChocolate_OPENBETA
{
    public class Game1 : Game
    {
        enum GameState
        {
            MainMenu,
            GamePlay,
            EndScreen,
        }

        GameState _state = (GameState)0;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SoundEffect win;
        public Player Jack;
        public Player Bryan;
        Random dice = new Random();
        SpriteFont Arial;
        int winner;
        int level;
        public static Vector2 jackpos;
        public static Vector2 bryanpos;

        //Bullet testerino;
        List<Bullet> activeBullets = new List<Bullet>();
        
        float scale = 0.7119f;

        public static SoundEffect hit;
        public static SoundEffect manHit;
        public static SoundEffect pew;
        

        // int level;
        public static float unitSize = 128;

        bool t1 = true;
        bool t2 = true;
        bool c1 = true;
        bool newgame = true;
        bool mouseover1 = false;
        bool mouseover2 = false;
        bool mouseover3 = false;
        bool playwin = false;
        float btimer1 = 0f;
        float btimer2 = 0f;
        float ctimer1 = 0f;
        float timetoplaywin = 0f;

        public static List<Sprite> scene = new List<Sprite>();
        public static List<Sprite> bulletcolisions = new List<Sprite>();
       
        public static Texture2D[,] character = new Texture2D[2, 4];
        Texture2D[] bullets = new Texture2D[2];
        List<Sprite> Menus = new List<Sprite>();
        List<Sprite> Endscene = new List<Sprite>();

        Vector2 jackposI;
        Vector2 bryanposI;
        
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            level = dice.Next(1, 4);
            loadScene("level"+ level +".json");
            if (_state != GameState.GamePlay)
            {
                this.IsMouseVisible = true;
            }
            
          
            base.Initialize();
        }
        void loadScene(string filename)
        {
            Console.WriteLine(filename);
            Menus.Add(new Sprite(Content, "MainMenu0", new Vector2(0, 0)));
            Menus.Add(new Sprite(Content, "MainMenu1", new Vector2(0, 0)));
            Menus.Add(new Sprite(Content, "MainMenu2", new Vector2(0, 0)));
            Endscene.Add(new Sprite(Content, "BlueWin0", new Vector2(0, 0)));
            Endscene.Add(new Sprite(Content, "BlueWin1", new Vector2(0, 0)));
            Endscene.Add(new Sprite(Content, "RedWin0", new Vector2(0, 0)));
            Endscene.Add(new Sprite(Content, "RedWin1", new Vector2(0, 0)));

            hit = Content.Load<SoundEffect>("hit");
            manHit = Content.Load<SoundEffect>("manHit");
            pew = Content.Load<SoundEffect>("shot");
            win = Content.Load<SoundEffect>("Win");

            Arial = Content.Load<SpriteFont>("ArialBlack20");
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
                    jackposI = new Vector2(x, y);
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
                    bryanposI = new Vector2(x, y);
                    /* playersprites[1, 0] = new Sprite(Content, "Red0R", Bryan.GetPosition());
                     playersprites[1, 1] = new Sprite(Content, "Red1R", Bryan.GetPosition());
                     playersprites[1, 2] = new Sprite(Content, "Red0L", Bryan.GetPosition());
                     playersprites[1, 3] = new Sprite(Content, "Red1L", Bryan.GetPosition());*/
                    character[1, 0] = (Content.Load<Texture2D>("Red0R"));
                    character[1, 1] = (Content.Load<Texture2D>("Red1R"));// desenhar
                    character[1, 2] = (Content.Load<Texture2D>("Red0L"));
                    character[1, 3] = (Content.Load<Texture2D>("Red1L"));

                    Sprite.cacheSprite("Red0R", character[1, 0]);
                    Sprite.cacheSprite("Red1R", character[1, 1]);// colisoes
                    Sprite.cacheSprite("Red1L", character[1, 3]);

                    bullets[0] = (Content.Load<Texture2D>("BulletR"));
                    bullets[1] = (Content.Load<Texture2D>("BulletL"));

                    Sprite.cacheSprite("BulletR", bullets[0]);
                    Sprite.cacheSprite("BulletL", bullets[1]);
                }
                else
                {
                    scene.Add(new Sprite(Content, imageName, new Vector2(x, y)));
                    
                }
                //layers para listas diferentes aqui
            }
            bulletcolisions.Add(Bryan);
            bulletcolisions.Add(Jack);
            
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
            
            base.Update(gameTime);
            if(_state != (GameState)1)
            {
                this.IsMouseVisible = true;
            }
            if(c1 == false)
                ctimer1 += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (ctimer1 > 500)
            {
                c1 = true;
                ctimer1 = 0f;
            }
            if(newgame)
            {
                activeBullets = new List<Bullet>();
                Jack = new Player(Content, "Blue0R", jackposI, 0);
                Bryan = new Player(Content, "Red0L", bryanposI, 1);
                bulletcolisions = new List<Sprite>();
                bulletcolisions.Add(Bryan);
                bulletcolisions.Add(Jack);
                level = dice.Next(1, 4);
                loadScene("level" + level + ".json");
            }
            switch(_state)
            {
                case GameState.MainMenu:
                    UpdateMainMenu(gameTime);
                    break;
                case GameState.GamePlay:
                    UpdateGamePlay(gameTime);
                    break;
                case GameState.EndScreen:
                    UpdateEndScreen(gameTime);
                    break;
            }
            
                
                
            
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();

            spriteBatch.Begin(transformMatrix:
                Matrix.CreateTranslation(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 - 64, 0) *
                Matrix.CreateScale(scale) *
                Matrix.CreateTranslation(scale * -GraphicsDevice.Viewport.Width / 2, scale * GraphicsDevice.Viewport.Height, 0)
                );

            base.Draw(gameTime);
            switch (_state)
            {
                case GameState.MainMenu:
                    DrawMainMenu(spriteBatch);
                    break;
                case GameState.GamePlay:
                    DrawGamePlay(spriteBatch);
                    break;
                case GameState.EndScreen:
                    DrawEndScreen(spriteBatch);
                    break;
            }
            
            spriteBatch.End();
            

        }


        //Update Do Menu
        protected void UpdateMainMenu(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();

            Rectangle click1 = new Rectangle(175, 361-GraphicsDevice.DisplayMode.Height/8, 1020, 230 - GraphicsDevice.DisplayMode.Height / 8);
            Rectangle click2 = new Rectangle(185, 575 - GraphicsDevice.DisplayMode.Height / 8, 1020, 230 - GraphicsDevice.DisplayMode.Height / 8);
            if (new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(click1))
            {
                mouseover1 = true;
            }
            else if (new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(click2))
            {
                mouseover2 = true;
            }
            else
            {
                mouseover1 = false;
                mouseover2 = false;

            }

            if ((c1 == true)&&(mouse.LeftButton == ButtonState.Pressed) && (new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(click2)))
            {
                Exit();
                
            }

            if ((mouse.LeftButton == ButtonState.Pressed) && (new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(click1)))
            {
                newgame = true;
                
                Bryan.healthpoints = 100f;
                Jack.healthpoints = 100f;
                _state = (GameState)1;
                this.IsMouseVisible = false;
            }

        }

        //Update Do Jogo
        protected void UpdateGamePlay(GameTime gameTime)
        {
            KeyboardState keys = Keyboard.GetState();
            newgame = false;
            Bryan.name = Bryan.spritename();
            Jack.name = Jack.spritename();
            Bryan.Update(gameTime, spriteBatch);
            Jack.Update(gameTime, spriteBatch);
            bryanpos = Bryan.GetPosition();
            jackpos = Jack.GetPosition();
            // reset do som de vitoria (timer)
            timetoplaywin = 0f;

            if (t1 == false)
                btimer1 += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (t2 == false)
                btimer2 += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            
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
                    activeBullets.Add(new Bullet(Content, "BulletR", new Vector2(jackpos.X + 161, jackpos.Y + 49), 0, 0));
                    t1 = false;
                    pew.Play();
                }

            }

            if (keys.IsKeyDown(Keys.D1) && (Jack.getside() == 2 || Jack.getside() == 3))
            {
                if (t1 == true)
                {
                    activeBullets.Add(new Bullet(Content, "BulletL", new Vector2(jackpos.X - 10, jackpos.Y + 49), 0, 2));
                    t1 = false;
                    pew.Play();
                }

            }

            if (keys.IsKeyDown(Keys.L) && (Bryan.getside() == 0 || Bryan.getside() == 1))
            {
                if (t2 == true)
                {
                    activeBullets.Add(new Bullet(Content, "BulletR", new Vector2(bryanpos.X + 161, bryanpos.Y+49), 0, 0));
                    t2 = false;
                    pew.Play();
                }
            }

            if (keys.IsKeyDown(Keys.L) && (Bryan.getside() == 2 || Bryan.getside() == 3))
            {
                if (t2 == true)
                {
                    activeBullets.Add(new Bullet(Content, "BulletL", new Vector2(bryanpos.X-10, bryanpos.Y+49), 0, 2));
                    t2 = false;
                    pew.Play();
                }
            }
            if (activeBullets != null)
                foreach (Bullet b in activeBullets.ToArray())
                {
                    b.Update(gameTime);
                    if (b.removaltimer >= 2 && b.velocityX == 0)
                        activeBullets.Remove(b);
                }
            //Jack.healthpoints = 0;
            if (Jack.healthpoints == 0)
            {
                winner = 1;
                _state = (GameState)2;
            }
            if (Bryan.healthpoints == 0)
            {
                winner = 0;
                _state = (GameState)2;
            }
        }

        //Update Do Endscreen
        protected void UpdateEndScreen(GameTime gameTime)
        {
            // CONDIÇAO PARA SO DAR PLAY 1 VEZ NO UPDATE
            
            timetoplaywin += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timetoplaywin < 16.7f)
                playwin = true;
            else playwin = false;
            if(playwin)
            win.Play();
            
            MouseState mouse = Mouse.GetState();
            Rectangle click3 = new Rectangle(580, 505, 665,90);
            if (new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(click3))
            {
                mouseover3 = true;
            }
            else
                mouseover3 = false;
            if ((mouse.LeftButton == ButtonState.Pressed) && (new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(click3)))
            {
                _state = (GameState)0;
                c1 = false;

            }

        }

        //Draw Menu
        protected void DrawMainMenu(SpriteBatch spriteBatch)
        {
            if (mouseover1 == true)
                Menus[1].Draw(spriteBatch);
            else if(mouseover2 == true)
                Menus[2].Draw(spriteBatch);
            else Menus[0].Draw(spriteBatch);
        }
        //Draw Gameplay
        protected void DrawGamePlay(SpriteBatch spriteBatch)
        {
           
            foreach (Sprite sprite in scene)
                sprite.Draw(spriteBatch);


            Bryan.DrawMoves(spriteBatch);
            Jack.DrawMoves(spriteBatch);

            spriteBatch.DrawString(Arial,"Blue HP: " + Jack.healthpoints,new Vector2(200,-75),Color.Blue);
            spriteBatch.DrawString(Arial, "Red HP: " + Bryan.healthpoints, new Vector2(1500, -75), Color.Red);

            if (activeBullets != null)
                foreach (Bullet b in activeBullets)
                    b.DrawBullet(spriteBatch);
        }

        //Draw Endscreen
        protected void DrawEndScreen(SpriteBatch spriteBatch)
        {
            if(winner == 0) //blue wins
                if(mouseover3 == true)
                    Endscene[1].Draw(spriteBatch);
                else 
                    Endscene[0].Draw(spriteBatch);
            if(winner == 1) // red wins
                if (mouseover3 == true)
                    Endscene[3].Draw(spriteBatch);
                else
                    Endscene[2].Draw(spriteBatch);
        }

    }
}
