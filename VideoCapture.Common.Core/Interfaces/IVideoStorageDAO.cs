namespace VideoCapature.Common.Core.Interfaces
{
    using System.Threading.Tasks;
    using VideoCapture.Common.Core.DTOs;

    public interface IVideoStorageDAO
    {
        Task Save(FileStorageDTO file, VideoBufferDTO buffer);
    }
}
