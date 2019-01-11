--CREATE DATABASE TODOLISTDB
--GO

USE TODOLISTDB
GO

--Drop table SYNCQUEUES;
DELETE FROM SYNCQUEUES;
--CREATE TABLE SYNCQUEUES(
--	id INT PRIMARY KEY IDENTITY,
--	evenid INT NOT NULL,
--	status BIT NOT NULL,
--	type INT NOT NULL
--);

--CREATE TABLE EVENS(
--	id INT PRIMARY KEY IDENTITY,
--	evenid INT NOT NULL,
--	evenname NVARCHAR(max) NOT NULL,
--	type INT NOT NULL,
--	daytime DATETIME NOT NULL,
--	notiday INT NOT NULL,
--	status BIT NOT NULL,
--	color INT NOT NULL,
--	objectid INT,
--	comment NVARCHAR(max)
--);
--GO

--USE TODOLISTDB
--GO
--INSERT INTO EVENS(evenname,type,daytime,notiday,status,color,comment) values ('abc',1,'20120618 10:34:09 AM',0,0,0,'hahaha')
--INSERT INTO EVENS(evenname,type,daytime,notiday,status,color,comment) values ('abc1',1,'20120618 10:34:09 AM',0,0,0,'hahaha')
--INSERT INTO EVENS(evenname,type,daytime,notiday,status,color,comment) values ('abc2',1,'20120618 10:34:09 AM',0,0,0,'hahaha')
--INSERT INTO EVENS(evenname,type,daytime,notiday,status,color,comment) values ('abc3',1,'20120618 10:34:09 AM',0,0,0,'hahaha')
--INSERT INTO EVENS(evenname,type,daytime,notiday,status,color,comment) values ('abc4',1,'20120618 10:34:09 AM',0,0,0,'hahaha')