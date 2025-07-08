import React from 'react';

interface LobbyProps {
    onCreateGame: () => void;
    onStartGame: () => void;
    roomcode: string;
}

const Lobby: React.FC<LobbyProps> = ({onStartGame, onCreateGame, roomcode}) => {
    return (
        <div>
            <div>
                Welcome to lobby!
                Room Code: {roomcode}
            </div>
            <button onClick={onCreateGame}>Create Game</button>
            <button onClick={onStartGame}>Start Game</button>
        </div>
    );
};

export default Lobby;