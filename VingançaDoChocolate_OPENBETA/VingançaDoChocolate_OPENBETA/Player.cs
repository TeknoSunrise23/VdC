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
        float accelerationY = 35f;
        float velocityY = 0f;
        float jumpTime = 0f;
        bool onGround = false;
        float velocityX = 0f;
        float timer = 0f;
        int player;
        char lado;

        Keys[,] teclas = new Keys[,] { { Keys.A, Keys.D, Keys.W, Keys.L},
            { Keys.Left, Keys.Right, Keys.Up, Keys.D1} };

        public Player(ContentManager content, string imagename, Vector2 position, int p) : base(content, imagename, position)
        {
            player = p;      
        }

        public void Update(GameTime gameTime)
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
                position.Y = colSprites.Min(sprite => sprite.bbox.Top) - bbox.Height;

            }
            
            KeyboardState keys = Keyboard.GetState();
            target = position;
            if ((keys.IsKeyDown(teclas[player,0])) == (keys.IsKeyDown(teclas[player, 1])))
                timer = 0;

                if (keys.IsKeyDown(teclas[player, 0]))
            {
                lado = 'L';
                velocityX = 150f;
                timer += (float)gameTime.ElapsedGameTime.TotalSeconds*3f;
                if (timer > 0)
                    velocityX += 150f * timer;
                target.X -= velocityX * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (velocityX > 300f)
                    velocityX = 300f;
            }
            
            if (keys.IsKeyDown(teclas[player, 1]))
            {
                lado = 'R';
                velocityX = 150f;
                timer += (float)gameTime.ElapsedGameTime.TotalSeconds*3f;
                if (timer > 0)
                    velocityX += 150f * timer;
                target.X += velocityX * (float)gameTime.ElapsedGameTime.TotalSeconds;
                
                if (velocityX > 300f)
                    velocityX = 300f;
            }
            

            if (onGround && keys.IsKeyDown(teclas[player, 2]))
            {
                onGround = false;
                jumpTime = 0.5f;
            }

            if (jumpTime > 0f)
            {
                target.Y -= 70f * jumpTime;
                jumpTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (target != position) //if we move
            {
                updateBoundingBox(target);
                if (collides().Count == 0) // if we do not collide
                    position = target;
                else jumpTime = 0f;
            }

            if(keys.IsKeyDown(teclas[player,3]))
            {
               
                
            }


        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public char GetLado()
        {
            return lado;
        }


    }
}
