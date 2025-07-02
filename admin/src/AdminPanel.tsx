import React, { useState, useEffect } from 'react';
import * as signalR from '@microsoft/signalr';
import { HubConnection } from '@microsoft/signalr';
import QuestionDisplay from './QuestionDisplay';
import Leaderboard from './Leaderboard';

interface AdminPanelProps {
    connection: signalR.HubConnection;
}

const questions = ["what is dog"];

const AdminPanel: React.FC<AdminPanelProps> = ({ connection }) => {
    const [current, setCurrent] = useState(0);
    const [showLeaderboard, setShowLeaderboard] = useState(false);

    const next = async () => {
        console.log("next");
        console.log(connection.state);
        setShowLeaderboard(false);
        setCurrent((prev) => prev + 1);
        try {
            console.log('NextQuestion');
            await connection.invoke('NextQuestion');
        } catch (err) {
            console.error('SignalR error:', err);
        }
    };

    return (
        <div>
            <h2>Admin Panel</h2>
            {!showLeaderboard ? (
                <QuestionDisplay question={questions[current]} />
            ) : (
                <Leaderboard />
            )}
            <button onClick={next}>Next Question</button>
            <button onClick={() => setShowLeaderboard(true)}>Show Leaderboard</button>
        </div>
    );
};

export default AdminPanel;