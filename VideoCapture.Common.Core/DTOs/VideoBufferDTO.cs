namespace VideoCapture.Common.Core.DTOs
{
    using System;
    using System.IO;

    public class VideoBufferDTO
    {
        #region Properties

        private MemoryStream buffer;

        public MemoryStream Buffer
        {
            get { return this.buffer; }
        }

        // TODO: Placeholder for now for the encoding type...  Is there a enumeration that is portable?  Or
        // do we need to create our own enumeration...
        private string videoEncodingType;

        public string VideoEncodingType
        {
            get { return this.videoEncodingType; }
        }

        #endregion

        #region Constructor(s)

        public VideoBufferDTO(MemoryStream buffer, string encodingType)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer", @"Memory stream argument is null");
            }

            // TODO: Any other checks on the memory stream make sense?  We don't really want to go
            // wild checking the validity of the memory stream here...
            if (string.IsNullOrWhiteSpace(encodingType))
            {
                throw new ArgumentNullException("encodingType", @"Encoding type argument is null");
            }

            // TODO: Does it make sense to copy the memory stream so we have our own copy, or should
            // we just pass the reference?  Could be a lot of copies of the same data if we do a copy...
            // Perhaps having an explicit deep copy method would make sense... For now just copying the
            // reference
            this.buffer = buffer;

            this.videoEncodingType = encodingType;
        }

        #endregion
    }
}
