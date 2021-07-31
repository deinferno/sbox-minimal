
using System;
using System.Threading;
using System.Threading.Tasks;
using Sandbox;

// ReSharper disable once FunctionNeverReturns

namespace ZombieSurvival
{

	public partial class WaveStates : NetworkComponent
	{
		//Internal left public for lulz
		[Net] public bool State { get; set; } = false;
		[Net] public int WaveNumber { get; set; } = 0;
		[Net] public float StartTime { get; set; } = 0f;
		[Net] public float EndTime { get; set; } = 0f;
		private Task _waveRoutine;
		private CancellationTokenSource _cts;
		
		// Must be set by config
		[ServerVar( "zs_numberofwaves", Saved = true , Help = "How many waves are there to survive" )]
		public static int NumberOfWaves { get; set; } = 6;
		
		[ServerVar( "zs_wavezerolength", Saved = true , Help = "How long wave zero lasts" )]
		public static float ZeroWaveLength { get; set; } = 15f;
		
		[ServerVar( "zs_intermissionlength", Saved = true, Help = "How long wave intermission lasts" )]
		public static float IntermissionLength { get; set; } = 5f;
		
		[ServerVar( "zs_basewavelength", Saved = true, Help = "How long wave 1 lasts" )]
		public static float BaseWaveLength { get; set; } = 10f;
		
		[ServerVar( "zs_wavelengthperwave", Saved = true, Help = "How much wave time added per wave" )]
		public static float WaveLengthPerWave { get; set; } = 5f;
		


		private async Task WaveRoutineFunction(CancellationToken ct = default)
		{
			try
			{
				while ( true )
				{
					StartTime = Time.Now + (WaveNumber == 0 ? ZeroWaveLength : IntermissionLength);
					EndTime = StartTime + BaseWaveLength + WaveNumber * WaveLengthPerWave;
					await GameTask.DelaySeconds( StartTime - Time.Now );
					ct.ThrowIfCancellationRequested();
					WaveNumber++;
					State = !State;
					OnWaveStateChanged();
					await GameTask.DelaySeconds( EndTime - Time.Now );
					ct.ThrowIfCancellationRequested();
					State = !State;
					OnWaveStateChanged();
				}
			}
			catch ( OperationCanceledException )
			{
				Log.Trace(  "WaveRoutine task canceled");
			}
		}

		public void Start()
		{
			_cts = new CancellationTokenSource();
			_waveRoutine = WaveRoutineFunction(_cts.Token);
		}

		public void Reset()
		{
			_cts.Cancel();
			_cts.Dispose();
			State = false;
			WaveNumber = 0;
			StartTime = 0;
			EndTime = 0;
			this.Start();
		}

		public void OnWaveStateChanged()
		{
			Log.Info(  $"#{State} #{WaveNumber} #{StartTime} #{EndTime}");
			// Do shit
			Event.Run(  "ZombieSurvival.WaveStateChanged");
			
			if (WaveNumber > NumberOfWaves) {
				//Reset it for now
				Reset();
			}
		}

	}

}
