using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VdC_Closed_Beta
{
    class Player : Sprite
    {
        float accelerationY = 50f;
        float velocityY = 0f;
        float jumpTime = 0f;
        bool onGround = false;

        public Player(ContentManager content, string imagename, Vector2 position) : base(content, imagename, position)
        {

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

            if (keys.IsKeyDown(Keys.A))
            {
                target.X -= 350f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (keys.IsKeyDown(Keys.D))
            {
                target.X += 350f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (onGround && keys.IsKeyDown(Keys.W))
            {
                onGround = false;
                jumpTime = 0.5f;
            }

            if (jumpTime > 0f)
            {
                target.Y -= 60f * jumpTime;
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

        public Vector2 GetPosition()
        {
            return position;
        }



    }
}
