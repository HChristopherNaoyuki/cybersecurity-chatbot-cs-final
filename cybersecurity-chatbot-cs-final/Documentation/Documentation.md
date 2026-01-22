# Cybersecurity Chatbot - Documentation

## Table of Contents
- [Overview](#overview)
- [Project Structure](#project-structure)
- [Features](#features)
- [System Architecture](#system-architecture)
- [Installation](#installation)
- [Usage](#usage)
- [Technical Specifications](#technical-specifications)
- [Disclaimer](#disclaimer)

## Overview

The Cybersecurity Chatbot is a Windows desktop application 
built using C# 7.3 and WPF (Windows Presentation Foundation). 
This educational tool provides interactive cybersecurity guidance, 
helping users learn about online safety, threat prevention, and 
security best practices through a conversational interface.

The application combines natural language processing with a 
comprehensive knowledge base to deliver personalized cybersecurity 
advice. It features persistent user memory, interactive quizzes, 
task management, and detailed activity logging to create an engaging 
learning environment.

## Project Structure

```
cybersecurity-chatbot-cs-final/
├── App.xaml
├── App.xaml.cs
├── ChatBot.cs
├── ConversationManager.cs
├── Converters.cs
├── KnowledgeBase.cs
├── LogWindow.xaml
├── LogWindow.xaml.cs
├── MainWindow.xaml
├── MainWindow.xaml.cs
├── MemoryManager.cs
├── NameInputDialog.xaml
├── NameInputDialog.xaml.cs
├── QuizWindow.xaml
├── QuizWindow.xaml.cs
├── TaskWindow.xaml
├── TaskWindow.xaml.cs
├── UserInterface.cs
├── Documentation/
│   └── Documentation.md
├── Audio/
│   └── welcome.wav
└── UserData/
    └── [User-Specific Data]
```

### Core Components

**Frontend Layer:**
- `MainWindow.xaml/.cs` - Primary chat interface
- `LogWindow.xaml/.cs` - Activity history viewer
- `QuizWindow.xaml/.cs` - Interactive quiz interface
- `TaskWindow.xaml/.cs` - Task management system
- `NameInputDialog.xaml/.cs` - User name input dialog
- `Converters.cs` - WPF data converters for UI styling

**Business Logic Layer:**
- `ChatBot.cs` - Main controller coordinating all components
- `ConversationManager.cs` - Natural language processing engine
- `KnowledgeBase.cs` - Cybersecurity information repository
- `MemoryManager.cs` - Persistent user data management
- `UserInterface.cs` - UI interaction handler

**Supporting Files:**
- `App.xaml/.cs` - Application entry point
- `Audio/welcome.wav` - Welcome greeting sound
- `UserData/` - Persistent storage directory

## Features

### Conversational Interface

The chatbot provides a responsive chat interface with distinct visual 
styling for user and bot messages. Users can ask questions about cybersecurity 
topics and receive immediate, context-aware responses.

### Knowledge Base

Contains comprehensive cybersecurity information covering essential topics 
including password security, phishing prevention, malware protection, VPN usage, 
WiFi security, two-factor authentication, encryption, and privacy practices.

### Interactive Quiz System

A 10-question cybersecurity quiz tests user knowledge with instant feedback. 
Questions are designed to reinforce key concepts from common security scenarios.

### Task Management

Users can create and manage cybersecurity-related tasks with completion tracking. 
Tasks persist between sessions and help users implement security recommendations.

### Persistent Memory

The application remembers user preferences, conversation history, and frequently 
discussed topics across sessions. User data is stored locally in structured text 
files.

### Activity Logging

Comprehensive logging tracks all user interactions, quiz attempts, task management, 
and system events for review and analysis.

## System Architecture

The application follows a layered architecture with clear separation of concerns:

1. **Presentation Layer**: WPF XAML interfaces with minimalistic Apple-inspired design
2. **Application Layer**: ChatBot class orchestrates workflow between components
3. **Domain Layer**: ConversationManager, KnowledgeBase, and MemoryManager handle core logic
4. **Infrastructure Layer**: File-based persistence and audio playback

### Data Flow

1. User input enters through MainWindow text box
2. ChatBot forwards input to ConversationManager
3. ConversationManager processes natural language, extracts keywords
4. KnowledgeBase provides relevant cybersecurity information
5. Response returns through ChatBot to MainWindow display
6. MemoryManager logs interaction and updates user data

## Installation

### Prerequisites

- Windows 7 or higher
- .NET Framework 4.7.2 or higher
- Visual Studio 2017 or higher (for development)

### Setup Instructions

1. Clone or download the project repository
2. Open the solution file in Visual Studio
3. Ensure all NuGet packages are restored
4. Build the solution (Ctrl+Shift+B)
5. Run the application (F5)

### Audio Setup

The application includes optional audio greetings. Place a `welcome.wav` file 
in the `Audio/` directory for voice greeting functionality. If no audio file is 
present, the application will function normally without audio.

## Usage

### Starting the Application

Upon launch, the application displays a welcome ASCII art banner and requests the user's name. 
After entering a name, users can begin asking cybersecurity questions immediately.

### Basic Commands

- Type any cybersecurity question (e.g., "How do I create a strong password?")
- Use `help` to see available topics
- Type `exit`, `quit`, or `bye` to close the application
- Ask "what is my name" to test memory functionality

### Accessing Features

- **Tasks Button**: Opens task management window for creating and tracking cybersecurity tasks
- **Quiz Button**: Launches interactive cybersecurity knowledge test
- **History Button**: Displays complete activity log with timestamps

### File Management

User data is stored in the `UserData/` directory, organized by username. 

Each user has separate files for:
- Activity history (`history.log`)
- Task list (`tasks.dat`)
- Keyword frequency (`keywords.dat`)

## Technical Specifications

### Technology Stack

- **Programming Language**: C# 7.3
- **Framework**: .NET Framework 4.7.2
- **UI Framework**: Windows Presentation Foundation (WPF)
- **Architecture**: MVVM-inspired layered architecture
- **Storage**: File-based persistence with structured text files

### Design Patterns

- **Singleton-like patterns** for core managers
- **Factory patterns** for UI component creation
- **Observer patterns** for UI updates
- **Strategy patterns** for conversation processing

### Performance Considerations

- In-memory caching of frequently accessed data
- Efficient keyword extraction algorithms
- Lazy loading of user data
- Asynchronous UI updates using Dispatcher

### Security Features

- Local file storage only (no network transmission)
- Input sanitization and validation
- Structured error handling with user-friendly messages
- No sensitive data collection or transmission

## Disclaimer

UNDER NO CIRCUMSTANCES SHOULD IMAGES OR EMOJIS BE INCLUDED DIRECTLY IN THE README FILE. 
ALL VISUAL MEDIA, INCLUDING SCREENSHOTS AND IMAGES OF THE APPLICATION, MUST BE STORED IN 
A DEDICATED FOLDER WITHIN THE PROJECT DIRECTORY. THIS FOLDER SHOULD BE CLEARLY STRUCTURED 
AND NAMED ACCORDINGLY TO INDICATE THAT IT CONTAINS ALL VISUAL CONTENT RELATED TO THE 
APPLICATION (FOR EXAMPLE, A FOLDER NAMED `images`, `screenshots`, OR `media`).

I AM NOT LIABLE OR RESPONSIBLE FOR ANY MALFUNCTIONS, DEFECTS, OR ISSUES THAT MAY OCCUR AS A 
RESULT OF COPYING, MODIFYING, OR USING THIS SOFTWARE. IF YOU ENCOUNTER ANY PROBLEMS OR ERRORS, 
PLEASE DO NOT ATTEMPT TO FIX THEM SILENTLY OR OUTSIDE THE PROJECT. INSTEAD, KINDLY SUBMIT A PULL 
REQUEST OR OPEN AN ISSUE ON THE CORRESPONDING GITHUB REPOSITORY, SO THAT IT CAN BE ADDRESSED 
APPROPRIATELY BY THE MAINTAINERS OR CONTRIBUTORS.

This software is provided "as is" without warranty of any kind, either express or implied. 
The developers assume no responsibility for any damage or data loss that may occur from using 
this application. Users are responsible for implementing proper cybersecurity measures beyond 
the educational guidance provided by this chatbot.

### Usage Restrictions

This application is intended for educational purposes only. 
The cybersecurity advice provided should not be considered 
comprehensive professional guidance. For critical security 
implementations, consult certified cybersecurity professionals.

### Modification Guidelines

When modifying this software, maintain the established code structure 
and documentation standards. All changes should be thoroughly tested 
before deployment. Follow the existing error handling patterns and ensure 
backward compatibility when possible.

### Contribution Policy

Contributions are welcome through the established GitHub repository workflow. 
Please ensure all contributions include appropriate documentation updates and 
maintain the project's educational focus on cybersecurity awareness.

---