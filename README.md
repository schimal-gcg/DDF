# Skill D365 - Requirement vs Test Script (ASP.NET Core scaffold)

Este repositorio contiene un scaffold inicial en C# (.NET 7) para el skill que compara un requerimiento (Word/PDF) contra un script de pruebas (Excel) y genera un JSON con coincidencias y sugerencias AOT.

Branch: feature/skill-d365-requirement-compare

Cómo ejecutar (local):
- Requiere .NET 7 SDK
- Desde la carpeta src/Skill.D365:
  - dotnet restore
  - dotnet run

Endpoint principal:
- POST /api/analyze
  - multipart/form-data
  - fields: requirement (file, docx/pdf), script (file, optional xlsx/csv)

El scaffold incluye servicios básicos de ingestión de archivos y un endpoint mínimo que devuelve un JSON de ejemplo con la estructura definida en la especificación del MVP.
