import React from 'react';
import type {Question} from "../api/Types";
interface QuestionDisplayProps {
    question: Question;
    onEndRound: () => void;
    playersAnswered: number;
}

const QuestionDisplay: React.FC<QuestionDisplayProps> = ({ question, onEndRound, playersAnswered }) => {
    return (
        <div>
            <h3>Question</h3>
            <p>{question.questionText}</p>
            <p>Players answered: {playersAnswered}</p>
            <button
                onClick={() => {
                    onEndRound();
                }}
            >
                End Round
            </button>
        </div>
    );
};

export default QuestionDisplay;
