import React from "react";
import {QuestionType, type MultipleChoiceAnswer, type Answer} from "./api/Types";

interface QuizUIProps {
  question: string;
  options: string[];
  type: QuestionType;
  onAnswerSelect: (answer: Answer) => void;
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
              const answer: MultipleChoiceAnswer = { selectedAlternativeIndex: index };
              onAnswerSelect(answer)
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
