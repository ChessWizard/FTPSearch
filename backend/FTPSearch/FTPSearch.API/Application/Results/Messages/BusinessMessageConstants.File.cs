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
            
            // 5xx Status Codes
            
            #region Status Code - 500
            
            public static readonly BusinessMessage TransferFromFtpToDatabaseFailed = new(
                Message: "FTP üzerinden gelen dosya bilgilerinin veritabanına aktarım işlemi yapılması sırasında " +
                         "bir hata meydana geldi!",
                Code: "FLE50001",
                HttpStatus: StatusCodes.Status500InternalServerError
            );
            
            #endregion
        }
    }
}