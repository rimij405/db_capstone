DROP DATABASE IF EXISTS capstoneDB;
CREATE DATABASE capstoneDB;
USE capstoneDB;

DROP TABLE IF EXISTS `users`;
CREATE TABLE `users` (
  `userID` int(11) NOT NULL DEFAULT '0',
  `username` VARCHAR(45) NOT NULL DEFAULT '',
  `password` VARCHAR(45) NOT NULL DEFAULT '',
  `firstName` VARCHAR(45) NOT NULL DEFAULT '',
  `lastName` VARCHAR(45) NOT NULL DEFAULT '',
  PRIMARY KEY (`userID`)
);

DROP TABLE IF EXISTS `statuses`;
CREATE TABLE `statuses` (
  `code` int(5) NOT NULL DEFAULT '0',
  `name` VARCHAR(45) NOT NULL DEFAULT '',
  `description` VARCHAR(45) DEFAULT '',
  PRIMARY KEY (`code`)
);

DROP TABLE IF EXISTS `term`;
CREATE TABLE `term` (
  `termCode` int(4) NOT NULL DEFAULT '0',
  `termStart` DATE NOT NULL DEFAULT '2000-01-01',
  `termEnd` DATE NOT NULL DEFAULT '2000-01-01',
  `gradeDeadline` DATE NOT NULL DEFAULT '2000-01-01',
  `addDropDeadline` DATE NOT NULL DEFAULT '2000-01-01',
  PRIMARY KEY (`termCode`)
);

DROP TABLE IF EXISTS `students`;
CREATE TABLE `students` (
  `userID` int(11) NOT NULL DEFAULT '0',
  `mastersStart` int(4) NOT NULL DEFAULT '0',
  PRIMARY KEY (`userID`),
  CONSTRAINT `students_userID_fk` FOREIGN KEY (`userID`) REFERENCES `users` (`userID`),
  CONSTRAINT `students_mastersStart_fk` FOREIGN KEY (`mastersStart`) REFERENCES `term` (`termCode`)
);

DROP TABLE IF EXISTS `phoneTypes`;
CREATE TABLE `phoneTypes` (
	`phoneCode` int(1) NOT NULL DEFAULT '0',
    `name` VARCHAR(45) NOT NULL DEFAULT '',
    `description` VARCHAR(45) DEFAULT '',
    PRIMARY KEY (`phoneCode`)
);

DROP TABLE IF EXISTS `userPhones`;
CREATE TABLE `userPhones` (
	`userID` int(11) NOT NULL DEFAULT '0',
    `number` VARCHAR(15) NOT NULL DEFAULT '',
    `phoneType` int(1) NOT NULL DEFAULT '0',
    PRIMARY KEY (`userID`, `number`),
    CONSTRAINT `userPhones_userID_fk` FOREIGN KEY (`userID`) REFERENCES `users` (`userID`),
    CONSTRAINT `userPhones_phoneType_fk` FOREIGN KEY (`phoneType`) REFERENCES `phoneTypes` (`phoneCode`)
);

DROP TABLE IF EXISTS `emailTypes`;
CREATE TABLE `emailTypes` (
	`emailCode` int(1) NOT NULL DEFAULT '0',
    `name` VARCHAR(45) NOT NULL DEFAULT '',
    `description` VARCHAR(45) DEFAULT '',
    PRIMARY KEY (`emailCode`)
);

DROP TABLE IF EXISTS `userEmails`;
CREATE TABLE `userEmails` (
	`userID` int(11) NOT NULL DEFAULT '0',
    `email` VARCHAR(45) NOT NULL DEFAULT '',
    `emailType` int(1) NOT NULL DEFAULT '0',
    PRIMARY KEY (`userID`, `email`),
    CONSTRAINT `userEmails_userID_fk` FOREIGN KEY (`userID`) REFERENCES `users` (`userID`),
    CONSTRAINT `userEmails_emailType_fk` FOREIGN KEY (`emailType`) REFERENCES `emailTypes` (`emailCode`)
);

DROP TABLE IF EXISTS `roles`;
CREATE TABLE `roles` (
	`code` int(1) NOT NULL DEFAULT '0',
    `name` VARCHAR(45) NOT NULL DEFAULT '',
    `description` VARCHAR(45) DEFAULT '',
    PRIMARY KEY (`code`)
);

DROP TABLE IF EXISTS `userRoles`;
CREATE TABLE `userRoles` (
	`userID` int(11) NOT NULL DEFAULT '0',
    `roleCode` int(1) NOT NULL DEFAULT '0',
    `currentStatus` int(5) NOT NULL DEFAULT '0',
    `statusTimestamp` DATETIME DEFAULT '2000-01-01 00:00:00',
    PRIMARY KEY (`userID`, `roleCode`),
    CONSTRAINT `userRoles_userID_fk` FOREIGN KEY (`userID`) REFERENCES `users` (`userID`),
    CONSTRAINT `userRoles_roleCode_fk` FOREIGN KEY (`roleCode`) REFERENCES `roles` (`code`),
    CONSTRAINT `userRoles_currentStatus_fk` FOREIGN KEY (`currentStatus`) REFERENCES `statuses` (`code`)
);

DROP TABLE IF EXISTS `capstone`;
CREATE TABLE `capstone` (
  `capstoneID` int(11) NOT NULL DEFAULT '0',
  `studentID` int(11) NOT NULL DEFAULT '0',
  `chairID` int(11) NOT NULL DEFAULT '0',
  `capstoneStartTerm` int(4) NOT NULL DEFAULT '0',
  `defenseDateApprovedBy` int(11) NOT NULL DEFAULT '0',
  `defenseDate` DATETIME DEFAULT '2000-01-01 00:00:00',
  `title` VARCHAR(100) DEFAULT '',
  `abstract` VARCHAR(140) DEFAULT '',
  `plagarismScore` INT,
  `grade` INT(2) DEFAULT '0',
  PRIMARY KEY (`capstoneID`),
  CONSTRAINT `capstone_studentID_fk` FOREIGN KEY (`studentID`) REFERENCES `users` (`userID`),
  CONSTRAINT `capstone_chairID_fk` FOREIGN KEY (`chairID`) REFERENCES `users` (`userID`),
  CONSTRAINT `capstone_capstoneStartTerm_fk` FOREIGN KEY (`capstoneStartTerm`) REFERENCES `term` (`termCode`),
  CONSTRAINT `capstone_defenseDateApprovedBy_fk` FOREIGN KEY (`defenseDateApprovedBy`) REFERENCES `users` (`userID`)
);

DROP TABLE IF EXISTS `statusHistoryEvent`;
CREATE TABLE `statusHistoryEvent` (
  `capstoneID` int(11) NOT NULL DEFAULT '0',
  `statusCode` int(5) NOT NULL DEFAULT '0',
  `timeStamp` DATETIME NOT NULL DEFAULT '2000-01-01 00:00:00',
  PRIMARY KEY (`capstoneID`, `statusCode`, `timeStamp`),
  CONSTRAINT `statusHistoryEvent_capstoneID_fk` FOREIGN KEY (`capstoneID`) REFERENCES `capstone` (`capstoneID`),
  CONSTRAINT `statusHistoryEvent_statusCode_fk` FOREIGN KEY (`statusCode`) REFERENCES `statuses` (`code`)
);

DROP TABLE IF EXISTS `capstoneFollowers`;
CREATE TABLE `capstoneFollowers` (
  `capstoneID` int(11) NOT NULL DEFAULT '0',
  `userID` int(11) NOT NULL DEFAULT '0',
  `trackCapstone` BOOLEAN DEFAULT '0',
  `onCommittee` BOOLEAN DEFAULT '0',
  `currentStatus` int(5) NOT NULL DEFAULT '0',
  `statusTimestamp` DATETIME DEFAULT '2000-01-01 00:00:00',
  PRIMARY KEY (`capstoneID`, `userID`),
  CONSTRAINT `capstoneFollowers_capstoneID_fk` FOREIGN KEY (`capstoneID`) REFERENCES `capstone` (`capstoneID`),
  CONSTRAINT `capstoneFollowers_userID_fk` FOREIGN KEY (`userID`) REFERENCES `users` (`userID`),
  CONSTRAINT `capstoneFollowers_currentStatus_fk` FOREIGN KEY (`currentStatus`) REFERENCES `statuses` (`code`)
);