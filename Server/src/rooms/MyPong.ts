import { Room, Client } from "colyseus";
import { Player } from "./schema/MyRoomState";
import { MyPongState, PongBall, PongPlayer } from "./schema/MyPongState";

export type PongPlayerPosition = {
    pos: number;
}

export type PongBallPosition = {
    x: number,
    y: number
}

export class MyPong extends Room<MyPongState> {
    onCreate(options: any): void | Promise<any> {
        this.setState(new MyPongState());
    }

    onJoin(client: Client, options?: any, auth?: any): void | Promise<any> {
        console.log(`Pong ${client.sessionId} joined`);

        const newPlayer = new PongPlayer();
        newPlayer.id = client.sessionId;
        this.state.players.set(client.sessionId, newPlayer);

        this.onMessage("pong_player_position", (client, position: PongPlayerPosition) => {
            const player = this.state.players.get(client.sessionId);
            player.pos = position.pos;

            this.broadcast(player, {except: client});
        });

        this.onMessage("pong_ball_position", (client, position: PongBallPosition) => {
            const ball = this.state.ball;
            ball.x = position.x;
            ball.y = position.y;

            this.broadcast(ball);
        });

        if (this.state.players.size == 2)
        {
            this.state.ball.x = 0;
            this.state.ball.y = 0;
            this.broadcast("pong_start_game", this.roomId);
        }
    }

    onLeave(client: Client, consented: boolean) {
        this.state.players.delete(client.sessionId);
        this.broadcast("pong_stop_game", this.roomId);
        console.log(client.sessionId, "left!", "consented", consented);
    }

    onDispose() {
        console.log("room", this.roomId, "disposing...");
    }
}