using System.Security;

namespace Digital_Assest_Management
{
    [TestClass]
    public class Test_Basic_Object_Classes
    {
        private static User InitUserData()
        {
            var user = new User
            {
                Name = "John",
                Id = 1
            };
            user.AddDrive(new Drive { DriveId = 1, DriveName = "GoogleDrive" });
            user.AddDrive(new Drive { DriveId = 2, DriveName = "OneDrive" });


/*            var newUser = new UserBuilder()
                .AddUser("John")
                .AddDrive(new Drive { DriveId = 1, DriveName = "GoogleDrive" })
                .AddDrive(new Drive { DriveId = 2, DriveName = "OneDrive" })
                .Build();*/
            return user;

        }
        [TestMethod]
        public void User_Can_Add_And_Remove_Drive()
        {
            var user = InitUserData();
            Assert.AreEqual(2, user.Drives.Count);
            user.RemoveDrive(1);
            Assert.AreEqual(1, user.Drives.Count);
            Assert.IsFalse(user.Drives.Any(d => d.DriveId == 1));
        }
        [TestMethod]
        public void User_HasMultiple_Drives()
        {
            User user = InitUserData();
            Assert.AreEqual(2, user.Drives.Count);
        }
        [TestMethod]
        public void User_HasMultiple_Drivers_With_Folders()
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
        public void User_HasPermission_ForDrive()
        {
            var user = InitUserData();
            user.GrantPermission(targetId: 1, permissionType: "Admin", targetType: "Drive");

            Assert.IsTrue(user.HasPermission(targetId: 1, permissionType: "Admin", targetType: "Drive"));
        }
        [TestMethod]
        public void User_HasNoPermission_ForDrive()
        {
            var user = InitUserData();
            Assert.IsFalse(user.HasPermission(targetId: 3, permissionType: "Admin", targetType: "Drive"));
        }
        [TestMethod]
        public void User_CanRemove_Permission_ForDrive()
        {
            var user = InitUserData();
            user.GrantPermission(targetId: 1, permissionType: "Admin", targetType: "Drive");
            Assert.IsTrue(user.HasPermission(targetId: 1, permissionType: "Admin", targetType: "Drive"));

            user.RemovePermission(targetId: 1, permissionType: "Admin", targetType: "Drive");
            Assert.IsFalse(user.HasPermission(targetId: 1, permissionType: "Admin", targetType: "Drive"));
        }
        [TestMethod]
        public void User_CanAssign_Folder_And_File_Permission()
        {
            var user = InitUserData();

            user.GrantPermission(targetId: 1, permissionType: "Contributor", targetType: "Folder");
            Assert.IsTrue(user.HasPermission(targetId: 1, permissionType: "Contributor", targetType: "Folder"));


            user.GrantPermission(targetId: 2, permissionType: "Reader", targetType: "File");
            Assert.IsTrue(user.HasPermission(targetId: 2, permissionType: "Reader", targetType: "File"));
        }


        [TestMethod]
        public void Drive_Can_Add_And_Remove_Folder()
        {
            var user = InitUserData();
            var folder = new Folder { FolderId = 1, FolderName = "TestFolder" };

            user.Drives[0].AddFolder(folder);
            Assert.IsTrue(user.Drives[0].Folders.Any(f => f.FolderId == 1));

            user.Drives[0].RemoveFolder(1);
            Assert.IsFalse(user.Drives[0].Folders.Any(f => f.FolderId == 1));
        }
        [TestMethod]
        public void Drive_Can_Add_And_Remove_File()
        {
            var user = InitUserData();
            var file = new File { FileId = 1, FileName = "TestFile" };

            user.Drives[0].AddFile(file);
            Assert.IsTrue(user.Drives[0].Files.Any(f => f.FileId == 1));

            user.Drives[0].RemoveFile(1);
            Assert.IsFalse(user.Drives[0].Files.Any(f => f.FileId == 1));
        }
        [TestMethod]
        public void Drive_HasMultiple_Folder_And_File()
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
        public void Drive_HasMultiple_Folders()
        {
            var Drive = new Drive { DriveId = 1, DriveName = "GoogleDrive" };

            Drive.AddFolder(new Folder { FolderId = 1, FolderName = "Mentorship2024" });
            Drive.AddFolder(new Folder { FolderId = 2, FolderName = "bbv" });

            Assert.AreEqual(2, Drive.Folders.Count);
        }
        [TestMethod]
        public void Drive_HasMultiple_Files()
        {
            var Drive = new Drive { DriveId = 1, DriveName = "GoogleDrive" };

            Drive.AddFile(new File { FileId = 1, FileName = "Mentorship2024.pdf" });
            Drive.AddFile(new File { FileId = 2, FileName = "daovo.docx" });

            Assert.AreEqual(2, Drive.Files.Count);
            var count = Drive.Files.Count(e => e.FileName.Equals("daovo.docx"));
            Assert.AreEqual(1, count);
        }


        [TestMethod]
        public void Folder_Can_Add_And_Remove_Folder()
        {
            var user = InitUserData();
            var subFolder = new Folder { FolderId = 1, FolderName = "TestFolder" };

            user.Drives[0].AddFolder(subFolder);
            Assert.IsTrue(user.Drives[0].Folders.Any(f => f.FolderId == 1));

            subFolder.AddFolder(new Folder { FolderId = 2, FolderName = "SubFolder" });
            Assert.IsTrue(subFolder.SubFolders.Any(f => f.FolderId == 2));

            subFolder.RemoveFolder(2);
            Assert.IsFalse(subFolder.SubFolders.Any(f => f.FolderId == 2));
        }
        [TestMethod]
        public void Folder_Can_Add_And_Remove_File()
        {
            var user = InitUserData();
            var subFolder = new Folder { FolderId = 1, FolderName = "TestFolder" };

            user.Drives[0].AddFolder(subFolder);
            Assert.IsTrue(user.Drives[0].Folders.Any(f => f.FolderId == 1));

            subFolder.AddFile(new File { FileId = 1, FileName = "TestFile" });
            Assert.IsTrue(subFolder.Files.Any(f => f.FileId == 1));

            subFolder.RemoveFile(1);
            Assert.IsFalse(subFolder.Files.Any(f => f.FileId == 1));
        }
        [TestMethod]
        public void Folder_CanRename()
        {
            var user = InitUserData();
            var folder = new Folder { FolderId = 1, FolderName = "OldName" };
            user.Drives[0].AddFolder(folder);
            folder.FolderName = "NewName";
            Assert.IsTrue(user.Drives[0].Folders.Any(f => f.FolderName == "NewName"));
        }
        [TestMethod]
        public void Folder_HasMultiple_SubFolder()
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

            Assert.AreEqual(4, folder.SubFolders.Count);

            folderWorking.AddFile(new File { FileId = 1, FileName = "sample.sql" });
            Assert.AreEqual(1, folderWorking.Files.Count);
        }
        [TestMethod]
        public void Folder_HasMultiple_File()
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
        public void File_CanModify()
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
        public void File_CanRemove()
        {
            var user = InitUserData();

            var folder = new Folder { FolderId = 1, FolderName = "TestFolder" };
            user.Drives[0].AddFolder(folder);

            var file = new File { FileId = 1, FileName = "TestFile.txt" };
            folder.AddFile(file);

            folder.Files.Remove(file);
            Assert.IsFalse(folder.Files.Any(f => f.FileId == 1));
        }
    }

    [TestClass]
    public class CheckPermission
    {
        private static (User, User) InitUserWithPermissions()
        {
            // User 1 setup
            var user1 = new User { Name = "Nam", Id = 1 };
            var drive1 = new Drive { DriveId = 1, DriveName = "GoogleDrive" };
            var folder1 = new Folder { FolderId = 1, FolderName = "Documents" };
            var subfolder1_1 = new Folder { FolderId = 2, FolderName = "Reports" }; // Subfolder in Drive1
            var file1 = new File { FileId = 1, FileName = "Report.pdf" };

            user1.AddDrive(drive1);
            drive1.AddFolder(folder1);
            folder1.AddFolder(subfolder1_1); // Adding subfolder to folder1
            folder1.AddFile(file1);

            user1.GrantPermission(targetId: 1, permissionType: "Admin", targetType: "Drive");
            user1.GrantPermission(targetId: 1, permissionType: "Contributor", targetType: "Folder");
            user1.GrantPermission(targetId: 2, permissionType: "Reader", targetType: "Folder"); // Grant Reader permission to subfolder

            // User 2 setup
            var user2 = new User { Name = "Linh", Id = 2 };
            var drive2 = new Drive { DriveId = 2, DriveName = "OneDrive" };
            var folder2 = new Folder { FolderId = 3, FolderName = "Photos" };
            var subfolder2_1 = new Folder { FolderId = 4, FolderName = "Vacation" }; // Subfolder in Drive2
            var file2 = new File { FileId = 2, FileName = "Vacation.jpg" };

            user2.AddDrive(drive2);
            drive2.AddFolder(folder2);
            folder2.AddFolder(subfolder2_1); // Adding subfolder to folder2
            folder2.AddFile(file2);

            user2.GrantPermission(targetId: 3, permissionType: "Reader", targetType: "Folder"); // Grant Reader permission to folder2
            user2.GrantPermission(targetId: 4, permissionType: "Reader", targetType: "Folder"); // Grant Reader permission to subfolder

            return (user1, user2);
        }

        [TestMethod]
        public void Grant_And_RemovePermission_To_Folder()
        {
            var (user1, _) = InitUserWithPermissions();

            var newFolder = new Folder { FolderId = 5, FolderName = "ImportantDocs" };
            user1.Drives[0].AddFolder(newFolder);

            // Grant Contributor permissions to the new Folder
            user1.GrantPermission(targetId: 5, permissionType: "Contributor", targetType: "Folder");
            Assert.IsTrue(user1.HasPermission(targetId: 5, permissionType: "Contributor", targetType: "Folder"));

            // Remove Contributor permissions from the new Folder
            user1.RemovePermission(targetId: 5, permissionType: "Contributor", targetType: "Folder");
            Assert.IsFalse(user1.HasPermission(targetId: 5, permissionType: "Contributor", targetType: "Folder"));
        }
        [TestMethod]
        public void Grant_And_RemovePermission_To_File()
        {
            var (user1, _) = InitUserWithPermissions();

            var newFile = new File { FileId = 3, FileName = "Document.txt" };
            user1.Drives[0].Folders[0].AddFile(newFile);

            // Grant Reader permissions to the new Folder
            user1.GrantPermission(targetId: 3, permissionType: "Reader", targetType: "File");
            Assert.IsTrue(user1.HasPermission(targetId: 3, permissionType: "Reader", targetType: "File"));

            // Remove Reader permissions from the new Folder
            user1.RemovePermission(targetId: 3, permissionType: "Reader", targetType: "File");
            Assert.IsFalse(user1.HasPermission(targetId: 3, permissionType: "Reader", targetType: "File"));
        }


        [TestMethod]
        public void User1_HasAdminPermission_ForDrive()
        {
            var (user1, _) = InitUserWithPermissions();
            Assert.IsTrue(user1.HasPermission(targetId: 1, permissionType: "Admin", targetType: "Drive"));
        }
        [TestMethod]
        public void User1_HasNoReaderPermission_ForDrive()
        {
            var (user1, _) = InitUserWithPermissions();
            Assert.IsFalse(user1.HasPermission(targetId: 1, permissionType: "Reader", targetType: "Drive"));
        }
        [TestMethod]
        public void User1_HasContributorPermission_ForFolder()
        {
            var (user1, _) = InitUserWithPermissions();
            Assert.IsTrue(user1.HasPermission(targetId: 1, permissionType: "Contributor", targetType: "Folder"));
        }
        [TestMethod]
        public void User1_HasReaderPermission_ForSubfolder()
        {
            var (user1, _) = InitUserWithPermissions();
            Assert.IsTrue(user1.HasPermission(targetId: 2, permissionType: "Reader", targetType: "Folder"));
        }
        [TestMethod]
        public void User1_HasNoAdminPermission_ForSubfolder()
        {
            var (user1, _) = InitUserWithPermissions();
            Assert.IsFalse(user1.HasPermission(targetId: 2, permissionType: "Admin", targetType: "Folder"));
        }
        [TestMethod]
        public void User1_HasNoReaderPermission_ForFile()
        {
            var (user1, _) = InitUserWithPermissions();
            Assert.IsFalse(user1.HasPermission(targetId: 1, permissionType: "Reader", targetType: "File"));
        }


        [TestMethod]
        public void User2_HasReaderPermission_ForFolder()
        {
            var (_, user2) = InitUserWithPermissions();
            Assert.IsTrue(user2.HasPermission(targetId: 3, permissionType: "Reader", targetType: "Folder"));
        }
        [TestMethod]
        public void User2_HasReaderPermission_ForSubfolder()
        {
            var (_, user2) = InitUserWithPermissions();
            Assert.IsTrue(user2.HasPermission(targetId: 4, permissionType: "Reader", targetType: "Folder"));
        }
        [TestMethod]
        public void User2_HasNoAdminPermission_ForFolder()
        {
            var (_, user2) = InitUserWithPermissions();
            Assert.IsFalse(user2.HasPermission(targetId: 3, permissionType: "Admin", targetType: "Folder"));
        }
        [TestMethod]
        public void User2_HasNoContributorPermission_ForFolder()
        {
            var (_, user2) = InitUserWithPermissions();
            Assert.IsFalse(user2.HasPermission(targetId: 3, permissionType: "Contributor", targetType: "Folder"));
        }
        [TestMethod]
        public void User2_HasNoReaderPermission_ForFile()
        {
            var (_, user2) = InitUserWithPermissions();
            Assert.IsFalse(user2.HasPermission(targetId: 2, permissionType: "Reader", targetType: "File"));
        }
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

    public class Folder {
        public int FolderId { get; set; }
        public string FolderName { get; set; }
        public List<Folder> SubFolders { get; set; } = new List<Folder>();
        public List<File> Files { get; set; } = new List<File>();
        public int? ParentFolderId { get; set; } // Nullable to indicate if it's a root folder

        
        public void AddFolder(Folder folder) { // Add a subfolder to this folder and set its ParentFolderId
            SubFolders.Add(folder);
            folder.ParentFolderId = this.FolderId; // Set parent folder ID for subfolder
        }

        public void RemoveFolder(int folderId) { // Remove a subfolder by its ID
            SubFolders.RemoveAll(f => f.FolderId == folderId);
        }

        public void AddFile(File file) { // Add a file to this folder
            Files.Add(file); 
        }

        public void RemoveFile(int fileId) { // Remove a file by its ID
            Files.RemoveAll(f => f.FileId == fileId);
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
    public class Permission
    {
        public int TargetId { get; set; } // ID of Drive, Folder, File
        public string TargetType { get; set; } //  Drive, Folder, File
        public string PermissionType { get; set; } // Permission: "Admin", "Contributor", "Reader"
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

        public void GrantPermission(int targetId, string permissionType, string targetType)
        {
            if (HasPermission(targetId, "admin", targetType))
            {
                throw new InvalidOperationException("Only admins can grant admin permissions.");
            }

            if (!HasPermission(targetId, permissionType, targetType))
            {
                Permissions.Add(new Permission
                {
                    TargetId = targetId,
                    PermissionType = permissionType,
                    TargetType = targetType
                });
            }
        }

        public void RemovePermission(int targetId, string permissionType, string targetType)
        {
            Permissions.RemoveAll(p =>
                p.TargetId == targetId &&
                p.PermissionType == permissionType &&
                p.TargetType == targetType);
        }
        public bool HasPermission(int targetId, string permissionType, string targetType)
        {
            return Permissions.Any(p =>
                p.TargetId == targetId &&
                p.PermissionType == permissionType &&
                p.TargetType == targetType);
        }
    }

    [TestClass]
    public class ScenariosTests
    {
        public static (User admin, User contributor, User reader) InitUsersWithPermissions()
        {
            var admin = new User { Name = "AdminUser", Id = 1 };
            var contributor = new User { Name = "Contributor1", Id = 2 };
            var reader = new User { Name = "Reader1", Id = 3 };

            var drive = new Drive { DriveId = 1, DriveName = "Root1" };
            var subFolder = new Folder { FolderId = 1, FolderName = "SubFolder1" };
            var file = new File { FileId = 1, FileName = "File3.docx" };

            drive.AddFolder(subFolder);
            subFolder.AddFile(file);

            admin.AddDrive(drive);
            admin.GrantPermission(targetId: 1, permissionType: "Admin", targetType: "Drive");

            contributor.GrantPermission(targetId: 1, permissionType: "Contributor", targetType: "Folder");
            reader.GrantPermission(targetId: 1, permissionType: "Reader", targetType: "Folder");

            return (admin, contributor, reader);
        }

        [TestMethod]
        public void Admin_Sharing_Contributor_Permission()
        {
            var (admin, contributor, _) = InitUsersWithPermissions();

            // Admin shares Contributor permission for SubFolder1 with contributor
            admin.GrantPermission(targetId: 1, permissionType: "Contributor", targetType: "Folder");

            // Check if contributor now has Contributor permissions on SubFolder1
            Assert.IsTrue(contributor.HasPermission(targetId: 1, permissionType: "Contributor", targetType: "Folder"));
        }

        [TestMethod]
        public void Contributor_Adding_And_Modifying_Files()
        {

        }

        [TestMethod]
        public void Reader_Trying_To_Modify_Files()
        {
            
        }

    }

}
