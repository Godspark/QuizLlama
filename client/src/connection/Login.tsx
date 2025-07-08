import React, { useState } from "react";

interface LoginProps {
    onJoinGame: (roomcode: string, nickname: string) => void;
}

const Login: React.FC<LoginProps> = ({ onJoinGame }) => {
    const [nickname, setNickname] = useState("");
    const [roomcode, setRoomcode] = useState("");

    return (
        <div className="quiz-container">
            <h1>Quiz Llama</h1>
            <p>Login</p>
            <p>Nickname</p>
            <input
                type="text"
                placeholder="Enter your nickname"
                value={nickname}
                onChange={e => setNickname(e.target.value)}
            />
            <p>Room Code</p>
            <input
                type="text"
                placeholder="Enter room code"
                value={roomcode}
                onChange={e => setRoomcode(e.target.value)}
            />
            <button
                onClick={() => {
                    if (nickname && roomcode) {
                        onJoinGame(roomcode, nickname);
                    } else {
                        alert("Please enter both nickname and room code.");
                    }
                }} />
        </div>
    );
}

export default Login;
