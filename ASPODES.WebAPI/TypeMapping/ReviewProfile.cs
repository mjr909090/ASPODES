using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

using ASPODES.DTO.Review;
using ASPODES.Model;

namespace ASPODES.WebAPI.TypeMapping
{
    public class ReviewProfile : Profile
    {
        protected override void Configure()
        {

            CreateMap<Recommendation, GetRecommendationDTO>()
                .ForMember( DTO=>DTO.CandidateName, config=>config.MapFrom( r=>r.Candidate.Name ))
                .ForMember(DTO=>DTO.CandidatePersonId, config=>config.MapFrom(r=>r.Candidate.PersonId))
                .ForMember(DTO => DTO.RecommenderName, config => config.MapFrom(r => r.Recommender.Name))
                .ForMember(DTO=>DTO.InstituteName, config=>config.MapFrom(r=>r.Institute.Name));

            //ReviewAssignment
            CreateMap<ReviewAssignment, GetReviewAssignmentDTO>()
                .ForMember( DTO=>DTO.ProjectName, config=>config.MapFrom( ra=>ra.Application.ProjectName))
                .ForMember( DTO=>DTO.InstititeName, config=>config.MapFrom( ra=>ra.Application.Institute.Name))
                .ForMember( DTO=>DTO.LeaderName, config=>config.MapFrom(ra=>ra.Application.Leader.Name))
                .ForMember( DTO=>DTO.ExpertName, config=>config.MapFrom(ra=>ra.Expert.Name))
                .ForMember( DTO=>DTO.ProjectTypeName, config=>config.MapFrom( ra=>ra.Application.ProjectType.Name));

            CreateMap<ReviewAssignment, GetReviewAssignmentVO>()
                .ForMember(DTO => DTO.ExpertName, config => config.MapFrom(ra => ra.Expert.Name));


            CreateMap<Application, GetApplicationReviwAssignmentDTO>()
                .ForMember(DTO => DTO.InstituteName, config => config.MapFrom(a => a.Institute.Name))
                .ForMember(DTO => DTO.LeaderName, config => config.MapFrom(a => a.Leader.Name))
                .ForMember(DTO => DTO.ReviewAssignments, config => config.MapFrom(a => a.ReviewAssignments == null ? null : a.ReviewAssignments.Where( ra=>ra.Status != ReviewAssignmentStatus.CHANGE).Select( Mapper.Map<GetReviewAssignmentVO>).ToList()));

           
            //ReviewComment
            CreateMap<AddReviewCommentDTO, ReviewComment>()
                .ForMember( rc=>rc.ApplicationId, config=>config.Ignore() )
                .ForMember( rc=>rc.ExpertId, config=>config.Ignore())
                .ForMember( rc=>rc.Year, config=>config.Ignore())
                .ForMember( rc=>rc.Status, config=>config.Ignore() );

            CreateMap<ReviewComment, GetReviewCommentDTO>()
                .ForMember(DTO => DTO.ProjectName, config => config.MapFrom(rc => rc.Application.ProjectName))
                .ForMember(DTO => DTO.ExpertName, config => config.MapFrom(rc => rc.Expert.Name));

            CreateMap<ReviewComment, GetReviewCommentVO>();
                //.ForMember(DTO => DTO.ExpertName, config => config.MapFrom(rc => rc.Expert.Name));

            CreateMap<Application, GetApplicationReviewCommentDTO>()
                .ForMember(DTO => DTO.InstituteName, config => config.MapFrom(a => a.Institute.Name))
                .ForMember(DTO => DTO.LeaderName, config => config.MapFrom(a => a.Leader.Name))
                .ForMember(DTO=>DTO.ProjectTypeName, config=>config.MapFrom( a=>a.ProjectType.Name))
                .ForMember(DTO => DTO.ReviewComments, config => 
                    config.MapFrom( a=>a.ReviewComments == null ? null: a.ReviewComments.Where( rc=>rc.Year == a.CurrentYear ).Select(Mapper.Map<GetReviewCommentVO>).ToList()));

            //CreateMap<Application, GetExportApplicationReviewCommentDTO>()
            //    .ForMember(DTO => DTO.InstituteName, config => config.MapFrom(a => a.Institute.Name))
            //    .ForMember(DTO => DTO.LeaderName, config => config.MapFrom(a => a.Leader.Name))
            //    .ForMember(DTO => DTO.ProjectTypeName, config => config.MapFrom(a => a.ProjectType.Name))
            //    .ForMember(DTO => DTO.ReviewComments, config =>config.MapFrom(a => a.ReviewComments == null ? null : a.ReviewComments.Where(rc => rc.Year == a.CurrentYear).Select(Mapper.Map<GetExportReviewCommentVO>).ToList())); ;

            //CreateMap<ReviewComment, GetExportReviewCommentVO>()
            //    .ForMember( erc=>erc.LeaderName, config=>config.MapFrom( rc=>rc.Expert.Name));
        }
    }
}