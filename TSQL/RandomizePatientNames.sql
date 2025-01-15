--assign randomized names to Patients and to PatientNames
--we had 6604 female names -> now 13704
-- had 5827 male names now 13270
-- 12837 last names
-- 12730 Patients

------truncate table PatientNames
------insert into PatientNames (LinkID, GivenName , FamilyName)
------VALUES
------(
------(SELECT Patients.PatientID FROM Patients WHERE PatientID = 5),
------(SELECT Patients.DisplayName FROM Patients WHERE PatientID = 5), 
------'Ford'
------)


--assign randomized names to Patients and to PatientNames
--we have 6604 female names
-- 5827 male names
-- 12837 last names
use MM4

--populate PatientNames
truncate table PatientNames
insert into PatientNames (LinkID)
SELECT Patients.PatientID FROM Patients;

-- now insert random names

UPDATE PatientNames
SET Patientnames.GivenName = TmpMaleNames.MaleFirstNameRandom 
FROM (PatientNames LEFT JOIN TmpMaleNames ON TmpMaleNames.MaleFirstNameID = PatientNames.PatientNameID) LEFT JOIN Patients on Patients.PatientID = PatientNames.LinkID
Where Patients.Gender = 1 ;

UPDATE PatientNames
SET Patientnames.GivenName = TmpFemaleNames.FemaleFirstNameRandom 
FROM (PatientNames LEFT JOIN TmpFemaleNames ON TmpFemaleNames.FemaleFirstNameID = PatientNames.PatientNameID) LEFT JOIN Patients on Patients.PatientID = PatientNames.LinkID
Where Patients.Gender <> 1 ;

update PatientNames
SET PatientNames.FamilyName = TmpLastNames.LastNameRandomized
	FROM PatientNames LEFT JOIN TmpLastNames ON TmpLastNames.LastNameID = PatientNames.PatientNameID;

-- assign special values
update PatientNames
SET GivenName = '', FamilyName = 'nobody'
WHERE PatientNames.LinkID = -1;

update PatientNames
SET GivenName = 'Aaron', FamilyName = 'Aardvark'
WHERE PatientNames.LinkID = 0;


-- now assign DisplayName in Patients
update Patients set DisplayName = 'no one';
update Patients set DisplayName =  (COALESCE(PatientNames.GivenName, '')) + ' ' + (COALESCE(PatientNames.FamilyName, ''))
from Patients LEFT JOIN PatientNames ON Patients.PatientID = PatientNames.LinkID;


