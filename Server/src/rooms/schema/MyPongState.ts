import { MapSchema, Schema, Context, type } from "@colyseus/schema";


export class PongPlayer extends Schema {
	@type("string") id: string = "";
	@type("number") pos: number = 0;
}

export class PongBall extends Schema {
	@type("number") x: number = 0;
	@type("number") y: number = 0;
	@type("int32") tick: number = 0;
}

export class MyPongState extends Schema {
	@type({ map: PongPlayer })
	players: MapSchema<PongPlayer> = new MapSchema<PongPlayer>();
	
	@type(PongBall)
	ball: PongBall = new PongBall();
}