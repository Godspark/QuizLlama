import React from 'react';

interface LobbyProps {
    onStartGame: () => void;
}

const Lobby: React.FC<LobbyProps> = ({onStartGame}) => {
    return (
        <div>
            <div>
                Welcome to lobby!
            </div>
            <button onClick={onStartGame}>Start Game</button>
        </div>
    );
};

export default Lobby;