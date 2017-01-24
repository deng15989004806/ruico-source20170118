IF schema_id('system') IS NULL
    EXECUTE('CREATE SCHEMA [system]')
ALTER SCHEMA [system] TRANSFER [base].[OperateRecordArchive]
ALTER SCHEMA [system] TRANSFER [base].[OperateRecordExtend]
ALTER SCHEMA [system] TRANSFER [base].[OperateRecord]