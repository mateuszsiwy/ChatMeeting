import React, { useEffect, useState } from 'react';
import createSignalRConnection from './services/signalRService';

const ChatComponent = () => {
    const [connection, setConnection] = useState(null);
    const [messages, setMessages] = useState([]);
    const [message, setMessage] = useState('');

    useEffect(() => {
        const token = localStorage.getItem("token"); 

        if (!token) {
            console.error("Brak tokena, Użytkownik musi się zalogować.");
            return;
        }

        const newConnection = createSignalRConnection(token);

        newConnection.start()
            .then(() => {
                console.log(" Połączono z SignalR!");

                newConnection.on("ReceiveMessage", (user, message) => {
                    setMessages((prevMessages) => [...prevMessages, { user, message }]);
                });
            })
            .catch((err) => console.error(" Błąd połączenia: ", err));

        setConnection(newConnection);

        return () => {
            if (newConnection) {
                newConnection.stop();
            }
        };
    }, []);


    const sendMessage = async () => {
        if (connection) {
            try {
                await connection.invoke('SendMessageToChat', localStorage.getItem("username"), message);
                setMessage('');
            } catch (err) {
                console.error('Error sending message: ', err);
            }
        }
    };

    return (
        <div>
            <div>
                {messages.map((msg, index) => (
                    <div key={index}>
                        <strong>{msg.user}</strong>: {msg.message}
                    </div>
                ))}
            </div>
            <input
                type="text"
                value={message}
                onChange={(e) => setMessage(e.target.value)}
            />
            <button onClick={sendMessage}>Send</button>
        </div>
    );
};

export default ChatComponent;
