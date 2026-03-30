# 4. Tic-Tac-Toe Multiplayer

## Overview

A **multiplayer Tic-Tac-Toe game** using a **server-authoritative architecture**.
The server manages the game state and validates moves.

## Protocol

```
START|X/O
MOVE|row|col
UPDATE|board
WIN|player
DRAW
ERROR|message
```

## Features

* Two-player game
* Server-side validation
* Real-time state synchronization
* Turn-based logic

## How to Run

### Start Server

```bash
cd GameServer
dotnet run
```

### Start Two Clients

```bash
cd GameClient
dotnet run
```

## How It Works

1. Server assigns players (X and O)
2. Clients send moves (`MOVE`)
3. Server validates and updates board
4. Server broadcasts `UPDATE`
5. On win → `WIN` message sent
