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
      setPlayersAnswered(prev => prev + 1);
    });    
  }, [connection]);

  const nextQuestion = async () => {
    if (!connection) {
      return;
    }
    try {
      console.log('NextQuestion');
      await connection.invoke('NextQuestion');
      setPlayersAnswered(0); // Bør sikkert vente på ack fra serveren
    } catch (err) {
      console.error('Admin: SignalR error on NextQuestion:', err);
    }
  };

  const endRound = async () => {
    if (!connection) {
      return;
    }
    try {
      console.log('EndRound');
      await connection.invoke('EndRound');
    } catch (err) {
      console.error('Admin: SignalR error on EndRound:', err);
    }
  };
  
  if (!connection || !connected) {
    return <div>Connecting...</div>;
  }
  return <AdminPanel onEndRound={endRound} onNextQuestion={nextQuestion} playersAnswered={playersAnswered} />;
};
export default App;
