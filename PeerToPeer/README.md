# 2. Peer-to-Peer Chat (P2P)

## Overview

A **fully decentralized chat system** where each node acts as both:

* server (accepts connections)
* client (connects to peers)

No central server is required.

## Protocol

```
HELLO|username
MSG|username|message
```

## Features

* Distributed architecture
* Direct peer connections
* Dynamic network topology
* Multiple connections per node

## How to Run

```bash
dotnet run
```

Then:

```
Username: Alice
Port: 5000
```

Connect to another peer:

```
/connect 127.0.0.1 5001
```

Send message:

```
/msg Hello
```

## How It Works

* Each instance listens on a port
* Peers connect manually
* Messages are broadcast to all connected peers
