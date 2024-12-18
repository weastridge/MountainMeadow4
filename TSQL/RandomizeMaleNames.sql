Select count(*) from TmpMaleNames;

WITH RandomizedNames AS (
SELECT MaleFirstNameRaw, MaleFirstNameID, ROW_NUMBER() OVER (ORDER BY NEWID()) AS NewOrder
    FROM TmpMaleNames
	)
	UPDATE e
SET e.MaleFirstNameRandom = r.MaleFirstNameRaw
FROM TmpMaleNames e
JOIN RandomizedNames r ON e.MaleFirstNameID  = r.NewOrder;