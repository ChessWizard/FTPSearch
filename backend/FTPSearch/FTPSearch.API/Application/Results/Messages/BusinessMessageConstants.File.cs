using FTPSearch.API.Application.Results.Messages.Common;

namespace FTPSearch.API.Application.Results.Messages;

public static partial class BusinessMessageConstants
{
    public partial struct Success
    {
        public partial struct File
        {
            // 2xx Status Codes

            #region Status Code - 200

            public static readonly BusinessMessage TransferFromFtpToDatabase = new(
                Message: "FTP üzerinden gelen dosya bilgilerinin veritabanına aktarım işlemi başarılı!",
                Code: "FLE20001",
                HttpStatus: StatusCodes.Status200OK
            );
            
            public static readonly BusinessMessage FoundByDirectory = new(
                Message: "Aradığınız dizindeki dosyalar bulundu!",
                Code: "FLE20002",
                HttpStatus: StatusCodes.Status200OK
            );
            
            public static readonly BusinessMessage AddedFtpAndDatabase = new(
                Message: "Eklemek istediğiniz dosya/dosyalar eklendi!",
                Code: "FLE20003",
                HttpStatus: StatusCodes.Status200OK
            );
            
            public static readonly BusinessMessage RemovedFtpAndDatabase = new(
                Message: "Silmek istediğiniz dosya/dosyalar silindi!",
                Code: "FLE20004",
                HttpStatus: StatusCodes.Status200OK
            );

            #endregion
        }
    }

    public partial struct Error
    {
        public partial struct File
        {
            // 4xx Status Codes
            
            #region Status Code - 400
            
            public static readonly BusinessMessage FtpFilesAlreadyExistOnDatabase = new(
                Message: "FTP üzerinden gelen tüm dosya bilgileri halihazırda veritabanında kayıtlı.",
                Code: "FLE40001",
                HttpStatus: StatusCodes.Status400BadRequest
            );
            
            #endregion
            
            #region Status Code - 404
            
            public static readonly BusinessMessage NotFoundByDirectory = new(
                Message: "Aradığınız dizinde herhangi bir dosya bulunamadı!",
                Code: "FLE40401",
                HttpStatus: StatusCodes.Status404NotFound
            );
            
            public static readonly BusinessMessage NotFoundOnRemove = new(
                Message: "Silmek üzere aradığınız dosya bulunamadı!",
                Code: "FLE40402",
                HttpStatus: StatusCodes.Status404NotFound
            );
            
            #endregion
            
            // 5xx Status Codes
            
            #region Status Code - 500
            
            public static readonly BusinessMessage TransferFromFtpToDatabaseFailed = new(
                Message: "FTP üzerinden gelen dosya bilgilerinin veritabanına aktarım işlemi yapılması sırasında " +
                         "bir hata meydana geldi!",
                Code: "FLE50001",
                HttpStatus: StatusCodes.Status500InternalServerError
            );
            
            public static readonly BusinessMessage RemovedFtpAndDatabaseFailed = new(
                Message: "Silmek istediğiniz dosya/dosyalar silinirken bir hata meydana geldi!",
                Code: "FLE50002",
                HttpStatus: StatusCodes.Status500InternalServerError
            );
            
            #endregion
        }
    }
}