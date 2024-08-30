namespace Digital_Assest_Management
{
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
        public List<Folder> SubFolders { get; set; } = new List<Folder>();
        public List<File> Files { get; set; } = new List<File>();
        public int? ParentFolderId { get; set; } // Nullable to indicate if it's a root folder


        public void AddFolder(Folder folder)
        { // Add a subfolder to this folder and set its ParentFolderId
            SubFolders.Add(folder);
            folder.ParentFolderId = this.FolderId; // Set parent folder ID for subfolder
        }

        public void RemoveFolder(int folderId)
        { // Remove a subfolder by its ID
            SubFolders.RemoveAll(f => f.FolderId == folderId);
        }

        public void AddFile(File file)
        { // Add a file to this folder
            Files.Add(file);
        }

        public void RemoveFile(int fileId)
        { // Remove a file by its ID
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
}*/

namespace Digital_Assest_Management
{
}
    public class Permission
    {
        public int Id { get; set; }
        public int StoreId { get; set; }  // ID của StoreItem (Drive, Folder, hoặc File)
        public int UserId { get; set; }    // ID của người dùng được cấp quyền
        public string PermissionType { get; set; }  // Loại quyền (Admin, Contributor, Reader)
    }

    public interface IStoreItem
    {
        int StoreId { get; set; }
        string StoreName { get; set; }
        string StoreType { get; set; }
        int? ParentStoreId { get; set; }  // ID của StoreItem cha (nếu có)
    }

    public abstract class StoreItem : IStoreItem
    {
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public string StoreType { get; set; }
        public int? ParentStoreId { get; set; }
    }

    public class File : StoreItem
    {
        public int DriveId { get; set; }  // ID của Drive chứa file

        // Đổi tên file
        public void RenameFile(string newName)
        {
            StoreName = newName;
        }
    }

    public class Folder : StoreItem
    {
        public int DriveId { get; set; }  // ID của Drive chứa folder
        public List<Folder> SubFolders { get; set; } = new List<Folder>();  // Danh sách thư mục con
        public List<File> Files { get; set; } = new List<File>();  // Danh sách file

        // Thêm thư mục con vào thư mục hiện tại và thiết lập ParentStoreId
        public void AddSubFolder(Folder folder)
        {
            SubFolders.Add(folder);
            folder.ParentStoreId = this.StoreId;
        }

        // Xóa thư mục con dựa trên StoreId
        public void RemoveSubFolder(int storeId)
        {
            SubFolders.RemoveAll(f => f.StoreId == storeId);
        }

        // Đổi tên thư mục con dựa trên StoreId
        public void RenameSubFolder(int storeId, string newName)
        {
            var folder = SubFolders.FirstOrDefault(f => f.StoreId == storeId);
            if (folder != null)
            {
                folder.StoreName = newName;
            }
        }

        // Thêm file vào thư mục
        public void AddFile(File file)
        {
            Files.Add(file);
        }

        // Xóa file dựa trên StoreId
        public void RemoveFile(int storeId)
        {
            Files.RemoveAll(f => f.StoreId == storeId);
        }

        // Đổi tên file dựa trên StoreId
        public void RenameFile(int storeId, string newName)
        {
            var file = Files.FirstOrDefault(f => f.StoreId == storeId);
            if (file != null)
            {
                file.StoreName = newName;
            }
        }
    }

    public class Drive
    {
        public int DriveId { get; set; }
        public string DriveName { get; set; }
        public int OwnerId { get; set; }  // ID của người sở hữu Drive
        public List<Folder> Folders { get; private set; } = new List<Folder>();  // Danh sách thư mục trong Drive
        public List<File> Files { get; private set; } = new List<File>();  // Danh sách file trong Drive

        // Thêm thư mục vào Drive
        public void AddFolder(Folder folder)
        {
            Folders.Add(folder);
        }

        // Xóa thư mục dựa trên StoreId
        public void RemoveFolder(int storeId)
        {
            Folders.RemoveAll(e => e.StoreId == storeId);
        }

        // Đổi tên thư mục dựa trên StoreId
        public void RenameFolder(int storeId, string newName)
        {
            var folder = Folders.FirstOrDefault(f => f.StoreId == storeId);
            if (folder != null)
            {
                folder.StoreName = newName;
            }
        }

        // Thêm file vào Drive
        public void AddFile(File file)
        {
            Files.Add(file);
        }

        // Xóa file dựa trên StoreId
        public void RemoveFile(int storeId)
        {
            Files.RemoveAll(e => e.StoreId == storeId);
        }

        // Đổi tên file dựa trên StoreId
        public void RenameFile(int storeId, string newName)
        {
            var file = Files.FirstOrDefault(f => f.StoreId == storeId);
            if (file != null)
            {
                file.StoreName = newName;
            }
        }
    }

    public class User
    {
        public int Id { get; set; } // ID người dùng
        public string Name { get; set; } // Tên người dùng
        public List<Drive> Drives { get; set; } = new List<Drive>();  // Danh sách Drive của người dùng
        private List<Permission> Permissions { get; set; } = new List<Permission>();  // Danh sách quyền của người dùng

        // Thêm một Drive vào danh sách của người dùng
        public void AddDrive(Drive driveInfo)
        {
            Drives.Add(driveInfo);
        }

        // Xóa một Drive khỏi danh sách của người dùng
        public void RemoveDrive(int driveId)
        {
            Drives.RemoveAll(e => e.DriveId == driveId);
        }

        // Kiểm tra quyền truy cập của người dùng đối với StoreItem
        public bool HasPermission(int storeId, string permissionType)
        {
            var visited = new HashSet<int>();
            return HasPermissionRecursive(storeId, permissionType, visited);
        }

        // Kiểm tra quyền truy cập của người dùng đối với StoreItem (đệ quy)
        private bool HasPermissionRecursive(int storeId, string permissionType, HashSet<int> visited)
        {
            if (visited.Contains(storeId))
            {
                return false;
            }
            visited.Add(storeId);

            if (Permissions.Any(p => p.StoreId == storeId && p.PermissionType == permissionType))
            {
                return true;
            }

            var parentStores = GetParentStores(storeId);
            return parentStores.Any(parentStore => HasPermissionRecursive(parentStore.StoreId, permissionType, visited));
        }

        // Lấy danh sách thư mục cha của StoreItem dựa trên StoreId
        private List<Folder> GetParentStores(int storeId)
        {
            return Drives.SelectMany(drive => drive.Folders)
                         .Where(folder => folder.SubFolders.Any(f => f.StoreId == storeId))
                         .ToList();
        }

        // Cấp quyền truy cập cho một StoreItem cụ thể, chỉ dành cho Owner và Admin
        public void GrantPermission(int storeId, string permissionType)
        {
            if (!IsOwner(storeId) && !HasPermission(storeId, "Admin"))
            {
                throw new InvalidOperationException("Only owners and admins can grant permissions.");
            }

            if (!HasPermission(storeId, permissionType))
            {
                Permissions.Add(new Permission
                {
                    StoreId = storeId,
                    PermissionType = permissionType,
                    UserId = this.Id
                });
            }

            CascadePermission(storeId, permissionType);
        }

        // Cấp quyền truy cập cho các StoreItem con của StoreItem cụ thể
        private void CascadePermission(int storeId, string permissionType)
        {
            var subStores = GetSubStores(storeId);
            foreach (var subStore in subStores)
            {
                if (!HasPermission(subStore.StoreId, permissionType))
                {
                    Permissions.Add(new Permission
                    {
                        StoreId = subStore.StoreId,
                        PermissionType = permissionType,
                        UserId = this.Id
                    });
                }
            }
        }

        // Lấy danh sách các StoreItem con của StoreItem cha dựa trên StoreId
        private List<StoreItem> GetSubStores(int parentStoreId)
        {
            return Drives.SelectMany(drive =>
                drive.Folders.Where(folder => folder.StoreId == parentStoreId)
                            .SelectMany(folder => GetAllDescendants(folder))
            ).ToList();
        }

        // Lấy tất cả các StoreItem con của Folder
        private IEnumerable<StoreItem> GetAllDescendants(Folder folder)
        {
            var allDescendants = new List<StoreItem>();

            allDescendants.AddRange(folder.SubFolders);
            allDescendants.AddRange(folder.Files);

            foreach (var subFolder in folder.SubFolders)
            {
                allDescendants.AddRange(GetAllDescendants(subFolder));
            }

            return allDescendants;
        }

        // Kiểm tra xem người dùng có phải là chủ sở hữu của StoreItem không
        private bool IsOwner(int storeId)
        {
            return Drives.Any(d => d.OwnerId == this.Id && d.DriveId == storeId);
        }

        // Xóa quyền truy cập từ một StoreItem cụ thể và quyền này cũng sẽ bị xóa từ các StoreItem con
        public void DeletePermission(int storeId, string permissionType)
        {
            if (!IsOwner(storeId) && !HasPermission(storeId, "Admin"))
            {
                throw new InvalidOperationException("Only owners and admins can delete permissions.");
            }

            RemovePermission(storeId, permissionType);
            RemoveCascadingPermissions(storeId, permissionType);
        }

        // Xóa quyền truy cập từ một StoreItem
        private void RemovePermission(int storeId, string permissionType)
        {
            Permissions.RemoveAll(p => p.StoreId == storeId && p.PermissionType == permissionType);
        }

        // Xóa quyền truy cập từ các StoreItem con của StoreItem cụ thể
        private void RemoveCascadingPermissions(int storeId, string permissionType)
        {
            var subStores = GetSubStores(storeId);
            foreach (var subStore in subStores)
            {
                RemovePermission(subStore.StoreId, permissionType);
            }
        }
    }
