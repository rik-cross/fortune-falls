using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using S = System.Diagnostics.Debug;

namespace AdventureGame.Engine
{
    public class BattleSystem : System
    {
        public BattleSystem()
        {
            RequiredComponent<BattleComponent>();
            RequiredComponent<SpriteComponent>();
        }

        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            BattleComponent battleComponent = entity.GetComponent<BattleComponent>();
            SpriteComponent spriteComponent = entity.GetComponent<SpriteComponent>();

            HBox hitBox = battleComponent.GetHitbox(entity.State);
            Sprite sprite = spriteComponent.GetSprite(entity.State);

            if (battleComponent.GetHitbox(entity.State) != null
                && (sprite.currentPosition == hitBox.frame || hitBox.frame == -1)
                && sprite.timer == 0)
            {

                bool hit = false;

                foreach (Engine.Entity e in scene.EntityList)
                {
                    if (e != entity && e.GetComponent<Engine.BattleComponent>() != null)
                    {
                        Engine.BattleComponent bc = e.GetComponent<Engine.BattleComponent>();
                        if (bc.GetHurtbox(e.State) != null)
                        {
                            
                            HBox r1 = bc.GetHurtbox(e.State);
                            HBox r2 = battleComponent.GetHitbox(entity.State);

                            Engine.TransformComponent t1 = e.GetComponent<Engine.TransformComponent>();
                            Engine.TransformComponent t2 = entity.GetComponent<Engine.TransformComponent>();

                            Rectangle r1a = new Rectangle((int)(r1.offset.X + t1.X), (int)(r1.offset.Y + t1.Y), (int)r1.size.X, (int)r1.size.Y);
                            Rectangle r2a = new Rectangle((int)(r2.offset.X + t2.X), (int)(r2.offset.Y + t2.Y), (int)r2.size.X, (int)r2.size.Y);

                            if (r1a.Intersects(r2a) && battleComponent.weapon != null)
                            {
                                hit = true;
                                if (battleComponent.weapon.hitSound != null)
                                    EngineGlobals.soundManager.PlaySoundEffect(battleComponent.weapon.hitSound);
                                if (battleComponent.OnHit != null)
                                    battleComponent.OnHit(entity, e, battleComponent.weapon, bc.weapon);
                                if (bc.OnHurt != null)
                                    bc.OnHurt(e, entity, bc.weapon, battleComponent.weapon);
                            }
                        }
                    }
                }

                if (!hit && battleComponent.weapon != null)
                {
                    if (battleComponent.weapon.missSound != null)
                        EngineGlobals.soundManager.PlaySoundEffect(battleComponent.weapon.missSound);
                    if (battleComponent.OnMiss != null)
                        battleComponent.OnMiss(entity, battleComponent.weapon);
                }    

            }
        }
        public override void DrawEntity(GameTime gameTime, Scene scene, Entity entity)
        {

            if (!EngineGlobals.DEBUG)
                return;

            BattleComponent battleComponent = entity.GetComponent<BattleComponent>();

            if (battleComponent.GetHurtbox(entity.State) != null)
            {
                HBox re = battleComponent.GetHurtbox(entity.State);
                TransformComponent tf = entity.GetComponent<TransformComponent>();
                RectangleF rect = new RectangleF(
                    tf.X + re.offset.X,
                    tf.Y + re.offset.Y,
                    re.size.X,
                    re.size.Y
                );
                Globals.spriteBatch.DrawRectangle(rect, Color.Blue);
            }

            if (battleComponent.GetHitbox(entity.State) != null)
            {
                HBox re = battleComponent.GetHitbox(entity.State);
                TransformComponent tf = entity.GetComponent<TransformComponent>();
                RectangleF rect = new RectangleF(
                    tf.X + re.offset.X,
                    tf.Y + re.offset.Y,
                    re.size.X,
                    re.size.Y
                );
                Globals.spriteBatch.DrawRectangle(rect, Color.Purple);
            }
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
