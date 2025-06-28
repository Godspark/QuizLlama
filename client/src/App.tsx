import { useEffect, useState } from 'react'
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr'
import QuizUI from './QuizUI'
import { QuestionType } from './Questions/QuestionType'
import './App.css'

function App() {
    const [connection, setConnection] = useState<HubConnection | null>(null)
    const [question, setQuestion] = useState<string>('Waiting for question...')
    const [options, setOptions] = useState<string[]>([])
    const [type, setType] = useState<QuestionType | null>(null)

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

                    connection.on('ReceiveQuestion', (question: Question) => {
                        setQuestion(question)
                        setOptions(options)
                        setType(type)
                    })
                })
                .catch(err => console.error('Error connecting to Quiz Hub:', err))
        }
    }, [connection])

    const handleAnswerSelect = (answer: string) => {
        console.log('Selected answer:', answer)
        try {
            if (!connection) {
                console.error('Connection is not established.');
                return;
            }
            connection.invoke('SubmitAnswer', "player", answer);
        } catch (err) {
            console.error('SignalR error:', err);
        }
    }

    return (
        <QuizUI
            question={question}
            options={options}
            type={type}
            onAnswerSelect={handleAnswerSelect}
        />
    )
}

export default App