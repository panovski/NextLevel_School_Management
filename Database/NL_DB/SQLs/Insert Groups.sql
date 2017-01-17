DECLARE @RowCount INT
DECLARE @RowString VARCHAR(10)
DECLARE @Random INT
DECLARE @Upper INT
DECLARE @Lower INT
DECLARE @InsertDate DATETIME

SET @Lower = -730
SET @Upper = -1
SET @RowCount = 0

WHILE @RowCount < 1000
BEGIN
	SET @RowString = CAST(@RowCount AS VARCHAR(10))
	SELECT @Random = ROUND(((@Upper - @Lower -1) * RAND() + @Lower), 0)
	SET @InsertDate = DATEADD(dd, @Random, GETDATE())
	
	INSERT INTO [Group]
		(GroupName, GroupTypeID, StartDate, NumberOfClasses, NumberOfPayments, Cost, EmployeeID, TeacherPercentage, Status, Invoice, CreatedBy)
	VALUES
		('Gr-'+ @RowString, 5,@InsertDate, 48, 4, 4000, 12, 40, 1, 0, 1)

	SET @RowCount = @RowCount + 1
END