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
    class Bullet : Player
    {
        float velocityX = 1000f;
        public string name; 
        protected Texture2D image;
        protected Vector2 positionb;
        int p, s;

        public Bullet(ContentManager content, string imagename, Vector2 position, int p, int s) : base(content, imagename, position, p)
        {
            name = imagename;
            image = content.Load<Texture2D>(imagename);
            positionb = position;
            this.p = p;
            this.s = s;
        }

        public void Update(GameTime gameTime)
        {
            List<Sprite> colSprites = collides();
            Vector2 target = positionb;
            if (s == 0 || s == 1)
                target.X += velocityX * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (s == 2 || s == 3)
                target.X -= velocityX * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (colSprites.Count == 0)
            {
                // nao colide
                positionb = target;
            }
            else
            {
                // colide fds
                velocityX = 0;
            }
            if (target != position) //if we move
            {
                updateBoundingBox(target);
                if (collides().Count == 0) // if we do not collide
                    position = target;
                else velocityX = 0;
            }
        }

        public void DrawBullet(SpriteBatch spriteBatch)
        {

            if (p == 0 && (s == 0 || s == 1))
            {
                spriteBatch.Draw(image,new Vector2(positionb.X+161,positionb.Y+49), Color.White);
            }
            if (p == 0 && (s == 2 || s == 3))
            {
                spriteBatch.Draw(image, new Vector2(positionb.X-10, positionb.Y + 49), Color.White);
            }
                
            if (p == 1 && (s == 0 || s == 1))
            {
                spriteBatch.Draw(image, Game1.bryanpos, Color.White);
            }
            if (p == 1 && (s == 2 || s == 3))
            {
                spriteBatch.Draw(image, Game1.bryanpos, Color.White);
            }
         //   spriteBatch.End();    
            
        }
    }
}
