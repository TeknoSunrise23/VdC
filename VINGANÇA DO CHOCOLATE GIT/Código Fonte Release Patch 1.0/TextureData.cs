using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VingançaDoChocolate_OPENBETA
{
    class TextureData
    {
        public int width, height;
        Color[] data;

        public TextureData(Texture2D texture)
        {
            width = texture.Width;
            height = texture.Height;
            data = new Color[width * height];
            texture.GetData<Color>(data);
        }

        public Color At(int x, int y)
        {
            return data[x + y * width];
        }
    }
}
