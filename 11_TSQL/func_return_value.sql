ALTER FUNCTION [dbo].[fn_DistanceCalculate]
(
	@ALat1 FLOAT,
	@ALon1 FLOAT,
	@ALat2 FLOAT,
	@ALon2 FLOAT
)
RETURNS FLOAT
AS
BEGIN
	DECLARE @Lat1 FLOAT, @Lat2 FLOAT, @dLat FLOAT, @dLon FLOAT, @a FLOAT
	SELECT
		@Lat1 = RADIANS(@ALat1),
		@Lat2 = RADIANS(@ALat2),
		@dLat = RADIANS(@ALat2 - @ALat1), 
		@dLon = RADIANS(@ALon2 - @ALon1)

	SET @a = sin(@dLat/2) * sin(@dLat/2) + sin(@dLon/2) * sin(@dLon/2) * cos(@lat1) * cos(@lat2)

	return 6371/*Earth radius*/ * 1000/*meters per kilometers*/ * (2 * atn2(sqrt(@a), sqrt(1-@a)))
END