using System.Net.Http;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MyNewMine
{
    public class Chunks
    {   public Vector3 Position { get; set; }
        public Model blockModel { get; set; }
        public Chunks(Vector3 position, Model model)
        {
            Position = position;
            blockModel = model;                    
        }
    }
}