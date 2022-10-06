ALTER FUNCTION [dbo].[rpt_FnRidesByDateUndSrv] (@InEmpID CabNum, @NewDate DateTime)
RETURNS @retRidesByDate TABLE 
(   Occations int NOT NULL,
    Flags     int NOT NULL,
    NumDays   int NOT NULL
)
AS
BEGIN
   WITH DirectReports(Occations, Flags, NumDays) AS
    (select Occations = isnull(sum(Occations),0), 
			Flags     = isnull(sum(Flags),0), 
            NumDays   = isnull(count(*),0)
from (
   select CallNumber, Flags = count(NULLIF(Flagged, 'F')), Occations = count(*)
    from History H WITH (NOLOCK) where CallNumber = @InEmpID
      and (FromZone in (select Zone from rptUnderSrvsdArea)
         or ToZone in (select Zone from rptUnderSrvsdArea))
      and FromTime between @NewDate and DateAdd(d, 1, @NewDate)
      and CallNumber is not null
      and RideID in (select RideID from LimoHistory where CancelNum is null)
   Group By CallNumber) A)
   INSERT @retRidesByDate
   SELECT Occations, Flags, NumDays
     FROM DirectReports 
   RETURN
END
