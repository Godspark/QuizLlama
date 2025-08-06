import React from "react";
import {type Guess, QuestionType} from "./api/Types";

interface QuizUIProps {
  question: string;
  options: string[];
  type: QuestionType;
  onAnswerSelect: (guess: Guess) => void;
}

const QuizUI: React.FC<QuizUIProps> = ({
  question,
  options,
  type,
  onAnswerSelect,
}) => (
  <div className="quiz-container">
    <h1>Quiz Llama</h1>
    <div className="question-container">
      <h2>{question}</h2>
      <h3>{QuestionType[type]}</h3>
      <div className="options-container">
        {options.map((option, index) => (
          <button
            key={index}
            className="option-button"
            onClick={() => {
              const guess: Guess = { multipleChoiceIndex: index };
              onAnswerSelect(guess)
            }}
          >
            {option}
          </button>
        ))}
      </div>
    </div>
  </div>
);

export default QuizUI;
