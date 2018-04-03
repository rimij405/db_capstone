DROP DATABASE IF EXISTS capstoneDB;
CREATE DATABASE capstoneDB;
USE capstoneDB;

DROP TABLE IF EXISTS `users`;
CREATE TABLE `users` (
  `userID` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `username` VARCHAR(45) NOT NULL,
  `password` VARCHAR(45) NOT NULL,
  `firstName` VARCHAR(45) NOT NULL,
  `lastName` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`userID`)
);

DROP TABLE IF EXISTS `statuses`;
CREATE TABLE `statuses` (
  `code` INT(3) UNSIGNED ZEROFILL NOT NULL,
  `name` VARCHAR(45) NOT NULL,
  `description` VARCHAR(45) NULL,
  PRIMARY KEY (`code`)
);

DROP TABLE IF EXISTS `term`;
CREATE TABLE `term` (
  `termCode` INT(5) UNSIGNED ZEROFILL NOT NULL,
  `termStart` DATE NOT NULL DEFAULT '2000-01-01',
  `termEnd` DATE NOT NULL DEFAULT '2000-01-01',
  `gradeDeadline` DATE NOT NULL DEFAULT '2000-01-01',
  `addDropDeadline` DATE NOT NULL DEFAULT '2000-01-01',
  PRIMARY KEY (`termCode`)
);

DROP TABLE IF EXISTS `students`;
CREATE TABLE `students` (
  `userID` INT UNSIGNED NOT NULL,
  `mastersStart` INT(5) UNSIGNED ZEROFILL NOT NULL,
  PRIMARY KEY (`userID`),
  CONSTRAINT `students_userID_fk` FOREIGN KEY (`userID`) REFERENCES `users` (`userID`),
  CONSTRAINT `students_mastersStart_fk` FOREIGN KEY (`mastersStart`) REFERENCES `term` (`termCode`)
);

DROP TABLE IF EXISTS `phoneTypes`;
CREATE TABLE `phoneTypes` (
	`phoneCode` INT UNSIGNED NOT NULL,
    `name` VARCHAR(45) NOT NULL,
    `description` VARCHAR(45) NULL,
    PRIMARY KEY (`phoneCode`)
);

DROP TABLE IF EXISTS `userPhones`;
CREATE TABLE `userPhones` (
	`userID` INT UNSIGNED NOT NULL,
    `number` VARCHAR(15) NOT NULL,
    `phoneType` INT UNSIGNED NOT NULL,
    PRIMARY KEY (`userID`, `number`),
    CONSTRAINT `userPhones_userID_fk` FOREIGN KEY (`userID`) REFERENCES `users` (`userID`),
    CONSTRAINT `userPhones_phoneType_fk` FOREIGN KEY (`phoneType`) REFERENCES `phoneTypes` (`phoneCode`)
);

DROP TABLE IF EXISTS `emailTypes`;
CREATE TABLE `emailTypes` (
	`emailCode` INT UNSIGNED NOT NULL,
    `name` VARCHAR(45) NOT NULL,
    `description` VARCHAR(45) NULL,
    PRIMARY KEY (`emailCode`)
);

DROP TABLE IF EXISTS `userEmails`;
CREATE TABLE `userEmails` (
	`userID` INT UNSIGNED NOT NULL,
    `email` VARCHAR(45) NOT NULL,
    `emailType` INT UNSIGNED NOT NULL,
    PRIMARY KEY (`userID`, `email`),
    CONSTRAINT `userEmails_userID_fk` FOREIGN KEY (`userID`) REFERENCES `users` (`userID`),
    CONSTRAINT `userEmails_emailType_fk` FOREIGN KEY (`emailType`) REFERENCES `emailTypes` (`emailCode`)
);

DROP TABLE IF EXISTS `roles`;
CREATE TABLE `roles` (
	`code` INT UNSIGNED NOT NULL,
    `name` VARCHAR(45) NOT NULL,
    `description` VARCHAR(45) NULL,
    PRIMARY KEY (`code`)
);

DROP TABLE IF EXISTS `userRoles`;
CREATE TABLE `userRoles` (
	`userID` INT UNSIGNED NOT NULL,
    `roleCode` INT UNSIGNED NOT NULL,
    `currentStatus` INT UNSIGNED NOT NULL,
    `statusTimestamp` DATETIME NOT NULL,
    PRIMARY KEY (`userID`, `roleCode`),
    CONSTRAINT `userRoles_userID_fk` FOREIGN KEY (`userID`) REFERENCES `users` (`userID`),
    CONSTRAINT `userRoles_roleCode_fk` FOREIGN KEY (`roleCode`) REFERENCES `roles` (`code`),
    CONSTRAINT `userRoles_currentStatus_fk` FOREIGN KEY (`currentStatus`) REFERENCES `statuses` (`code`)
);

DROP TABLE IF EXISTS `capstone`;
CREATE TABLE `capstone` (
  `capstoneID` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `studentID` INT UNSIGNED NOT NULL,
  `chairID` INT UNSIGNED NOT NULL,
  `capstoneStartTerm` INT(5) UNSIGNED ZEROFILL NOT NULL,
  `defenseDateApprovedBy` INT UNSIGNED NULL,
  `defenseDate` DATETIME NULL,
  `title` VARCHAR(100) NOT NULL,
  `abstract` VARCHAR(140) NOT NULL,
  `plagarismScore` INT UNSIGNED NULL,
  `grade` INT UNSIGNED NULL,
  PRIMARY KEY (`capstoneID`),
  CONSTRAINT `capstone_studentID_fk` FOREIGN KEY (`studentID`) REFERENCES `users` (`userID`),
  CONSTRAINT `capstone_chairID_fk` FOREIGN KEY (`chairID`) REFERENCES `users` (`userID`),
  CONSTRAINT `capstone_capstoneStartTerm_fk` FOREIGN KEY (`capstoneStartTerm`) REFERENCES `term` (`termCode`),
  CONSTRAINT `capstone_defenseDateApprovedBy_fk` FOREIGN KEY (`defenseDateApprovedBy`) REFERENCES `users` (`userID`)
);

DROP TABLE IF EXISTS `statusHistoryEvent`;
CREATE TABLE `statusHistoryEvent` (
  `capstoneID` INT UNSIGNED NOT NULL,
  `statusCode` INT UNSIGNED NOT NULL,
  `timeStamp` DATETIME NOT NULL,
  PRIMARY KEY (`capstoneID`, `statusCode`, `timeStamp`),
  CONSTRAINT `statusHistoryEvent_capstoneID_fk` FOREIGN KEY (`capstoneID`) REFERENCES `capstone` (`capstoneID`),
  CONSTRAINT `statusHistoryEvent_statusCode_fk` FOREIGN KEY (`statusCode`) REFERENCES `statuses` (`code`)
);

DROP TABLE IF EXISTS `capstoneFollowers`;
CREATE TABLE `capstoneFollowers` (
  `capstoneID` INT UNSIGNED NOT NULL,
  `userID` INT UNSIGNED NOT NULL,
  `trackCapstone` BOOLEAN NOT NULL DEFAULT '0',
  `onCommittee` BOOLEAN NOT NULL DEFAULT '0',
  `currentStatus` INT UNSIGNED NOT NULL,
  `statusTimestamp` DATETIME NOT NULL,
  PRIMARY KEY (`capstoneID`, `userID`),
  CONSTRAINT `capstoneFollowers_capstoneID_fk` FOREIGN KEY (`capstoneID`) REFERENCES `capstone` (`capstoneID`),
  CONSTRAINT `capstoneFollowers_userID_fk` FOREIGN KEY (`userID`) REFERENCES `users` (`userID`),
  CONSTRAINT `capstoneFollowers_currentStatus_fk` FOREIGN KEY (`currentStatus`) REFERENCES `statuses` (`code`)
);