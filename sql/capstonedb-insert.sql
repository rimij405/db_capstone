INSERT INTO `capstonedb`.`users` (`userID`, `username`, `password`, `firstName`, `lastName`)
VALUES (1, 'jxd1234', 'ABC1234PASSWORD', 'John', 'Doe');

INSERT INTO `capstonedb`.`users` (`username`, `password`, `firstName`, `lastName`)
VALUES ('jxd1235', 'ABC1234PASSWORD', 'Jane', 'Doe');

INSERT INTO `capstonedb`.`users` (`username`, `password`, `firstName`, `lastName`)
VALUES ('jxd1236', 'ABC1234PASSWORD', 'Jim', 'Doe');

INSERT INTO `capstonedb`.`users` (`username`, `password`, `firstName`, `lastName`)
VALUES ('jmd1234', 'ABC1234PASSWORD', 'Jimmy', 'Dog');

INSERT INTO `capstonedb`.`users` (`username`, `password`, `firstName`, `lastName`)
VALUES ('jos1234', 'ABC1234PASSWORD', 'John', 'Smith');

INSERT INTO `capstonedb`.`users` (`username`, `password`, `firstName`, `lastName`)
VALUES ('jas1234', 'ABC1234PASSWORD', 'Jane', 'Smith');

INSERT INTO `capstonedb`.`users` (`username`, `password`, `firstName`, `lastName`)
VALUES ('jis1234', 'ABC1234PASSWORD', 'Jim', 'Smith');

INSERT INTO `capstonedb`.`users` (`username`, `password`, `firstName`, `lastName`)
VALUES ('chb1234', 'ABC1234PASSWORD', 'Cherry', 'Blossom');

SELECT `users`.`userID`, `users`.`username`, `users`.`password`, `users`.`firstName`, `users`.`lastName`
FROM `users`
ORDER BY
	`users`.`lastName` ASC;
    
INSERT INTO `capstonedb`.`emailtypes` (`emailCode`, `name`, `description`) VALUES ('1', 'SCHOOL', 'School email.');
INSERT INTO `capstonedb`.`emailtypes` (`emailCode`, `name`, `description`) VALUES ('2', 'PRIMARY', 'Primary email.');
INSERT INTO `capstonedb`.`emailtypes` (`emailCode`, `name`, `description`) VALUES ('3', 'WORK', 'Work email.');
INSERT INTO `capstonedb`.`emailtypes` (`emailCode`, `name`, `description`) VALUES ('4', 'PERSONAL', 'Personal email.');

SELECT * FROM capstonedb.emailtypes;

INSERT INTO `capstonedb`.`phonetypes` (`phoneCode`, `name`, `description`) VALUES ('1', 'PRIMARY', 'Primary number.');
INSERT INTO `capstonedb`.`phonetypes` (`phoneCode`, `name`, `description`) VALUES ('2', 'HOME', 'Home number.');
INSERT INTO `capstonedb`.`phonetypes` (`phoneCode`, `name`, `description`) VALUES ('3', 'MOBILE', 'Mobile number.');
INSERT INTO `capstonedb`.`phonetypes` (`phoneCode`, `name`, `description`) VALUES ('4', 'WORK', 'Work/Office number.');

SELECT * FROM capstonedb.phonetypes;

INSERT INTO `capstonedb`.`roles` (`code`, `name`, `description`) VALUES ('1', 'STUDENT', 'Student role.');
INSERT INTO `capstonedb`.`roles` (`code`, `name`, `description`) VALUES ('2', 'FACULTY', 'Faculty role.');
INSERT INTO `capstonedb`.`roles` (`code`, `name`, `description`) VALUES ('3', 'STAFF', 'Staff role.');
INSERT INTO `capstonedb`.`roles` (`code`, `name`, `description`) VALUES ('4', 'CHAIR', 'Chair role.');
INSERT INTO `capstonedb`.`roles` (`code`, `name`, `description`) VALUES ('5', 'DIRECTOR', 'Director role.');

SELECT * FROM capstonedb.roles;

INSERT INTO `capstonedb`.`statuses` (`code`, `name`, `description`) VALUES ('100', 'DEFAULT', 'Default (null) status.');
INSERT INTO `capstonedb`.`statuses` (`code`, `name`, `description`) VALUES ('200', 'CAPSTONE CREATED', 'Student made capstone.');
INSERT INTO `capstonedb`.`statuses` (`code`, `name`, `description`) VALUES ('201', 'TITLE ADDED', 'Student added title.');
INSERT INTO `capstonedb`.`statuses` (`code`, `name`, `description`) VALUES ('202', 'ABSTRACT', 'Student added abstract.');
INSERT INTO `capstonedb`.`statuses` (`code`, `name`, `description`) VALUES ('203', 'FILE', 'Student uploaded file.');
INSERT INTO `capstonedb`.`statuses` (`code`, `name`, `description`) VALUES ('300', 'ACCEPT', 'Accepted status.');
INSERT INTO `capstonedb`.`statuses` (`code`, `name`, `description`) VALUES ('400', 'REJECTED', 'Rejected status.');
INSERT INTO `capstonedb`.`statuses` (`code`, `name`, `description`) VALUES ('500', 'PENDING', 'Status pending.');
INSERT INTO `capstonedb`.`statuses` (`code`, `name`, `description`) VALUES ('600', 'ACTIVE', 'Active role since...');
INSERT INTO `capstonedb`.`statuses` (`code`, `name`, `description`) VALUES ('602', 'INACTIVE', 'Inactive role since...');

SELECT * FROM capstonedb.statuses;

INSERT INTO `capstonedb`.`term` (`termCode`, `termStart`, `termEnd`, `gradeDeadline`, `addDropDeadline`) VALUES ('2171', '2017-08-19', '2018-01-01', '2017-12-21', '2017-09-05');
INSERT INTO `capstonedb`.`term` (`termCode`, `termStart`, `termEnd`, `gradeDeadline`, `addDropDeadline`) VALUES ('2175', '2018-01-08', '2018-05-12', '2018-05-10', '2018-01-23');
INSERT INTO `capstonedb`.`term` (`termCode`, `termStart`, `termEnd`, `gradeDeadline`, `addDropDeadline`) VALUES ('2178', '2018-05-17', '2018-08-16', '2018-08-16', '2018-05-24');

SELECT * FROM capstonedb.term;

INSERT INTO `capstonedb`.`useremails` (`userID`, `email`, `emailType`) VALUES ('1', 'jxd1234@rit.edu', '1');
INSERT INTO `capstonedb`.`useremails` (`userID`, `email`, `emailType`) VALUES ('2', 'jxd1235@rit.edu', '1');
INSERT INTO `capstonedb`.`useremails` (`userID`, `email`, `emailType`) VALUES ('3', 'jxd1236@rit.edu', '1');
INSERT INTO `capstonedb`.`useremails` (`userID`, `email`, `emailType`) VALUES ('4', 'jmd1234@rit.edu', '1');
INSERT INTO `capstonedb`.`useremails` (`userID`, `email`, `emailType`) VALUES ('5', 'jos1234@rit.edu', '1');
INSERT INTO `capstonedb`.`useremails` (`userID`, `email`, `emailType`) VALUES ('6', 'jis1234@rit.edu', '1');
INSERT INTO `capstonedb`.`useremails` (`userID`, `email`, `emailType`) VALUES ('7', 'chb1234@rit.edu', '1');
INSERT INTO `capstonedb`.`useremails` (`userID`, `email`, `emailType`) VALUES ('1', 'john.doe@gmail.com', '2');
INSERT INTO `capstonedb`.`useremails` (`userID`, `email`, `emailType`) VALUES ('1', 'johnnyrockets97@gmail.com', '4');
INSERT INTO `capstonedb`.`useremails` (`userID`, `email`, `emailType`) VALUES ('1', 'jmgla@rit.edu', '3');

SELECT * FROM capstonedb.useremails;

INSERT INTO `capstonedb`.`userphones` (`userID`, `number`, `phoneType`) VALUES ('1', '10001234567', '1');
INSERT INTO `capstonedb`.`userphones` (`userID`, `number`, `phoneType`) VALUES ('1', '13471234567', '3');
INSERT INTO `capstonedb`.`userphones` (`userID`, `number`, `phoneType`) VALUES ('2', '10001234567', '1');
INSERT INTO `capstonedb`.`userphones` (`userID`, `number`, `phoneType`) VALUES ('2', '13471234567', '3');
INSERT INTO `capstonedb`.`userphones` (`userID`, `number`, `phoneType`) VALUES ('3', '10001234567', '1');
INSERT INTO `capstonedb`.`userphones` (`userID`, `number`, `phoneType`) VALUES ('3', '13471234567', '3');
INSERT INTO `capstonedb`.`userphones` (`userID`, `number`, `phoneType`) VALUES ('4', '10001234567', '1');
INSERT INTO `capstonedb`.`userphones` (`userID`, `number`, `phoneType`) VALUES ('4', '13471234567', '3');
INSERT INTO `capstonedb`.`userphones` (`userID`, `number`, `phoneType`) VALUES ('5', '10001234567', '1');
INSERT INTO `capstonedb`.`userphones` (`userID`, `number`, `phoneType`) VALUES ('5', '13471234567', '3');
INSERT INTO `capstonedb`.`userphones` (`userID`, `number`, `phoneType`) VALUES ('6', '10001234567', '1');
INSERT INTO `capstonedb`.`userphones` (`userID`, `number`, `phoneType`) VALUES ('6', '13471234567', '3');
INSERT INTO `capstonedb`.`userphones` (`userID`, `number`, `phoneType`) VALUES ('7', '10001234567', '1');
INSERT INTO `capstonedb`.`userphones` (`userID`, `number`, `phoneType`) VALUES ('7', '13471234567', '3');

SELECT * FROM capstonedb.userphones;

INSERT INTO `capstonedb`.`userroles` (`userID`, `roleCode`, `currentStatus`, `statusTimestamp`) VALUES ('1', '2', '100', '2018-04-03 10:55:59');
INSERT INTO `capstonedb`.`userroles` (`userID`, `roleCode`, `currentStatus`, `statusTimestamp`) VALUES ('2', '2', '100', '2018-04-03 10:55:59');
INSERT INTO `capstonedb`.`userroles` (`userID`, `roleCode`, `currentStatus`, `statusTimestamp`) VALUES ('2', '4', '100', '2018-04-03 10:55:59');
INSERT INTO `capstonedb`.`userroles` (`userID`, `roleCode`, `currentStatus`, `statusTimestamp`) VALUES ('3', '1', '100', '2018-04-03 10:55:59');
INSERT INTO `capstonedb`.`userroles` (`userID`, `roleCode`, `currentStatus`, `statusTimestamp`) VALUES ('4', '1', '100', '2018-04-03 10:55:59');
INSERT INTO `capstonedb`.`userroles` (`userID`, `roleCode`, `currentStatus`, `statusTimestamp`) VALUES ('5', '3', '100', '2018-04-03 10:55:59');
INSERT INTO `capstonedb`.`userroles` (`userID`, `roleCode`, `currentStatus`, `statusTimestamp`) VALUES ('6', '3', '100', '2018-04-03 10:55:59');
INSERT INTO `capstonedb`.`userroles` (`userID`, `roleCode`, `currentStatus`, `statusTimestamp`) VALUES ('7', '3', '100', '2018-04-03 10:55:59');
INSERT INTO `capstonedb`.`userroles` (`userID`, `roleCode`, `currentStatus`, `statusTimestamp`) VALUES ('7', '5', '100', '2018-04-03 10:55:59');

SELECT * FROM capstonedb.userroles;

INSERT INTO `capstonedb`.`students` (`userID`, `mastersStart`) VALUES ('3', '2171');
INSERT INTO `capstonedb`.`students` (`userID`, `mastersStart`) VALUES ('4', '2178');

SELECT * FROM capstonedb.students;

INSERT INTO `capstonedb`.`capstone` (`capstoneID`, `studentID`, `chairID`, `capstoneStartTerm`, `title`, `abstract`) VALUES ('1', '3', '2', '2171', 'Example', 'This is an example proposal.');
INSERT INTO `capstonedb`.`capstone` (`capstoneID`, `studentID`, `chairID`, `capstoneStartTerm`, `title`, `abstract`) VALUES ('2', '4', '2', '2178', 'Example 2', 'This is another example.');

SELECT * FROM capstonedb.capstone;

SELECT 
	capstone.title,
    CONCAT(u.firstName, ' ', u.lastName) AS `Name`,
    students.mastersStart AS `Program Start`,
    CONCAT(p.firstName, ' ', p.lastName) As `Chair Name`
FROM capstone
	INNER JOIN students ON(capstone.studentID = students.userID)
    INNER JOIN users u ON(capstone.studentID = u.userID)
    INNER JOIN users p ON(capstone.chairID = p.userID)
ORDER BY
	u.lastName ASC;