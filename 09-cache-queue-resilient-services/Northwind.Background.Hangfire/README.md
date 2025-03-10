# Northwind Background Jobs with Hangfire

This application demonstrates how to use Hangfire to schedule and execute background jobs in a .NET application.

## Overview

The application provides a simple API to schedule a job that will write a message after a specified delay. The job is scheduled using Hangfire and executed in the background.

## Running the Application

1. Ensure you have SQL Server running with the connection details matching those in `Program.cs`
2. Run the application using `dotnet run`
3. Navigate to `http://localhost:5095/hangfire` to view the Hangfire dashboard

## Scheduling a Job

You can schedule a job by sending a POST request to the `/schedulejob` endpoint.

### Using postman
1. Open Postman
2. Create a new POST request to `http://localhost:5095/schedulejob`
3. Set the Content-Type header to `application/json`
4. In the request body, add the following JSON:
   ```json
   {
     "message": "Hello from Hangfire!",
     "seconds": 10
   }
   ```
5. Send the request
6. The job will be scheduled to run after the specified number of seconds
7. You can view the scheduled job in the Hangfire dashboard

The `message` field contains the text that will be displayed when the job runs, and the `seconds` field specifies how many seconds to wait before executing the job.

