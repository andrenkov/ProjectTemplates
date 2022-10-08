ALTER PROCEDURE [dbo].[_MoveAllToHistory]
  AS
declare
  @RideID INT, 
  @driverID varchar(6),
  @DefDriverID varchar(6),
  @Notes VARCHAR(255)

  declare GetRides cursor for 
    select RideID, Notes from rides with (nolock) where status in ('A','T', 'D', 'N','Z','X') 

  select @DefDriverID = min(driverID) from drivers where cabNo is not null
 
  OPEN GetRides
  FETCH NEXT FROM GetRides INTO @RideID, @Notes
  WHILE @@FETCH_STATUS = 0
  BEGIN
     select @driverID from rides where rideID = @RideID
     if @driverID is null
     begin
      set @driverID = @DefDriverID
      update rides set DriverID = @DriverID, Status = 'A'  where rideID = @RideID
     end
       
     exec MoveRideToHistory @RideID,  @Notes, 1, 1,  '5'   

     FETCH NEXT FROM GetRides INTO @RideID, @Notes
  END
  CLOSE GetRides
  DEALLOCATE GetRides