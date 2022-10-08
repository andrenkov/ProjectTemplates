GetClosestNodeALTER PROCEDURE [dbo].[GetClosestSgmAlt]
	@x float,
	@y float,
        @Radius FLOAT(53),--Ken 04/25/01
	@s int OUT
AS
DECLARE @MinLong float,@MaxLong float,@MinLat float,@MaxLat float --Ken 11/30/00
DECLARE @RDegree float --Ken 11/30/00
SET @RDegree = (@Radius * .00001) --Ken 11/30/00
SET @MinLong = @x - @RDegree --Ken 11/30/00
SET @MaxLong = @x + @RDegree --Ken 11/30/00
SET @MinLat = @y - @RDegree --Ken 11/30/00
SET @MaxLat = @y + @RDegree --Ken 11/30/00
--Ken 11/30/00
Select Top 1 @s = SgmID 
From Sgm_Vertices SV1 
Where (SV1.Long BETWEEN @MinLong AND @MaxLong) and 
      (SV1.Lat BETWEEN @MinLat AND @MaxLat) and
      (sqrt( square(@x - SV1.Long) + square(@y - SV1.Lat)) = (select Min( sqrt( square(@x - sv.Long) + square(@y - sv.Lat))) 
							      from Sgm_Vertices sv Where (SV.Long BETWEEN @MinLong AND @MaxLong) and 
                           

--###########################################
ALTER PROCEDURE [dbo].[GetClosestNode]
	@x float,
	@y float,
	@s int OUT
AS
   Select @s= nl.ID 
   from Node_List nl 
   where sqrt( square(@x - nl.Long) +
         square(@y - nl.Lat)) = (select Min( sqrt( square(@x - n.Long) + square(@y -n.Lat))) from Node_List n )



