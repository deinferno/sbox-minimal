using Sandbox;

namespace ZombieSurvival
{
	public abstract partial class BaseTeam : NetworkComponent
	{
		public virtual string Name { get; protected set; } = "Base Team";
	}
}
