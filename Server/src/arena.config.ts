import Arena from "@colyseus/arena";
import { monitor } from "@colyseus/monitor";
import { MyPong } from "./rooms/MyPong";

/**
 * Import your Room files
 */
import { MyRoom } from "./rooms/MyRoom";

export default Arena({
    getId: () => "Your Colyseus App",

    initializeGameServer: (gameServer) => {
        // Make sure to never call the `simulateLatency()` method in production.
        if (process.env.NODE_ENV !== "production") {
            console.log(`*********** Simulate Latency ${process.env.LATENCY} **************`);
            // simulate 200ms latency between server and client.
            gameServer.simulateLatency(Number.parseInt(process.env.LATENCY));
        }
        /**
         * Define your room handlers:
         */
        gameServer.define('my_room', MyRoom);
        gameServer.define('pong_room', MyPong);
    },

    initializeExpress: (app) => {
        /**
         * Bind your custom express routes here:
         */
        app.get("/", (req, res) => {
            res.send("It's time to kick ass and chew bubblegum!");
        });

        /**
         * Bind @colyseus/monitor
         * It is recommended to protect this route with a password.
         * Read more: https://docs.colyseus.io/tools/monitor/
         */
        app.use("/colyseus", monitor());
    },


    beforeListen: () => {
        /**
         * Before before gameServer.listen() is called.
         */
    }
});