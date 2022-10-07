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

--#################################################################
/*
https://stackoverflow.com/questions/21011276/difference-between-temptable-and-temptable

#table  refers to a local  (visible to only the user who created it) temporary table. 
##table refers to a global (visible to all users) temporary table
*/

    qryStatisticsSql_1 =  'CREATE TABLE ##CheckedFolderIds (FolderId int NOT NULL, Parent int NOT NULL, IsTopLevel int NULL)'+#13+
                          'ALTER TABLE [##CheckedFolderIds] ADD PRIMARY KEY CLUSTERED (FolderId) ON [PRIMARY]';
    qryStatisticsSql_2 = 'INSERT ##CheckedFolderIds SELECT FolderId, Parent, CAST(0 AS int) FROM _Folders WHERE FolderId IN (%s)';
                          //STEP 3 : Get top level folders...
    qryStatisticsSql_3 = 'UPDATE t1 SET Parent = t2.Parent, IsTopLevel = 1 FROM ##CheckedFolderIds t1 INNER JOIN _Folders t2 ON t1.FolderId = t2.FolderId'+#13+
                         'UPDATE t1 SET IsTopLevel = 0 FROM ##CheckedFolderIds t1 INNER JOIN ##CheckedFolderIds t2 ON t1.Parent = t2.FolderId'+#13+
                         'SELECT FolderId, Parent INTO ##TopLevelFolders FROM ##CheckedFolderIds WHERE IsTopLevel = 1';

qryProfileFiledsToExportSql_0 = //'IF OBJECT_ID(''##AllDocInfo'') IS NULL SELECT DocId INTO ##AllDocInfo FROM _Docs'+#13+
                                  'IF OBJECT_ID(''tempdb..##ProfileFieldHeadings'') IS NOT NULL DROP TABLE ##ProfileFieldHeadings';
      qryProfileFiledsToExportSql_1 =
                          'select FldId,'+#13+
                          'CsvHeading AS Heading,'+#13+
                          'Type_ AS FldType'+#13+
                          'INTO ##ProfileFieldHeadings'+#13+
                          'FROM _PropFields WHERE Isnull(Flags, 0) & 0xFFFFFDD1 = 0 AND'+#13+
                          'Type_ IN (1, 2, 3, 4, 5, 6, 7, 8, 9) AND'+#13+
                          'FldId <> :DocDateFldId AND'+#13+
                          'FldId IN ('+#13+
                          'SELECT DISTINCT FldId FROM _PropDocs WHERE DocId IN'+#13+
                          '(SELECT DocId FROM ##AllDocInfo) AND'+#13+
                          'ISNULL(ValId, 0) = 0'+#13+
                          ')';
      qryProfileFiledsToExportSql_1b =
                          'select FldId,'+#13+
                          'CsvHeading AS Heading,'+#13+
                          'Type_ AS FldType'+#13+
                          'INTO ##ProfileFieldHeadings'+#13+
                          'FROM _PropFields WHERE Isnull(Flags, 0) & 0xFFFFFDD1 = 0 AND'+#13+
                          'Type_ IN (1, 2, 3, 4, 5, 6, 7, 8, 9) AND'+#13+
                          'FldId <> :DocDateFldId AND'+#13+
                          'FldId IN ('+#13+
                          'SELECT DISTINCT FldId FROM _PropDocs WHERE DocId IN'+#13+
                          '(SELECT DocId FROM ##AllDocInfo)'+#13+
                          ')';
      qryProfileFiledsToExportSql_2 =
                          //Update blank Headings...
                          'UPDATE t1 SET Heading = t2.ColHeading FROM'+#13+
                          '##ProfileFieldHeadings t1 INNER JOIN'+#13+
                          '_PropFields t2 ON t1.FldId = t2.FldId AND'+#13+
                          'LEN(ISNULL(Heading, '''')) = 0';
      qryProfileFiledsToExportSql_3 =
                          'UPDATE t1 SET Heading = t2.Desc_ FROM'+#13+
                          '##ProfileFieldHeadings t1 INNER JOIN'+#13+
                          '_PropFields t2 ON t1.FldId = t2.FldId AND'+#13+
                          'LEN(ISNULL(Heading, '''')) = 0';