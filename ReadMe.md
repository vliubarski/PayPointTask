# **Customer Charge Notification System**

## **Project Overview**

This project, _Customer Charge Notification System_, is a .NET-based application designed to process customer charge notifications, generate corresponding PDF files for each customer, and manage background jobs for recurring tasks. Key functionalities include:

1.  **Charge Notification Processing**: Retrieves and processes charge notifications based on provided dates.
2.  **PDF Generation**: Generates PDF files with detailed charge information and stores them in a designated output directory.
3.  **Hangfire Integration**: Manages recurring background tasks to automate the notification process at configured intervals.
4.  **Database Setup**: Utilizes SQL Server for managing customer data, notifications, and Hangfire jobs.

## **CustomerChargeNotification Project Structure**

-   **Domain**: Core domain classes and business logic.
-   **BackgroundServices**: Service for  recurring background processing and notification generation
-   **PDFUtils**: Generates and saves customer charge PDFs on output location.
-   **DAL**: SQL Server DB context and repo classes.
-   **Infrastructure**: Class for Hangfire job configuration.
-   **Models**: Dto's used in the project
----------

## **Database Deployment**

### Step 1: Setting up SQL Server

Ensure you have access to an SQL Server instance, either locally or in the cloud. The project uses SQL Server for storing customer charges, managing Hangfire jobs, and storing related data.

### Step 2: Deploying the Databases

1.  **Customer Database**: is managed by *ChargeNotificationDbProj* project. It has scripts for creating Customer and CustomerGameCharge tables provided in the `Scripts` directory. Script for populating Customer table will run by default, to populate CustomerGameCharge table with random data please uncomment call to PopulateCustomerGameCharge in PostDeployment.sql before deployment (see section 3).
2.  **Hangfire Database**: is managed by *HangfireDbProj* project. Hangfire requires its own schema for managing background jobs. Configure the Hangfire connection string in the config file (see **Configuration** below), and Hangfire will automatically create its required tables.
3. **Deployment**: to deploy these DB's please double click ChargeNotificationDbProj.publish.xml and HangfireDbProj.publish.xml, *amending Target db connection if needed*, and then press **Publish**.
----------

## **Configuration File (appsettings.json)**

The `appsettings.json` file is the main configuration file for the project:

-   **Database Connections**: Contains connection strings for the main application database and Hangfire.
    -   `"DefaultConnection"`: Connection string for the application database.
    -   `"HangfireConnection"`: Connection string for the Hangfire job database.
-   **NotificationJob**: Contains Hangfire settings.
     -   `"CronExpression"`: Sets the interval for the Hangfire background job that generates charge notifications.

### Running the Application

1.  **Database Initialization**: Deploy the databases as outlined in the **Database Deployment** section.
2.  **Configure Settings**: Update `appsettings.json` with your connection strings and preferred job interval.
3.  **Build and Run it !**

### Accessing Hangfire Dashboard and Swagger

-   **Swagger**: After starting the application, navigate to `https://localhost:7172/swagger` to access the API documentation or generate customer charges using api.
-   **Hangfire Dashboard**: Accessible at `https://localhost:7172/hangfire` to monitor and manage background jobs.
