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
        //static Dictionary<string, TextureData> cache = new Dictionary<string, TextureData>();
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
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, position, Color.White);
        }
    }
}
