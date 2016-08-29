namespace VideoCapture.Common.Core.DTOs
{
    // TODO: Expand as necessary to support any file specific parameters
    public class FileStorageDTO
    {
        #region Properties

        private string fileName;

        public string FileName
        {
            get { return this.fileName; }
        }

        #endregion

        #region Constructor(s)

        public FileStorageDTO(string fileName)
        {
            this.fileName = fileName;
        }

        #endregion
    }
}
