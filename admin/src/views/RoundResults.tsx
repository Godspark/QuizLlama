import React from "react";

interface RoundResultProps {
    onNextQuestion: () => void;
}

const RoundResult: React.FC<RoundResultProps> = ({onNextQuestion}) => {
    return (
        <div>
            <p>Round results will be displayed here.</p>
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
