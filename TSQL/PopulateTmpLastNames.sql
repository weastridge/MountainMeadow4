INSERT INTO TmpLastNames (LastNameRaw) 
SELECT REPLACE(REPLACE(LTRIM(RTRIM(PatientNames.FamilyName)), '*', ''), 'Z', '')
FROM PatientNames 
--LEFT JOIN Patients On PatientNames.LinkID = Patients.PatientID 
WHERE PatientNames.FamilyName IS NOT NULL AND LTRIM(RTRIM(REPLACE(REPLACE(LTRIM(RTRIM(PatientNames.FamilyName)), '*', ''), 'Z', ''))) <> '';
