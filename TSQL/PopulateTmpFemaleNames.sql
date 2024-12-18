INSERT INTO TmpFemaleNames (FemaleFirstNameRaw) 
SELECT REPLACE(REPLACE(LTRIM(RTRIM(PatientNames.GivenName)), '*', ''), 'Z', '')
FROM PatientNames 
LEFT JOIN Patients On PatientNames.LinkID = Patients.PatientID 
WHERE Patients.Gender = 2 
AND PatientNames.GivenName IS NOT NULL AND LTRIM(RTRIM(REPLACE(REPLACE(LTRIM(RTRIM(PatientNames.GivenName)), '*', ''), 'Z', ''))) <> '';