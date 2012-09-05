#pragma once

namespace System{
	namespace IO{
		enum FileMode
		{
			/// <summary>Specifies that the operating system should create a new file. This requires <see cref="F:System.Security.Permissions.FileIOPermissionAccess.Write" />. If the file already exists, an <see cref="T:System.IO.IOException" /> is thrown.</summary>
			CreateNew = 1,
			/// <summary>Specifies that the operating system should create a new file. If the file already exists, it will be overwritten. This requires <see cref="F:System.Security.Permissions.FileIOPermissionAccess.Write" />. System.IO.FileMode.Create is equivalent to requesting that if the file does not exist, use <see cref="F:System.IO.FileMode.CreateNew" />; otherwise, use <see cref="F:System.IO.FileMode.Truncate" />. If the file already exists but is a hidden file, an <see cref="T:System.UnauthorizedAccessException" /> is thrown.</summary>
			Create,
			/// <summary>Specifies that the operating system should open an existing file. The ability to open the file is dependent on the value specified by <see cref="T:System.IO.FileAccess" />. A <see cref="T:System.IO.FileNotFoundException" /> is thrown if the file does not exist.</summary>
			Open,
			/// <summary>Specifies that the operating system should open a file if it exists; otherwise, a new file should be created. If the file is opened with FileAccess.Read, <see cref="F:System.Security.Permissions.FileIOPermissionAccess.Read" /> is required. If the file access is FileAccess.Write then <see cref="F:System.Security.Permissions.FileIOPermissionAccess.Write" /> is required. If the file is opened with  FileAccess.ReadWrite, both <see cref="F:System.Security.Permissions.FileIOPermissionAccess.Read" /> and <see cref="F:System.Security.Permissions.FileIOPermissionAccess.Write" /> are required.  If the file access is FileAccess.Append, then <see cref="F:System.Security.Permissions.FileIOPermissionAccess.Append" /> is required.</summary>
			OpenOrCreate,
			/// <summary>Specifies that the operating system should open an existing file. Once opened, the file should be truncated so that its size is zero bytes. This requires <see cref="F:System.Security.Permissions.FileIOPermissionAccess.Write" />. Attempts to read from a file opened with Truncate cause an exception.</summary>
			Truncate,
			/// <summary>Opens the file if it exists and seeks to the end of the file, or creates a new file. FileMode.Append can only be used in conjunction with FileAccess.Write. Attempting to seek to a position before the end of the file will throw an <see cref="T:System.IO.IOException" /> and any attempt to read fails and throws an <see cref="T:System.NotSupportedException" />.</summary>
			Append
		};
	}
}