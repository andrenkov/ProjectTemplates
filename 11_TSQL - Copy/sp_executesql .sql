ALTER Procedure [dbo].[CCMakeNewTable]
As
--### create new database #########################################################
IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'TaxiCC')
BEGIN
	
  DECLARE @tempS nvarchar(4000)
	
 SET @tempS = N'IF NOT EXISTS ( SELECT * FROM   TaxiCC..sysobjects  WHERE  name = ''CCServerTransactionLog'' AND  xtype = ''U'')
	BEGIN
		CREATE TABLE TaxiCC..CCServerTransactionLog
		(
			[CCTransID] [int] IDENTITY(1,1) NOT NULL,
			[ConfirmationNo] [int] NOT NULL,
			[CCTransDate] [datetime] NOT NULL,
			[CCType] [Varchar](50) NULL,
			[CCNumber] [Varchar](20) NOT NULL,
			[CCExpiryDate] [Varchar](20) NULL,
			[CCAmount] [money] NOT NULL,
			[CCTransType] [Varchar](50) NOT NULL,
			[CCAuthCode] [Varchar](20) NULL,
			[CCSeqn] [Varchar](20) NULL,
			[CCResponse] [Varchar](50) NULL,
			[CCResponseCode] [Varchar](50) NULL,
			[CCResponseText] [Varchar](50) NULL,
			[CCResponseStatus] [Varchar](2) NULL,
			[LoginID] [Varchar](30) NULL,
			[ClientIPAddress] [Varchar](50) NULL,
			[StationID] [int] NULL,
			[State] [int] NOT NULL,
			[CCTipsAmount] [money] NOT NULL,
			[CCTransFee] [money] NOT NULL,
			[MerchantID] [Varchar](16) NOT NULL,
			[TerminalID] [Varchar](10) NOT NULL,
			[EFTSequenceNum] [Varchar](51) NOT NULL,
			[TransTimeStamp] [Varchar](14) NULL,
			[ClientTransactionID] [int] NULL,
			[ClerkName] [Varchar](30) NULL,
			[OrigClerkName] [Varchar](30) NULL,
			[IsSwiped] [char](1) NOT NULL,
			[CCInfo] [Varchar](2000) NULL,
			CONSTRAINT [PK_CCServerTransactionLog] PRIMARY KEY NONCLUSTERED 
			([CCTransID] ASC) ON [PRIMARY]) ON [PRIMARY]
		IF NOT EXISTS ( SELECT * FROM   TaxiCC..sysobjects  WHERE  id = OBJECT_ID(N''[DF_CCServerTransactionLog_CCTipsAmount]'') AND  type = ''D'')
			ALTER TABLE TaxiCC..CCServerTransactionLog ADD  CONSTRAINT 
			[DF_CCServerTransactionLog_CCTipsAmount]  DEFAULT (0) FOR [CCTipsAmount]
		IF NOT EXISTS ( SELECT *  FROM   TaxiCC..sysobjects  WHERE  id = OBJECT_ID(N''[DF_CCServerTransactionLog_CCTransFee]'') AND  type = ''D'' )
			ALTER TABLE TaxiCC..CCServerTransactionLog ADD  CONSTRAINT [DF_CCServerTransactionLog_CCTransFee]  DEFAULT (0) FOR [CCTransFee]
		IF NOT EXISTS ( SELECT * FROM   TaxiCC..sysobjects WHERE  id = OBJECT_ID(N''[DF_CCServerTransactionLog_CCMerchantID]'') AND  type = ''D'' )
			ALTER TABLE TaxiCC..CCServerTransactionLog ADD  CONSTRAINT [DF_CCServerTransactionLog_CCMerchantID]  DEFAULT ('''') FOR [MerchantID]
		IF NOT EXISTS (  SELECT * FROM   TaxiCC..sysobjects  WHERE  id = OBJECT_ID(N''[DF_CCServerTransactionLog_CCTerminalID]'') AND  type = ''D'')
			ALTER TABLE TaxiCC..CCServerTransactionLog ADD  CONSTRAINT	[DF_CCServerTransactionLog_CCTerminalID]  DEFAULT ('''') FOR [TerminalID]
		IF NOT EXISTS ( SELECT * FROM   TaxiCC..sysobjects WHERE  id = OBJECT_ID(N''[DF_CCServerTransactionLog_EFTSequenceNum]'') AND  type = ''D''  )
			ALTER TABLE TaxiCC..CCServerTransactionLog ADD  CONSTRAINT [DF_CCServerTransactionLog_EFTSequenceNum]  DEFAULT ('''') FOR [EFTSequenceNum]
		IF NOT EXISTS ( SELECT * FROM   TaxiCC..sysobjects WHERE  id = OBJECT_ID(N''[DF_CCServerTransactionLog_ClerkName]'' ) AND  type = ''D'' )
			ALTER TABLE TaxiCC..CCServerTransactionLog ADD  CONSTRAINT [DF_CCServerTransactionLog_ClerkName]  DEFAULT ('''') FOR [ClerkName]
		IF NOT EXISTS ( SELECT * FROM   TaxiCC..sysobjects WHERE  id = OBJECT_ID(N''[DF_CCServerTransactionLog_OrigClerkName]'')AND  type = ''D'' )
			ALTER TABLE TaxiCC..CCServerTransactionLog ADD  CONSTRAINT [DF_CCServerTransactionLog_OrigClerkName]  DEFAULT ('''') FOR [OrigClerkName]		        
		IF NOT EXISTS (SELECT * FROM   dbo.sysobjects WHERE  id = OBJECT_ID(N''[DF__CCServerT__IsSwi__16DE1398]'') AND  type = ''D'')
			ALTER TABLE TaxiCC..CCServerTransactionLog ADD  DEFAULT (''F'') FOR [IsSwiped]
	  END'
	EXEC dbo.sp_executesql @tempS

	EXEC CCCopyData 
	
END




ALTER PROCEDURE [dbo].[GetDriverCurrZone] 
  @CallNumber varchar(6),
  @MapDBName varchar(50),
  @Zone varchar(6) out
AS
 set deadlock_priority low

  declare   @ZoneTypeID int,
			@SchemaID int,
			@ZoneName varchar(3), 
			@FLon float ,
			@FLat float,

			@SQLString	NVARCHAR(4000),
			@ParmDefinition NVARCHAR(4000)


 
  set @Zone = 'NOZ'
  set @ZoneTypeID = 1
  set @SchemaID = 0
  select @FLon = Long, @FLat = Lat from Drivers where CallNumber = @CallNumber

  
  select top 1 @SchemaID = SchemaID from CompaniesSetp where setupID = (select SetupID from setup)

  set @ParmDefinition = N'@FLon float,'+char(13)+
						 '@FLat float,'+char(13)+
						 '@ZoneTypeID int,'+char(13)+
						 '@SchemaID int,'+char(13)+
						 '@ZoneName varchar(3) out'+char(13)

  set @SQLString      =  'exec '+@MapDBName+'..LonLatZone @FLon, @FLat, @ZoneTypeID, @SchemaID,  @ZoneName out'
  exec sp_executesql @SQLString,@ParmDefinition, @FLon, @FLat, @ZoneTypeID, @SchemaID,  @ZoneName out



  if (@ZoneName <> 'NOZ') and (LTrim(@ZoneName) <> '')
  begin
	set @Zone = @ZoneName
	return 1
  end


  --###################################################################

  ALTER PROCEDURE [dbo].[IVR_spGetRiderRides]
  @Phone varchar(20),
  @Street varchar(255) = null,
  @Intersection varchar(255) = null
 --WITH ENCRYPTION 
AS

 set @Phone = IsNull(@Phone,'')
 set @Street = IsNull(@Street,'')
 set @Intersection = IsNull(@Intersection,'')

 declare @SQLString nvarchar(2000)
 declare @SQLWhere  nvarchar(1000)

 set @SQLString	=	'select R.RideID, R.FromTime as [Pickup Time], R.FromPhone as Phone, R.FrStreetName as Street, L.MajorIntersect as Intersection, R.Contact as Passenger,'+char(13)+ 
					'  case R.Status when ''D'' then ''Scheduled'' when ''T'' then ''To be dispatched'' when ''A'' then ''Dispatched'' end as [Order Status]'+char(13)+
					'from TaxiMan..Rides R with (nolock), TaxiMan..LimoOrder L with (nolock) '+char(13)+
					'  where R.RideID = L.RideID and R.[Status] in (''D'',''T'',''A'') and '
		
 set @SQLWhere	= '('

 if @Phone <> ''		
   set @SQLWhere	= @SQLWhere +'(R.FromPhone = '''+@Phone+''')'

 if @Street <> ''
 begin		
   if @SQLWhere = '('
	 set @SQLWhere	= @SQLWhere +'(R.FrStreetName = '''+@Street+''')'
   else
	 set @SQLWhere	= @SQLWhere +'or (R.FrStreetName = '''+@Street+''')'
 end

 if @Intersection <> ''		
 begin		
   if @SQLWhere = '('
	 set @SQLWhere	= @SQLWhere +'(L.MajorIntersect = '''+@Intersection+''')'
   else
	 set @SQLWhere	= @SQLWhere +'or (L.MajorIntersect = '''+@Intersection+''')'
 end

set @SQLWhere	= @SQLWhere +')'

set @SQLString	= @SQLString +char(13)+@SQLWhere+char(13)+' order by FromTime desc'

if Len(@SQLWhere) > 5
  exec (@SQLString)--exec sp_executesql @SQLString

