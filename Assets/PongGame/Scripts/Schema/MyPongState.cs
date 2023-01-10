// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 1.0.45
// 

using Colyseus.Schema;

public partial class MyPongState : Schema {
	[Type(0, "map", typeof(MapSchema<PongPlayer>))]
	public MapSchema<PongPlayer> players = new MapSchema<PongPlayer>();

	[Type(1, "ref", typeof(PongBall))]
	public PongBall ball = new PongBall();
}

