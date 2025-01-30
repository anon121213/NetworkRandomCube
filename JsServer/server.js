const express = require("express");
const app = express();
app.use(express.json());

let lobbies = {};
let nextLobbyId = 1;

app.get("/lobbies", (req, res) => {
    res.json(Object.values(lobbies));
});

app.post("/create-lobby", (req, res) => {
    const { ip, tcpPort, udpPort } = req.body;

    if (!ip || !tcpPort || !udpPort) {
        return res.status(400).json({ message: "Не хватает данных (ip, tcpPort, udpPort)" });
    }

    const lobbyId = nextLobbyId++;
    lobbies[lobbyId] = { lobbyId, ip, tcpPort, udpPort };

    res.json({ lobbyId, message: `Лобби создано ${ip}` });
});

app.get("/delete-lobby", (req, res) => {
    const { ip, tcpPort, udpPort } = req.query;

    if (!ip || !tcpPort || !udpPort) {
        return res.status(400).json({ message: "Не хватает данных (ip, tcpPort, udpPort)" });
    }

    let deleted = false;
    for (const lobbyId in lobbies) {
        const lobby = lobbies[lobbyId];
        if (lobby.ip == ip && lobby.tcpPort == tcpPort && lobby.udpPort == udpPort) {
            delete lobbies[lobbyId];
            deleted = true;
            break;
        }
    }

    if (deleted) {
        res.json({ message: `Лобби с IP ${ip}, TCP порт ${tcpPort}, UDP порт ${udpPort} удалено.` });
    } else {
        res.status(404).json({ message: "Лобби не найдено." });
    }
});

const PORT = 3000;
app.listen(PORT, () => {
    console.log(`Lobby server running on port ${PORT}`);
});