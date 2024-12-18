WITH RandomizedNames AS (
SELECT LastNameRaw, LastNameID, ROW_NUMBER() OVER (ORDER BY NEWID()) AS NewOrder
    FROM TmpLastNames
	)
	UPDATE e
SET e.LastNameRandomized = r.LastNameRaw
FROM TmpLastNames e
JOIN RandomizedNames r ON e.LastNameID  = r.NewOrder;