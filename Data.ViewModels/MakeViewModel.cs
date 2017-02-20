namespace Data.ViewModels
{
    using AutoMapper;
    using Data.ViewModels.Base;
    using WebService.Infrastructure.Automapper;

    public class MakeViewModel : ValuesViewModel, IMapFrom<Make>, IMapTo<Make>//, IHaveCustomMappings
    {
        //public void CreateMappings(IMapperConfiguration configuration)
        //{
        //    configuration.CreateMap<MakeViewModel, Make>()
        //        .ForMember(x => ()x.Id, m => m.Ignore());
        //}
    }
}