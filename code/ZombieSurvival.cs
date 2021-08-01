
using System;
using System.IO;
using System.Threading.Tasks;
using Sandbox;
using Sandbox.UI.Construct;
using ZombieSurvival.Events;

//
// You don't need to put things in a namespace, but it doesn't hurt.
//
namespace ZombieSurvival
{

	/// <summary>
	/// This is your game class. This is an entity that is created serverside when
	/// the game starts, and is replicated to the client. 
	/// 
	/// You can use this to create things like HUDs and declare which player class
	/// to use for spawned players.
	/// 
	/// Your game needs to be registered (using [Library] here) with the same name 
	/// as your game addon. If it isn't then we won't be able to find it.
	/// </summary>
	[Library( "zombiesurvival" )]
	public partial class Game : Sandbox.Game
	{
		public static Game Instance
		{
			get => Current as Game;
		}

		[Net] public WaveStates WaveStates { get; private set; } = new WaveStates();

		public Game()
		{

			if ( IsServer )
			{
				Log.Info( "My Gamemode Has Created Serverside!" );

				// Create a HUD entity. This entity is globally networked
				// and when it is created clientside it creates the actual
				// UI panels. You don't have to create your HUD via an entity,
				// this just feels like a nice neat way to do it.
				new MinimalHudEntity();

				WaveStates.Start();
			}

			if ( IsClient )
			{
				Log.Info( "My Gamemode Has Created Clientside!" );
			}

			_ = StartSecondTimer();
		}

		public async Task StartSecondTimer()
		{
			while (true)
			{
				await Task.DelaySeconds( 1 );
				Event.Run( ZombieSurvivalEvents.Second );
			}
		}
		
		/// <summary>
		/// A client has joined the server. Make them a pawn to play with
		/// </summary>
		public override void ClientJoined( Client client )
		{
			base.ClientJoined( client );

			var player = new BasePlayer();
			client.Pawn = player;

			player.Respawn();
		}
	}

}
