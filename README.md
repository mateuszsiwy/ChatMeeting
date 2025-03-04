# Chat Meeting - Real-time Chat Application

## Overview
Chat Meeting is a real-time chat application built using the .NET framework with SignalR for real-time communication. The frontend is developed using React.js, and the application utilizes Apache Kafka for message streaming, hosted in a Docker environment. The database is powered by SQL Server, deployed on Microsoft Azure.

The backend follows **Onion Architecture**, ensuring a clean separation of concerns, maintainability, and testability. The application is built in accordance with **SOLID** principles to enhance scalability and maintainability.

## Technologies Used
- **Backend:** .NET, SignalR  
- **Frontend:** React.js  
- **Architecture:** Onion Architecture, SOLID Principles  
- **Message Streaming:** Apache Kafka (Docker)  
- **Database:** SQL Server (Azure)  

## Features
- ✅ Real-time chat with SignalR  
- ✅ Scalable message handling using Kafka  
- ✅ Responsive and interactive UI built with React.js  
- ✅ Secure and scalable data storage with SQL Server on Azure  
- ✅ Clean and maintainable code with **Onion Architecture**  
- ✅ Follows **SOLID** principles for better software design  

## API Endpoints

### Frontend:
#### Registration:
- **Type:** REST `PUT`  
- **URL:** `/api/Auth/register`  
- **Body:**
  ```json
  {
    "username": "string",
    "password": "string"
  }
  ```
- **Response:**
  - `200 OK` (no content)

#### Login:
- **Type:** REST `POST`  
- **URL:** `/api/Auth/login`  
- **Body:**
  ```json
  {
    "username": "string",
    "password": "string"
  }
  ```
- **Response:**
  - `200 OK`
    ```json
    {
      "Token": "string",
      "ExpiredDate": "DateTime"
    }
    ```
  - `4xx Error`

#### Chat Initialization:
- **Type:** REST `POST`  
- **URL:** `/api/Chat/GetPaginatedChat?chatName=name&pageNumber=1&pageSize=20`  
- **Body:** No content  
- **Response:**
  ```json
  {
    "Id": "Guid",
    "Name": "string",
    "Messages": [
      {
        "MessageId": "Guid",
        "Sender": "string",
        "MessageText": "string",
        "ChatId": "Guid",
        "CreatedAt": "string",
        "SenderId": "Guid"
      }
    ]
  }
  ```

### WebSocket Endpoints:
#### Message Listening Initialization:
- **Type:** WebSocket  
- **URL:** `/messageHub?token={token}`  

#### Receiving Messages:
- **Type:** WebSocket  
- **Receiver:** `ReceiveMessage`  
- **Params:**
  ```json
  {
    "user": "string",
    "message": "string",
    "chat": "string"
  }
  ```

#### Sending Messages:
- **Type:** WebSocket  
- **Listener:** `SendMessageToChat`  
- **Params:**
  ```json
  {
    "chatName": "string",
    "message": "string"
  }
  ```
