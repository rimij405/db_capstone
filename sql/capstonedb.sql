CREATE DATABASE  IF NOT EXISTS `capstonedb` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `capstonedb`;
-- MySQL dump 10.13  Distrib 5.7.17, for Win64 (x86_64)
--
-- Host: localhost    Database: capstonedb
-- ------------------------------------------------------
-- Server version	5.7.19-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `capstone`
--

DROP TABLE IF EXISTS `capstone`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `capstone` (
  `capstoneID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `studentID` int(10) unsigned NOT NULL,
  `chairID` int(10) unsigned NOT NULL,
  `capstoneStartTerm` int(5) unsigned zerofill NOT NULL,
  `defenseDateApprovedBy` int(10) unsigned DEFAULT NULL,
  `defenseDate` datetime DEFAULT NULL,
  `title` varchar(100) NOT NULL,
  `abstract` varchar(140) NOT NULL,
  `plagiarismScore` int(10) unsigned DEFAULT NULL,
  `grade` int(10) unsigned DEFAULT NULL,
  PRIMARY KEY (`capstoneID`),
  KEY `capstone_studentID_fk` (`studentID`),
  KEY `capstone_chairID_fk` (`chairID`),
  KEY `capstone_capstoneStartTerm_fk` (`capstoneStartTerm`),
  KEY `capstone_defenseDateApprovedBy_fk` (`defenseDateApprovedBy`),
  CONSTRAINT `capstone_capstoneStartTerm_fk` FOREIGN KEY (`capstoneStartTerm`) REFERENCES `term` (`code`),
  CONSTRAINT `capstone_chairID_fk` FOREIGN KEY (`chairID`) REFERENCES `users` (`userID`),
  CONSTRAINT `capstone_defenseDateApprovedBy_fk` FOREIGN KEY (`defenseDateApprovedBy`) REFERENCES `users` (`userID`),
  CONSTRAINT `capstone_studentID_fk` FOREIGN KEY (`studentID`) REFERENCES `users` (`userID`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `capstone`
--
-- ORDER BY:  `capstoneID`

LOCK TABLES `capstone` WRITE;
/*!40000 ALTER TABLE `capstone` DISABLE KEYS */;
INSERT INTO `capstone` (`capstoneID`, `studentID`, `chairID`, `capstoneStartTerm`, `defenseDateApprovedBy`, `defenseDate`, `title`, `abstract`, `plagiarismScore`, `grade`) VALUES (1,3,2,02171,NULL,NULL,'Example','This is an example proposal.',NULL,NULL),(2,4,2,02178,NULL,NULL,'Example 2','This is another example.',NULL,NULL);
/*!40000 ALTER TABLE `capstone` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `capstonefollowers`
--

DROP TABLE IF EXISTS `capstonefollowers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `capstonefollowers` (
  `capstoneID` int(10) unsigned NOT NULL,
  `userID` int(10) unsigned NOT NULL,
  `trackCapstone` tinyint(1) NOT NULL DEFAULT '0',
  `onCommittee` tinyint(1) NOT NULL DEFAULT '0',
  `currentStatus` int(10) unsigned NOT NULL,
  `statusTimestamp` datetime NOT NULL,
  PRIMARY KEY (`capstoneID`,`userID`),
  KEY `capstoneFollowers_userID_fk` (`userID`),
  KEY `capstoneFollowers_currentStatus_fk` (`currentStatus`),
  CONSTRAINT `capstoneFollowers_capstoneID_fk` FOREIGN KEY (`capstoneID`) REFERENCES `capstone` (`capstoneID`),
  CONSTRAINT `capstoneFollowers_currentStatus_fk` FOREIGN KEY (`currentStatus`) REFERENCES `statuses` (`code`),
  CONSTRAINT `capstoneFollowers_userID_fk` FOREIGN KEY (`userID`) REFERENCES `users` (`userID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `capstonefollowers`
--
-- ORDER BY:  `capstoneID`,`userID`

LOCK TABLES `capstonefollowers` WRITE;
/*!40000 ALTER TABLE `capstonefollowers` DISABLE KEYS */;
/*!40000 ALTER TABLE `capstonefollowers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `emailtypes`
--

DROP TABLE IF EXISTS `emailtypes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `emailtypes` (
  `code` int(10) unsigned NOT NULL,
  `name` varchar(45) NOT NULL,
  `description` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `emailtypes`
--
-- ORDER BY:  `code`

LOCK TABLES `emailtypes` WRITE;
/*!40000 ALTER TABLE `emailtypes` DISABLE KEYS */;
INSERT INTO `emailtypes` (`code`, `name`, `description`) VALUES (1,'SCHOOL','School email.'),(2,'PRIMARY','Primary email.'),(3,'WORK','Work email.'),(4,'PERSONAL','Personal email.');
/*!40000 ALTER TABLE `emailtypes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `phonetypes`
--

DROP TABLE IF EXISTS `phonetypes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `phonetypes` (
  `code` int(10) unsigned NOT NULL,
  `name` varchar(45) NOT NULL,
  `description` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `phonetypes`
--
-- ORDER BY:  `code`

LOCK TABLES `phonetypes` WRITE;
/*!40000 ALTER TABLE `phonetypes` DISABLE KEYS */;
INSERT INTO `phonetypes` (`code`, `name`, `description`) VALUES (1,'PRIMARY','Primary number.'),(2,'HOME','Home number.'),(3,'MOBILE','Mobile number.'),(4,'WORK','Work/Office number.');
/*!40000 ALTER TABLE `phonetypes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `roles`
--

DROP TABLE IF EXISTS `roles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `roles` (
  `code` int(10) unsigned NOT NULL,
  `name` varchar(45) NOT NULL,
  `description` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `roles`
--
-- ORDER BY:  `code`

LOCK TABLES `roles` WRITE;
/*!40000 ALTER TABLE `roles` DISABLE KEYS */;
INSERT INTO `roles` (`code`, `name`, `description`) VALUES (1,'STUDENT','Student role.'),(2,'FACULTY','Faculty role.'),(3,'STAFF','Staff role.'),(4,'CHAIR','Chair role.'),(5,'DIRECTOR','Director role.');
/*!40000 ALTER TABLE `roles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `statuses`
--

DROP TABLE IF EXISTS `statuses`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `statuses` (
  `code` int(3) unsigned zerofill NOT NULL,
  `name` varchar(45) NOT NULL,
  `description` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `statuses`
--
-- ORDER BY:  `code`

LOCK TABLES `statuses` WRITE;
/*!40000 ALTER TABLE `statuses` DISABLE KEYS */;
INSERT INTO `statuses` (`code`, `name`, `description`) VALUES (100,'DEFAULT','Default (null) status.'),(200,'CAPSTONE CREATED','Student made capstone.'),(201,'TITLE ADDED','Student added title.'),(202,'ABSTRACT','Student added abstract.'),(203,'FILE','Student uploaded file.'),(300,'ACCEPT','Accepted status.'),(400,'REJECTED','Rejected status.'),(500,'PENDING','Status pending.'),(600,'ACTIVE','Active role since...'),(602,'INACTIVE','Inactive role since...');
/*!40000 ALTER TABLE `statuses` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `statushistoryevent`
--

DROP TABLE IF EXISTS `statushistoryevent`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `statushistoryevent` (
  `capstoneID` int(10) unsigned NOT NULL,
  `statusCode` int(10) unsigned NOT NULL,
  `timeStamp` datetime NOT NULL,
  PRIMARY KEY (`capstoneID`,`statusCode`,`timeStamp`),
  KEY `statusHistoryEvent_statusCode_fk` (`statusCode`),
  CONSTRAINT `statusHistoryEvent_capstoneID_fk` FOREIGN KEY (`capstoneID`) REFERENCES `capstone` (`capstoneID`),
  CONSTRAINT `statusHistoryEvent_statusCode_fk` FOREIGN KEY (`statusCode`) REFERENCES `statuses` (`code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `statushistoryevent`
--
-- ORDER BY:  `capstoneID`,`statusCode`,`timeStamp`

LOCK TABLES `statushistoryevent` WRITE;
/*!40000 ALTER TABLE `statushistoryevent` DISABLE KEYS */;
/*!40000 ALTER TABLE `statushistoryevent` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `students`
--

DROP TABLE IF EXISTS `students`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `students` (
  `userID` int(10) unsigned NOT NULL,
  `mastersStart` int(5) unsigned zerofill NOT NULL,
  PRIMARY KEY (`userID`),
  KEY `students_mastersStart_fk` (`mastersStart`),
  CONSTRAINT `students_mastersStart_fk` FOREIGN KEY (`mastersStart`) REFERENCES `term` (`code`),
  CONSTRAINT `students_userID_fk` FOREIGN KEY (`userID`) REFERENCES `users` (`userID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `students`
--
-- ORDER BY:  `userID`

LOCK TABLES `students` WRITE;
/*!40000 ALTER TABLE `students` DISABLE KEYS */;
INSERT INTO `students` (`userID`, `mastersStart`) VALUES (3,02171),(4,02178);
/*!40000 ALTER TABLE `students` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `term`
--

DROP TABLE IF EXISTS `term`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `term` (
  `code` int(5) unsigned zerofill NOT NULL,
  `termStart` date NOT NULL DEFAULT '2000-01-01',
  `termEnd` date NOT NULL DEFAULT '2000-01-01',
  `gradeDeadline` date NOT NULL DEFAULT '2000-01-01',
  `addDropDeadline` date NOT NULL DEFAULT '2000-01-01',
  PRIMARY KEY (`code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `term`
--
-- ORDER BY:  `code`

LOCK TABLES `term` WRITE;
/*!40000 ALTER TABLE `term` DISABLE KEYS */;
INSERT INTO `term` (`code`, `termStart`, `termEnd`, `gradeDeadline`, `addDropDeadline`) VALUES (02171,'2017-08-19','2018-01-01','2017-12-21','2017-09-05'),(02175,'2018-01-08','2018-05-12','2018-05-10','2018-01-23'),(02178,'2018-05-17','2018-08-16','2018-08-16','2018-05-24');
/*!40000 ALTER TABLE `term` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `useremails`
--

DROP TABLE IF EXISTS `useremails`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `useremails` (
  `userID` int(10) unsigned NOT NULL,
  `email` varchar(45) NOT NULL,
  `emailType` int(10) unsigned NOT NULL,
  PRIMARY KEY (`userID`,`email`),
  KEY `userEmails_emailType_fk` (`emailType`),
  CONSTRAINT `userEmails_emailType_fk` FOREIGN KEY (`emailType`) REFERENCES `emailtypes` (`code`),
  CONSTRAINT `userEmails_userID_fk` FOREIGN KEY (`userID`) REFERENCES `users` (`userID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `useremails`
--
-- ORDER BY:  `userID`,`email`

LOCK TABLES `useremails` WRITE;
/*!40000 ALTER TABLE `useremails` DISABLE KEYS */;
INSERT INTO `useremails` (`userID`, `email`, `emailType`) VALUES (1,'jmgla@rit.edu',3),(1,'john.doe@gmail.com',2),(1,'johnnyrockets97@gmail.com',4),(1,'jxd1234@rit.edu',1),(2,'jxd1235@rit.edu',1),(3,'jxd1236@rit.edu',1),(4,'jmd1234@rit.edu',1),(5,'jos1234@rit.edu',1),(6,'jis1234@rit.edu',1),(7,'chb1234@rit.edu',1);
/*!40000 ALTER TABLE `useremails` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `userphones`
--

DROP TABLE IF EXISTS `userphones`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `userphones` (
  `userID` int(10) unsigned NOT NULL,
  `number` varchar(15) NOT NULL,
  `phoneType` int(10) unsigned NOT NULL,
  PRIMARY KEY (`userID`,`number`),
  KEY `userPhones_phoneType_fk` (`phoneType`),
  CONSTRAINT `userPhones_phoneType_fk` FOREIGN KEY (`phoneType`) REFERENCES `phonetypes` (`code`),
  CONSTRAINT `userPhones_userID_fk` FOREIGN KEY (`userID`) REFERENCES `users` (`userID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `userphones`
--
-- ORDER BY:  `userID`,`number`

LOCK TABLES `userphones` WRITE;
/*!40000 ALTER TABLE `userphones` DISABLE KEYS */;
INSERT INTO `userphones` (`userID`, `number`, `phoneType`) VALUES (1,'10001234567',1),(1,'13471234567',3),(2,'10001234567',1),(2,'13471234567',3),(3,'10001234567',1),(3,'13471234567',3),(4,'10001234567',1),(4,'13471234567',3),(5,'10001234567',1),(5,'13471234567',3),(6,'10001234567',1),(6,'13471234567',3),(7,'10001234567',1),(7,'13471234567',3);
/*!40000 ALTER TABLE `userphones` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `userroles`
--

DROP TABLE IF EXISTS `userroles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `userroles` (
  `userID` int(10) unsigned NOT NULL,
  `roleCode` int(10) unsigned NOT NULL,
  `currentStatus` int(10) unsigned NOT NULL,
  `statusTimestamp` datetime NOT NULL,
  PRIMARY KEY (`userID`,`roleCode`),
  KEY `userRoles_roleCode_fk` (`roleCode`),
  KEY `userRoles_currentStatus_fk` (`currentStatus`),
  CONSTRAINT `userRoles_currentStatus_fk` FOREIGN KEY (`currentStatus`) REFERENCES `statuses` (`code`),
  CONSTRAINT `userRoles_roleCode_fk` FOREIGN KEY (`roleCode`) REFERENCES `roles` (`code`),
  CONSTRAINT `userRoles_userID_fk` FOREIGN KEY (`userID`) REFERENCES `users` (`userID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `userroles`
--
-- ORDER BY:  `userID`,`roleCode`

LOCK TABLES `userroles` WRITE;
/*!40000 ALTER TABLE `userroles` DISABLE KEYS */;
INSERT INTO `userroles` (`userID`, `roleCode`, `currentStatus`, `statusTimestamp`) VALUES (1,2,100,'2018-04-03 10:55:59'),(2,2,100,'2018-04-03 10:55:59'),(2,4,100,'2018-04-03 10:55:59'),(3,1,100,'2018-04-03 10:55:59'),(4,1,100,'2018-04-03 10:55:59'),(5,3,100,'2018-04-03 10:55:59'),(6,3,100,'2018-04-03 10:55:59'),(7,3,100,'2018-04-03 10:55:59'),(7,5,100,'2018-04-03 10:55:59');
/*!40000 ALTER TABLE `userroles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `users` (
  `userID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `username` varchar(7) NOT NULL,
  `password` varchar(256) NOT NULL,
  `firstName` varchar(45) NOT NULL,
  `lastName` varchar(45) NOT NULL,
  PRIMARY KEY (`userID`),
  UNIQUE KEY `username_UNIQUE` (`username`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--
-- ORDER BY:  `userID`

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` (`userID`, `username`, `password`, `firstName`, `lastName`) VALUES (1,'jxd1234', SHA2('PASSWORD', 0),'John','Doe'),(2,'jxd1235',SHA2('PASSWORD', 0),'Jane','Doe'),(3,'jxd1236',SHA2('PASSWORD', 0),'Jim','Doe'),(4,'jmd1234',SHA2('PASSWORD', 0),'Jimmy','Dog'),(5,'jos1234',SHA2('PASSWORD', 0),'John','Smith'),(6,'jas1234',SHA2('PASSWORD', 0),'Jane','Smith'),(7,'jis1234',SHA2('PASSWORD', 0),'Jim','Smith'),(8,'chb1234',SHA2('PASSWORD', 0),'Cherry','Blossom');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'capstonedb'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2018-04-03 11:13:53
