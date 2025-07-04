import React, {useState} from 'react';
import QuestionDisplay from './QuestionDisplay';
import Leaderboard from './Leaderboard';

interface AdminPanelProps {
    playersAnswered: number;
    onNextQuestion: () => void;
}

const questions = ["what is dog"];

const AdminPanel: React.FC<AdminPanelProps> = ({playersAnswered, onNextQuestion}) => {
    const [currentQuestion, setCurrentQuestionQuestion] = useState(0);
    const [showLeaderboard, setShowLeaderboard] = useState(false);

    return (
        <div>
            <h2>Admin Panel</h2>
            <h3>Players answered: {playersAnswered}</h3>
            {!showLeaderboard ? (
                <QuestionDisplay question={questions[currentQuestion]}/>
            ) : (
                <Leaderboard/>
            )}
            <button
                onClick={() => {
                    onNextQuestion();
                    setShowLeaderboard(false);
                    setCurrentQuestionQuestion((prev) => prev + 1);                    
                }}
            >
                Next Question
            </button>
            <button onClick={() => setShowLeaderboard(true)}>Show Leaderboard</button>
        </div>
    );
};

export default AdminPanel;