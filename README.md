# UrlShortening_Service

URL Shortening Service
Overview
A robust, scalable URL shortening service built with .NET 8, implementing clean architecture and domain-driven design principles.
Features

Generate short URLs from long URLs
Redirect short URLs to original destinations
Track URL access statistics
IP-based rate limiting
Caching with Redis
Comprehensive error handling

Technologies

.NET 8
Entity Framework Core
MediatR (CQRS)
Redis
SQL Server
FluentValidation

Architecture

Domain-Driven Design
Clean Architecture
CQRS Pattern

API Endpoints

POST /api/url/shorten: Create short URL
GET /{shortCode}: Redirect to original URL
GET /api/url/stats/{shortCode}: Get URL statistics

Setup

Clone repository
Configure connection strings
Run database migrations
Start application

Requirements

.NET 8 SDK
SQL Server
Redis
