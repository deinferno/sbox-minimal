


using Sandbox;

namespace ZombieSurvival
{
	public abstract partial class BaseRound : NetworkComponent 
	{
		public static string RoundName = "Base";
		[Net] public BaseTeam Winner { get; set; }

		public void Start()
		{
			
		}
		
		public void End()
		{
			
		}

	}
}
