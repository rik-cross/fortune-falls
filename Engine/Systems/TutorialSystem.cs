using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    public class TutorialSystem : System
    {
        public TutorialSystem()
        {
            RequiredComponent<TutorialComponent>();
        }

        public void ClearAllTutorials()
        {
            foreach (Entity e in EntityList)
            {
                e.GetComponent<TutorialComponent>().ClearTutorials();
            }
        }

        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            TutorialComponent tutorialComponent = entity.GetComponent<TutorialComponent>();
            foreach (Tutorial t in tutorialComponent.GetTutorials())
            {
                // start tutorial if not already started
                if (t.isStarted == false)
                {
                    t.isStarted = true;
                    t.onStart?.Invoke();
                }
                // check the tutorial condition is met
                if (t.condition != null && t.condition() == true)
                {
                    t.numberOfTimes -= 1;
                    if (t.numberOfTimes <= 0)
                    {
                        if(t.onComplete != null)
                        {
                            t.onComplete();
                            t.markForDeletion = true;
                        }
                    }
                }
            }

            tutorialComponent.GetTutorials().RemoveAll( t => t.markForDeletion == true);

        }
    }
}
