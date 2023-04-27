using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using S = System.Diagnostics.Debug;

namespace AdventureGame.Engine
{
    public class WeaponSystem : System
    {
        public WeaponSystem()
        {
            RequiredComponent<WeaponComponent>();
        }

        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {

        }
        public override void Draw(GameTime gameTime, Scene scene)
        {
            foreach (Engine.Camera c in scene.CameraList)
            {
                if (scene.EntityList.Contains(c.ownerEntity))
                {
                    if (c.ownerEntity.GetComponent<Engine.WeaponComponent>() != null)
                    {
                        int w = 64;
                        int h = 64;
                        int x = (int)(c.screenPosition.X + 28);
                        int y = (int)(c.screenPosition.Y + c.size.Y - 28 - h);
                        
                        Globals.spriteBatch.Draw(UICustomisations.labelLeft, new Rectangle(x, y, 16, h), Color.White);
                        Globals.spriteBatch.Draw(UICustomisations.labelMiddle, new Rectangle(x+16, y, w-(16*2), h), Color.White);
                        Globals.spriteBatch.Draw(UICustomisations.labelRight, new Rectangle(x+(w-16), y, 16, h), Color.White);
                        if (c.ownerEntity.GetComponent<Engine.WeaponComponent>().weapon != null)
                        {
                            Engine.Image i = new Engine.Image(
                                c.ownerEntity.GetComponent<Engine.WeaponComponent>().weapon.image,
                                size: new Vector2(40, 40),
                                position: new Vector2(x+((64-40)/2), y+((64-40)/2))
                            );
                            i.Draw();
                        }
                    }
                }
            }
        }
    }
}
