using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended;

using MonoGame.Extended.Content;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Shapes;

using AdventureGame.Engine;

namespace AdventureGame
{
    public class GameScene : Scene
    {

        public Engine.Camera camera;
        public override void Init()
        {

            // main camera
            Engine.Camera mainCam = new Engine.Camera();
            mainCam.SetWorldPos(400, 240);
            AddCamera(mainCam);

            // player camera
            Engine.Camera playerCam = new Engine.Camera(0, 0, 580, 260, 200, 200, 2, 0, 2);
            AddCamera(playerCam);

            // player entity
            Entity e = new Entity();
            e.AddComponent(new Engine.TransformComponent());

            //e.AddComponent(new Engine.SpriteComponent());
            //Engine.SpriteComponent sc = e.GetComponent<Engine.SpriteComponent>();
            //sc.sprite = Globals.content.Load<Texture2D>("sprite");

            e.AddComponent(new Engine.AnimationComponent());
            Engine.AnimationComponent ac = e.GetComponent<Engine.AnimationComponent>();
            ac.animation = new AnimatedSprite(Globals.content.Load<SpriteSheet>("motw.sf", new JsonContentLoader()));
            ac.state = "idle";

            entities.Add(e);

            Entity l = new Entity();
            l.AddComponent(new Engine.TransformComponent());
            Engine.TransformComponent tc = l.GetComponent<Engine.TransformComponent>();
            tc.position.X = 250;
            tc.position.Y = 250;
            l.AddComponent(new Engine.LightComponent());
            Engine.LightComponent lc = l.GetComponent<Engine.LightComponent>();
            lc.radius = 200;
            entities.Add(l);

            playerCam.trackedEntity = e;

        }

        public override void LoadContent()
        {
            Init();
        }

        public override void Update(GameTime gameTime)
        {
            lightLevel = (float)DayNightCycle.GetLightLevel();
        }
        public override void Draw(GameTime gameTime)
        {
            if (Globals.font != null)
            {
                Texture2D dayNight = Globals.content.Load<Texture2D>("daynight");
                Texture2D dayNightOverlay = Globals.content.Load<Texture2D>("daynightoverlay");
                Globals.spriteBatch.Draw(dayNightOverlay, new Rectangle(740, 10, 50, 50), Color.White);
                Globals.spriteBatch.Draw(dayNight, new Vector2(765,35), null, Color.White, (float)((Math.PI*2)/100*DayNightCycle.GetPercentage()), new Vector2(25, 25), 1, SpriteEffects.None, 0);
                Globals.spriteBatch.DrawString(Globals.fontSmall, "Day " + Engine.DayNightCycle.day.ToString(), new Vector2(740, 65), Color.White);
            }
                
        }

    }

}
