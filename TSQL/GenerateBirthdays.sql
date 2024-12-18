UPDATE Patients
Set DateOfBirth =  DATEADD(DAY, PatientID % 365, DATEFROMPARTS(DATEPART(year,DateOfBirth),1,1))
from Patients
--note we already added 7 years to birthdate

--