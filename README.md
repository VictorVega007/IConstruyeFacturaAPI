
# Prueba de concepto DTE Factura para el SII

Esta prueba de concepto consiste en el desarrollo de una API RESTful que permite la carga de archivos en formato XML que representan Facturas Electrónicas o DTE (Documentos Tributarios Electrónicos) utilizados por el Servicio de Impuestos Internos (SII) de Chile.

Al procesar el archivo, la API genera automáticamente una URL corta asociada al documento, la cual puede ser utilizada para realizar consultas posteriores. La funcionalidad está diseñada con las siguientes restricciones y características de seguridad:

🔐 Autenticación y seguridad: Todos los endpoints protegidos requieren un JWT (JSON Web Token). Este token se obtiene a través del endpoint de autenticación /api/v1/Auth/token y debe ser incluido como Bearer Token en el encabezado de las peticiones.

⏱️ Expiración automática: La URL generada tiene una vigencia de 2 minutos desde su creación. Una vez expirado ese tiempo, la URL ya no será válida para consultas.

🔄 Límite de visualizaciones: Cada documento puede ser consultado un máximo de 3 veces. Al superarse ese límite, el acceso queda restringido y se devuelve un mensaje de error informativo.

Esta API ha sido construida aplicando principios de arquitectura hexagonal y el patrón CQRS, lo que garantiza una alta escalabilidad, mantenibilidad y separación de responsabilidades.


## ©️ Autor

- [Victor Vega](https://github.com/VictorVega007)


## 🧱 Arquitectura del Proyecto

Este proyecto se construyó siguiendo el patrón de arquitectura hexagonal (también conocido como Ports and Adapters) combinado con el enfoque de CQRS (Command Query Responsibility Segregation).

🧭 ¿Por qué arquitectura hexagonal?

Esta arquitectura tiene como objetivo aislar el núcleo de negocio de las dependencias externas como bases de datos, servicios web, interfaces gráficas, etc. Esta separación permite:

- Alta mantenibilidad: el dominio queda aislado de los cambios en tecnologías externas.

- Alta testabilidad: el núcleo puede probarse sin necesidad de instanciar bases de datos ni frameworks.

- Independencia tecnológica: permite cambiar detalles de infraestructura sin tocar la lógica de negocio.

- Claridad y separación de responsabilidades: cada capa tiene un propósito bien definido.

⚔️ ¿Por qué usar CQRS?

CQRS separa las operaciones de lectura (queries) y escritura (commands), lo que ofrece múltiples beneficios:

- Simplificación de lógica: los comandos no devuelven datos, las queries no modifican el estado.

- Escalabilidad: permite escalar independientemente las operaciones de lectura y escritura.

- Rendimiento optimizado: se pueden usar distintos modelos de datos para cada lado (lectura vs escritura).

- Facilita validaciones: auditoría y eventos, esto debido a su clara división de flujos.

## 🧱 Estructura del proyecto

```
IConstruye.Factura.sln
│
├── IConstruye.Factura.API
│   ├── Controllers (V1)
│   ├── Models
│   ├── Swagger Config
│   ├── Program.cs / Startup.cs
│
├── IConstruye.Factura.Application
│   ├── Commands
│   ├── Handlers
│   ├── Queries
│   ├── Mappers
│   ├── Responses
│
├── IConstruye.Factura.Core
│   ├── Entities
│   ├── Interfaces
│   ├── Repositories (abstractos)
│   ├── Services
│   ├── ValueObjects
│
├── IConstruye.Factura.Infrastructure
│   ├── Repositories (implementaciones)
│   ├── Data
│   ├── Helpers
│   ├── Validators
│
└── IConstruye.Factura.Test
    ├── Pruebas unitarias y de integración
```


## ⚠️ Requisitos previos

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) instalado y corriendo
- JetBrains Rider, Visual Studio Code o cualquier IDE compatible


## 💻 Ejecución local del proyecto

1. Clonar repositorio

```bash
  git clone https://link-to-project
```

2. Ir al directorio del proyecto

```bash
  cd {directorio del proyecto}
```

3. Levantar la base de datos con Docker

```bash
docker-compose up -d
```
Esto iniciará un contenedor con SQL Server 2019 accesible en localhost:1433, según la configuración del archivo del proyecto ```docker-compose.yml```

4. Ejecutar la API

Ubicarse en la raíz del proyecto y ejecutar el siguiente comando en la terminal

```
dotnet run --project IConstruye.Factura.API/IConstruye.Factura.API.csproj
```

## 🐞 Problemas comunes

### Docker no inicia o lanza error del socket
Verifica que Docker Desktop esté corriendo. En macOS:
- Busca el ícono de Docker en la barra de menús.
- Si no aparece, abre la app Docker Desktop manualmente.

### Error 500 al consultar endpoints
Esto ocurre si la base de datos no está levantada. Asegúrate de ejecutar:

```bash
docker-compose up -d
```
## 👨🏻‍💻 Uso de la funcionalidad de la API

Al iniciar el proyecto desde el IDE (JetBrains Rider, Visual Studio, etc.), se abrirá el navegador en el puerto correspondiente, se deberá colocar al final de la url del puerto '/swagger' para que Swagger esté disponible en los siguientes puertos:

- **HTTP**: [`http://localhost:5254/swagger`](http://localhost:5254/swagger)
- **HTTPS**: [`https://localhost:7262/swagger`](https://localhost:7262/swagger)

Swagger permite generar un token y aplicarlo fácilmente a los endpoints protegidos mediante el botón **"Authorize"**. Allí deberás ingresar el token en el formato:
```
Bearer {token generado}
```

---

### 📫 Uso desde Postman

También puedes consumir la API desde Postman. Asegúrate de incluir el token de autorización en la cabecera de las solicitudes.


---

## 📌 Endpoints Disponibles

### 🔐 `POST /api/v1/Auth/token`
Este endpoint genera un **token JWT** de autenticación. No requiere parámetros.  
El token obtenido debe ser utilizado en los encabezados de las demás peticiones.
Este token tiene un tiempo de expiración de 30 minutos; tiempo que se puede verificar colocando el token en la plataforma [jwt.ms](https://jwt.ms/)

---

### 📤 `POST /api/v1/Invoice`
Permite subir un archivo XML correspondiente a un **Documento Tributario Electrónico (DTE)** utilizado por el SII (Chile).

- La API procesa el archivo y retorna una respuesta `200 OK` con un objeto JSON que contiene los datos clave extraídos del documento.
- El objeto de respuesta incluye un campo llamado `shortUrl` que se usará en el siguiente endpoint.

**Ejemplo de respuesta:**
```json
{
  "success": true,
  "shortUrl": "8H7z0ZqNCLG",
  "error": null,
  "issuer": "55555555-5",
  "receiver": "66666666-6",
  "amount": 372075,
  "date": "2025-05-01T11:59:24.11737-04:00",
  "expiresAt": "2025-05-01T16:00:24.118298Z"
}
```

---

### 📥 `GET /api/v1/Invoice/{shortUrl}`
Consulta los datos del DTE previamente cargado utilizando la URL corta (`shortUrl`) generada en la carga anterior.

> ⚠️ Este endpoint incluye dos restricciones de acceso:

1. **Expiración temporal**: El `shortUrl` es válido solo durante los **2 minutos posteriores** a su creación.
2. **Límite de visualizaciones**: El documento puede ser consultado hasta un máximo de **3 veces**. Al superar este límite, la API responderá con un mensaje de error indicando que se alcanzó el número máximo de visualizaciones.

La respuesta incluye un campo `timesConsulted` que indica cuántas veces se ha accedido al documento.

**Ejemplo de respuesta:**
```json
{
  "issuer": "55555555-5",
  "receiver": "66666666-6",
  "amount": 372075,
  "date": "2025-05-01T11:59:24.11737-04:00",
  "shortUrl": "8H7z0ZqNCLG",
  "timesConsulted": 1,
  "canBeConsulted": true
}
```
## 🧪 Pruebas unitarias

Se incorporó una capa para pruebas unitarias que cubren los siguientes casos de usos:

1. **Pruebas para el vencimiento de la URL generada para el DTE**

Se realiza el test para verificar cuando la url generada expira y cuando se encuentra vigente para obtener los datos del DTE.

2. **Pruebas para generación de Url al DTE**

Se verifica que la Url que se genera para cada DTE sea único, es decir, que funja como un ID único para cada Documento cargado y procesado.

3. **Pruebas para verificar la cantidad de vistas del DTE**

El test se basa en la verificación de la cantidad de veces que se obtienen los datos de un DTE.

Para probar la efectividad de estas pruebas se deberá abrir el terminal en la raiz del proyecto y ejecutar el siguiente comando: 

```bash
dotnet test
```