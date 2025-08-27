import React from 'react';
import type {Scoreboard} from "../api/Types.ts";

interface LeaderboardProps {
    scoreboard: Scoreboard | null;
}

const Leaderboard: React.FC<LeaderboardProps> = ({scoreboard}) => {
    return (
        <div>
            <h3>Leaderboard</h3>
            {scoreboard && <pre>{JSON.stringify(scoreboard, null, 2)}</pre>}
            <ul>
                <li>Player 1 - 100 pts</li>
                <li>Player 2 - 80 pts</li>
                <li>Player 3 - 60 pts</li>
            </ul>
        </div>
    );
};

export default Leaderboard;