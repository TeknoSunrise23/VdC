using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VingançaDoChocolate_OPENBETA
{
    public class Sprite
    {
        static Dictionary<string, TextureData> cache = new Dictionary<string, TextureData>();

        protected Texture2D image;
        protected Vector2 position;
        public Rectangle bbox;
        public string name;

        public Sprite(ContentManager content, string imagename, Vector2 pos)
        {
            name = imagename;
            image = content.Load<Texture2D>(imagename);
            position = new Vector2();
            position.X = pos.X * Game1.unitSize;
            position.Y = -image.Height - pos.Y * Game1.unitSize;
            bbox = new Rectangle(position.ToPoint(), new Point(image.Width, image.Height));

            cacheSprite(name, image);
        }

        protected void updateBoundingBox(Vector2 pos)
        {
            bbox.Location = pos.ToPoint();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, position, Color.White);
        }

        protected List<Sprite> collides()
        {
            return Game1.scene.Where(el => Intersects(el)).ToList();
        }

        public static void cacheSprite(string name, Texture2D image)
        {
            if (!cache.ContainsKey(name))
                cache.Add(name, new TextureData(image));
        }

        static TextureData Get(string name)
        {
            return cache[name]; // CUIDADE SE O NAME NAO EXISTIR!"#!"#!"#!"#!"#!"#!"#!"#!"#!"#!"#!"#!"#!
        }

        protected bool Intersects(Sprite other)
        {
            if (bbox.Intersects(other.bbox))
            {
                // pixeis colidem?
                if (bbox.Width * bbox.Height < other.bbox.Width * other.bbox.Height)
                {
                    // eu sou mais pequeno....
                    return pixelIntersects(other);
                }
                else
                {
                    // other e mais pequeno...
                    return other.pixelIntersects(this);
                }
            }
            else
            {
                return false;
            }
        }

        

        bool pixelIntersects(Sprite other)
        {
            TextureData our = Sprite.Get(name);// nome da imagem ativa
            TextureData yours = Sprite.Get(other.name);
            
            for (int x = 0; x < our.width; x++)
            {
                for (int y = 0; y < our.height; y++)
                {
                    if (our.At(x, y).A != 0)
                    {
                        // nao e transparente em our.
                        int xl = (int)(x + bbox.X - other.bbox.X);
                        int yl = (int)(y + bbox.Y - other.bbox.Y);

                        if (xl >= 0 && yl >= 0 && xl < yours.width && yl < yours.height)
                        {
                            if (yours.At(xl, yl).A != 0)
                                return true;
                        }


                    }
                }
            } // yes my friend
            return false;


        }
    }
}
