import { Room, Client } from "colyseus";
import { Player } from "./schema/MyRoomState";
import { MyPongState, PongBall, PongInitBall, PongPlayer } from "./schema/MyPongState";

export type PongPlayerPosition = {
    pos: number;
}

export type PongBallForce = {
    x: number;
    y: number
}

export class MyPong extends Room<MyPongState>
{
    maxClients = 2;

    onCreate(options: any): void | Promise<any>
    {
        this.setState(new MyPongState());

        this.onMessage("pong_player_position", (client, position: PongPlayerPosition) =>
        {
            const player = this.state.players.get(client.sessionId);
            player.pos = position.pos;

            this.broadcast(player, { except: client });
        });

        this.onMessage("pong_ball_position", (client, position: PongBall) =>
        {
            const ball = this.state.ball;
            ball.x = position.x;
            ball.y = position.y;
            ball.tick = position.tick;

            console.log(`server received ball ${position.x}:${position.y} tick ${position.tick} from ${client.sessionId} `)
            this.broadcast(ball, { except: client });
        });
    }

    onJoin(client: Client, options?: any, auth?: any): void | Promise<any>
    {
        console.log(`Pong ${client.sessionId} joined with options ${JSON.stringify(options, null, 2)}`);

        const newPlayer = new PongPlayer();
        newPlayer.id = client.sessionId;
        newPlayer.ai = options?.isAI ?? false;
        this.state.players.set(client.sessionId, newPlayer);

        if (this.state.players.size == 2)
        {
            this.state.ball.x = 0;
            this.state.ball.y = 0;

            let force = this.BallForce();
            this.clients.forEach((client, index) =>
            {
                if (index == 1)
                {
                    // because player camera always behind him
                    // so that, we invert force
                    force.x *= -1;
                    force.y *= -1;
                }
                let initBall = new PongInitBall();
                initBall.x = force.x;
                initBall.y = force.y;
                initBall.hostID = this.clients[0].id;
                // this.broadcast("pong_start_game", force, { except: client });
                this.broadcast(initBall, { except: client });
            });
        }
    }

    onLeave(client: Client, consented: boolean)
    {
        this.state.players.delete(client.sessionId);
        this.broadcast("pong_stop_game");
        console.log(client.sessionId, "left!", "consented", consented);
    }

    onDispose()
    {
        console.log("room", this.roomId, "disposing...");
    }

    RandomDirection(): number
    {
        // return (Math.floor(Math.random() * 2)) * 2 - 1;
        return Math.random() < 0.5 ? -1 : 1;
    }

    BallForce(): PongBallForce
    {
        let force: PongBallForce = { x: 0, y: 0 };
        force.x = 3 * this.RandomDirection();
        force.y = 15 * this.RandomDirection();
        return force;
    }
}