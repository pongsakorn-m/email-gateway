# Email Gateway Service (.NET)

A production-minded Email Gateway API built with .NET, designed to demonstrate senior backend engineering practices such as idempotency, background processing, retry handling, and containerized infrastructure.

This project is intended as a portfolio showcase and focuses on correct system design, not just feature completeness.

---

## Key Features

- Idempotent Email API
  - Prevents duplicate emails using the Idempotency-Key HTTP header
- Asynchronous Email Sending
  - Requests are accepted immediately and processed in the background
- Background Worker
  - Retry handling
  - Failure handling
- Clean Architecture
  - Domain / Application / Infrastructure separation
- Docker-first Setup
  - One command to run everything
- MailHog Integration
  - Safe local email testing
- PostgreSQL Persistence
  - EF Core + migrations

---

## Architecture Overview

EmailGateway.Api  
│  
├── EmailGateway.Domain  
│   └── Core business entities & rules  
│  
├── EmailGateway.Application  
│   └── Use cases, interfaces, business logic  
│  
├── EmailGateway.Infrastructure  
│   ├── EF Core (PostgreSQL)  
│   ├── Background Worker  
│   └── Email sender implementation  
│  
└── docker-compose.yaml  

This structure keeps business logic independent from infrastructure concerns, making the system easier to test, maintain, and extend.

---

## How to Run Locally

### Prerequisites

- Docker
- Docker Compose (v2+)

### Start the system

docker compose up --build

What happens automatically:

- PostgreSQL starts and waits until healthy
- EF Core migrations are applied automatically
- API starts only after the database is ready
- Background worker begins processing emails

Swagger UI will be available at:

http://localhost:8080/swagger

---

## Sending an Email

### Endpoint

POST http://localhost:8080/api/emails

### Required Header

Idempotency-Key: <unique-value>

### Request Body

{
  "to": "test@example.com",
  "template": "welcome"
}

### Example curl command

curl -X POST http://localhost:8080/api/emails \
  -H "Content-Type: application/json" \
  -H "Idempotency-Key: email-test-001" \
  -d '{
    "to": "test@example.com",
    "template": "welcome"
  }'

Sending the same request again with the same Idempotency-Key will not send a duplicate email.

---

## Viewing Emails (MailHog)

MailHog is used for local email testing.

- Web UI: http://localhost:8025
- SMTP server: mailhog:1025

All outgoing emails can be viewed safely without sending real emails.

---

## Idempotency Design

Email sending is not idempotent by default.  
Clients may retry requests due to network issues or timeouts.

To prevent duplicate emails:

- The API requires an Idempotency-Key HTTP header
- Requests with the same key are treated as the same logical operation
- Duplicate sends are prevented at the persistence level

---

## Database & Migrations

- Database: PostgreSQL
- ORM: EF Core
- Migrations are applied automatically on application startup
- Docker volumes are used to persist data

### Reset the database completely

docker compose down -v  
docker compose up --build

---

## Author

Built by Ton  
Full-stack developer with a focus on backend systems, clean architecture, and production readiness.
