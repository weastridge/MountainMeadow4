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

truncate table PatientNames
insert into PatientNames (LinkID)

SELECT Patients.PatientID FROM Patients
WHERE Patients.PatientID  > -1;

-- now insert random names

UPDATE PatientNames
SET Patientnames.GivenName = TmpMaleNames.MaleFirstNameRandom 
FROM (PatientNames LEFT JOIN TmpMaleNames ON TmpMaleNames.MaleFirstNameID = PatientNames.PatientNameID) LEFT JOIN Patients on Patients.PatientID = PatientNames.LinkID
Where Patients.Gender = 1;

UPDATE PatientNames
SET Patientnames.GivenName = TmpFemaleNames.FemaleFirstNameRandom 
FROM (PatientNames LEFT JOIN TmpFemaleNames ON TmpFemaleNames.FemaleFirstNameID = PatientNames.PatientNameID) LEFT JOIN Patients on Patients.PatientID = PatientNames.LinkID
Where Patients.Gender <> 1;

update PatientNames
SET PatientNames.FamilyName = TmpLastNames.LastNameRandomized
--    got here:   FROM PatientNames LEFT JOIN TmpLastNames.LastNameID = PatientNames.PatientNameID
