# FTP Search 🔍

**FTP Search** is an innovative project designed to provide integrated data access by leveraging the power of PostgreSQL on an FTP Server. The project has been developed to address the inherent limitations of FTP Servers, which do not possess query capabilities as advanced as those of modern databases.

## About the Project 📄

FTP Search has been created to simplify data access and search operations on FTP Servers. By integrating with PostgreSQL's powerful querying capabilities, the project enables advanced search operations on file names and data stored on an FTP Server.

## Problem Statement 🤔

The core issue stems from the nature of FTP Servers, which inherently lack the sophisticated query structures and search capabilities found in modern databases. To overcome this limitation, FTP Search utilizes PostgreSQL's **TsVector** feature for text search and the **Trigram Index** to enable high-performance querying even when only a portion of the filename is provided.

## Key Features 🚀

- **Integrated Data Access:** Seamless and reliable data exchange between the FTP Server and PostgreSQL.
- **Advanced Search:** High-performance search capabilities, including partial matching, enabled by PostgreSQL's TsVector and Trigram Index features.
- **Modern Technologies:** Built on a robust infrastructure utilizing .NET 8 Minimal API, Entity Framework Core, and other modern libraries/architectures.
- **Vertical Slice Architecture:** Enhances modularity and scalability of the application.
- **Swagger Support:** Provides a user-friendly interface for API documentation and testing.

## Technologies and Architectures Used 💻

- **.NET 8 Web API (Minimal API):** Lightweight and high-performance API development.
- **PostgreSQL:** Advanced querying capabilities and reliable data management.
- **FTP Server:** Centralized system for file storage and transfer.
- **FluentFTP:** A .NET library that simplifies FTP operations.
- **Vertical Slice Architecture:** An architectural approach that organizes application components into functional slices.
- **Carter:** Provides routing and modular support compatible with Minimal API.
- **Entity Framework Core:** A modern ORM solution for database operations.
- **Mapster:** Offers flexibility in object mapping and transformations.
- **Swagger:** Ensures transparency in API documentation and testing processes.