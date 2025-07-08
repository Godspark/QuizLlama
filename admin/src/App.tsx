import React, { useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";
import type {Question} from "./api/Types";
import Lobby from "./views/Lobby";
import QuestionDisplay from "./views/QuestionDisplay";
import RoundResults from "./views/RoundResults";
// import Leaderboard from "./views/Leaderboard";

const App: React.FC = () => {
  const [connection, setConnection] = useState<signalR.HubConnection | null>(
    null
  );
  const [connected, setConnected] = useState(false);
  const [roomcode, setRoomcode] = useState("");
  const [playersAnsweredCounter, setplayersAnsweredCounter] = useState(0);   
  const [currentQuestion, setCurrentQuestion] = useState<Question | null>(null);
  const [showLobby, setShowLobby] = useState(true);
  const [showQuestion, setShowQuestion] = useState(false);
  // const [showRoundLeaderboard, setShowRoundLeaderboard] = useState(false);
  const [showRoundResults, setShowRoundResults] = useState(false);
  
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
    connection.on("GameCreated", (roomcode: string) => {
      setRoomcode(roomcode);
      setShowLobby(true);
      setShowQuestion(false);
      setShowRoundResults(false);
    });
  }, [connection]);

  useEffect(() => {
    if (!connection) {
      return;
    }
    connection.on("GameStarted", (question: Question) => {
      console.log(GameStarted);
      setplayersAnsweredCounter(0);
      setCurrentQuestion(question);
      setShowLobby(false);
      setShowQuestion(true);
      setShowRoundResults(false);
      console.log(showQuestion);
    });
  }, [connection]);

  useEffect(() => {
    if (!connection) {
      return;
    }
    connection.on("ReceiveQuestion", (question: Question) => {
      setplayersAnsweredCounter(0);
      setCurrentQuestion(question);      
      setShowQuestion(true);
      setShowRoundResults(false);
    });
  }, [connection]);
  
  useEffect(() => {
    if (!connection) {
      return;
    }
    connection.on("UpdatePlayersAnsweredCounter", (playersAnswered: number) => {
      setplayersAnsweredCounter(playersAnswered);
    });
  }, [connection]);
  
  useEffect(() => {
    if (!connection) {
      return;
    }
    connection.on("RoundEnded", () => {      
      setShowQuestion(false);
      setShowRoundResults(true);
    });
  }, [connection]);

  const createGame = async () => {
    if (!connection) {
      return;
    }
    try {
      console.log('createGame');
      await connection.invoke('CreateGame');
    } catch (err) {
      console.error('Admin: SignalR error on CreateGame:', err);
    }
  };
  
  const startGame = async () => {
    if (!connection) {
      return;
    }
    try {
      console.log('startGame');
      await connection.invoke('StartGame');
    } catch (err) {
      console.error('Admin: SignalR error on StartGame:', err);
    }
  };
  
  const nextQuestion = async () => {
    if (!connection) {
      return;
    }
    try {
      console.log('NextQuestion');
      await connection.invoke('NextQuestion');
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
  if (showLobby) {
    return <Lobby onStartGame={startGame} onCreateGame={createGame} roomcode={roomcode} />;
  }
  if (showQuestion && currentQuestion !== null) {
      return <QuestionDisplay question={currentQuestion} onEndRound={endRound} playersAnswered={playersAnsweredCounter} />;
  }
  if (showRoundResults) {
    return <RoundResults onNextQuestion={nextQuestion} />;
  }
  // if (showRoundLeaderboard) {
  //   return <Leaderboard />;
  // }
  
};
export default App;
