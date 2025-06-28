import { useEffect, useState } from 'react'
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr'
import './App.css'

function App() {
    const [connection, setConnection] = useState<HubConnection | null>(null)
    const [question, setQuestion] = useState<string>('Waiting for question...')
    const [options, setOptions] = useState<string[]>([])

    useEffect(() => {
        const newConnection = new HubConnectionBuilder()
            .withUrl('http://100.105.115.119:5114/quizhub')
            .withAutomaticReconnect()
            .build()

        setConnection(newConnection)

        return () => {
            newConnection.stop()
        }
    }, [])

    useEffect(() => {
        if (connection) {
            connection.start()
                .then(() => {
                    console.log('Connected to Quiz Hub!')

                    connection.on('ReceiveQuestion', (question: string, options: string[]) => {
                        setQuestion(question)
                        setOptions(options)
                    })
                })
                .catch(err => console.error('Error connecting to Quiz Hub:', err))
        }
    }, [connection])

    const handleAnswerSelect = (answer: string) => {
        console.log('Selected answer:', answer)
        // TODO: Implement sending answer back to server
    }

    
    return (
        <div className="quiz-container">
            <h1>Quiz Llama</h1>
            <div className="question-container">
                <h2>{question}</h2>
                <div className="options-container">
                    {options.map((option, index) => (
                        <button
                            key={index}
                            className="option-button"
                            onClick={() => handleAnswerSelect(option)}
                        >
                            {option}
                        </button>
                    ))}
                </div>
            </div>
        </div>
    )
}

export default App
