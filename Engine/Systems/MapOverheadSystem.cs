using Microsoft.Xna.Framework;
using MonoGame.Extended.Sprites;
using S = System.Diagnostics.Debug;
using MonoGame.Extended.Tiled;

namespace AdventureGame.Engine
{
    public class MapOverheadSystem : System
    {
        public override void Draw(GameTime gameTime, Scene scene)
        {
            if (scene.map != null)
            {
                foreach (TiledMapLayer layer in scene.map.Layers)
                {
                    if (layer.Properties.ContainsValue("above"))
                    {
                        scene.mapRenderer.Draw(layer);
                    }
                }
            }
        }

    }
}
