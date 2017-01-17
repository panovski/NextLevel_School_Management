DECLARE @RowCount INT
DECLARE @RowString VARCHAR(10)
DECLARE @Random INT
DECLARE @Upper INT
DECLARE @Lower INT
DECLARE @InsertDate DATETIME

SET @Lower = -730
SET @Upper = -1
SET @RowCount = 10

WHILE @RowCount < 1900
BEGIN
	SET @RowString = CAST(@RowCount AS VARCHAR(10))
	SELECT @Random = ROUND(((@Upper - @Lower -1) * RAND() + @Lower), 0)
	SET @InsertDate = DATEADD(dd, @Random, GETDATE())
	
	INSERT INTO [Service]
		(ServiceTypeID, CustomerID, EmployeeID, Status, ToDate, CreatedBy)
	VALUES
		(5, @RowCount, 12, 1, @InsertDate, 1)

	SET @RowCount = @RowCount + 1
END