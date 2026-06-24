# Prueba Técnica Monolegal Backend

API REST en **.NET 10** diseñada para orquestar y automatizar notificaciones progresivas de cobro a firmas jurídicas, aplicando **Clean Architecture** y principios **SOLID**.

## Características Principales

* **Transición Automática de Estados:** Escala facturas de `primerrecordatorio` ➔ `segundorecordatorio` ➔ `desactivado`.
* **Smart Fallback de Emails:** Integración con SendGrid para correos reales, con simulación automática por consola si no hay credenciales (para desarrollo rápido).
* **Persistencia Robusta:** MongoDB 7.0 integrado con un *Seed* automático de 12 facturas y 3 clientes diferentes.
* **Resiliencia:** Si el envío de un correo falla, la base de datos no se actualiza y el proceso continúa con la siguiente factura sin romper el lote.

## Stack Tecnológico

* **Framework:** ASP.NET Core 10 (C# 12)
* **Base de Datos:** MongoDB 7.0 + Driver Oficial de C#
* **Servicio de Correos:** SendGrid API
* **Pruebas Unitarias:** xUnit + Moq (Patrón AAA)
* **Infraestructura:** Docker & Docker Compose

## Guía Rápida de Instalación (Quick Start)

### 1. Clonar e Iniciar Infraestructura
El proyecto incluye un `docker-compose.yml` que levanta la base de datos y la puebla automáticamente con los clientes de prueba.

```bash
git clone https://github.com/decamacho/prueba-tec-monolegal-backend.git
cd prueba-tec-monolegal-backend
docker-compose up -d