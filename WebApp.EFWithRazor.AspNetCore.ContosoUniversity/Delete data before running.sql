DBCC CHECKIDENT ('Course', RESEED, 0)
DBCC CHECKIDENT ('Enrollment', RESEED, 0)
DBCC CHECKIDENT ('Student', RESEED, 0)
delete from Course
delete from Student
delete from Enrollment

select * from Enrollment
select * from Student 
select * from Course