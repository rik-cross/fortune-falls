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
            //RequiredComponent<AnimatedSpriteComponent>();
        }

        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            BattleComponent battleComponent = entity.GetComponent<BattleComponent>();
            AnimatedSpriteComponent animatedComponent = entity.GetComponent<AnimatedSpriteComponent>();
            if (animatedComponent == null)
                return;

            AnimatedSprite animatedSprite = animatedComponent.GetAnimatedSprite(entity.State);
            if (animatedSprite == null)
                return;

            HBox hitBox = battleComponent.GetHitbox(entity.State);

            if (battleComponent.GetHitbox(entity.State) != null
                && (animatedSprite.SpriteList[0].CurrentFrame == hitBox.frame || hitBox.frame == -1)
                && animatedSprite.Timer == 0)
            {
                bool hit = false;

                //List<Entity> hitEntities;

                foreach (Entity otherE in scene.EntitiesInScene) // use EntityList and check entities in same scene?
                {
                    //if (e != entity && e.GetComponent<BattleComponent>() != null)
                    if (EntityMapper.ContainsKey(otherE.Id) && entity != otherE) // // check other entity is in system
                    {
                        BattleComponent bc = otherE.GetComponent<BattleComponent>();
                        if (bc.GetHurtbox(otherE.State) != null)
                        {
                            
                            HBox r1 = bc.GetHurtbox(otherE.State);
                            HBox r2 = battleComponent.GetHitbox(entity.State);

                            TransformComponent t1 = otherE.GetComponent<TransformComponent>();
                            TransformComponent t2 = entity.GetComponent<TransformComponent>();

                            Rectangle r1a = new Rectangle((int)(r1.offset.X + t1.X), (int)(r1.offset.Y + t1.Y), (int)r1.size.X, (int)r1.size.Y);
                            Rectangle r2a = new Rectangle((int)(r2.offset.X + t2.X), (int)(r2.offset.Y + t2.Y), (int)r2.size.X, (int)r2.size.Y);

                            if (r1a.Intersects(r2a) && battleComponent.weapon != null)
                            {

                                // todo: add to a list, and then have the option
                                // to only hit the one closest entity

                                hit = true;
                                if (battleComponent.weapon.hitSound != null)
                                    EngineGlobals.soundManager.PlaySoundEffect(battleComponent.weapon.hitSound);
                                if (battleComponent.OnHit != null)
                                    battleComponent.OnHit(entity, otherE, battleComponent.weapon, bc.weapon);
                                if (bc.OnHurt != null)
                                    bc.OnHurt(otherE, entity, bc.weapon, battleComponent.weapon);

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

        // todo - Ask Rik what this draw is for and why it's using CameraList?
        public override void Draw(GameTime gameTime, Scene scene)
        {
            foreach (Camera c in scene.CameraList)
            {
                if (scene.EntitiesInScene.Contains(c.ownerEntity))
                {
                    if (c.ownerEntity.GetComponent<BattleComponent>() != null)
                    {
                        int w = 64;
                        int h = 64;
                        int x = (int)(c.screenPosition.X + 28);
                        int y = (int)(c.screenPosition.Y + 28);

                        UI.DrawRect(x, y, w, h);

                        if (c.ownerEntity.GetComponent<BattleComponent>().weapon != null)
                        {
                            Image i = new Image(
                                c.ownerEntity.GetComponent<BattleComponent>().weapon.image,
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
