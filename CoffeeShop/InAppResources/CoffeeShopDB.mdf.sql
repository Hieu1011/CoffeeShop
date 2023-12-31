INSERT INTO "AccessPermission" ("AccessPermissionID","AccessPermissionName") VALUES ('AP001',N'Thu ngân');
INSERT INTO "AccessPermission" ("AccessPermissionID","AccessPermissionName") VALUES ('AP002',N'Quản lý tài khoản');
INSERT INTO "AccessPermission" ("AccessPermissionID","AccessPermissionName") VALUES ('AP003',N'Quản lý loại tài khoản');
INSERT INTO "AccessPermission" ("AccessPermissionID","AccessPermissionName") VALUES ('AP004',N'Quản lý kho');
INSERT INTO "AccessPermission" ("AccessPermissionID","AccessPermissionName") VALUES ('AP005',N'Quản lý chi phí');
INSERT INTO "AccessPermission" ("AccessPermissionID","AccessPermissionName") VALUES ('AP006',N'Quản lý menu');
INSERT INTO "AccessPermission" ("AccessPermissionID","AccessPermissionName") VALUES ('AP007',N'Quản lý ưu đãi');
INSERT INTO "AccessPermission" ("AccessPermissionID","AccessPermissionName") VALUES ('AP008',N'Báo cáo');
INSERT INTO "AccessPermission" ("AccessPermissionID","AccessPermissionName") VALUES ('AP009',N'Thiết lập quy định');
INSERT INTO "AccessPermission" ("AccessPermissionID","AccessPermissionName") VALUES ('AP010',N'Quản lý dữ liệu');
INSERT INTO "Employees" ("EmployeeID","EmployeeName","EmployeeTypeID","Password","State") VALUES ('E111',N'Root Admin',N'ET001',N'123456',1);
INSERT INTO "AccessPermissionGroup" ("EmployeeTypeID","AccessPermissionID") VALUES ('ET001',N'AP001');
INSERT INTO "AccessPermissionGroup" ("EmployeeTypeID","AccessPermissionID") VALUES ('ET001',N'AP002');
INSERT INTO "AccessPermissionGroup" ("EmployeeTypeID","AccessPermissionID") VALUES ('ET001',N'AP003');
INSERT INTO "AccessPermissionGroup" ("EmployeeTypeID","AccessPermissionID") VALUES ('ET001',N'AP004');
INSERT INTO "AccessPermissionGroup" ("EmployeeTypeID","AccessPermissionID") VALUES ('ET001',N'AP005');
INSERT INTO "AccessPermissionGroup" ("EmployeeTypeID","AccessPermissionID") VALUES ('ET001',N'AP006');
INSERT INTO "AccessPermissionGroup" ("EmployeeTypeID","AccessPermissionID") VALUES ('ET001',N'AP007');
INSERT INTO "AccessPermissionGroup" ("EmployeeTypeID","AccessPermissionID") VALUES ('ET001',N'AP008');
INSERT INTO "AccessPermissionGroup" ("EmployeeTypeID","AccessPermissionID") VALUES ('ET001',N'AP009');
INSERT INTO "AccessPermissionGroup" ("EmployeeTypeID","AccessPermissionID") VALUES ('ET001',N'AP010');
INSERT INTO "EmployeeType" ("EmployeeTypeID","EmployeeTypeName") VALUES ('ET001',N'Chủ quán');
INSERT INTO "EmployeeType" ("EmployeeTypeID","EmployeeTypeName") VALUES ('ET002',N'Thu ngân');
INSERT INTO "EmployeeType" ("EmployeeTypeID","EmployeeTypeName") VALUES ('ET003',N'Pha chế');
INSERT INTO "EmployeeType" ("EmployeeTypeID","EmployeeTypeName") VALUES ('ET004',N'Phục vụ');
INSERT INTO "EmployeeType" ("EmployeeTypeID","EmployeeTypeName") VALUES ('ET005',N'Quản lý');
INSERT INTO "EmployeeType" ("EmployeeTypeID","EmployeeTypeName") VALUES ('ET006',N'Empty Permission');
INSERT INTO "Parameter" ("Name","Value") VALUES ('RowInList',20);
INSERT INTO "Parameter" ("Name","Value") VALUES ('DayDeleteReceipt',2);
INSERT INTO "Parameter" ("Name","Value") VALUES ('DayDeletePayment',2);
INSERT INTO "Parameter" ("Name","Value") VALUES ('DayDeleteImport',2);
INSERT INTO "Parameter" ("Name","Value") VALUES ('DayDeleteExport',2);