using FTPSearch.API.Application.Results.Messages.Common;

namespace FTPSearch.API.Application.Results.Messages;

public static partial class BusinessMessageConstants
{
    public partial struct Success
    {
        public partial struct Ftp
        {
            // 2xx Status Codes

            #region Status Code - 200

            public static readonly BusinessMessage AllFound = new(
                Message: "FTP üzerindeki tüm dosyaların getirilmesi işlemi başarılı!",
                Code: "FTP20001",
                HttpStatus: StatusCodes.Status200OK
            );
            
            public static readonly BusinessMessage Added = new(
                Message: "FTP üzerinde dosya oluşturma işlemi başarılı!",
                Code: "FTP20002",
                HttpStatus: StatusCodes.Status200OK
            );
            
            public static readonly BusinessMessage Removed = new(
                Message: "FTP üzerinde dosya silme işlemi başarılı!",
                Code: "FTP20003",
                HttpStatus: StatusCodes.Status200OK
            );
            
            public static readonly BusinessMessage RemovedDirectory = new(
                Message: "FTP üzerinde dosya yolu silme işlemi başarılı!",
                Code: "FTP20004",
                HttpStatus: StatusCodes.Status200OK
            );
            
            public static readonly BusinessMessage Downloaded = new(
                Message: "FTP üzerindeki dosyanın indirme işlemi başarılı!",
                Code: "FTP20005",
                HttpStatus: StatusCodes.Status200OK
            );

            #endregion
        }
    }

    public partial struct Error
    {
        public partial struct Ftp
        {
            // 4xx Status Codes
            
            #region Status Code - 404
            
            public static readonly BusinessMessage NotFound = new(
                Message: "FTP üzerinde herhangi bir dosya bulunamadı.",
                Code: "FTP40401",
                HttpStatus: StatusCodes.Status404NotFound
            );
            
            public static readonly BusinessMessage NotFoundDirectory = new(
                Message: "FTP üzerinde verilen dosya yolu bulunamadı.",
                Code: "FTP40402",
                HttpStatus: StatusCodes.Status404NotFound
            );
            
            #endregion
            
            // 5xx Status Codes
            
            #region Status Code - 500
            
            public static readonly BusinessMessage RemoveFailed = new(
                Message: "FTP üzerinde dosya silinmeye çalışılırken bir hata meydana geldi.",
                Code: "FTP50001",
                HttpStatus: StatusCodes.Status500InternalServerError
            );
            
            public static readonly BusinessMessage RemoveDirectoryFailed = new(
                Message: "FTP üzerinde dosya yolu silinmeye çalışılırken bir hata meydana geldi.",
                Code: "FTP50002",
                HttpStatus: StatusCodes.Status500InternalServerError
            );
            
            #endregion
        }
    }
}