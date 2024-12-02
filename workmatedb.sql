-- MySQL dump 10.13  Distrib 8.0.38, for Win64 (x86_64)
--
-- Host: localhost    Database: workmatedb
-- ------------------------------------------------------
-- Server version	8.0.39

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `__efmigrationshistory`
--

DROP TABLE IF EXISTS `__efmigrationshistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__efmigrationshistory`
--

LOCK TABLES `__efmigrationshistory` WRITE;
/*!40000 ALTER TABLE `__efmigrationshistory` DISABLE KEYS */;
INSERT INTO `__efmigrationshistory` VALUES ('20241028031132_IniCreate','8.0.5'),('20241123125711_AddForum','8.0.5'),('20241124094806_AddForum2','8.0.5'),('20241202143534_UpdateComment','8.0.5'),('20241202145101_UpdatePost','8.0.5');
/*!40000 ALTER TABLE `__efmigrationshistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `accounts`
--

DROP TABLE IF EXISTS `accounts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `accounts` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Email` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Password` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `AvatarUrl` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `FaceUrl` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `Status` int NOT NULL,
  `EmployeeId` int NOT NULL,
  `RoleId` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Accounts_EmployeeId` (`EmployeeId`),
  KEY `IX_Accounts_RoleId` (`RoleId`),
  CONSTRAINT `FK_Accounts_Employees_EmployeeId` FOREIGN KEY (`EmployeeId`) REFERENCES `employees` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Accounts_Roles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `roles` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `accounts`
--

LOCK TABLES `accounts` WRITE;
/*!40000 ALTER TABLE `accounts` DISABLE KEYS */;
INSERT INTO `accounts` VALUES (1,'baopq@gmail.com','$2a$11$wrrWPovB6oGOCL2KBdKkFebQP9FuvCq1bfNSSPQBZAtY/Oodqszxq','string','string','2024-10-28 10:51:37.877149',1,1,1),(2,'landt@gmail.com','$2a$11$POLafgv7H1MScYKyhCnunOWSaN3rWwMNgC1cUkvVF05kZnlGFCkwW','string','string','2024-10-28 10:52:41.199687',1,2,2),(3,'namtt@gmail.com','$2a$11$pgEDaFp5T7H.FTrzOS9fseWWa9VJ.O9tGiVv0Da0xDQ3E60KIm1Lm','string','string','2024-10-28 10:52:55.938021',1,3,3),(4,'chientp@gmail.com','$2a$11$EzQLKMPXzxXCC6EFiDGJW.WRdKAYM7JfGmJyTL4OZ62yP4WAA.aNa','string','string','2024-10-28 10:53:12.202615',1,4,3);
/*!40000 ALTER TABLE `accounts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `assets`
--

DROP TABLE IF EXISTS `assets`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `assets` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Location` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `Status` int NOT NULL,
  `EmployeeId` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Assets_EmployeeId` (`EmployeeId`),
  CONSTRAINT `FK_Assets_Employees_EmployeeId` FOREIGN KEY (`EmployeeId`) REFERENCES `employees` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `assets`
--

LOCK TABLES `assets` WRITE;
/*!40000 ALTER TABLE `assets` DISABLE KEYS */;
INSERT INTO `assets` VALUES (1,'Screen A1','Screen 20 inch','HN','0001-01-01 00:00:00.000000',0,NULL),(2,'Screen A2','Screen 18 inch','HN','0001-01-01 00:00:00.000000',0,NULL),(3,'Screen A3','Screen 25 inch','HN','0001-01-01 00:00:00.000000',0,NULL),(4,'Keyboard A1','Keyboard Blue Switch','HN','0001-01-01 00:00:00.000000',0,NULL),(5,'Keyboard A2','Keyboard Red Switch','HN','0001-01-01 00:00:00.000000',0,NULL),(6,'Keyboard A3','Keyboard Red Switch','HN','0001-01-01 00:00:00.000000',0,NULL);
/*!40000 ALTER TABLE `assets` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `attendances`
--

DROP TABLE IF EXISTS `attendances`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `attendances` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `CheckIn` datetime(6) NOT NULL,
  `CheckOut` datetime(6) DEFAULT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `Status` int NOT NULL,
  `Late` int NOT NULL,
  `AccountId` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Attendances_AccountId` (`AccountId`),
  CONSTRAINT `FK_Attendances_Accounts_AccountId` FOREIGN KEY (`AccountId`) REFERENCES `accounts` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=28 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `attendances`
--

LOCK TABLES `attendances` WRITE;
/*!40000 ALTER TABLE `attendances` DISABLE KEYS */;
INSERT INTO `attendances` VALUES (5,'2024-10-01 08:30:00.000000','2024-10-01 18:00:00.000000','2024-10-01 08:30:00.000000',1,0,1),(6,'2024-10-02 08:30:00.000000','2024-10-02 18:00:00.000000','2024-10-02 08:30:00.000000',1,0,1),(7,'2024-10-03 08:30:00.000000','2024-10-03 18:00:00.000000','2024-10-03 08:30:00.000000',1,0,1),(8,'2024-10-04 08:30:00.000000','2024-10-04 18:00:00.000000','2024-10-04 08:30:00.000000',1,0,1),(9,'2024-10-07 08:30:00.000000','2024-10-07 18:00:00.000000','2024-10-07 08:30:00.000000',1,0,1),(10,'2024-10-08 08:30:00.000000',NULL,'2024-10-08 08:30:00.000000',1,0,1),(11,'2024-10-09 08:30:00.000000',NULL,'2024-10-09 08:30:00.000000',1,0,1),(12,'2024-10-10 08:30:00.000000','2024-10-10 18:00:00.000000','2024-10-10 08:30:00.000000',1,0,1),(13,'2024-10-11 08:30:00.000000',NULL,'2024-10-11 08:30:00.000000',1,0,1),(14,'2024-10-14 08:30:00.000000','2024-10-14 18:00:00.000000','2024-10-14 08:30:00.000000',1,0,1),(15,'2024-10-15 08:30:00.000000','2024-10-15 18:00:00.000000','2024-10-15 08:30:00.000000',1,0,1),(16,'2024-10-16 08:30:00.000000','2024-10-16 18:00:00.000000','2024-10-16 08:30:00.000000',1,0,1),(17,'2024-10-17 08:30:00.000000','2024-10-17 18:00:00.000000','2024-10-17 08:30:00.000000',1,0,1),(18,'2024-10-18 08:30:00.000000','2024-10-18 18:00:00.000000','2024-10-18 08:30:00.000000',1,0,1),(19,'2024-10-21 08:30:00.000000','2024-10-21 18:00:00.000000','2024-10-21 08:30:00.000000',1,0,1),(20,'2024-10-22 08:30:00.000000','2024-10-22 18:00:00.000000','2024-10-22 08:30:00.000000',1,0,1),(21,'2024-10-23 08:30:00.000000','2024-10-23 18:00:00.000000','2024-10-23 08:30:00.000000',1,0,1),(22,'2024-10-24 08:30:00.000000','2024-10-24 18:00:00.000000','2024-10-24 08:30:00.000000',1,0,1),(23,'2024-10-25 08:30:00.000000','2024-10-25 18:00:00.000000','2024-10-25 08:30:00.000000',1,0,1),(24,'2024-10-28 08:30:00.000000','2024-10-28 18:00:00.000000','2024-10-28 08:30:00.000000',1,0,1),(25,'2024-10-29 08:30:00.000000','2024-10-29 18:00:00.000000','2024-10-29 08:30:00.000000',1,0,1),(26,'2024-10-30 08:30:00.000000','2024-10-30 18:00:00.000000','2024-10-30 08:30:00.000000',1,0,1),(27,'2024-10-31 08:30:00.000000','2024-10-31 18:00:00.000000','2024-10-31 08:30:00.000000',1,0,1);
/*!40000 ALTER TABLE `attendances` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `comments`
--

DROP TABLE IF EXISTS `comments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `comments` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Content` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `Status` int NOT NULL,
  `PostId` int NOT NULL,
  `AccountId` int NOT NULL,
  `FullName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Comments_AccountId` (`AccountId`),
  KEY `IX_Comments_PostId` (`PostId`),
  CONSTRAINT `FK_Comments_Accounts_AccountId` FOREIGN KEY (`AccountId`) REFERENCES `accounts` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Comments_Posts_PostId` FOREIGN KEY (`PostId`) REFERENCES `posts` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `comments`
--

LOCK TABLES `comments` WRITE;
/*!40000 ALTER TABLE `comments` DISABLE KEYS */;
/*!40000 ALTER TABLE `comments` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `departments`
--

DROP TABLE IF EXISTS `departments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `departments` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `Status` int NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `departments`
--

LOCK TABLES `departments` WRITE;
/*!40000 ALTER TABLE `departments` DISABLE KEYS */;
INSERT INTO `departments` VALUES (1,'AI','Department about AI','2024-10-28 10:19:30.519999',0),(2,'Web','Department about Web','2024-10-28 10:19:42.942324',0),(3,'Mobile','Department about Mobile','2024-10-28 10:19:52.097272',0),(4,'Human Resource','Department about HR','2024-10-28 10:21:24.285133',0);
/*!40000 ALTER TABLE `departments` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `employees`
--

DROP TABLE IF EXISTS `employees`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `employees` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `FullName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Phone` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Birthday` datetime(6) NOT NULL,
  `IdentificationId` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Position` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Address` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `BaseSalary` int NOT NULL,
  `Status` int NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `DepartmentId` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Employees_DepartmentId` (`DepartmentId`),
  CONSTRAINT `FK_Employees_Departments_DepartmentId` FOREIGN KEY (`DepartmentId`) REFERENCES `departments` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `employees`
--

LOCK TABLES `employees` WRITE;
/*!40000 ALTER TABLE `employees` DISABLE KEYS */;
INSERT INTO `employees` VALUES (1,'Pham Quoc Bao','0818847831','2002-02-03 00:00:00.000000','042202006013','BackendDeveloper','Ha Tinh',15000000,0,'2024-10-28 10:24:09.779289',2),(2,'Duong Tuan Lan','0394338518','2002-03-12 00:00:00.000000','123456123456','HR','Quang Ninh',20000000,0,'2024-10-28 10:25:16.562615',4),(3,'Tran Tien Nam','0123456789','2002-03-07 00:00:00.000000','654321654321','Project Manage','Vinh Phuc',13000000,0,'2024-10-28 10:35:23.397895',3),(4,'Trinh Phuc Chien','0987654321','2002-03-07 00:00:00.000000','098765098765','Frontend Developer','Hung Yen',8000000,0,'2024-10-28 10:36:18.355689',2);
/*!40000 ALTER TABLE `employees` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `leaverequests`
--

DROP TABLE IF EXISTS `leaverequests`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `leaverequests` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Date` datetime(6) NOT NULL,
  `Reason` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `Status` int NOT NULL,
  `EmployeeId` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_LeaveRequests_EmployeeId` (`EmployeeId`),
  CONSTRAINT `FK_LeaveRequests_Employees_EmployeeId` FOREIGN KEY (`EmployeeId`) REFERENCES `employees` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `leaverequests`
--

LOCK TABLES `leaverequests` WRITE;
/*!40000 ALTER TABLE `leaverequests` DISABLE KEYS */;
/*!40000 ALTER TABLE `leaverequests` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `posts`
--

DROP TABLE IF EXISTS `posts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `posts` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Content` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ImageUrl` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `CreatedAt` datetime(6) NOT NULL,
  `Status` int NOT NULL,
  `AccountId` int NOT NULL,
  `FullName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Posts_AccountId` (`AccountId`),
  CONSTRAINT `FK_Posts_Accounts_AccountId` FOREIGN KEY (`AccountId`) REFERENCES `accounts` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `posts`
--

LOCK TABLES `posts` WRITE;
/*!40000 ALTER TABLE `posts` DISABLE KEYS */;
INSERT INTO `posts` VALUES (2,'abcxyz','string','2024-12-02 21:41:57.704168',0,1,''),(3,'string','string','2024-12-02 21:52:13.831704',0,1,'Pham Quoc Bao');
/*!40000 ALTER TABLE `posts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `roles`
--

DROP TABLE IF EXISTS `roles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `roles` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `roles`
--

LOCK TABLES `roles` WRITE;
/*!40000 ALTER TABLE `roles` DISABLE KEYS */;
INSERT INTO `roles` VALUES (1,'Manager','Super Admin','2024-10-28 10:40:54.676878'),(2,'Admin','Super User','2024-10-28 10:41:37.946206'),(3,'User','Account for Employee','2024-10-28 10:41:54.642834');
/*!40000 ALTER TABLE `roles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `salaries`
--

DROP TABLE IF EXISTS `salaries`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `salaries` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `BaseSalary` int NOT NULL,
  `Bonus` int NOT NULL,
  `Deduction` int NOT NULL,
  `Total` int NOT NULL,
  `Month` int NOT NULL,
  `Year` int NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `Status` int NOT NULL,
  `EmployeeId` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Salaries_EmployeeId` (`EmployeeId`),
  CONSTRAINT `FK_Salaries_Employees_EmployeeId` FOREIGN KEY (`EmployeeId`) REFERENCES `employees` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `salaries`
--

LOCK TABLES `salaries` WRITE;
/*!40000 ALTER TABLE `salaries` DISABLE KEYS */;
INSERT INTO `salaries` VALUES (2,15000000,0,0,15000000,10,2024,'2024-11-12 11:09:16.261674',0,1);
/*!40000 ALTER TABLE `salaries` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-12-02 22:11:19
