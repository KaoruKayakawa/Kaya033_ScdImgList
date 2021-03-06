
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[vi_SHOHIN_IMG_FILE_1]') AND type in (N'V'))
DROP VIEW [dbo].[vi_SHOHIN_IMG_FILE_1]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vi_SHOHIN_IMG_FILE_1]
AS
WITH
	t1 AS (
		SELECT SIF_PIC_UNIT, SIF_SCD, SIF_SFILENAME, MAX(SIF_YMD) AS SIF_YMD
		FROM SHOHIN_IMG_FILE
		GROUP BY SIF_PIC_UNIT, SIF_SCD, SIF_SFILENAME
	),
	t2 AS (
		SELECT SIF_PIC_UNIT, SIF_IMG_PATH, SIF_SCD, SIF_SFILENAME, SIF_IMG_EXIST, SIF_YMD
		FROM SHOHIN_IMG_FILE
		WHERE SIF_IMG_TYPE = 'IMG'
	),
	t3 AS (
		SELECT SIF_PIC_UNIT, SIF_IMG_PATH, SIF_SCD, SIF_SFILENAME, SIF_IMG_EXIST, SIF_YMD
		FROM SHOHIN_IMG_FILE
		WHERE SIF_IMG_TYPE = 'DTL_IMG'
	),
	t4 AS (
		SELECT SIF_PIC_UNIT, SIF_IMG_PATH, SIF_SCD, SIF_SFILENAME, SIF_IMG_EXIST, SIF_YMD
		FROM SHOHIN_IMG_FILE
		WHERE SIF_IMG_TYPE = 'KLS_IMG'
	)
SELECT
	t1.SIF_PIC_UNIT, t1.SIF_SCD, t1.SIF_SFILENAME, t1.SIF_YMD,
	ISNULL(t2.SIF_IMG_EXIST, 2) AS IMG, ISNULL(t2.SIF_IMG_PATH, '') AS PATH_IMG,
	ISNULL(t3.SIF_IMG_EXIST, 2) AS DTL_IMG, ISNULL(t3.SIF_IMG_PATH, '') AS PATH_DTL_IMG,
	ISNULL(t4.SIF_IMG_EXIST, 2) AS KLS_IMG, ISNULL(t4.SIF_IMG_PATH, '') AS PATH_KLS_IMG
FROM t1
LEFT OUTER JOIN t2
ON t1.SIF_PIC_UNIT = t2.SIF_PIC_UNIT
	AND t1.SIF_SCD = t2.SIF_SCD
	AND t1.SIF_SFILENAME = t2.SIF_SFILENAME
LEFT OUTER JOIN t3
ON t1.SIF_PIC_UNIT = t3.SIF_PIC_UNIT
	AND t1.SIF_SCD = t3.SIF_SCD
	AND t1.SIF_SFILENAME = t3.SIF_SFILENAME
LEFT OUTER JOIN t4
ON t1.SIF_PIC_UNIT = t4.SIF_PIC_UNIT
	AND t1.SIF_SCD = t4.SIF_SCD
	AND t1.SIF_SFILENAME = t4.SIF_SFILENAME

GO
