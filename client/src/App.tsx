import { useEffect, useState } from 'react'
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr'
import QuizUI from './QuizUI'
import './App.css'
import { QuestionType, MultipleChoiceQuestion, TrueFalseQuestion, TypeAnswerQuestion, MultipleChoiceAlternative } from './api/Types'

function App() {
    const [connection, setConnection] = useState<HubConnection | null>(null)
    const [question, setQuestion] = useState<Question>(null)
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
                        switch (question.questionType) {
                            case QuestionType.MultipleChoice:
                                const mcq = question as MultipleChoiceQuestion;
                                setQuestion(mcq);
                                setOptions(mcq.alternatives?.map((alt: MultipleChoiceAlternative) => alt.text) || []);
                                break;
                            case QuestionType.TrueFalse:
                                const tf = question as TrueFalseQuestion;
                                setQuestion(tf);
                                setOptions(["True", "False"]);
                                break;
                            case QuestionType.TypeAnswer:
                                const ta = question as TypeAnswerQuestion;
                                setQuestion(ta);
                                setOptions(["Type your answer here..."]);
                                break;
                            default:
                                setOptions(["dummy1", "dummy2", "dummy3", "dummy4"]);
                        }
                        setType(question.questionType);
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
            question={question?.questionText ?? 'Waiting for question...'}
            options={options}
            type={type}
            onAnswerSelect={handleAnswerSelect}
        />
    )
}

export default App