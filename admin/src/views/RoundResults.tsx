import React from "react";
import type {AnswerDistribution, Solution} from "../api/Types.ts";

interface RoundResultProps {
    onNextQuestion: () => void;
    correctAnswers: Solution | null;
    answerDistribution: AnswerDistribution | null;
}

const RoundResult: React.FC<RoundResultProps> = ({onNextQuestion, correctAnswers, answerDistribution}) => {
    return (
        <div>
            <p>Round results will be displayed here.</p>
            <p>Answer distribution:</p>
            {answerDistribution && Object.entries(answerDistribution.multipleChoiceDistribution ?? {}).map(
                ([key, value]) => (
                    <div key={key}>
                        {key}: {value}
                    </div>
                )
            )}
            <p>Correct answers:</p>
            {correctAnswers?.multipleChoiceSolutionIndices?.map((index, i) => (
                <div key={i}>{index}</div>
            ))}


            <button
                onClick={() => {
                    onNextQuestion();
                }}
            >
                Next Question
            </button>
        </div>
    );
};
export default RoundResult;
