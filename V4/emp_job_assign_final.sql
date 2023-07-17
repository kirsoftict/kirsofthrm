/*
   Saturday, August 03, 20134:11:54 PM
   User: 
   Server: .\SQLEXPRESS
   Database: 
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_emp_job_assign
	(
	id int NOT NULL IDENTITY (1, 1),
	emp_id char(10) NULL,
	emptid int NULL,
	task varbinary(50) NULL,
	position varchar(150) NULL,
	department varchar(50) NULL,
	project_id char(10) NULL,
	ishead char(10) NULL,
	date_from datetime NULL,
	date_end datetime NULL
	)  ON [PRIMARY]
GO
SET IDENTITY_INSERT dbo.Tmp_emp_job_assign ON
GO
IF EXISTS(SELECT * FROM dbo.emp_job_assign)
	 EXEC('INSERT INTO dbo.Tmp_emp_job_assign (id, emp_id, emptid, position, department, project_id, ishead, date_from, date_end)
		SELECT id, emp_id, emptid, position, department, project_id, ishead, date_from, date_end FROM dbo.emp_job_assign WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_emp_job_assign OFF
GO
DROP TABLE dbo.emp_job_assign
GO
EXECUTE sp_rename N'dbo.Tmp_emp_job_assign', N'emp_job_assign', 'OBJECT' 
GO
COMMIT
select Has_Perms_By_Name(N'dbo.emp_job_assign', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.emp_job_assign', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.emp_job_assign', 'Object', 'CONTROL') as Contr_Per 