import React, { useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";
import AdminPanel from "./AdminPanel";

const App: React.FC = () => {
  const [connection, setConnection] = useState<signalR.HubConnection | null>(
    null
  );
  const [connected, setConnected] = useState(false);
  const [playersAnswered, setPlayersAnswered] = useState(0);
  
  useEffect(() => {
    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:5114/quizhub")
      .withAutomaticReconnect()
      .build();
    setConnection(newConnection);
  }, []);

  useEffect(() => {
    console.log(connection);
    if (!connection) {
      return;
    }
    connection
      .start()
      .then(() => setConnected(true))
      .catch(console.error);
    return () => {
      connection.stop();
    };
  }, [connection]);

  useEffect(() => {
    if (!connection) {
      return;
    }
    connection.on("PlayerAnswered", () => {
      setPlayersAnswered(playersAnswered + 1);
    });    
  }, [connection]);

  if (!connection || !connected) {
    return <div>Connecting...</div>;
  }
  return <AdminPanel connection={connection} playersAnswered={playersAnswered} />;
};
export default App;
