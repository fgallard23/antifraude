# Arquitectura de la solución 

![Alt text](Architecture.png)

**NOTA: revisar el video cargado en antifraude.mp4, cargado al github** 

## Tecnologías Utilizadas 
1)	Arquitectura Hexagonal
2)	Microservicios
3)	DDD
4)	CQRS
5)	TDD
6)	Aggregate
7)	Mediator Pattern
8)	Event Sourcing
9)	SOLID
10)	DRY
11)	YAGNI
12)	Kafka
13)	Entity Framework Core
14)	Web API
15)	SQL Server
16)	MongoDB
17)	Docker
18)	Postman
19)	REST
20	NET 8
21)	LINQ
22)	SSMS

## Versión de dotnet instalada 
dotnet --version 

## Versión de Docker instalada 
docker --version

## Versión de Docker Compose instalado
docker-compose --version 

## Crear una red con el siguiente comando  
docker network create --attachable -d bridge mydockernetwork

## Para verificar la red si fue creada 
docker network ls

## Para apache kafka se distribuye el docker compose 
docker-compose.yml

Ejecutar el comando 
**docker-compose up -d**

## Para mongo container  
docker run -it -d --name mongo-container -p 27017:27017 --network mydockernetwork --restart always -v mongodb_data_container:/data/db mongo:latest

## Para SQL Server
docker run -d --name sql-container --network mydockernetwork --restart always -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=$tr0ngS@P@ssw0rd02' -e 'MSSQL_PID=Express' -p 1433:1433 mcr.microsoft.com/mssql/server:2017-latest-ubuntu

En Docker Desktop se puede visualizar los dockers creados hasta el momento 

![Alt text](docker%20desktop.png)

## En SSMS 
1) Ejecutar WebTransactionAPI con el usuario sa para que se creen la tabla y base de datos del journal
2) Ejecutar el script Create-SMUser.sql el cual permite crear el usuario propietario para la base de datos creada

![Alt text](crear%20usuario%20en%20la%20base%20de%20datos%20de%20sql%20server.png)
   
3) Cambiar las conexiones de los servicios en las dos microservicios anti-fraude y transacciones

![Alt text](cambiar%20el%20string%20de%20conexion.png)
 
## Estructura del proyecto

![Alt text](hexagonal-architectura.png)

## Testing 

1) Con postman se ejecuta 

![Alt text](postman%20test.PNG)

2) Se puede visualizar en la base de datos la trx con estado pendiente 

![Alt text](sql%20server%20pending.PNG)

3) Event Sourcing se muestra en MongoDB el evento almacenado 

![Alt text](event%20sourcing.PNG)
 
4) En Kafka en el topic request se puede visualizar el mensaje de la transacción 

![Alt text](kafka%20topic%20request.PNG)

5) El micro de antifraude realiza la validación y el resultado se muestra en el otro topic, que en base al monto la transacción es exitosa 

![Alt text](kafka%20topic%20response.PNG)

6) para visualizar el estado en la base de datos sql server con SSMS.
   
![Alt text](sql%20server%20approved%20trx.PNG)
 
