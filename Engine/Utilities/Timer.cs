using Microsoft.Xna.Framework;
using System;

namespace Engine
{
	// todo - unused class!!!
    public class Timer // abstract?
    {
		// timeLength
		// timeLeft
		// active

		private float _timePassed; // Property private set?
		private float _duration;
		private float _delay;
		private bool _repeat;
		private int _counter; // for things like damage over time
		private int _limit;

		int counter = 1;
		int limit = 50;
		float countDuration = 2f; //every  2s.
		float currentTime = 0f;

		private float delay;
		private bool repeat;
		private float acc;
		private bool done;
		private bool stopped;

		public Timer(float duration, bool repeat, int limit = 5, float delay = 0.0f)
		{
			Console.WriteLine("Timer started");
			_timePassed = 0.0f;
			_duration = duration;
			_repeat = repeat;
			_limit = limit;
			_delay = delay;
		}

		public event Action Destroyed;

		private void CustomDestroy()
		{
			Destroyed?.Invoke();
		}

		public void Update(GameTime gameTime)
		{
			currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds; //Time passed since last Update() 

			if (currentTime >= countDuration)
			{
				counter++;
				currentTime -= countDuration; // "use up" the time
											  //any actions to perform
			}
			if (counter >= limit)
			{
				counter = 0;//Reset the counter;
							//any actions to perform
			}


			_timePassed += (float)gameTime.ElapsedGameTime.TotalSeconds;
			if (_timePassed > _duration) // && repeat
			{
				_timePassed -= _duration;
				_counter++;
				//doYourStuff();
				Console.WriteLine("Timer ended {_repeatCounter}");
			}

			if (_repeat && _limit != 0 && _counter >= _limit)
            {
				// DELETE timer
				Console.WriteLine("Timer limit reached");
			}
		}
	}
}
