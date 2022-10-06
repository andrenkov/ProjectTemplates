ALTER PROCEDURE [dbo].[IBS_CheckDuplicates]
AS
  begin tran
	delete Rides where OrigCallTaker = 'WEB_BOOK' and status <> 'N' and FrStreetName is null
	delete LimoOrder where RideID not in (select RideID from Rides with (nolock))
	delete RideStop where RideID not in (select RideID from Rides with (nolock))
	delete RideCharges where RideID not in (select RideID from Rides with (nolock))
	delete RideExceptions where RideID not in (select RideID from Rides with (nolock))
  commit

  declare @FromTime DateTime, 
		  @TimeCalled DateTime, 
		  @FrStreetName varchar(255),
		  @CustOwnerID int,
		  @FromPhone varchar(20),
		  @RideID int,
		  @CheckDupl int
 
  select @CheckDupl = IsNull(CheckDupl,0) from Setup_IBS where Setup_IBS_ID > 0
  if @CheckDupl <= 0 
   goto ExitWithErrors; 


  CREATE TABLE #DuplAddress(FrStreetName VARCHAR(255));
  CREATE TABLE #DuplRideID(RideID int);

  insert into #DuplAddress select FrStreetName from Rides with (nolock) where OrigCallTaker = 'WEB_BOOK' and Status in ('T','D') group by FrStreetName having count(*) > 1

  declare GetTrip CURSOR FOR
	select RideID, FrStreetName, FromTime, CustOwnerID, TimeCalled, FromPhone from Rides with (nolock) where OrigCallTaker = 'WEB_BOOK' and Status in ('T','D') and 
		   FrStreetName in (select FrStreetName from #DuplAddress)
	 order by FrStreetName, TimeCalled
	
  OPEN GetTrip
  FETCH GetTrip INTO @RideID, @FrStreetName, @FromTime, @CustOwnerID, @TimeCalled, @FromPhone
	  
  WHILE @@Fetch_Status = 0
  BEGIN
	BEGIN TRAN
	  insert into #DuplRideID select RideID from Rides with (nolock) where FrStreetName = @FrStreetName and 
						((CustOwnerID = @CustOwnerID) or (FromPhone = @FromPhone)) and RideID <> @RideID and 
						ABS(DATEDIFF(mi, @FromTime, FromTime)) <= @CheckDupl 
	COMMIT

	FETCH NEXT FROM GetTrip INTO @RideID, @FrStreetName, @FromTime, @CustOwnerID, @TimeCalled, @FromPhone
  END --WHILE @@Fetch_Statuas = 0 

  CLOSE GetTrip
  DEALLOCATE GetTrip

  declare @RideIDDel int
  declare DeleteTrip CURSOR FOR
	select RideID from #DuplRideID
	
  OPEN DeleteTrip
  FETCH DeleteTrip INTO @RideIDDel
	  
  WHILE @@Fetch_Status = 0
  BEGIN
	BEGIN TRAN
	  exec DeleteRideAndRecur @RideIDDel, 'Deleted - IBS_CheckDuplicates'
	COMMIT

	FETCH NEXT FROM DeleteTrip INTO @RideIDDel
  END --WHILE @@Fetch_Statuas = 0 

  CLOSE DeleteTrip
  DEALLOCATE DeleteTrip

  DROP TABLE #DuplAddress;
  DROP TABLE #DuplRideID;

NormalExit: 
  RETURN 1
  
ExitWithErrors:
  RETURN 0

