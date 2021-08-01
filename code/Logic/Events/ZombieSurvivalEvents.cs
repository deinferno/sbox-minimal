using System;
using Sandbox;

namespace ZombieSurvival.Events
{
	public static class ZombieSurvivalEvents
	{
		public const string WaveStateChanged = "wavestatechanged";
		public const string Second = "second";
		
		public class WaveStateChangedAttribute : EventAttribute
		{
			public WaveStateChangedAttribute() : base(WaveStateChanged) { }
		}
		
		public class SecondAttribute : EventAttribute
		{
			public SecondAttribute() : base(Second)
			{
			}
		}
	}
}
