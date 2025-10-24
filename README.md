# Local AI Assistant (ASP.NET MVC + Ollama + Gemma)

A **local, privacy-first AI chat assistant** built with **ASP.NET MVC**, **Ollama**, and **Gemma**.  
No cloud APIs. No telemetry. 100% offline.

> **See the full article here →** [Running LLMs Locally for Privacy, Control, and Cost Efficiency](https://www.linkedin.com/pulse/running-llms-locally-privacy-control-cost-efficiency-masoud-imanpour-y3sme/)

---

## Overview

This project integrates **.NET MVC** with **Ollama** to create a fully local AI assistant, a private, self-hosted LLM for developers.

**Key Features**
- Text-based chat with local LLMs (Gemma:2b)
- PDF upload and text extraction (via PdfPig)
- Layered architecture (Controller → Services → Bridge)
- No external dependencies or API keys

---

## ⚠️ Educational Purpose Only

This project is for **educational and demonstration purposes**.  
It’s a **sample implementation**, not production-ready software.

Before using or modifying this in a real environment:

- Review all **Ollama** and **Gemma** documentation  
- Understand relevant **policies**, **licenses**, and **model behavior**  
- Perform your own **research and validation**  
- Comply with **organizational**, **legal**, and **ethical standards**  
- You are responsible for verifying all tools, dependencies, and models before using them  

**Note:** The author provides **no warranties or guarantees**.  
Use this code and associated models **at your own risk**.

---

## Architecture

&emsp;&emsp;&emsp;Frontend (HTML + JS)
<br>&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;↓<br>

&emsp;&emsp;ASP.NET MVC API (Controller)
<br>&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;↓<br>

Service Layer (ChatService, GemmiModelService)
<br>&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;↓<br>

&emsp;&emsp;Bridge Layer (OllamaBridgeService)
<br>&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;↓

&emsp;&emsp;Ollama (Gemma:2b running locally)

Each layer is independent, testable, and replaceable.

---

## Setup

**1. Run Ollama from Docker**
<br>
**2. Run the ASP.NET MVC App**

## Project Structure
```bash
Controllers/        → ChatController
Services/           → ChatService, FileService, GemmiModelService, OllamaBridgeService
Dtos/               → Request/Response DTOs
wwwroot/            → Frontend (HTML + JS)
```

## Tech Stack
```bash
- .NET 8 / ASP.NET MVC

- Ollama (local model runtime)

- Gemma:2b (LLM)

- PdfPig (PDF parsing)

- Dependency Injection / Logging
```

## Privacy
All data stays on your machine.
No network calls, cloud APIs, or external telemetry.

## Author:
Masoud. Software Engineer, Vancouver 🇨🇦


