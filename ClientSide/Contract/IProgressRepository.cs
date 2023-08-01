﻿using ClientSide.ViewModels.Progress;
using ClientSide.Utilities.Handlers;


namespace ClientSide.Contract
{
    public interface IProgressRepository : IGeneralRepository<ProgressVM>
    {
        Task<ResponseHandlers<ProgressVM>> GetProgressById(Guid guid);
        Task<ResponseHandlers<IEnumerable<ProgressVM>>> GetAllProgress();
        Task<ResponseHandlers<ProgressVM>> CreateProgress(ProgressVM progress);
        Task<ResponseHandlers<UpdateProgressVM>> UpdateProgress(UpdateProgressVM updateProgress);
        Task<ResponseHandlers<Guid>> DeepDeleteProgress(Guid guid);
        /*   Task<ResponseHandlers<Guid>> DeleteProgress(Guid guid);*/
    }
}