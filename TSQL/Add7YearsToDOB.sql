UPDATE Patients SET DateOfBirth = DATEADD(year, 7, DateOfBirth) --WHERE PatientID = 0;
go


SELECT TOP (1000) [PatientID]
      ,[DisplayName]
      ,[Salutation]
      ,[DateOfBirth]
      ,[Gender]
      ,[ChartNum]
      ,[SSN]
      ,[PrimaryDocID]
      ,[Comment]
      ,[SimilarNamesAlert]
      ,[WhoLastEditedID]
      ,[WhenLastEdited]
      ,[IsActive]
      ,[IsDeceased]
      ,[CareSparkYesNo]
      ,[SSNLast4]
      ,[Race]
      ,[Ethnicity]
      ,[RcopiaID]
      ,[SubGroup]
      ,[MedicareID]
      ,[GenderIdentity]
      ,[SexualOrientation]
      ,[RacesCodified]
      ,[EthnicitiesCodified]
      ,[Language]
      ,[DateOfDeath]
  FROM [MM4].[dbo].[Patients]
