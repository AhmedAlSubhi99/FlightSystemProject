SELECT DB_NAME() AS CurrentDb;

SELECT COUNT(*) AS Airports   FROM dbo.Airports;
SELECT COUNT(*) AS Aircraft   FROM dbo.Aircrafts;
SELECT COUNT(*) AS Crew       FROM dbo.CrewMembers;
SELECT COUNT(*) AS Routes     FROM dbo.Routes;
SELECT COUNT(*) AS Flights    FROM dbo.Flights;
SELECT COUNT(*) AS Passengers FROM dbo.Passengers;
SELECT COUNT(*) AS Bookings   FROM dbo.Bookings;
SELECT COUNT(*) AS Tickets    FROM dbo.Tickets;
SELECT COUNT(*) AS Baggage    FROM dbo.Baggage;

-- Flights today (UTC):
SELECT COUNT(*) AS FlightsToday
FROM dbo.Flights
WHERE DepartureUtc >= CONVERT(date, SYSUTCDATETIME())
  AND DepartureUtc  < DATEADD(day, 1, CONVERT(date, SYSUTCDATETIME()));