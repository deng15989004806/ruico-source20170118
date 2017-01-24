--------------------------------------------------MSSQL UTC时间----------------------------------

--SELECT GETUTCDATE()

--SELECT NEWID()


--------------------------------------------------管理员账号-------------------------------------
INSERT INTO [auth].[User]
           ([Id]
           ,[Name]
           ,[LoginName]
           ,[LoginPwd]
           ,[Email]
           ,[Created]
           ,[LastLoginToken]
           ,[LastLogin])
     VALUES
           ('7CFCD31C-337C-467D-9160-AC0DFA5525E4'
           ,'管理员'
           ,'admin'
           ,'EEE20F30B1BBED9EE160F5882A35129A2C4D9087'
           ,'a@b.com'
           ,'2015-07-18 01:17:08.490'
           ,null
           ,'1900-01-01')
GO

-----------------------------------------------------模块----------------------------------------
INSERT INTO [auth].[Module]
           ([Id]
           ,[Name]
           ,[SortOrder]
           ,[Created])
     VALUES
           ('470880FF-B7EC-496B-9854-F57DA8E471DF'
           ,'市场发展'
           ,1
           ,'2015-07-18 01:17:08.490')

INSERT INTO [auth].[Module]
           ([Id]
           ,[Name]
           ,[SortOrder]
           ,[Created])
     VALUES
           ('952A8616-5EAA-4960-9A44-CBA66124239E'
           ,'物流管理'
           ,2
           ,'2015-07-18 01:17:08.490')

INSERT INTO [auth].[Module]
           ([Id]
           ,[Name]
           ,[SortOrder]
           ,[Created])
     VALUES
           ('80A1D7FB-8F97-4983-8E73-68E6316C5E1F'
           ,'人事行政'
           ,3
           ,'2015-07-18 01:17:08.490')

INSERT INTO [auth].[Module]
           ([Id]
           ,[Name]
           ,[SortOrder]
           ,[Created])
     VALUES
           ('77000B67-1929-48C4-9A8C-843ED747F14C'
           ,'海运管理'
           ,4
           ,'2015-07-18 01:17:08.490')

INSERT INTO [auth].[Module]
           ([Id]
           ,[Name]
           ,[SortOrder]
           ,[Created])
     VALUES
           ('C9174EFA-C05A-47A8-BC2D-E0FC76647AEC'
           ,'系统管理'
           ,20
           ,'2015-07-18 01:17:08.490')

GO

---------------------------------------------------一级菜单----------------------------------------
INSERT INTO [auth].[Menu]
           ([Id]
           ,[Name]
           ,[Code]
           ,[Url]
           ,[SortOrder]
           ,[Created]
           ,[Module_Id]
		   ,[Parent_Menu_Id]
		   ,[Depth])
     VALUES
           ('4FE2DF84-52F9-43E1-8132-70304BF4C07B'
           ,'客户管理'
           ,''
           ,'/Market/Customer'
           ,1
           ,'2015-07-18 03:01:36.830'
           ,'470880FF-B7EC-496B-9854-F57DA8E471DF'
		   ,null
		   ,1)

INSERT INTO [auth].[Menu]
           ([Id]
           ,[Name]
           ,[Code]
           ,[Url]
           ,[SortOrder]
           ,[Created]
           ,[Module_Id]
		   ,[Parent_Menu_Id]
		   ,[Depth])
     VALUES
           ('60EABD4F-FEF1-472B-9A4D-69853DF66071'
           ,'合同管理'
           ,''
           ,'/Market/Contract'
           ,2
           ,'2015-07-18 03:01:36.830'
           ,'470880FF-B7EC-496B-9854-F57DA8E471DF'
		   ,null
		   ,1)

INSERT INTO [auth].[Menu]
           ([Id]
           ,[Name]
           ,[Code]
           ,[Url]
           ,[SortOrder]
           ,[Created]
           ,[Module_Id]
		   ,[Parent_Menu_Id]
		   ,[Depth])
     VALUES
           ('15F65B68-F76E-422A-AB18-B4CF0ECD0743'
           ,'调度管理'
           ,''
           ,'/Market/Dispatch'
           ,3
           ,'2015-07-18 03:01:36.830'
           ,'470880FF-B7EC-496B-9854-F57DA8E471DF'
		   ,null
		   ,1)

INSERT INTO [auth].[Menu]
           ([Id]
           ,[Name]
           ,[Code]
           ,[Url]
           ,[SortOrder]
           ,[Created]
           ,[Module_Id]
		   ,[Parent_Menu_Id]
		   ,[Depth])
     VALUES
           ('96437DCE-5D4C-4E76-8CB8-D05B97699E5B'
           ,'商务管理'
           ,''
           ,'/Market/Commerce'
           ,4
           ,'2015-07-18 03:01:36.830'
           ,'470880FF-B7EC-496B-9854-F57DA8E471DF'
		   ,null
		   ,1)

GO


-------系统菜单------

INSERT INTO [auth].[Menu]
           ([Id]
           ,[Name]
           ,[Code]
           ,[Url]
           ,[SortOrder]
           ,[Depth]
           ,[Created]
           ,[Module_Id]
           ,[Parent_Menu_Id])
     VALUES
           ('0D2205D6-EC5E-4951-BA62-C7056B76ABBF'
           ,'模块和菜单'
           ,''
           ,'/System/ModuleMenu'
           ,1
           ,1
           ,'2015-08-02 04:10:45.030'
           ,'C9174EFA-C05A-47A8-BC2D-E0FC76647AEC'
           ,null)

INSERT INTO [auth].[Menu]
           ([Id]
           ,[Name]
           ,[Code]
           ,[Url]
           ,[SortOrder]
           ,[Depth]
           ,[Created]
           ,[Module_Id]
           ,[Parent_Menu_Id])
     VALUES
           ('D08A71D3-D4E5-4542-A41C-1C8C3B49DA31'
           ,'角色和用户'
           ,''
           ,'/System/RoleUser'
           ,2
           ,1
           ,'2015-08-02 04:10:45.030'
           ,'C9174EFA-C05A-47A8-BC2D-E0FC76647AEC'
           ,null)
GO



--------------------------------------------------二三级菜单----------------------------------------
INSERT INTO [auth].[Menu]
           ([Id]
           ,[Name]
           ,[Code]
           ,[Url]
           ,[SortOrder]
           ,[Created]
           ,[Module_Id]
		   ,[Parent_Menu_Id]
		   ,[Depth])
     VALUES
           ('6CBA299A-BB33-45AA-A64F-7F3639ADEBD8'
           ,'功能模块'
           ,''
           ,''
           ,1
           ,'2015-07-18 03:01:36.830'
           ,'470880FF-B7EC-496B-9854-F57DA8E471DF'
		   ,'4FE2DF84-52F9-43E1-8132-70304BF4C07B'
		   ,2)

INSERT INTO [auth].[Menu]
           ([Id]
           ,[Name]
           ,[Code]
           ,[Url]
           ,[SortOrder]
           ,[Created]
           ,[Module_Id]
		   ,[Parent_Menu_Id]
		   ,[Depth])
     VALUES
           ('1DC0B09F-0331-4381-8048-A516429D3E34'
           ,'货物品名维护'
           ,''
           ,''
           ,1
           ,'2015-07-18 03:01:36.830'
           ,'470880FF-B7EC-496B-9854-F57DA8E471DF'
		   ,'6CBA299A-BB33-45AA-A64F-7F3639ADEBD8'
		   ,3)

INSERT INTO [auth].[Menu]
           ([Id]
           ,[Name]
           ,[Code]
           ,[Url]
           ,[SortOrder]
           ,[Created]
           ,[Module_Id]
		   ,[Parent_Menu_Id]
		   ,[Depth])
     VALUES
           ('4E49C419-E4B2-46D7-B7EF-3F3C2CE17BA6'
           ,'货主资料维护'
           ,''
           ,''
           ,2
           ,'2015-07-18 03:01:36.830'
           ,'470880FF-B7EC-496B-9854-F57DA8E471DF'
		   ,'6CBA299A-BB33-45AA-A64F-7F3639ADEBD8'
		   ,3)
		   

INSERT INTO [auth].[Menu]
           ([Id]
           ,[Name]
           ,[Code]
           ,[Url]
           ,[SortOrder]
           ,[Created]
           ,[Module_Id]
		   ,[Parent_Menu_Id]
		   ,[Depth])
     VALUES
           ('CCA24FD4-F36A-4C1D-93DE-213BC9F47DEF'
           ,'船东资料维护'
           ,''
           ,''
           ,3
           ,'2015-07-18 03:01:36.830'
           ,'470880FF-B7EC-496B-9854-F57DA8E471DF'
		   ,'6CBA299A-BB33-45AA-A64F-7F3639ADEBD8'
		   ,3)

-----------------------
INSERT INTO [auth].[Menu]
           ([Id]
           ,[Name]
           ,[Code]
           ,[Url]
           ,[SortOrder]
           ,[Created]
           ,[Module_Id]
		   ,[Parent_Menu_Id]
		   ,[Depth])
     VALUES
           ('8779C385-ABE3-4F74-96BD-6C8A7301B6DC'
           ,'功能模块'
           ,''
           ,''
           ,1
           ,'2015-07-18 03:01:36.830'
           ,'470880FF-B7EC-496B-9854-F57DA8E471DF'
		   ,'60EABD4F-FEF1-472B-9A4D-69853DF66071'
		   ,2)

INSERT INTO [auth].[Menu]
           ([Id]
           ,[Name]
           ,[Code]
           ,[Url]
           ,[SortOrder]
           ,[Created]
           ,[Module_Id]
		   ,[Parent_Menu_Id]
		   ,[Depth])
     VALUES
           ('8AA6A148-F0CE-475A-8F14-2F002BBC2F90'
           ,'货主合同维护'
           ,''
           ,''
           ,1
           ,'2015-07-18 03:01:36.830'
           ,'470880FF-B7EC-496B-9854-F57DA8E471DF'
		   ,'8779C385-ABE3-4F74-96BD-6C8A7301B6DC'
		   ,3)

INSERT INTO [auth].[Menu]
           ([Id]
           ,[Name]
           ,[Code]
           ,[Url]
           ,[SortOrder]
           ,[Created]
           ,[Module_Id]
		   ,[Parent_Menu_Id]
		   ,[Depth])
     VALUES
           ('BAE386FB-237A-4E1F-BF2E-53F839FE1D39'
           ,'租船合同维护'
           ,''
           ,''
           ,2
           ,'2015-07-18 03:01:36.830'
           ,'470880FF-B7EC-496B-9854-F57DA8E471DF'
		   ,'8779C385-ABE3-4F74-96BD-6C8A7301B6DC'
		   ,3)

----------------------------------------------------
INSERT INTO [auth].[Menu]
           ([Id]
           ,[Name]
           ,[Code]
           ,[Url]
           ,[SortOrder]
           ,[Created]
           ,[Module_Id]
		   ,[Parent_Menu_Id]
		   ,[Depth])
     VALUES
           ('F0D21A55-0D65-49F9-A65B-8BB9B6101509'
           ,'功能模块'
           ,''
           ,''
           ,1
           ,'2015-07-18 03:01:36.830'
           ,'470880FF-B7EC-496B-9854-F57DA8E471DF'
		   ,'15F65B68-F76E-422A-AB18-B4CF0ECD0743'
		   ,2)

INSERT INTO [auth].[Menu]
           ([Id]
           ,[Name]
           ,[Code]
           ,[Url]
           ,[SortOrder]
           ,[Created]
           ,[Module_Id]
		   ,[Parent_Menu_Id]
		   ,[Depth])
     VALUES
           ('357E658E-E433-4128-A316-4ADC7A7DEE77'
           ,'报表模块'
           ,''
           ,''
           ,2
           ,'2015-07-18 03:01:36.830'
           ,'470880FF-B7EC-496B-9854-F57DA8E471DF'
		   ,'15F65B68-F76E-422A-AB18-B4CF0ECD0743'
		   ,2)

INSERT INTO [auth].[Menu]
           ([Id]
           ,[Name]
           ,[Code]
           ,[Url]
           ,[SortOrder]
           ,[Created]
           ,[Module_Id]
		   ,[Parent_Menu_Id]
		   ,[Depth])
     VALUES
           ('4ABDB264-126A-4746-89FD-EA8D7508AE26'
           ,'货物航次维护'
           ,''
           ,''
           ,1
           ,'2015-07-18 03:01:36.830'
           ,'470880FF-B7EC-496B-9854-F57DA8E471DF'
		   ,'F0D21A55-0D65-49F9-A65B-8BB9B6101509'
		   ,3)

INSERT INTO [auth].[Menu]
           ([Id]
           ,[Name]
           ,[Code]
           ,[Url]
           ,[SortOrder]
           ,[Created]
           ,[Module_Id]
		   ,[Parent_Menu_Id]
		   ,[Depth])
     VALUES
           ('EC999EEA-3465-400B-8E9D-FBC02EDB3DB1'
           ,'码头资料管理'
           ,''
           ,''
           ,2
           ,'2015-07-18 03:01:36.830'
           ,'470880FF-B7EC-496B-9854-F57DA8E471DF'
		   ,'F0D21A55-0D65-49F9-A65B-8BB9B6101509'
		   ,3)

INSERT INTO [auth].[Menu]
           ([Id]
           ,[Name]
           ,[Code]
           ,[Url]
           ,[SortOrder]
           ,[Created]
           ,[Module_Id]
		   ,[Parent_Menu_Id]
		   ,[Depth])
     VALUES
           ('15EA8687-7BF8-4F85-B727-C6BBA9445046'
           ,'船舶代理管理'
           ,''
           ,''
           ,3
           ,'2015-07-18 03:01:36.830'
           ,'470880FF-B7EC-496B-9854-F57DA8E471DF'
		   ,'F0D21A55-0D65-49F9-A65B-8BB9B6101509'
		   ,3)

INSERT INTO [auth].[Menu]
           ([Id]
           ,[Name]
           ,[Code]
           ,[Url]
           ,[SortOrder]
           ,[Created]
           ,[Module_Id]
		   ,[Parent_Menu_Id]
		   ,[Depth])
     VALUES
           ('B318B1B5-3114-4DF8-AA79-139459B15ED9'
           ,'货物航次报告'
           ,''
           ,''
           ,1
           ,'2015-07-18 03:01:36.830'
           ,'470880FF-B7EC-496B-9854-F57DA8E471DF'
		   ,'357E658E-E433-4128-A316-4ADC7A7DEE77'
		   ,3)

INSERT INTO [auth].[Menu]
           ([Id]
           ,[Name]
           ,[Code]
           ,[Url]
           ,[SortOrder]
           ,[Created]
           ,[Module_Id]
		   ,[Parent_Menu_Id]
		   ,[Depth])
     VALUES
           ('57699B52-2ED5-4D79-ADB3-0F6B75E331A5'
           ,'船舶动态报告'
           ,''
           ,''
           ,2
           ,'2015-07-18 03:01:36.830'
           ,'470880FF-B7EC-496B-9854-F57DA8E471DF'
		   ,'357E658E-E433-4128-A316-4ADC7A7DEE77'
		   ,3)

INSERT INTO [auth].[Menu]
           ([Id]
           ,[Name]
           ,[Code]
           ,[Url]
           ,[SortOrder]
           ,[Created]
           ,[Module_Id]
		   ,[Parent_Menu_Id]
		   ,[Depth])
     VALUES
           ('E432FDEC-DFFD-4B00-97B0-8014B30CA794'
           ,'代付项目明细'
           ,''
           ,''
           ,3
           ,'2015-07-18 03:01:36.830'
           ,'470880FF-B7EC-496B-9854-F57DA8E471DF'
		   ,'357E658E-E433-4128-A316-4ADC7A7DEE77'
		   ,3)

---------------------------------------------------------------
INSERT INTO [auth].[Menu]
           ([Id]
           ,[Name]
           ,[Code]
           ,[Url]
           ,[SortOrder]
           ,[Created]
           ,[Module_Id]
		   ,[Parent_Menu_Id]
		   ,[Depth])
     VALUES
           ('8A1DE919-BDC9-471A-B67C-B51534765ED0'
           ,'物流菜单一'
           ,''
           ,'/Logistics/Menu1'
           ,1
           ,'2015-07-18 03:01:36.830'
           ,'952A8616-5EAA-4960-9A44-CBA66124239E'
		   ,null
		   ,1)

INSERT INTO [auth].[Menu]
           ([Id]
           ,[Name]
           ,[Code]
           ,[Url]
           ,[SortOrder]
           ,[Created]
           ,[Module_Id]
		   ,[Parent_Menu_Id]
		   ,[Depth])
     VALUES
           ('87E216FD-2D19-4E77-8E41-2C38F92A0DB8'
           ,'功能模块'
           ,''
           ,''
           ,1
           ,'2015-07-18 03:01:36.830'
           ,'952A8616-5EAA-4960-9A44-CBA66124239E'
		   ,'8A1DE919-BDC9-471A-B67C-B51534765ED0'
		   ,2)

INSERT INTO [auth].[Menu]
           ([Id]
           ,[Name]
           ,[Code]
           ,[Url]
           ,[SortOrder]
           ,[Created]
           ,[Module_Id]
		   ,[Parent_Menu_Id]
		   ,[Depth])
     VALUES
           ('B6023B3C-F93F-4AA6-BDA1-4F4529B86ECE'
           ,'物流功能一'
           ,''
           ,''
           ,1
           ,'2015-07-18 03:01:36.830'
           ,'952A8616-5EAA-4960-9A44-CBA66124239E'
		   ,'87E216FD-2D19-4E77-8E41-2C38F92A0DB8'
		   ,3)

INSERT INTO [auth].[Menu]
           ([Id]
           ,[Name]
           ,[Code]
           ,[Url]
           ,[SortOrder]
           ,[Created]
           ,[Module_Id]
		   ,[Parent_Menu_Id]
		   ,[Depth])
     VALUES
           ('8A1719CD-133B-46DA-93FB-43270943AAA2'
           ,'物流功能二'
           ,''
           ,''
           ,2
           ,'2015-07-18 03:01:36.830'
           ,'952A8616-5EAA-4960-9A44-CBA66124239E'
		   ,'87E216FD-2D19-4E77-8E41-2C38F92A0DB8'
		   ,3)

GO

-------系统菜单--------

INSERT INTO [auth].[Menu]
           ([Id]
           ,[Name]
           ,[Code]
           ,[Url]
           ,[SortOrder]
           ,[Depth]
           ,[Created]
           ,[Module_Id]
           ,[Parent_Menu_Id])
     VALUES
           ('6FA4DD9D-4795-453B-B622-EBAB2F9A777D'
           ,'模块管理'
           ,''
           ,'/System/Module'
           ,1
           ,2
           ,'2015-08-02 04:10:45.030'
           ,'C9174EFA-C05A-47A8-BC2D-E0FC76647AEC'
           ,'0D2205D6-EC5E-4951-BA62-C7056B76ABBF')

INSERT INTO [auth].[Menu]
           ([Id]
           ,[Name]
           ,[Code]
           ,[Url]
           ,[SortOrder]
           ,[Depth]
           ,[Created]
           ,[Module_Id]
           ,[Parent_Menu_Id])
     VALUES
           ('20CD3F8A-D9BB-4DBB-959F-4C94BCE4FEF3'
           ,'菜单管理'
           ,''
           ,'/System/Menu'
           ,2
           ,2
           ,'2015-08-02 04:10:45.030'
           ,'C9174EFA-C05A-47A8-BC2D-E0FC76647AEC'
           ,'0D2205D6-EC5E-4951-BA62-C7056B76ABBF')
GO

INSERT INTO [auth].[Menu]
           ([Id]
           ,[Name]
           ,[Code]
           ,[Url]
           ,[SortOrder]
           ,[Depth]
           ,[Created]
           ,[Module_Id]
           ,[Parent_Menu_Id])
     VALUES
           ('5150E687-DE94-4632-87C1-BC795199F48E'
           ,'角色管理'
           ,''
           ,'/System/Role'
           ,1
           ,2
           ,'2015-08-02 04:10:45.030'
           ,'C9174EFA-C05A-47A8-BC2D-E0FC76647AEC'
           ,'D08A71D3-D4E5-4542-A41C-1C8C3B49DA31')

INSERT INTO [auth].[Menu]
           ([Id]
           ,[Name]
           ,[Code]
           ,[Url]
           ,[SortOrder]
           ,[Depth]
           ,[Created]
           ,[Module_Id]
           ,[Parent_Menu_Id])
     VALUES
           ('38A6C659-4915-4D85-B67E-B9549EAC1838'
           ,'用户管理'
           ,''
           ,'/System/User'
           ,2
           ,2
           ,'2015-08-02 04:10:45.030'
           ,'C9174EFA-C05A-47A8-BC2D-E0FC76647AEC'
           ,'D08A71D3-D4E5-4542-A41C-1C8C3B49DA31')
GO

--------------------------------------------------角色组&角色----------------------------------------

INSERT INTO [auth].[RoleGroup]
           ([Id]
           ,[Name]
           ,[Description]
           ,[SortOrder]
           ,[Created])
     VALUES
           ('AA1BEDB0-1B10-4461-8346-B52BF58007FD'
           ,'管理员'
           ,''
           ,''
           ,'2015-07-30 15:08:05.567')


INSERT INTO [auth].[Role]
           ([Id]
           ,[Name]
           ,[Description]
           ,[SortOrder]
           ,[Created]
           ,[RoleGroup_Id])
     VALUES
           ('B359CDDA-9BE7-4BA4-895A-F1CA513EA361'
           ,'系统管理'
           ,''
           ,1
           ,'2015-07-30 15:09:41.447'
           ,'AA1BEDB0-1B10-4461-8346-B52BF58007FD')

INSERT INTO [auth].[RoleGroup_User]
           ([User_Id]
           ,[RoleGroup_Id])
     VALUES
           ('7CFCD31C-337C-467D-9160-AC0DFA5525E4'
           ,'AA1BEDB0-1B10-4461-8346-B52BF58007FD')

GO

INSERT INTO [auth].[RoleGroup]
           ([Id]
           ,[Name]
           ,[Description]
           ,[SortOrder]
           ,[Created])
     VALUES
           ('B4B31D7C-716E-4FAA-827E-B2F4FA864DB7'
           ,'业务管理员'
           ,''
           ,''
           ,'2015-08-02 07:04:03.890')


INSERT INTO [auth].[Role]
           ([Id]
           ,[Name]
           ,[Description]
           ,[SortOrder]
           ,[Created]
           ,[RoleGroup_Id])
     VALUES
           ('93FFFBD6-1EB6-4CC7-8AE6-3B52D26CB91E'
           ,'业务管理'
           ,''
           ,1
           ,'2015-08-02 07:04:03.890'
           ,'B4B31D7C-716E-4FAA-827E-B2F4FA864DB7')

INSERT INTO [auth].[RoleGroup_User]
           ([User_Id]
           ,[RoleGroup_Id])
     VALUES
           ('7CFCD31C-337C-467D-9160-AC0DFA5525E4'
           ,'B4B31D7C-716E-4FAA-827E-B2F4FA864DB7')

GO


--------------------------------------------------菜单权限----------------------------------------

INSERT INTO [auth].[Permission]
           ([Id]
           ,[Name]
           ,[Code]
           ,[ActionUrl]
           ,[SortOrder]
           ,[Created]
           ,[Menu_Id])
     VALUES
           ('DF08C492-9F9A-443A-AD62-50DB4B8552AF'
           ,'浏览'
           ,''
           ,''
           ,1
           ,'2015-08-01 06:20:12.477'
           ,'1DC0B09F-0331-4381-8048-A516429D3E34')

INSERT INTO [auth].[Permission]
           ([Id]
           ,[Name]
           ,[Code]
           ,[ActionUrl]
           ,[SortOrder]
           ,[Created]
           ,[Menu_Id])
     VALUES
           ('A46B0B7B-BA4A-43C6-93DD-07C5D22B5F2B'
           ,'新增'
           ,''
           ,''
           ,2
           ,'2015-08-01 06:20:12.477'
           ,'1DC0B09F-0331-4381-8048-A516429D3E34')

INSERT INTO [auth].[Permission]
           ([Id]
           ,[Name]
           ,[Code]
           ,[ActionUrl]
           ,[SortOrder]
           ,[Created]
           ,[Menu_Id])
     VALUES
           ('85D47AC6-27A5-4334-AC05-53DF940857AD'
           ,'编辑'
           ,''
           ,''
           ,3
           ,'2015-08-01 06:20:12.477'
           ,'1DC0B09F-0331-4381-8048-A516429D3E34')

INSERT INTO [auth].[Permission]
           ([Id]
           ,[Name]
           ,[Code]
           ,[ActionUrl]
           ,[SortOrder]
           ,[Created]
           ,[Menu_Id])
     VALUES
           ('4EBB5664-2F88-47FC-9C0F-B8B01DF8FD19'
           ,'删除'
           ,''
           ,''
           ,4
           ,'2015-08-01 06:20:12.477'
           ,'1DC0B09F-0331-4381-8048-A516429D3E34')
GO


INSERT INTO [auth].[Permission]
           ([Id]
           ,[Name]
           ,[Code]
           ,[ActionUrl]
           ,[SortOrder]
           ,[Created]
           ,[Menu_Id])
     VALUES
           ('D47E3575-263B-44EA-A627-33B348E910DD'
           ,'浏览1'
           ,''
           ,''
           ,1
           ,'2015-08-01 06:20:12.477'
           ,'8AA6A148-F0CE-475A-8F14-2F002BBC2F90')

INSERT INTO [auth].[Permission]
           ([Id]
           ,[Name]
           ,[Code]
           ,[ActionUrl]
           ,[SortOrder]
           ,[Created]
           ,[Menu_Id])
     VALUES
           ('31BA9226-4B7C-4FD9-9C99-B158A2E44DAC'
           ,'新增1'
           ,''
           ,''
           ,2
           ,'2015-08-01 06:20:12.477'
           ,'8AA6A148-F0CE-475A-8F14-2F002BBC2F90')

INSERT INTO [auth].[Permission]
           ([Id]
           ,[Name]
           ,[Code]
           ,[ActionUrl]
           ,[SortOrder]
           ,[Created]
           ,[Menu_Id])
     VALUES
           ('C992A9BD-5D56-4F75-9081-A1329C62B76A'
           ,'编辑1'
           ,''
           ,''
           ,3
           ,'2015-08-01 06:20:12.477'
           ,'8AA6A148-F0CE-475A-8F14-2F002BBC2F90')

INSERT INTO [auth].[Permission]
           ([Id]
           ,[Name]
           ,[Code]
           ,[ActionUrl]
           ,[SortOrder]
           ,[Created]
           ,[Menu_Id])
     VALUES
           ('0B02567F-DB44-4C7B-A9F3-DCA8915DBD0A'
           ,'删除1'
           ,''
           ,''
           ,4
           ,'2015-08-01 06:20:12.477'
           ,'8AA6A148-F0CE-475A-8F14-2F002BBC2F90')
GO

--------------------------------------------------  系统管理权限  ----------------------------------------

INSERT INTO [auth].[Permission]
           ([Id]
           ,[Name]
           ,[Code]
           ,[ActionUrl]
           ,[SortOrder]
           ,[Created]
           ,[Menu_Id])
     VALUES
           ('993A115B-8427-4D85-B116-BD7EDDF116B1'
           ,'浏览'
           ,''
           ,''
           ,1
           ,'2015-08-01 06:20:12.477'
           ,'6FA4DD9D-4795-453B-B622-EBAB2F9A777D')

INSERT INTO [auth].[Permission]
           ([Id]
           ,[Name]
           ,[Code]
           ,[ActionUrl]
           ,[SortOrder]
           ,[Created]
           ,[Menu_Id])
     VALUES
           ('2CAE69A0-47C8-4487-9AA1-D9B9E566B3BD'
           ,'浏览'
           ,''
           ,''
           ,1
           ,'2015-08-01 06:20:12.477'
           ,'20CD3F8A-D9BB-4DBB-959F-4C94BCE4FEF3')

INSERT INTO [auth].[Permission]
           ([Id]
           ,[Name]
           ,[Code]
           ,[ActionUrl]
           ,[SortOrder]
           ,[Created]
           ,[Menu_Id])
     VALUES
           ('BF25D6F0-26FC-4B4E-A7C1-BA980AA8E18B'
           ,'浏览'
           ,''
           ,''
           ,1
           ,'2015-08-01 06:20:12.477'
           ,'5150E687-DE94-4632-87C1-BC795199F48E')

INSERT INTO [auth].[Permission]
           ([Id]
           ,[Name]
           ,[Code]
           ,[ActionUrl]
           ,[SortOrder]
           ,[Created]
           ,[Menu_Id])
     VALUES
           ('5453C36A-6DE5-4C02-B261-53C900B2CEB2'
           ,'浏览'
           ,''
           ,''
           ,1
           ,'2015-08-01 06:20:12.477'
           ,'38A6C659-4915-4D85-B67E-B9549EAC1838')

GO


--------------------------------------------------  权限其他  ----------------------------------------

INSERT INTO [auth].[Permission]
           ([Id]
           ,[Name]
           ,[Code]
           ,[ActionUrl]
           ,[SortOrder]
           ,[Created]
           ,[Menu_Id])
     VALUES
           ('2104E879-CCBB-4D76-9A37-65D7406CDF5E'
           ,'浏览'
           ,''
           ,''
           ,1
           ,'2015-08-01 06:20:12.477'
           ,'CCA24FD4-F36A-4C1D-93DE-213BC9F47DEF')

INSERT INTO [auth].[Permission]
           ([Id]
           ,[Name]
           ,[Code]
           ,[ActionUrl]
           ,[SortOrder]
           ,[Created]
           ,[Menu_Id])
     VALUES
           ('21F2FB52-9FCC-49EC-B8C8-B8DCB5BCECA7'
           ,'浏览'
           ,''
           ,''
           ,1
           ,'2015-08-01 06:20:12.477'
           ,'4ABDB264-126A-4746-89FD-EA8D7508AE26')

GO

INSERT INTO [auth].[Permission]
           ([Id]
           ,[Name]
           ,[Code]
           ,[ActionUrl]
           ,[SortOrder]
           ,[Created]
           ,[Menu_Id])
     VALUES
           ('65D00D52-570D-4112-A681-17442AA2B2CC'
           ,'浏览'
           ,''
           ,''
           ,1
           ,'2015-08-01 06:20:12.477'
           ,'B6023B3C-F93F-4AA6-BDA1-4F4529B86ECE')

GO

--------------------------------------------------角色权限----------------------------------------

------业务管理------

INSERT INTO [auth].[Role_Permission]
           ([Role_Id]
           ,[Permission_Id])
     VALUES
           ('93FFFBD6-1EB6-4CC7-8AE6-3B52D26CB91E'
		   ,'DF08C492-9F9A-443A-AD62-50DB4B8552AF')

INSERT INTO [auth].[Role_Permission]
           ([Role_Id]
           ,[Permission_Id])
     VALUES
           ('93FFFBD6-1EB6-4CC7-8AE6-3B52D26CB91E'
           ,'A46B0B7B-BA4A-43C6-93DD-07C5D22B5F2B')

INSERT INTO [auth].[Role_Permission]
           ([Role_Id]
           ,[Permission_Id])
     VALUES
           ('93FFFBD6-1EB6-4CC7-8AE6-3B52D26CB91E'
           ,'D47E3575-263B-44EA-A627-33B348E910DD')

INSERT INTO [auth].[Role_Permission]
           ([Role_Id]
           ,[Permission_Id])
     VALUES
           ('93FFFBD6-1EB6-4CC7-8AE6-3B52D26CB91E'
           ,'31BA9226-4B7C-4FD9-9C99-B158A2E44DAC')
GO


INSERT INTO [auth].[Role_Permission]
           ([Role_Id]
           ,[Permission_Id])
     VALUES
           ('93FFFBD6-1EB6-4CC7-8AE6-3B52D26CB91E'
           ,'65D00D52-570D-4112-A681-17442AA2B2CC')
GO

------系统管理------


INSERT INTO [auth].[Role_Permission]
           ([Role_Id]
           ,[Permission_Id])
     VALUES
           ('B359CDDA-9BE7-4BA4-895A-F1CA513EA361'
		   ,'993A115B-8427-4D85-B116-BD7EDDF116B1')

INSERT INTO [auth].[Role_Permission]
           ([Role_Id]
           ,[Permission_Id])
     VALUES
           ('B359CDDA-9BE7-4BA4-895A-F1CA513EA361'
           ,'2CAE69A0-47C8-4487-9AA1-D9B9E566B3BD')

INSERT INTO [auth].[Role_Permission]
           ([Role_Id]
           ,[Permission_Id])
     VALUES
           ('B359CDDA-9BE7-4BA4-895A-F1CA513EA361'
           ,'BF25D6F0-26FC-4B4E-A7C1-BA980AA8E18B')

INSERT INTO [auth].[Role_Permission]
           ([Role_Id]
           ,[Permission_Id])
     VALUES
           ('B359CDDA-9BE7-4BA4-895A-F1CA513EA361'
           ,'5453C36A-6DE5-4C02-B261-53C900B2CEB2')
GO