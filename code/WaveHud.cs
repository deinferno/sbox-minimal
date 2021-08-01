
using System;
using System.Threading.Tasks;
using Sandbox;
using Sandbox.UI;
using ZombieSurvival.Events;


// TODO : Move this to other folder when we found a way to fix hotloading this crap

namespace ZombieSurvival
{
	/// <summary>
	/// This is the HUD entity. It creates a RootPanel clientside, which can be accessed
	/// via RootPanel on this entity, or Local.Hud.
	/// </summary>
	public partial class WaveHud : Panel
	{
		public String WaveNumber { get; set; }
		public Label WaveTime { get; set; }
		public String Score { get; set; }

		private bool _laststate = true;
		private float _flashstop;

		public WaveHud()
		{
			this.SetTemplate( "/WaveHud.html" );
		}

		public override void OnHotloaded()
		{
			this.SetTemplate( "/WaveHud.html" );
		}
		
		[ZombieSurvivalEvents.Second]
		public void Second()
		{
			var waves = Game.Instance.WaveStates;
			if (waves.WaveNumber == 0)
				WaveNumber = "Prepare for apocalypse";
			else
				WaveNumber = $"Wave {waves.WaveNumber} out of {WaveStates.NumberOfWaves}";

			string stateStr;
			float time;
			if ( waves.State )
			{
				stateStr = "ends";
				time = waves.EndTime;
			}
			else
			{
				stateStr = "starts";
				time = waves.StartTime;
			}

			time = time - Time.Now;
			
			var timeStr = TimeSpan.FromSeconds(time).ToString(@"mm\:ss");

			if ( time <= 11f )
			{
				// Play shitty sound here
				if ( _laststate != waves.State )
				{
					_laststate = waves.State;
					_flashstop = Time.Now + time;
					_ = WaveTimeFlasherRoutine();
				}
			}

			WaveTime.Text = $"Wave {stateStr} in {timeStr}";
		}

		public async Task WaveTimeFlasherRoutine()
		{
			while ( _flashstop > Time.Now )
			{
				await GameTask.DelayRealtimeSeconds(0.5f);
				WaveTime.Style.FontColor = Color.White;
				WaveTime.Style.Dirty();
				await GameTask.DelayRealtimeSeconds(0.5f);
				WaveTime.Style.FontColor = Color.Red;
				WaveTime.Style.Dirty();
			}

			WaveTime.Style.FontColor = Color.White;
			WaveTime.Style.Dirty();
		}
	}

}
