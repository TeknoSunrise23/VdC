using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VingançaDoChocolate_OPENBETA
{
    public class Player : Sprite
    {
        
        
        float accelerationY = 20f;
        float velocityY = 0f;
        float jumpTime = 0f;
        
        bool onGround = false;
        float velocityX = 0f;
        float timer = 0f;
        float frames = 0f;
        int player;
        int side;
        
        

        public static Keys[,] teclas = new Keys[,] { { Keys.D, Keys.A, Keys.W, Keys.D1},
            { Keys.Right, Keys.Left, Keys.Up, Keys.L} };

        public Player(ContentManager content, string imagename, Vector2 position, int p) : base(content, imagename, position)
        {
            
            
            player = p;
            
        }

        public void Update(GameTime gameTime, SpriteBatch spriteBatch)
        {
            
                // gravity
            velocityY += accelerationY * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 target = position;
            target.Y += velocityY;
            updateBoundingBox(target);

            List<Sprite> colSprites = collides();
            if (colSprites.Count == 0)
            {
                // nenhuma colisao
                onGround = false;
                position = target;
            }
            else
            {
                // colidimos!"#$%&/()=?»
                onGround = true;
                velocityY = 0f;
                jumpTime = 0f;

                // posicao passa a ser baseada na colisao mais acima (com topo minimo)
               // position.Y = colSprites.Min(sprite => sprite.bbox.Top) - bbox.Height;

            }
            
            KeyboardState keys = Keyboard.GetState();
            target = position;
            if ((keys.IsKeyDown(teclas[player, 0])) == (keys.IsKeyDown(teclas[player, 1])))
            {
                timer = 0;
                if (side > 1) side = 2;
                else side = 0;
            }
            

            if (keys.IsKeyDown(teclas[player, 1]))
            {
                
                side = 2;
                frames += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if(frames > 125)
                {
                    side=3;
                }
                if(frames > 250)
                {
                    side = 2;
                    frames = 0f;
                }
                velocityX = 200f;
                timer += (float)gameTime.ElapsedGameTime.TotalSeconds*3f;
                if (timer > 0)
                    velocityX += 200f * timer;
                if (velocityX > 600f)
                    velocityX = 600f;
                target.X -= velocityX * (float)gameTime.ElapsedGameTime.TotalSeconds;

                
            }
            
            if (keys.IsKeyDown(teclas[player, 0]))
            {
                
                side = 0;
                frames += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (frames > 125)
                {
                    side = 1;
                }
                if (frames > 250)
                {
                    side = 0;
                    frames = 0f;
                }
                velocityX = 200f;
                timer += (float)gameTime.ElapsedGameTime.TotalSeconds*3f;
                if (timer > 0)
                    velocityX += 200f * timer;
                if (velocityX > 600f)
                    velocityX = 600f;
                target.X += velocityX * (float)gameTime.ElapsedGameTime.TotalSeconds;
                
                
            }
            

            if (onGround && keys.IsKeyDown(teclas[player, 2]))
            {
                onGround = false;
                jumpTime = 0.5f;
            }

            if (jumpTime > 0f)
            {
                target.Y -= 65f * jumpTime;
                jumpTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (target != position) //if we move
            {
                updateBoundingBox(target);
                if (collides().Count == 0) // if we do not collide
                    position = target;
                else jumpTime = 0f;
            }
        }

        
        
        public void DrawMoves(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.character[player, side], position, Color.White);                           
        }


        public Vector2 GetPosition()
        {
            return position;
        }

        public int getplayer()
        {
            return player;
        }
        public int getside()
        {
            return side;
        }

        public string spritename()
        {

            if (player == 0 && (side == 0 || side == 1))
                return "Blue0R";
            if (player == 0 && (side == 2 || side == 3))
                return "Blue0L";
            if (player == 1 && (side == 0 || side == 1))
                return "Red0R";
            if (player == 1 && (side == 2 || side == 3))
                return "Red0L";
            else return "cala-te Visualstudio";
        }


        
    }
}
