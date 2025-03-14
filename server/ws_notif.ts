console.log("port 8080");

Bun.serve({
  fetch(req, server) {
    server.upgrade(req, {});
  },
  websocket: {
    open(ws) {
      console.log("someone is here.");
      ws.send("welcome to the server");
    },
    message(ws, message) {
      ws.send(message);
    },
  },
});
