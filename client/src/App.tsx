import React, { useEffect, useState } from "react";
import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import QuizUI from "./QuizUI";
import Login from "./connection/Login";
import "./App.css";
import type {
  Guess,
  MultipleChoiceAlternative,
  MultipleChoiceQuestion,
  Question, 
  Solution,
  TrueFalseQuestion,
  TypeAnswerQuestion
} from "./api/Types";
import { QuestionType, Correctness } from "./api/Types";

const App: React.FC = () => {
  const [connection, setConnection] = useState<HubConnection | null>(null);
  const [hasJoined, setHasJoined] = useState(false);
  const [question, setQuestion] = useState<Question | null>(null);
  const [options, setOptions] = useState<string[]>([]);
  const [type, setType] = useState<QuestionType | undefined>(undefined);
  const [hasAnswered, setHasAnswered] = useState(false);
  const [showRoundResults, setShowRoundResults] = useState(false);
  const [correctness, setCorrectness] = useState<Correctness | null>(null);
  const [correctAnswers, setCorrectAnswers] = useState("");
  const [score, setScore] = useState(0);

  const handleReceiveQuestion = (question: Question) => {
    setHasAnswered(false);
    setShowRoundResults(false);
    switch (question.questionType) {
      case QuestionType.MultipleChoice: {
        const mcq = question as MultipleChoiceQuestion;
        setQuestion(mcq);
        setOptions(
            mcq.alternatives
                ?.map((alt: MultipleChoiceAlternative) => alt.text)
                .filter((text): text is string => text != null) || []
        );
        break;
      }
      case QuestionType.TrueFalse: {
        const tf = question as TrueFalseQuestion;
        setQuestion(tf);
        setOptions(["True", "False"]);
        break;
      }
      case QuestionType.TypeAnswer: {
        const ta = question as TypeAnswerQuestion;
        setQuestion(ta);
        setOptions(["Type your answer here..."]);
        break;
      }
      default:
        setOptions(["dummy1", "dummy2", "dummy3", "dummy4"]);
    }
    setType(question.questionType);
  };
  
  
  useEffect(() => {
    const newConnection = new HubConnectionBuilder()
      .withUrl("http://localhost:5114/quizhub")
      .withAutomaticReconnect()
      .build();

    setConnection(newConnection);

    return () => {
      newConnection.stop();
    };
  }, []);

  useEffect(() => {
    if (connection) {
      connection
        .start()
        .then(() => {
          console.log("Connected to Quiz Hub!");  
        })
        .catch((err) => console.error("Error connecting to Quiz Hub:", err));
    }
  }, [connection]);

  useEffect(() => {
    if (!connection) {
      return;
    }
    connection.on("GameStarted", handleReceiveQuestion);
    connection.on("ReceiveQuestion", handleReceiveQuestion);
  }, [connection]);
  
  useEffect(() => {
    if (!connection) {
      return;
    }
    connection.on("GameJoined", () => {
      console.log("GameJoined");
      setHasJoined(true);
    });
  }, [connection]);

  useEffect(() => {
    if (!connection) {
      return;      
    }
    connection.on("AnswerReceived", () => {
      setHasAnswered(true);
    });
  }, [connection]);

  useEffect(() => {
    if (!connection) {
      return;
    }
    connection.on("RoundEnded", (correctAnswers: Solution, correctness: Correctness, score: number) => {
      console.log(correctAnswers.multipleChoiceSolutionIndices);
      setCorrectAnswers(
          correctAnswers.multipleChoiceSolutionIndices
          ? correctAnswers.multipleChoiceSolutionIndices.join(", ")
          : ""
      );
      setCorrectness(correctness);
      setScore(score);
      setShowRoundResults(true);
    });
  }, [connection]);

  const joinGame = (roomcode: string, nickname: string) => {
    console.log("Joining game");
    try {
      if (!connection) {
        console.error("Connection is not established.");
        return;
      }
      connection.invoke("JoinGame", roomcode, nickname);
    } catch (err) {
      console.error("SignalR error:", err);
    }
  };
  
  const handleAnswerSelect = (guess: Guess) => {
    console.log("Selected guess:", guess);
    try {
      if (!connection) {
        console.error("Connection is not established.");
        return;
      }
      connection.invoke("SubmitAnswer", guess);
    } catch (err) {
      console.error("SignalR error:", err);
    }
  };

  if (showRoundResults) {
    return (
    <div>
      <p>Correctness: {correctness}</p>
      <p>Correct answer(s): {correctAnswers}</p>
      <p>Score: {score}</p>
    </div>);
  }
  if (hasAnswered) {
    return <div>I hope it was the right answer!</div>
  }
  return (
      !hasJoined ?
          <Login onJoinGame={joinGame}/> :
          <QuizUI
              question={question?.questionText ?? "Waiting for question..."}
              options={options}
              type={type!}
              onAnswerSelect={handleAnswerSelect}
          />
  );
}

export default App;
