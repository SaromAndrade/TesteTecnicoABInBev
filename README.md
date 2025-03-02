# Ambev Developer Evaluation - Running with Docker Compose
This project is a web API application developed for developer evaluation at Ambev. It uses a set of Docker containerized services to provide a complete development environment, including a PostgreSQL database, a MongoDB database, a Redis cache, and a web interface for MongoDB management (Mongo Express).

## Prerequisites
Make sure you have the following installed on your system:
- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/install/)

## How to Run the Project
Follow these steps to set up and run the project using Docker Compose:

### 1. Clone the Repository
```sh
git clone <repository_url>
cd <repository_folder>
```

### 2. Start the Containers
Run the following command in the project root directory:
```sh
docker-compose up -d --build
```
This will:
- Build and start the `ambev.developerevaluation.webapi` service.
- Start a PostgreSQL database (`ambev.developerevaluation.database`).
- Start a MongoDB NoSQL database (`ambev.developerevaluation.nosql`).
- Start a Redis cache (`ambev.developerevaluation.cache`).
- Start `mongo-express` for MongoDB management.

### 3. Verify Running Containers
To check if all containers are running:
```sh
docker ps
```

### 4. Access the Application
- **API Service:** http://localhost:8080
- **Mongo Express UI:** http://localhost:8085

### 5. Stop the Containers
To stop the running services:
```sh
docker-compose down
```

## Configuration Details

### Environment Variables
The following environment variables are configured within `docker-compose.yml`:
- **PostgreSQL Connection:** `ConnectionStrings__DefaultConnection=Host=ambev.developerevaluation.database;Port=5432;Database=developer_evaluation;Username=developer;Password=ev@luAt10n`
- **MongoDB Connection:** `ConnectionStrings__MongoDb=mongodb://developer:MongoDB2025@ambev.developerevaluation.nosql:27017/admin`
- **Redis Password:** `ev@luAt10n`

### Network Configuration
All services are connected through the `backend` network using a bridge driver.

## Troubleshooting
- **Check logs of a specific service:**
  ```sh
  docker logs -f <container_name>
  ```
- **Restart a specific container:**
  ```sh
  docker restart <container_name>
  ```
- **Remove all stopped containers:**
  ```sh
  docker system prune -f
  ```

If you encounter any issues, make sure that ports `8080`, `8081`, `8085`, `5423`, `27017`, and `6379` are not in use by other applications.

## Postman Collection

This project includes a Postman collection to help you test the API endpoints. You can find the collection and environment files in the `postman/` directory.

### Importing the Collection
1. Open Postman.
2. Click on **Import** in the top-left corner.
3. Select the `testeABInBev.postman_collection.json` file from the root directory.
4. Start testing the API endpoints!

## License
This project is licensed under [MIT License](LICENSE).

