using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using ASPODES.Model;
using ASPODES.DTO.Consultation;

namespace ASPODES.WebAPI.Repository
{
    public interface IConsultationRepository
    {
        List<Consultation> GetConsultaion(int year);

        HttpResponseMessage DownloadConsultationTemplate();

        List<Consultation> UploadConsultationResult();

    }
}
