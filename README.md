Arquitectura de la solución 
 
Tecnologías Utilizadas 
•	Arquitectura Hexagonal
•	Microservicios
•	DDD
•	CQRS
•	TDD
•	Aggregate
•	Mediator Pattern
•	Event Sourcing
•	SOLID
•	DRY
•	YAGNI
•	Kafka
•	Entity Framework Core
•	Web API
•	SQL Server
•	MongoDB
•	Docker
•	Postman
•	REST
•	NET 8
•	LINQ

Versión de dotnet instalada 
dotnet --version 
Versión de Docker instalada 
docker --version
Versión de Docker Compose instalado
docker-compose --version 
Crear una red con el siguiente comando  
docker network create --attachable -d bridge mydockernetwork
Para verificar la red si fue creada 
docker network ls
Para apache kafka se distribuye el docker compose 
docker-compose.yml
Ejecutar el comando 
docker-compose up -d
Para mongo container  
docker run -it -d --name mongo-container -p 27017:27017 --network mydockernetwork --restart always -v mongodb_data_container:/data/db mongo:latest
Para SQL Server
docker run -d --name sql-container --network mydockernetwork --restart always -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=$tr0ngS@P@ssw0rd02' -e 'MSSQL_PID=Express' -p 1433:1433 mcr.microsoft.com/mssql/server:2017-latest-ubuntu
 
Con Entity Framework ejecutar WebTransactionAPI con usuario sa para crear la tabla 
Cambiar el string de conexión archivo Create-SMUser.sql
 
 

Estructura del proyecto
 
Se requiere ejecutar los dos microservicios o como se muestra en el video haciendo debug 
Aprobado 
 
Se puede visualizar en la base de datos almacenado con el estado pendiente 
 
Para el tema de evento sourcing se puede visualizar en mongo la transacción que se ejecuto 
 
En Kafka podemos visualizar que el topic se tiene el mensaje de la transacción 
 
En el otro tópico después de ejecutar se puede visualizar que la transacción fue exitosa 
 
El estado fue actualizado en la base de datos. 
 
