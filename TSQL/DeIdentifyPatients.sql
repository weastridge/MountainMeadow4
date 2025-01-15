
--note this is used AFTER we truncated PatientNames and populated it with random names and concatenated them to Patients DisplayName...
-- this is to delete any other identifying information

use mm4
update Patients
set ChartNum = concat('C', PatientID + 7),
Salutation = null,
DateOfBirth = null,
SSN = null,
Comment = null,
SSNLast4 = null,
Race = null,
Ethnicity = null,
RcopiaID = null,
SubGroup = null,
MedicareID = null,
GenderIdentity = null,
SexualOrientation = null,
RacesCodified = null,
EthnicitiesCodified = null,
Language = null,
DateOfDeath = null
where PatientID > 0
