


namespace Digital_Assest_Management
{
    [TestClass]
    public class Test_DAM
    {
        private static User InitUserData()
        {
            var user = new User();
            user.Name = "John";
            user.Id = 1;

            user.AddDrive(new Drive { DriveId = 1, DriveName = "GoogleDrive" });
            user.AddDrive(new Drive { DriveId = 2, DriveName = "OneDrive" });
            return user;
        }
        [TestMethod]
        public void Test_User_Can_Add_And_Remove_Drive()
        {
            var user = InitUserData();
            Assert.AreEqual(2, user.Drives.Count);
            user.RemoveDrive(1);
            Assert.AreEqual(1, user.Drives.Count);
            Assert.IsFalse(user.Drives.Any(d => d.DriveId == 1));
        }
        [TestMethod]
        public void Test_User_HasMultiple_Drives()
        {
            User user = InitUserData();
            Assert.AreEqual(2, user.Drives.Count);
        }
        [TestMethod]
        public void Test_User_HasMultiple_Drivers_With_Folders()
        {
            var user = new User();
            user.Name = "John";
            user.Id = 1;

            var Drive1 = new Drive { DriveId = 1, DriveName = "GoogleDrive" };
            Drive1.AddFolder(new Folder { FolderId = 1, FolderName = "Mentorship2024" });
            Drive1.AddFolder(new Folder { FolderId = 2, FolderName = "bbv" });

            var Drive2 = new Drive { DriveId = 2, DriveName = "OneDrive" };
            Drive2.AddFolder(new Folder { FolderId = 3, FolderName = "Mentorship2024" });
            Drive2.AddFolder(new Folder { FolderId = 4, FolderName = "bbv" });

            user.AddDrive(Drive1);
            user.AddDrive(Drive2);

            Assert.AreEqual(2, user.Drives.Count);
            Assert.AreEqual(2, user.Drives[0].Folders.Count);
            Assert.AreEqual(2, user.Drives[1].Folders.Count);
        }
        [TestMethod]
        public void Test_User_HasPermission_ForDrive()
        {
            var user = InitUserData();
            user.GrantPermission(targetId: 1, permissionType: "Admin", targetType: "Drive");

            Assert.IsTrue(user.HasPermission(targetId: 1, permissionType: "Admin", targetType: "Drive"));
        }
        [TestMethod]
        public void Test_User_HasNoPermission_ForDrive()
        {
            var user = InitUserData();
            Assert.IsFalse(user.HasPermission(targetId: 3, permissionType: "Admin", targetType: "Drive"));
        }
        [TestMethod]
        public void Test_User_CanRemove_Permission_ForDrive()
        {
            var user = InitUserData();
            user.GrantPermission(targetId: 1, permissionType: "Admin", targetType: "Drive");
            Assert.IsTrue(user.HasPermission(targetId: 1, permissionType: "Admin", targetType: "Drive"));

            user.RemovePermission(targetId: 1, permissionType: "Admin", targetType: "Drive");
            Assert.IsFalse(user.HasPermission(targetId: 1, permissionType: "Admin", targetType: "Drive"));
        }
        [TestMethod]
        public void Test_User_CanAssign_Folder_And_File_Permission()
        {
            var user = InitUserData();

            user.GrantPermission(targetId: 1, permissionType: "Contributor", targetType: "Folder");
            Assert.IsTrue(user.HasPermission(targetId: 1, permissionType: "Contributor", targetType: "Folder"));


            user.GrantPermission(targetId: 2, permissionType: "Reader", targetType: "File");
            Assert.IsTrue(user.HasPermission(targetId: 2, permissionType: "Reader", targetType: "File"));
        }


        [TestMethod]
        public void Test_Drive_Can_Add_And_Remove_Folder()
        {
            var user = InitUserData();
            var folder = new Folder { FolderId = 1, FolderName = "TestFolder" };

            user.Drives[0].AddFolder(folder);
            Assert.IsTrue(user.Drives[0].Folders.Any(f => f.FolderId == 1));

            user.Drives[0].RemoveFolder(1);
            Assert.IsFalse(user.Drives[0].Folders.Any(f => f.FolderId == 1));
        }
        [TestMethod]
        public void Test_Drive_Can_Add_And_Remove_File()
        {
            var user = InitUserData();
            var file = new File { FileId = 1, FileName = "TestFile" };

            user.Drives[0].AddFile(file);
            Assert.IsTrue(user.Drives[0].Files.Any(f => f.FileId == 1));

            user.Drives[0].RemoveFile(1);
            Assert.IsFalse(user.Drives[0].Files.Any(f => f.FileId == 1));
        }
        [TestMethod]
        public void Test_Drive_HasMultiple_Folder_And_File()
        {
            var user = InitUserData();

            user.Drives[0].AddFolder(new Folder { FolderId = 1, FolderName = "Internship" });
            user.Drives[0].AddFolder(new Folder { FolderId = 2, FolderName = "bbv" });
            user.Drives[0].AddFile(new File { FileId = 1, FileName = "Mentorship.pdf" });
            user.Drives[0].AddFile(new File { FileId = 2, FileName = "bbv.pdf" });

            Assert.AreEqual(2, user.Drives[0].Folders.Count);
            Assert.AreEqual(2, user.Drives[0].Files.Count);
        }
        [TestMethod]
        public void Test_Drive_HasMultiple_Folders()
        {
            var Drive = new Drive { DriveId = 1, DriveName = "GoogleDrive" };

            Drive.AddFolder(new Folder { FolderId = 1, FolderName = "Mentorship2024" });
            Drive.AddFolder(new Folder { FolderId = 2, FolderName = "bbv" });

            Assert.AreEqual(2, Drive.Folders.Count);
        }
        [TestMethod]
        public void Test_Drive_HasMultiple_Files()
        {
            var Drive = new Drive { DriveId = 1, DriveName = "GoogleDrive" };

            Drive.AddFile(new File { FileId = 1, FileName = "Mentorship2024.pdf" });
            Drive.AddFile(new File { FileId = 2, FileName = "daovo.docx" });

            Assert.AreEqual(2, Drive.Files.Count);
            var count = Drive.Files.Count(e => e.FileName.Equals("daovo.docx"));
            Assert.AreEqual(1, count);
        }


        [TestMethod]
        public void Test_Folder_Can_Add_And_Remove_Folder()
        {
            var user = InitUserData();
            var subFolder = new Folder { FolderId = 1, FolderName = "TestFolder" };

            user.Drives[0].AddFolder(subFolder);
            Assert.IsTrue(user.Drives[0].Folders.Any(f => f.FolderId == 1));

            subFolder.AddFolder(new Folder { FolderId = 2, FolderName = "SubFolder" });
            Assert.IsTrue(subFolder.Folders.Any(f => f.FolderId == 2));

            subFolder.Folders.RemoveAll(f => f.FolderId == 2);
            Assert.IsFalse(subFolder.Folders.Any(f => f.FolderId == 2));
        }
        [TestMethod]
        public void Test_Folder_Can_Add_And_Remove_File()
        {
            var user = InitUserData();
            var subFolder = new Folder { FolderId = 1, FolderName = "TestFolder" };

            user.Drives[0].AddFolder(subFolder);
            Assert.IsTrue(user.Drives[0].Folders.Any(f => f.FolderId == 1));

            subFolder.AddFile(new File { FileId = 1, FileName = "TestFile" });
            Assert.IsTrue(subFolder.Files.Any(f => f.FileId == 1));

            subFolder.Files.RemoveAll(f => f.FileId == 1);
            Assert.IsFalse(subFolder.Files.Any(f => f.FileId == 1));
        }
        [TestMethod]
        public void Test_Folder_CanRename()
        {
            var user = InitUserData();
            var folder = new Folder { FolderId = 1, FolderName = "OldName" };
            user.Drives[0].AddFolder(folder);
            folder.FolderName = "NewName";
            Assert.IsTrue(user.Drives[0].Folders.Any(f => f.FolderName == "NewName"));
        }
        [TestMethod]
        public void Test_Folder_HasMultiple_SubFolder()
        {
            var user = InitUserData();

            user.Drives[0].AddFolder(new Folder { FolderId = 2, FolderName = "Internship" });

            Folder folder = new Folder { FolderId = 1, FolderName = "bbv" };
            user.Drives[0].AddFolder(folder);
            user.Drives[0].AddFile(new File { FileId = 1, FileName = "Mentorship.pdf" });

            Folder folderWorking = new Folder { FolderId = 1, FolderName = "working" };
            folder.AddFolder(folderWorking);
            folder.AddFolder(new Folder { FolderId = 1, FolderName = "projects" });
            folder.AddFolder(new Folder { FolderId = 1, FolderName = "design" });
            folder.AddFolder(new Folder { FolderId = 1, FolderName = "training" });

            Assert.AreEqual(4, folder.Folders.Count);

            folderWorking.AddFile(new File { FileId = 1, FileName = "sample.sql" });
            Assert.AreEqual(1, folderWorking.Files.Count);
        }
        [TestMethod]
        public void Test_Folder_HasMultiple_File()
        {
            var user = InitUserData();

            user.Drives[0].AddFolder(new Folder { FolderId = 2, FolderName = "Internship" });

            Folder folder = new Folder { FolderId = 1, FolderName = "bbv" };
            user.Drives[0].AddFolder(folder);
            user.Drives[0].AddFile(new File { FileId = 1, FileName = "Mentorship.pdf" });

            folder.AddFile(new File { FileId = 1, FileName = "sample.sql" });
            folder.AddFile(new File { FileId = 2, FileName = "sample.docx" });

            Assert.AreEqual(2, folder.Files.Count);
        }


        [TestMethod]
        public void Test_File_CanModify()
        {
            var user = InitUserData();

            var folder = new Folder { FolderId = 1, FolderName = "TestFolder" };
            user.Drives[0].AddFolder(folder);

            var file = new File { FileId = 1, FileName = "TestFile.txt" };
            folder.AddFile(file);

            file.ModifyFile("NewFileName.txt");
            Assert.IsTrue(folder.Files.Any(f => f.FileName == "NewFileName.txt"));
        }
        [TestMethod]
        public void Test_File_CanRemove()
        {
            var user = InitUserData();

            var folder = new Folder { FolderId = 1, FolderName = "TestFolder" };
            user.Drives[0].AddFolder(folder);

            var file = new File { FileId = 1, FileName = "TestFile.txt" };
            folder.AddFile(file);

            folder.Files.Remove(file);
            Assert.IsFalse(folder.Files.Any(f => f.FileId == 1));
        }


        [TestMethod]
        public void Test_Admin_InvitesUserToDrive_WithPermissions()
        {
            var user = InitUserData();
            var guestUser = new User();
            guestUser.Name = "ChiTai";
            guestUser.Id = 3;

            var drive = new Drive { DriveId = 1, DriveName = "DropBox" };
            user.Drives.Add(drive);

            var driverPermission = new DriverPermission();
            driverPermission.Invite(guestUser.Id, driverId: 3, permission: "ADMIN");

            Assert.IsTrue(driverPermission.HasAdminPermission(3, driverId: 3));
        }
    }

    public class DriverPermission
    {
        public List<DriverPermissionUser> DriverPermissionSet { get; private set; } = new List<DriverPermissionUser>();

        internal void Invite(int userId, int driverId, string permission)
        {
            DriverPermissionSet.Add(new DriverPermissionUser() { UserId = userId, DriverId = driverId, Permission = permission});
        }

        internal bool HasAdminPermission(int userId, int driverId)
        {
            return DriverPermissionSet.Any(e => e.UserId == userId && e.DriverId == driverId);
        }
    }

    public class DriverPermissionUser
    {
        public int UserId { get; set; }
        public int DriverId { get; set; }
        public string Permission { get; set; }
    }

    public class File
    {
        public int FileId { get; set; }
        public string FileName { get; set; }
        public void ModifyFile(string newFileName)
        {
            FileName = newFileName;
        }

    }

    public class Folder
    {
        public int FolderId { get; set; }
        public string FolderName { get; set; }
        public List<Folder> Folders { get; private set; } = new List<Folder>();
        public List<File> Files { get; private set; } = new List<File>();

        public void AddFolder(Folder folder)
        {
            Folders.Add(folder);
        }

        public void AddFile(File file)
        {
            Files.Add(file);
        }
    }

    public class Drive
    {
        public int DriveId { get; set; }
        public string DriveName { get; set; }
        public List<Folder> Folders { get; private set; } = new List<Folder>();
        public List<File> Files { get; private set; } = new List<File>();

        public void AddFolder(Folder folder)
        {
            Folders.Add(folder);
        }
        public void RemoveFolder(int folderId)
        {
            Folders.RemoveAll(e => e.FolderId == folderId);
        }
        public void AddFile(File file)
        {
            Files.Add(file);
        }
        public void RemoveFile(int fileId)
        {
            Files.RemoveAll(e => e.FileId == fileId);

        }
    }

    public class User
    {

        public string Name { get; internal set; }
        public int Id { get; internal set; }
        public List<Drive> Drives { get; set; } = new List<Drive>();
        private List<Permission> Permissions { get; set; } = new List<Permission>();

        public void AddDrive(Drive driveInfo)
        {
            Drives.Add(driveInfo);
        }
        public void RemoveDrive(int driveId)
        {
            Drives.RemoveAll(e => e.DriveId == driveId);
        }
        public bool HasOwnerPermission(int driveId)
        {
            return Drives.Any(e => e.DriveId == driveId);
        }
        public void GrantPermission(int targetId, string permissionType, string targetType)
        {
            if (Permissions.Any(p => p.TargetId == targetId && p.PermissionType == permissionType && p.TargetType == targetType))
            {
                throw new InvalidOperationException("Permission already granted.");
            }

            Permissions.Add(new Permission
            {
                TargetId = targetId,
                PermissionType = permissionType,
                TargetType = targetType
            });
        }
        public void RemovePermission(int targetId, string permissionType, string targetType)
        {
            Permissions.RemoveAll(p => p.TargetId == targetId && p.PermissionType == permissionType && p.TargetType == targetType);
        }

        public bool HasPermission(int targetId, string permissionType, string targetType)
        {
            return Permissions.Any(p => p.TargetId == targetId && p.PermissionType == permissionType && p.TargetType == targetType);
        }
    }

    public class Permission
    {
        public int TargetId { get; set; } // ID of Drive, Folder, File
        public string TargetType { get; set; } //  Drive, Folder, File
        public string PermissionType { get; set; } // Permission: "Admin", "Contributor", "Reader"
    }

}