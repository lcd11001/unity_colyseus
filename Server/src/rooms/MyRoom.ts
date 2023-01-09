import { Room, Client } from "colyseus";
import { MyRoomState, Player } from "./schema/MyRoomState";

export type Position = {
  x: number,
  y: number
}

export class MyRoom extends Room<MyRoomState> {

  onCreate (options: any) {
    this.setState(new MyRoomState());
  }

  onJoin (client: Client, options: any) {
    console.log(client.sessionId, "joined!");
    
    const newPlayer = new Player();
    newPlayer.id = client.sessionId;
    this.state.players.set(client.sessionId, newPlayer);

    // Send welcome message to the client.
    client.send("welcomeMessage", "Welcome to Colyseus!");
    client.send("sessionId", client.sessionId);

    // Listen to position changes from the client.
    this.onMessage("position", (client, position: Position) => {
      const player = this.state.players.get(client.sessionId);
      player.x = position.x;
      player.y = position.y;
      
      this.broadcast(player);
    });
  }

  onLeave (client: Client, consented: boolean) {
    this.state.players.delete(client.sessionId);
    console.log(client.sessionId, "left!", "consented", consented);
  }

  onDispose() {
    console.log("room", this.roomId, "disposing...");
  }

}
