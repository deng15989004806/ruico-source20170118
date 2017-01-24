IF schema_id('system') IS NULL
    EXECUTE('CREATE SCHEMA [system]')
CREATE TABLE [system].[SerialNumber] (
    [Id] [bigint] NOT NULL IDENTITY,
    [Prefix] [nvarchar](10),
    [DateNumber] [nvarchar](8),
    [Sequence] [int] NOT NULL,
    [Created] [datetime] NOT NULL,
    CONSTRAINT [PK_system.SerialNumber] PRIMARY KEY ([Id])
)

---------------------------------------

CREATE PROCEDURE [system].[GetSerialNumber]
(
  @Prefix NVARCHAR(16) ,
  @DateNumber NVARCHAR(8) ,
  @Increase INT 
)
AS 
BEGIN

    DECLARE @Number INT 
    DECLARE @Id BIGINT

    BEGIN TRAN    

    BEGIN TRY  
		--在同一个事务中，执行了update语句之后就会启动锁
        UPDATE  [system].[SerialNumber]
        SET     Sequence = Sequence
        WHERE   Prefix = @Prefix AND
                DateNumber = @DateNumber

        SELECT  @Number = Sequence,
                @Id = Id
        FROM    [system].[SerialNumber]
        WHERE   Prefix = @Prefix AND
                DateNumber = @DateNumber

		--表中没有记录，插入初始值
        IF @ID IS NULL 
            BEGIN

                SELECT  @Number = @Increase
                INSERT  [system].[SerialNumber]
                        ( Prefix ,
						  DateNumber,
                          Sequence,
                          Created
                        )
                VALUES  ( @Prefix ,
                          @DateNumber ,
                          @Increase ,
                          GETUTCDATE()
                        )
            END
        ELSE 
            BEGIN   
                SELECT  @Number = @Number + @Increase
                
                UPDATE  [system].[SerialNumber]
                SET     Sequence = @Number
                WHERE   Id = @Id

            END

        SELECT  @Number   

        COMMIT TRAN  

    END TRY  

    BEGIN CATCH  

        ROLLBACK TRAN  

        SELECT  0

    END CATCH  

END 


GO

--------------------------------------------

-- testing
-- exec [system].[GetSerialNumber] 'BB','',1
-- exec [system].[GetSerialNumber] 'DY','20150909',1
-- exec [system].[GetSerialNumber] 'DY','20150909',3
