using System.Security;

namespace Digital_Assest_Management
{
    [TestClass]
    public class Test_Basic_Object_Classes
    {
        private static User InitUserData()
        {
            var user = new User { Name = "John", Id = 1 };

            var Nam = new  { Name = "Nam", Id = 2 };
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
        public void Test_User_Can_Add_And_Removedrive1()
        {
            var user = InitUserData();
            Assert.AreEqual(2, user.Drives.Count);
            user.RemoveDrive(1);
            Assert.AreEqual(1, user.Drives.Count);
            Assert.IsFalse(user.Drives.Any(d => d.DriveId == 1));
        }
        [TestMethod]
        public void Test_User_HasMultipledrive1s()
        {
            User user = InitUserData();
            Assert.AreEqual(2, user.Drives.Count);
        }
        [TestMethod]
        public void Test_User_HasMultipledrive1rs_Withfolder1s()
        {
            var user = new User();
            user.Name = "John";
            user.Id = 1;

            var Drive1 = new Drive { DriveId = 1, DriveName = "GoogleDrive" };
            Drive1.AddFolder(new Folder { StoreId = 1, StoreName = "Mentorship2024" });
            Drive1.AddFolder(new Folder { StoreId = 2, StoreName = "bbv" });

            var Drive2 = new Drive { DriveId = 2, DriveName = "OneDrive" };
            Drive2.AddFolder(new Folder { StoreId = 3, StoreName = "Mentorship2024" });
            Drive2.AddFolder(new Folder { StoreId = 4, StoreName = "bbv" });

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
            user.GrantPermission(storeId: 1, permissionType: "Admin");

            Assert.IsTrue(user.HasPermission(storeId: 1, permissionType: "Admin"));
        }
        [TestMethod]
        public void Test_User_HasNoPermission_ForDrive()
        {
            var user = InitUserData();
            Assert.IsFalse(user.HasPermission(storeId: 3, permissionType: "Admin"));
        }
        [TestMethod]
        public void Test_User_CanDelete_Permission_ForDrive()
        {
            var user = InitUserData();
            user.GrantPermission(storeId: 1, permissionType: "Admin");
            Assert.IsTrue(user.HasPermission(storeId: 1, permissionType: "Admin"));

            user.DeletePermission(storeId: 1, permissionType: "Admin");
            Assert.IsFalse(user.HasPermission(storeId: 1, permissionType: "Admin"));
        }
        [TestMethod]
        public void Test_User_CanAssignfolder1_Andfile1_Permission()
        {
            var user = InitUserData();

            user.GrantPermission(storeId: 1, permissionType: "Contributor");
            Assert.IsTrue(user.HasPermission(storeId: 1, permissionType: "Contributor"));


            user.GrantPermission(storeId: 2, permissionType: "Reader");
            Assert.IsTrue(user.HasPermission(storeId: 2, permissionType: "Reader"));
        }


        [TestMethod]
        public void Test_Drive_Can_Add_And_Removefolder1()
        {
            var user = InitUserData();
            var folder = new Folder { StoreId = 1, StoreName = "TestFolder" };

            user.Drives[0].AddFolder(folder);
            Assert.IsTrue(user.Drives[0].Folders.Any(f => f.StoreId == 1));

            user.Drives[0].RemoveFolder(1);
            Assert.IsFalse(user.Drives[0].Folders.Any(f => f.StoreId == 1));
        }
        [TestMethod]
        public void Test_Drive_Can_Add_And_Removefile1()
        {
            var user = InitUserData();
            var file = new File { StoreId = 1, StoreName = "TestFile" };

            user.Drives[0].AddFile(file);
            Assert.IsTrue(user.Drives[0].Files.Any(f => f.StoreId == 1));

            user.Drives[0].RemoveFile(1);
            Assert.IsFalse(user.Drives[0].Files.Any(f => f.StoreId == 1));
        }
        [TestMethod]
        public void Test_Drive_HasMultiplefolder1_Andfile1()
        {
            var user = InitUserData();

            user.Drives[0].AddFolder(new Folder { StoreId = 1, StoreName = "Internship" });
            user.Drives[0].AddFolder(new Folder { StoreId = 2, StoreName = "bbv" });
            user.Drives[0].AddFile(new File { StoreId = 1, StoreName = "Mentorship.pdf" });
            user.Drives[0].AddFile(new File { StoreId = 2, StoreName = "bbv.pdf" });

            Assert.AreEqual(2, user.Drives[0].Folders.Count);
            Assert.AreEqual(2, user.Drives[0].Files.Count);
        }
        [TestMethod]
        public void Test_Drive_HasMultiplefolder1s()
        {
            var Drive = new Drive { DriveId = 1, DriveName = "GoogleDrive" };

            Drive.AddFolder(new Folder { StoreId = 1, StoreName = "Mentorship2024" });
            Drive.AddFolder(new Folder { StoreId = 2, StoreName = "bbv" });

            Assert.AreEqual(2, Drive.Folders.Count);
        }
        [TestMethod]
        public void Test_Drive_HasMultiplefile1s()
        {
            var Drive = new Drive { DriveId = 1, DriveName = "GoogleDrive" };

            Drive.AddFile(new File { StoreId = 1, StoreName = "Mentorship2024.pdf" });
            Drive.AddFile(new File { StoreId = 2, StoreName = "daovo.docx" });

            Assert.AreEqual(2, Drive.Files.Count);
            var count = Drive.Files.Count(e => e.StoreName.Equals("daovo.docx"));
            Assert.AreEqual(1, count);
        }


        [TestMethod]
        public void Test_Folder_Can_Add_And_Removefolder1()
        {
            var user = InitUserData();
            var subFolder = new Folder { StoreId = 1, StoreName = "TestFolder" };

            user.Drives[0].AddFolder(subFolder);
            Assert.IsTrue(user.Drives[0].Folders.Any(f => f.StoreId == 1));

            subFolder.AddSubFolder(new Folder { StoreId = 2, StoreName = "SubFolder" });
            Assert.IsTrue(subFolder.SubFolders.Any(f => f.StoreId == 2));

            subFolder.RemoveSubFolder(2);
            Assert.IsFalse(subFolder.SubFolders.Any(f => f.StoreId == 2));
        }
        [TestMethod]
        public void Test_Folder_Can_Add_And_Removefile1()
        {
            var user = InitUserData();
            var subFolder = new Folder { StoreId = 1, StoreName = "TestFolder" };

            user.Drives[0].AddFolder(subFolder);
            Assert.IsTrue(user.Drives[0].Folders.Any(f => f.StoreId == 1));

            subFolder.AddFile(new File { StoreId = 1, StoreName = "TestFile" });
            Assert.IsTrue(subFolder.Files.Any(f => f.StoreId == 1));

            subFolder.RemoveFile(1);
            Assert.IsFalse(subFolder.Files.Any(f => f.StoreId == 1));
        }
        [TestMethod]
        public void Test_Folder_CanRename()
        {
            var user = InitUserData();
            var folder = new Folder { StoreId = 1, StoreName = "OldName" };
            user.Drives[0].AddFolder(folder);
            folder.StoreName = "NewName";
            Assert.IsTrue(user.Drives[0].Folders.Any(f => f.StoreName == "NewName"));
        }
        [TestMethod]
        public void Test_Folder_HasMultiplesubFolder1()
        {
            var user = InitUserData();

            user.Drives[0].AddFolder(new Folder { StoreId = 2, StoreName = "Internship" });

            Folder folder = new Folder { StoreId = 1, StoreName = "bbv" };
            user.Drives[0].AddFolder(folder);
            user.Drives[0].AddFile(new File { StoreId = 1, StoreName = "Mentorship.pdf" });

            Folder folderWorking = new Folder { StoreId = 1, StoreName = "working" };
            folder.AddSubFolder(folderWorking);
            folder.AddSubFolder(new Folder { StoreId = 1, StoreName = "projects" });
            folder.AddSubFolder(new Folder { StoreId = 1, StoreName = "design" });
            folder.AddSubFolder(new Folder { StoreId = 1, StoreName = "training" });

            Assert.AreEqual(4, folder.SubFolders.Count);

            folderWorking.AddFile(new File { StoreId = 1, StoreName = "sample.sql" });
            Assert.AreEqual(1, folderWorking.Files.Count);
        }
        [TestMethod]
        public void Test_Folder_HasMultiplefile1()
        {
            var user = InitUserData();

            user.Drives[0].AddFolder(new Folder { StoreId = 2, StoreName = "Internship" });

            Folder folder = new Folder { StoreId = 1, StoreName = "bbv" };
            user.Drives[0].AddFolder(folder);
            user.Drives[0].AddFile(new File { StoreId = 1, StoreName = "Mentorship.pdf" });

            folder.AddFile(new File { StoreId = 1, StoreName = "sample.sql" });
            folder.AddFile(new File { StoreId = 2, StoreName = "sample.docx" });

            Assert.AreEqual(2, folder.Files.Count);
        }


        [TestMethod]
        public void Test_File_CanModify()
        {
            var user = InitUserData();

            var folder = new Folder { StoreId = 1, StoreName = "TestFolder" };
            user.Drives[0].AddFolder(folder);

            var file = new File { StoreId = 1, StoreName = "TestFile.txt" };
            folder.AddFile(file);

            file.RenameFile("NewStoreName.txt");
            Assert.IsTrue(folder.Files.Any(f => f.StoreName == "NewStoreName.txt"));
        }
        [TestMethod]
        public void Test_File_CanRemove()
        {
            var user = InitUserData();

            var folder = new Folder { StoreId = 1, StoreName = "TestFolder" };
            user.Drives[0].AddFolder(folder);

            var file = new File { StoreId = 1, StoreName = "TestFile.txt" };
            folder.AddFile(file);

            folder.Files.Remove(file);
            Assert.IsFalse(folder.Files.Any(f => f.StoreId == 1));
        }
    }

    [TestClass]
    public class Test_Basic_Object
    {
        private static User InitUserData()
        {
            var user1 = new User { Id = 1, Name = "John" };
            user1.AddDrive(new Drive { DriveId = 1, DriveName = "GoogleDrive" });
            user1.AddDrive(new Drive { DriveId = 2, DriveName = "OneDrive" });

            return user1;
        }

        [TestMethod]
        public void Test_User_CanCreateDrives()
        {
            var user = InitUserData();
            var newDrive = new Drive { DriveId = 3, DriveName = "Dropbox" };
            user.AddDrive(newDrive);
            Assert.IsTrue(user.Drives.Any(d => d.DriveId == 3 && d.DriveName == "Dropbox"));
        }

        [TestMethod]
        public void Test_User_CanAssignRole()
        {
            var user = InitUserData();
            user.GrantPermission(storeId: 1, permissionType: "Contributor");
            Assert.IsTrue(user.HasPermission(storeId: 1, permissionType: "Contributor"));
        }

        [TestMethod]
        public void Test_User_HasCorrectPermissions()
        {
            var user = InitUserData();
            user.GrantPermission(storeId: 1, permissionType: "Admin");
            Assert.IsTrue(user.HasPermission(storeId: 1, permissionType: "Admin"));
            Assert.IsFalse(user.HasPermission(storeId: 2, permissionType: "Admin"));
        }

        [TestMethod]
        public void Test_User_CanAccessMultipleRoles()
        {
            var user = InitUserData();
            user.GrantPermission(storeId: 1, permissionType: "Contributor");
            user.GrantPermission(storeId: 2, permissionType: "Reader");

            Assert.IsTrue(user.HasPermission(storeId: 1, permissionType: "Contributor"));
            Assert.IsTrue(user.HasPermission(storeId: 2, permissionType: "Reader"));
        }

        [TestMethod]
        public void Test_Drive_CanContainMultipleFolders()
        {
            var user = InitUserData();
            var drive = user.Drives[0];
            drive.AddFolder(new Folder { StoreId = 1, StoreName = "Folder1" });
            drive.AddFolder(new Folder { StoreId = 2, StoreName = "Folder2" });

            Assert.AreEqual(2, drive.Folders.Count);
            Assert.IsTrue(drive.Folders.Any(f => f.StoreId == 1 && f.StoreName == "Folder1"));
            Assert.IsTrue(drive.Folders.Any(f => f.StoreId == 2 && f.StoreName == "Folder2"));
        }

        [TestMethod]
        public void Test_Drive_CanContainMultipleFiles()
        {
            var user = InitUserData();
            var drive = user.Drives[0];
            drive.AddFile(new File { StoreId = 1, StoreName = "File1.pdf" });
            drive.AddFile(new File { StoreId = 2, StoreName = "File2.docx" });

            Assert.AreEqual(2, drive.Files.Count);
            Assert.IsTrue(drive.Files.Any(f => f.StoreId == 1 && f.StoreName == "File1.pdf"));
            Assert.IsTrue(drive.Files.Any(f => f.StoreId == 2 && f.StoreName == "File2.docx"));
        }

        [TestMethod]
        public void Test_Drive_CascadePermissions()
        {
            var user = InitUserData();
            var drive = user.Drives[0];
            var folder = new Folder { StoreId = 1, StoreName = "Folder1" };
            drive.AddFolder(folder);
            folder.AddFile(new File { StoreId = 1, StoreName = "File1.pdf" });
            user.GrantPermission(storeId: drive.DriveId, permissionType: "Admin");

            Assert.IsTrue(user.HasPermission(storeId: drive.DriveId, permissionType: "Admin"));
            Assert.IsTrue(user.HasPermission(storeId: folder.StoreId, permissionType: "Admin"));
            Assert.IsTrue(user.HasPermission(storeId: 1, permissionType: "Admin"));
        }

        [TestMethod]
        public void Test_Drive_Rename()
        {
            var user = InitUserData();
            var drive = user.Drives[0];
            drive.DriveName = "NewDriveName";
            Assert.AreEqual("NewDriveName", drive.DriveName);
        }

        [TestMethod]
        public void Test_Drive_Delete()
        {
            var user = InitUserData();
            user.RemoveDrive(1);
            Assert.IsFalse(user.Drives.Any(d => d.DriveId == 1));
        }

        [TestMethod]
        public void Test_Folder_CanBeCreated()
        {
            var user = InitUserData();
            var drive = user.Drives[0];
            var folder = new Folder { StoreId = 1, StoreName = "NewFolder" };
            drive.AddFolder(folder);
            Assert.IsTrue(drive.Folders.Any(f => f.StoreId == 1 && f.StoreName == "NewFolder"));
        }

        [TestMethod]
        public void Test_Folder_CanContainMultipleSubFolders()
        {
            var user = InitUserData();
            var drive = user.Drives[0];
            var folder = new Folder { StoreId = 1, StoreName = "ParentFolder" };
            drive.AddFolder(folder);

            folder.AddSubFolder(new Folder { StoreId = 2, StoreName = "SubFolder1" });
            folder.AddSubFolder(new Folder { StoreId = 3, StoreName = "SubFolder2" });

            Assert.AreEqual(2, folder.SubFolders.Count);
            Assert.IsTrue(folder.SubFolders.Any(f => f.StoreId == 2 && f.StoreName == "SubFolder1"));
            Assert.IsTrue(folder.SubFolders.Any(f => f.StoreId == 3 && f.StoreName == "SubFolder2"));
        }

        [TestMethod]
        public void Test_Folder_CanContainMultipleFiles()
        {
            var user = InitUserData();
            var drive = user.Drives[0];
            var folder = new Folder { StoreId = 1, StoreName = "Folder" };
            drive.AddFolder(folder);

            folder.AddFile(new File { StoreId = 1, StoreName = "File1.pdf" });
            folder.AddFile(new File { StoreId = 2, StoreName = "File2.docx" });

            Assert.AreEqual(2, folder.Files.Count);
            Assert.IsTrue(folder.Files.Any(f => f.StoreId == 1 && f.StoreName == "File1.pdf"));
            Assert.IsTrue(folder.Files.Any(f => f.StoreId == 2 && f.StoreName == "File2.docx"));
        }

        [TestMethod]
        public void Test_Folder_RoleIsolation()
        {
            var user = InitUserData();
            var drive = user.Drives[0];
            var folder1 = new Folder { StoreId = 1, StoreName = "Folder1" };
            var folder2 = new Folder { StoreId = 2, StoreName = "Folder2" };
            drive.AddFolder(folder1);
            drive.AddFolder(folder2);

            user.GrantPermission(storeId: folder1.StoreId, permissionType: "Contributor");
            Assert.IsTrue(user.HasPermission(storeId: folder1.StoreId, permissionType: "Contributor"));
            Assert.IsFalse(user.HasPermission(storeId: folder2.StoreId, permissionType: "Contributor"));
        }

        [TestMethod]
        public void Test_Folder_CanBeDeleted()
        {
            var user = InitUserData();
            var drive = user.Drives[0];
            var folder = new Folder { StoreId = 1, StoreName = "FolderToDelete" };
            drive.AddFolder(folder);
            Assert.IsTrue(drive.Folders.Any(f => f.StoreId == 1));

            drive.RemoveFolder(1);
            Assert.IsFalse(drive.Folders.Any(f => f.StoreId == 1));
        }

        [TestMethod]
        public void Test_Folder_MoveToAnotherDrive()
        {
            var user = InitUserData();
            var drive1 = user.Drives[0];
            var drive2 = user.Drives[1];
            var folder = new Folder { StoreId = 1, StoreName = "Folder" };
            drive1.AddFolder(folder);

            drive1.RemoveFolder(1);
            drive2.AddFolder(folder);

            Assert.IsFalse(drive1.Folders.Any(f => f.StoreId == 1));
            Assert.IsTrue(drive2.Folders.Any(f => f.StoreId == 1));
        }

        [TestMethod]
        public void Test_Folder_Rename()
        {
            var user = InitUserData();
            var drive = user.Drives[0];
            var folder = new Folder { StoreId = 1, StoreName = "OldName" };
            drive.AddFolder(folder);
            folder.StoreName = "NewName";

            Assert.AreEqual("NewName", folder.StoreName);
        }

        [TestMethod]
        public void Test_File_CanBeCreated()
        {
            var user = InitUserData();
            var drive = user.Drives[0];
            var folder = new Folder { StoreId = 1, StoreName = "Folder" };
            drive.AddFolder(folder);

            var file = new File { StoreId = 1, StoreName = "File1.txt" };
            folder.AddFile(file);

            Assert.IsTrue(folder.Files.Any(f => f.StoreId == 1 && f.StoreName == "File1.txt"));
        }

        [TestMethod]
        public void Test_File_MoveToAnotherFolder()
        {
            var user = InitUserData();
            var drive = user.Drives[0];
            var folder1 = new Folder { StoreId = 1, StoreName = "Folder1" };
            var folder2 = new Folder { StoreId = 2, StoreName = "Folder2" };
            drive.AddFolder(folder1);
            drive.AddFolder(folder2);

            var file = new File { StoreId = 1, StoreName = "FileToMove.txt" };
            folder1.AddFile(file);
            folder1.RemoveFile(1);
            folder2.AddFile(file);

            Assert.IsFalse(folder1.Files.Any(f => f.StoreId == 1));
            Assert.IsTrue(folder2.Files.Any(f => f.StoreId == 1));
        }

        [TestMethod]
        public void Test_File_CanBeDeleted()
        {
            var user = InitUserData();
            var drive = user.Drives[0];
            var folder = new Folder { StoreId = 1, StoreName = "Folder" };
            drive.AddFolder(folder);
            var file = new File { StoreId = 1, StoreName = "FileToDelete.txt" };
            folder.AddFile(file);

            folder.RemoveFile(1);
            Assert.IsFalse(folder.Files.Any(f => f.StoreId == 1));
        }

        [TestMethod]
        public void Test_File_CascadePermissions()
        {
            var user = InitUserData();
            var drive = user.Drives[0];
            var folder = new Folder { StoreId = 1, StoreName = "Folder" };
            drive.AddFolder(folder);
            var file = new File { StoreId = 1, StoreName = "File" };
            folder.AddFile(file);

            user.GrantPermission(storeId: file.StoreId, permissionType: "Editor");
            Assert.IsTrue(user.HasPermission(storeId: file.StoreId, permissionType: "Editor"));
            Assert.IsTrue(user.HasPermission(storeId: folder.StoreId, permissionType: "Editor"));
            Assert.IsTrue(user.HasPermission(storeId: drive.DriveId, permissionType: "Editor"));
        }
    }
}
    
