DECLARE @RowCount INT
DECLARE @RowString VARCHAR(10)
DECLARE @Random INT
DECLARE @Upper INT
DECLARE @Lower INT
DECLARE @InsertDate DATETIME
DECLARE @Student INT
DECLARE @StudentID VARCHAR(10)

SET @Lower = -730
SET @Upper = -1
SET @RowCount = 1020
SET @Student = 6

WHILE @RowCount < 2019
BEGIN
	
	SELECT @Random = ROUND(((@Upper - @Lower -1) * RAND() + @Lower), 0)
	SET @InsertDate = DATEADD(dd, @Random, GETDATE())
	
	WHILE @Student < 100
	BEGIN
	SET @StudentID = CAST(@Student AS VARCHAR(10))
	SET @RowString = CAST(@RowCount AS VARCHAR(10))
	INSERT INTO GroupStudent
		(GroupID, StudentID, Status, Discount, TotalCost,Transfered, CreatedBy)
	VALUES
		(@RowString,@StudentID,1,0,4000,0,1)
	SET @Student = @Student + 1
	END

	SET @RowCount = @RowCount + 1
END