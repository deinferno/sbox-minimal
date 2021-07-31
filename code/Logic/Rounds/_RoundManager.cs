


using Sandbox;

namespace ZombieSurvival
{
	public partial class RoundManager : NetworkComponent
	{
		public BaseRound Current { get; private set; }

		[Net] public int RoundNumber { get; set; } = 1;

		[ServerVar( "zs_maxrounds", Saved = true, Help = "How many rounds are played on one map" )]
		public int MaxRounds { get; set; } = 2;

		[ServerVar( "zs_timelimit", Saved = true, Help = "Limits time played on one map" )]
		public float TimeLimit { get; set; } = 30f;

		protected RoundManager()
		{
			//TODO : Implement obj and ze logic
			Log.Info(  $"Map loaded {Global.MapName}");

			Current = new ZSRound();
			Current.Start();
		}
}
}
