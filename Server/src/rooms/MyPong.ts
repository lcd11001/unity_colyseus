import { Room, Client } from "colyseus";
import { Player } from "./schema/MyRoomState";
import { MyPongState, PongBall, PongPlayer } from "./schema/MyPongState";

export type PongPlayerPosition = {
    pos: number;
}

export type PongBallForce = {
    x: number;
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

            this.broadcast(player, { except: client });
        });

        this.onMessage("pong_ball_position", (client, position: PongBall) => {
            const ball = this.state.ball;
            ball.x = position.x;
            ball.y = position.y;
            ball.tick = position.tick;

            console.log(`server received ball ${position.x}:${position.y} tick ${position.tick} from ${client.sessionId} `)
            this.broadcast(ball, { except: client });
        });

        if (this.state.players.size == 2) {
            this.state.ball.x = 0;
            this.state.ball.y = 0;

            let force = this.BallForce();
            this.clients.forEach((client, index) => {
                if (index == 1) {
                    // because player camera always behind him
                    // so that, we invert force
                    force.x *= -1;
                    force.y *= -1;
                }
                this.broadcast("pong_start_game", force, { except: client });
            });
        }
    }

    onLeave(client: Client, consented: boolean) {
        this.state.players.delete(client.sessionId);
        this.broadcast("pong_stop_game");
        console.log(client.sessionId, "left!", "consented", consented);
    }

    onDispose() {
        console.log("room", this.roomId, "disposing...");
    }

    RandomDirection(): number {
        return (Math.floor(Math.random() * 2)) * 2 - 1;
    }

    BallForce(): PongBallForce {
        let force: PongBallForce = { x: 0, y: 0 };
        force.x = 3 * this.RandomDirection();
        force.y = 15 * this.RandomDirection();
        return force;
    }
}