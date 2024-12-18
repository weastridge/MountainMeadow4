WITH RandomizedNames AS (
SELECT FemaleFirstNameRaw, FemaleFirstNameID, ROW_NUMBER() OVER (ORDER BY NEWID()) AS NewOrder
    FROM TmpFemaleNames
	)
	UPDATE e
SET e.FemaleFirstNameRandom = r.FemaleFirstNameRaw
FROM TmpFemaleNames e
JOIN RandomizedNames r ON e.FemaleFirstNameID  = r.NewOrder;