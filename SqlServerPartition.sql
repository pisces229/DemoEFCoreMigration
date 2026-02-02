-- CREATE PARTITION
ALTER DATABASE [Temp]  
ADD FILEGROUP partitionFileGroup1;  
GO  
ALTER DATABASE [Temp]  
ADD FILEGROUP partitionFileGroup2;  
GO  
ALTER DATABASE [Temp]  
ADD FILEGROUP partitionFileGroup3;  
GO  
ALTER DATABASE [Temp]  
ADD FILEGROUP partitionFileGroup4;   

ALTER DATABASE [Temp]   
ADD FILE   
(  
    NAME = partitionFileGroup1,  
    FILENAME = 'd:\Database\partitionFileName1.ndf',  
    SIZE = 5MB,  
    FILEGROWTH = 5MB  
)  
TO FILEGROUP partitionFileGroup1;  
ALTER DATABASE [Temp]   
ADD FILE   
(  
    NAME = partitionFileGroup2,  
    FILENAME = 'd:\Database\partitionFileName2.ndf',  
    SIZE = 5MB,  
    FILEGROWTH = 5MB  
)  
TO FILEGROUP partitionFileGroup2;  
GO  
ALTER DATABASE [Temp]   
ADD FILE   
(  
    NAME = partitionFileGroup3,  
    FILENAME = 'd:\Database\partitionFileName3.ndf',  
    SIZE = 5MB,  
    FILEGROWTH = 5MB  
)  
TO FILEGROUP partitionFileGroup3;  
GO  
ALTER DATABASE [Temp]   
ADD FILE   
(  
    NAME = partitionFileGroup4,  
    FILENAME = 'd:\Database\partitionFileName4.ndf',  
    SIZE = 5MB,  
    FILEGROWTH = 5MB  
)  
TO FILEGROUP partitionFileGroup4;  
GO  

CREATE PARTITION FUNCTION partitionRangeFunction (datetime)  
    AS RANGE RIGHT FOR VALUES ('2022-04-01', '2022-05-01', '2022-06-01') ;  
GO  

CREATE PARTITION SCHEME partitionRangeScheme  
    AS PARTITION partitionRangeFunction  
    TO (partitionFileGroup1, partitionFileGroup2, partitionFileGroup3, partitionFileGroup4) ;  
GO  

CREATE TABLE PartitionTable (col1 datetime, col2 varchar(10))  
    ON partitionRangeScheme (col1) ;  
GO  

CREATE NONCLUSTERED INDEX [idx_PartitionTable_col1] ON [dbo].[PartitionTable]
(
	[col1] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [idx_PartitionTable_col2] ON [dbo].[PartitionTable]
(
	[col2] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

INSERT INTO PartitionTable 
(col1, col2)
VALUES
('2022-03-31', '1'),
('2022-04-01', '2'),
('2022-04-02', '3'),
('2022-04-30', '4'),
('2022-05-01', '5'),
('2022-05-02', '6'),
('2022-05-31', '7'),
('2022-06-01', '8'),
('2022-06-02', '9')
GO

-- ADD PARTITION
ALTER DATABASE [Temp]
ADD FILEGROUP partitionFileGroup5;
GO

ALTER DATABASE [Temp] 
ADD FILE 
(
    NAME = partitionFileGroup5,  
    FILENAME = 'd:\Database\partitionFileName5.ndf',  
    SIZE = 5120KB,
    MAXSIZE = UNLIMITED,
    FILEGROWTH = 1024KB
  
)
TO FILEGROUP partitionFileGroup5;
GO
 
ALTER PARTITION SCHEME partitionRangeScheme NEXT USED [partitionFileGroup5];
ALTER PARTITION FUNCTION partitionRangeFunction() SPLIT RANGE ('2022-07-01');
GO
 
INSERT INTO PartitionTable 
(col1, col2)
VALUES
('2022-06-30', '10'),
('2022-07-01', '11'),
('2022-07-02', '12')
GO

-- MERGE PARTITION
ALTER PARTITION FUNCTION partitionRangeFunction() MERGE RANGE ('2022-07-01')
ALTER DATABASE [Temp] REMOVE FILE [partitionFileGroup5]
ALTER DATABASE [Temp] REMOVE FILEGROUP partitionFileGroup5

-- PARTITION INFOMATION
SELECT
OBJECT_NAME(p.object_id) as TableName
,p.partition_number as PartitionNumber
,prv_left.value as LowerBoundary
,prv_right.value as  UpperBoundary
,ps.name as PartitionScheme
,pf.name as PartitionFunction
,fg.name as FileGroupName
,CAST(p.used_page_count * 8.0 / 1024 AS NUMERIC(18,2)) AS UsedPages_MB
,p.row_count as Rows
FROM  sys.dm_db_partition_stats p
INNER JOIN sys.indexes i ON i.object_id = p.object_id AND i.index_id = p.index_id
INNER JOIN sys.partition_schemes ps ON ps.data_space_id = i.data_space_id
INNER JOIN sys.partition_functions pf ON ps.function_id = pf.function_id
INNER JOIN sys.destination_data_spaces dds ON dds.partition_scheme_id = ps.data_space_id AND dds.destination_id = p.partition_number
INNER JOIN sys.filegroups fg ON fg.data_space_id = dds.data_space_id
LEFT  JOIN sys.partition_range_values prv_right ON prv_right.function_id = ps.function_id AND prv_right.boundary_id = p.partition_number
LEFT  JOIN sys.partition_range_values prv_left  ON prv_left.function_id = ps.function_id AND prv_left.boundary_id = p.partition_number - 1
WHERE p.object_id = OBJECT_ID('PartitionTable') and p.index_id < 2

-- PARTITION INDEX INFOMATION
SELECT 
OBJECT_SCHEMA_NAME(t.object_id) AS schema_name
,t.name AS table_name
,i.index_id
,i.name AS index_name
,p.partition_number
,fg.name AS filegroup_name
,FORMAT(p.rows, '#,###') AS rows
FROM sys.tables t
INNER JOIN sys.indexes i ON t.object_id = i.object_id
INNER JOIN sys.partitions p ON i.object_id=p.object_id AND i.index_id=p.index_id
LEFT OUTER JOIN sys.partition_schemes ps ON i.data_space_id=ps.data_space_id
LEFT OUTER JOIN sys.destination_data_spaces dds ON ps.data_space_id=dds.partition_scheme_id AND p.partition_number=dds.destination_id
INNER JOIN sys.filegroups fg ON COALESCE(dds.data_space_id, i.data_space_id)=fg.data_space_id
WHERE t.name = 'PartitionTable'
