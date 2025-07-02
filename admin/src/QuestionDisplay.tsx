import React from 'react';
interface QuestionDisplayProps {
    question: string;
}

const QuestionDisplay: React.FC<QuestionDisplayProps> = ({ question }) => (
    <div>
        <h3>Question</h3>
        <p>{question}</p>
    </div>
);

export default QuestionDisplay;