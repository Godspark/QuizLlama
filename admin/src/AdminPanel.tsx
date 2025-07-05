import React, {useState} from 'react';
import QuestionDisplay from './QuestionDisplay';
import Leaderboard from './Leaderboard';

interface AdminPanelProps {
    playersAnswered: number;
    onNextQuestion: () => void;
    onEndRound: () => void;
}

const questions = ["what is dog"];

const AdminPanel: React.FC<AdminPanelProps> = ({playersAnswered, onNextQuestion, onEndRound}) => {
    const [currentQuestion, setCurrentQuestion] = useState(0);
    const [showLeaderboard, setShowLeaderboard] = useState(false);
    const [showRoundResults, setShowRoundResults] = useState(false);

    return (
        <div>
            <h2>Admin Panel</h2>
            <h3>Players answered: {playersAnswered}</h3>
            {!showLeaderboard ? (
                <QuestionDisplay question={questions[currentQuestion]}/>
            ) : (
                <Leaderboard/>
            )}
            {showRoundResults && <div>Round Results Placeholder</div>}
            <button
                onClick={() => {
                    onNextQuestion();
                    setShowLeaderboard(false);
                    setShowRoundResults(false);
                    setCurrentQuestion((prev) => prev + 1);                    
                }}
            >
                Next Question
            </button>
            <button
                onClick={() => {
                    onEndRound();
                    setShowLeaderboard(false);
                    setShowRoundResults(true);
                }}
            >
                End Round
            </button>
            <button 
                onClick={() => {
                    setShowLeaderboard(true);
                    setShowRoundResults(true);  
                }}
            >
                Show Leaderboard
            </button>
        </div>
    );
};

export default AdminPanel;