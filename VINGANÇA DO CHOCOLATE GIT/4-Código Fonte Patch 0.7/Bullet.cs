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
        public float velocityX = 1000f;
        public string name; 
        protected Texture2D image;
        protected Vector2 positionb;
        int p, s;
        public float removaltimer;
        bool collided;

        public Bullet(ContentManager content, string imagename, Vector2 position, int p, int s) : base(content, imagename, position, p)
        {
            name = imagename;
            image = content.Load<Texture2D>(imagename);
            positionb = position;
            this.p = p;
            this.s = s;
            removaltimer = 0f;
            collided = false;
        }

        public void Update(GameTime gameTime)
        {
            List<Sprite> colSprites = collides();
            Vector2 target = positionb;
            if(collided)removaltimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
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
                // colide 
                velocityX = 0;
                collided = true;
            }
            if (target != position) //if we move
            {
                updateBoundingBox(target);
                List<Sprite> col = collidesbullets();
                if (col.Count == 0) // if we do not collide
                    position = target;
                else
                {
                    velocityX = 0;
                    collided = true;
                    removaltimer = 2;
                    ((Player)col[0]).healthpoints -= 25;

                }
            }
        }

        public void DrawBullet(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, positionb, Color.White);
        }
    }
}
