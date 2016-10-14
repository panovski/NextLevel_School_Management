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
	
	INSERT INTO Student
		(FirstName, LastName, SocialNumber, ContactPhone, Status, DateOfBirth, Address, HouseNumber, Place, CreatedBy, Gender)
	VALUES
		('Student '+ @RowString, 'Student '+ @RowString, '4515144444'+@RowString, '07044444', 1, @InsertDate, 'Adresa Student' +@RowString,
		@RowString, 'Kavadarci', 1, 1)

	SET @RowCount = @RowCount + 1
END