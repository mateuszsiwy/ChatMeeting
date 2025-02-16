import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';

const createSignalRConnection = (token) => {
    const connection = new HubConnectionBuilder()
        .withUrl('https://localhost:7099/messageHub', {
            accessTokenFactory: () => token,
        })
        .configureLogging(LogLevel.Information)
        .build();

    return connection;
};

export default createSignalRConnection;
