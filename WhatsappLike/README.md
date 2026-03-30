# 1. Professional Chat with Custom Protocol

## Overview

This project implements a **client-server chat system** with a **custom text-based protocol**.
It supports multiple users, structured messaging, and event notifications (join/leave).

## Protocol

```
LOGIN|username
MSG|message
JOIN|username
LEAVE|username
```

## Features

* Multi-client support
* Broadcast messaging
* Structured protocol parsing
* Join/leave notifications
* Clean separation (Server / Client / Protocol)

## How to Run

### Start Server

```bash
cd ChatServer
dotnet run
```

### Start Client(s)

```bash
cd ChatClient
dotnet run
```

## How It Works

1. Client sends `LOGIN`
2. Server notifies others with `JOIN`
3. Messages are sent via `MSG`
4. On disconnect → `LEAVE` is broadcast

---