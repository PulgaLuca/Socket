# 3. Pub/Sub (Mini Broker)

## Overview

A **publish/subscribe messaging system** with a central broker.
Clients can subscribe to topics and receive messages asynchronously.

## Protocol

```
SUB|topic
UNSUB|topic
PUB|topic|message
MSG|topic|message
```

## Features

* Topic-based messaging
* Decoupled communication
* Multiple subscribers per topic
* Real-time message delivery

## How to Run

### Start Broker

```bash
cd Broker
dotnet run
```

### Start Client(s)

```bash
cd Client
dotnet run
```

## Commands

```
/sub topic
/unsub topic
/pub topic message
```

## How It Works

* Clients subscribe to topics
* Publishers send messages to topics
* Broker routes messages to subscribers
